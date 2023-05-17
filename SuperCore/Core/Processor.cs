using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace ru.novolabs.SuperCore
{
    // Процессор запрашивается через ProcessorPool.
    // После вызова Execute процессор не удаляется, а помечается как свободный.
    // Пул процессоров возвращает любой свободный процессор с нужным именем.
    // Если свободного процессора нет, то пул создает новый процессор.

    public abstract class Processor : IDisposable
    {
        /// <summary>
        /// Служит для обработки "события"
        /// </summary>
        /// <param name="content"></param>
        public virtual void Execute(XmlNode content)
        {
            // В случае, если процессор предназначен для обработки события (ответ клиенту не требуется), необходимо переопределить метод в наследнике
            throw new NotImplementedException();
        }

        /// <summary>
        /// Служит для синхронной обработки запроса в стандартном для .Net XML-формате с синтаксисом элементов
        /// </summary>
        /// <param name="content"></param>
        /// <param name="writer"></param>
        public virtual void Execute(Object content, StreamWriter writer)
        {
            // В случае, если процессор предназначен для синхронной обработки запроса, необходимо переопределить метод в наследнике
            throw new NotImplementedException();
        }

        /// <summary>
        /// Служит для синхронной обработки запроса в нестандартном для .Net XML-формате с синтаксисом атрибутов(применяемый в ЛИС и Службе крови)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="writer"></param>
        public virtual void Execute(XmlNode content, StreamWriter writer)
        {
            // В случае, если процессор предназначен для синхронной обработки запроса, необходимо переопределить метод в наследнике
            throw new NotImplementedException();
        }

        /// <summary>
        /// Служит для асинхронной обработки запроса в стандартном для .Net XML-формате с синтаксисом элементов
        /// </summary>
        /// <param name="content"></param>
        /// <param name="writer"></param>
        /// <param name="callbackAddress"></param>
        public virtual void ExecuteAsync(Object content, StreamWriter writer, String callbackAddress)
        {
            // В случае, если процессор предназначен для асинхронной обработки запроса, необходимо переопределить метод в наследнике
            throw new NotImplementedException();
        }

        /// <summary>
        /// Служит для асинхронной обработки запроса в нестандартном для .Net XML-формате с синтаксисом атрибутов(применяемый в ЛИС и Службе крови)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="writer"></param>
        /// <param name="callbackAddress"></param>
        public virtual void ExecuteAsync(XmlNode content, StreamWriter writer, String callbackAddress)
        {
            // В случае, если процессор предназначен для асинхронной обработки запроса, необходимо переопределить метод в наследнике
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            ProcessorPool.FreeProcessor(this);
            RequestDone = false;
        }

        public Boolean RequestDone { get; set; }
    }

    // Пул процессоров.
    // Расширяется по мере необходимости. Все запрошенные из пула процессоры в конечном счете
    // вернутся в пул.
    public static class ProcessorPool
    {
        class ProcessorInfo
        {
            public Type Type { get; set; }
            public String Name { get; set; }
        }

        class ProcessorPoolInfo
        {
            public Processor Processor { get; set; }
            public Boolean Locked { get; set; }
        }

        static Object locker = new Object();

        static List<ProcessorInfo> processors = new List<ProcessorInfo>();
        static List<ProcessorPoolInfo> pool = new List<ProcessorPoolInfo>();

        /// <summary>
        /// Регистрирует все процессоры, доступные в текущей и вызывающей исполняемой сборке. Класс должен иметь конструктор без параметров и наследоваться от Processor.
        /// </summary>
        public static void RegisterProcessors()
        {
            // Ищем все классы процессоров, объявленных в сборке, содержащей данный код (в библиотеке nlscorlib.dll)
            var processorsTypes =
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.IsSubclassOf(typeof(Processor))
                select type;

            // Ищем все классы процессоров, объявленных в вызывающей исполняемой сборке, обратившейся к данной библиотеке
            var processorsTypes2 =
                from type in Assembly.GetEntryAssembly().GetTypes()
                where type.IsSubclassOf(typeof(Processor))
                select type;

            processorsTypes = processorsTypes.Union(processorsTypes2);
            foreach (Type processorType in processorsTypes)
            {
                Object[] attrs = processorType.GetCustomAttributes(typeof(ProcessorName), false);

                if (attrs.Length == 0)
                {
                    Log.WriteError(String.Format("Процессору {0} нужно назначить имя.", processorType.Name));
                    continue;
                }

                String processorName = ((ProcessorName)attrs[0]).Name;

                if ((processorName == BloodProcessorsNames.Stub) || (processorName == LisProcessorsNames.Stub))
                    continue;

                if (FindProcessorInfo(processorType) != null)
                    continue;

                ProcessorInfo error = FindProcessorInfo(processorName);

                if (error != null)
                {
                    Log.WriteError(String.Format("Процессор {0} имеет такое же имя ({1}) как и процессор {2}.",
                        processorType.Name, processorName, error.Name));
                    continue;
                }

                ProcessorInfo processorInfo = new ProcessorInfo();

                processorInfo.Name = processorName;
                processorInfo.Type = processorType;

                lock (processors)
                    processors.Add(processorInfo);

                Log.WriteText(String.Format("Зарегистрирован процессор {0} c именем {1}.",
                    processorInfo.Type.Name, processorInfo.Name));
            }
        }

        static ProcessorInfo FindProcessorInfo(Type processorType)
        {
            lock (processors)
                return processors.Find(x => x.Type == processorType);
        }
        static ProcessorInfo FindProcessorInfo(String processorName)
        {
            lock (processors)
                return processors.Find(x => x.Name == processorName);
        }

        static ProcessorPoolInfo FindNotLockedProcessor(Type processorType)
        {
            lock (pool)
                return pool.Find(x => !x.Locked && x.Processor.GetType() == processorType);
        }

        public static void FreeProcessor(Processor processor)
        {
            lock (pool)
            {
                ProcessorPoolInfo ppi = pool.Find(x => x.Processor == processor);

                if (ppi != null)
                    ppi.Locked = false;
            }
        }

        // Ищет процессор по его имени.
        public static Processor GetProcessor(String processorName)
        {
            lock (locker)
            {
                ProcessorInfo procInfo = FindProcessorInfo(processorName);

                if (procInfo == null)
                    return null;

                ProcessorPoolInfo ppi = FindNotLockedProcessor(procInfo.Type);

                if (ppi == null)
                {
                    ppi = new ProcessorPoolInfo();
                    ppi.Processor = (Processor)Activator.CreateInstance(procInfo.Type);

                    lock (pool)
                        pool.Add(ppi);
                }

                ppi.Locked = true;
                return ppi.Processor;
            }
        }
    }
}