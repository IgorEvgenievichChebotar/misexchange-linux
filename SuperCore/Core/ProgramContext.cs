using System;
using System.IO;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Diagnostics;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Глобальная точка доступа к основным компонентам SuperCore
    /// </summary>
    public class ProgramContext
    {
        /// <summary>
        /// Диспетчер задач, запускаемых по расписанию
        /// </summary>
        public class Taskmanager
        {
            private void InitErrorCompensation()
            {
                var requestErrorCompensationFolderName = (String)ProgramContext.Settings["requestErrorCompensationFolder", false];
                if (String.IsNullOrEmpty(requestErrorCompensationFolderName))
                    RequestErrorCompensation.DefaultFolder = Path.Combine(PathHelper.AssemblyDirectory, "RequestErrorCompensation");
                else
                    RequestErrorCompensation.DefaultFolder = (String)requestErrorCompensationFolderName;
            }
            /// <summary>
            /// Пытается запустить выполнение задач
            /// </summary>
            /// <param name="backgroundMode"></param>
            /// <returns></returns>
            private bool TryRun(bool backgroundMode)
            {
                bool result = false;
                InitErrorCompensation();

                if (TaskManagerWaitInterval != null)
                {
                    if (TaskManagerWaitInterval.Value < taskManagerMinimumWaitInterval)
                        throw new ApplicationException(String.Format("Значение настройки taskManagerWaitInterval должно быть не менее \"{0}\" (секунд). Приложение не запущено", taskManagerMinimumWaitInterval));
                    else
                    {
                        SuperCore.TaskManager.RegisterTasks();
                        SuperCore.TaskManager.Run(TaskManagerWaitInterval.Value, backgroundMode);
                        result = true;
                    }
                }
                return result;
            }
            /// <summary>
            /// Пытается запустить выполнение задач в блокирующем вызвавший поток режиме 
            /// </summary>
            /// <returns>Признак упешности запуска</returns>
            public void TryRunForeground()
            {
                TryRun(backgroundMode: false);
            }

            /// <summary>
            /// Пытается запустить выполнение задач в неблокирующем фоновом потоке 
            /// </summary>
            /// <returns>Признак упешности запуска</returns>
            public bool TryRunBackground()
            {
                return TryRun(backgroundMode: true);
            }

            public void Stop()
            {
                SuperCore.TaskManager.Stop();
            }
        }
        /// <summary>
        /// Получает и обрабатывает сообщения от серверного приложения
        /// </summary>
        public class Serverlistener
        {
            protected AppServerListener listener = null;

            public bool IsInitialized { get { return (listener != null); } }

            /// <summary>
            /// Пытается начать прослушивание сообщений
            /// </summary>
            /// <returns>Признак упешности начала прослушивания</returns>
            public virtual bool TryRun()
            {
                bool result = false;

                Object appServerListenerURI = ProgramContext.Settings["appServerListenerURI", false];
                if (appServerListenerURI != null)
                {
                    // Регистрируем все доступные в сборке процессоры
                    ProcessorPool.RegisterProcessors();
                    // Создаём и запускаем HTTP-слушателя
                    listener = new AppServerListener((String)appServerListenerURI);
                    listener.Run();
                    result = true;
                }

                return result;
            }

            public void RunProcessor(String requestXml, StreamWriter writer)
            {
                listener.RunProcessor(requestXml, writer);
            }

            /// <summary>
            /// Останавливает прослушивание порта и обработку сообщений
            /// </summary>
            public void Stop()
            {
                if (listener != null)
                    listener.Dispose();
            }
        }

        /// <summary>
        /// Получает и обрабатывает xml-сообщения от внешнего приложения
        /// </summary>
        public class XmlRequestlistener : Serverlistener
        {
            /// <summary>
            /// Пытается начать прослушивание сообщений
            /// </summary>
            /// <returns>Признак упешности начала прослушивания</returns>
            public override bool TryRun()
            {
                bool result = false;

                Object appServerListenerURI = ProgramContext.Settings["appServerListenerURI", false];
                if (appServerListenerURI != null)
                {
                    // Регистрируем все доступные в сборке процессоры
                    ProcessorPool.RegisterProcessors();
                    // Создаём и запускаем HTTP-слушателя
                    listener = new XmlRequestListener((String)appServerListenerURI);
                    listener.Run();
                    result = true;
                }

                return result;
            }
        }

        private const Int32 taskManagerMinimumWaitInterval = 60; // seconds

        private static Lazy<Serverlistener> serverListener = new Lazy<Serverlistener>(() => new Serverlistener());
        private static Lazy<XmlRequestlistener> xmlRequestServerListener = new Lazy<XmlRequestlistener>(() => new XmlRequestlistener());
        private static Lazy<Taskmanager> taskManager = new Lazy<Taskmanager>(() => new Taskmanager());

        public static ProgramSettings Settings { get; set; }

        /// <summary>
        /// Коммуникатор с некоторым обощённым сервером. Свойство наиболее общего типа
        /// </summary>
        public static BaseCommunicator BaseCommunicator
        {
            get
            {
                if (SolutionType == SolutionTypes.LIMS)
                    return LisCommunicator;
                else if (SolutionType == SolutionTypes.HEM)
                    return HemCommunicator;
                else
                    return null;
            }
        }

        public static int? TaskManagerWaitInterval
        {
            get
            {
                return (int?)ProgramContext.Settings["taskManagerWaitInterval", false];
            }
        }

        /// <summary>
        /// Коммуникатор с сервером ЛИС
        /// </summary>
        public static LimsCommunicator LisCommunicator { get; set; }
        /// <summary>
        /// Коммуникатор с сервером Службы Крови
        /// </summary>
        public static HemCommunicator HemCommunicator { get; set; }
        /// <summary>
        /// Коммуникатор с сервером отчетов
        /// </summary>
        private static ReportServerCommunicator reportServerCommunicator;
        /// <summary>
        /// Коммуникатор с "Сервером Отчётов"
        /// </summary>
        public static ReportServerCommunicator ReportServerCommunicator
        {
            get
            {
                if (reportServerCommunicator == null)
                {
                    string reportServerAddress = (string)Settings["reportServerAddress", false];

                    if (SolutionType == SolutionTypes.LIMS)
                    {
                        // Если в настройках приложения отсутствует настройка "reportServerAddress", берём адрес сервера отчётов из глобальных опций
                        if (String.IsNullOrEmpty(reportServerAddress))
                        {
                            if (null == LisCommunicator.ServerOptions)
                                return null;

                            ru.novolabs.SuperCore.LimsBusinessObjects.ServerOption serverOption = LisCommunicator.ServerOptions.GetServerOption("ReportServerURI");
                            if (serverOption != null)
                                reportServerAddress = serverOption.Value;
                        }
                        reportServerCommunicator = new ReportServerCommunicator(reportServerAddress);
                    }
                    else if (SolutionType == SolutionTypes.HEM)
                    {
                        // Если в настройках приложения отсутствует настройка "reportServerAddress", берём адрес сервера отчётов из глобальных опций
                        if (String.IsNullOrEmpty(reportServerAddress))
                        {
                            if (null == HemCommunicator.ServerOptions)
                                return null;
                            ru.novolabs.SuperCore.HemBusinessObjects.ServerOption serverOption = HemCommunicator.ServerOptions.GetServerOption("reportServerURI");
                            if (serverOption != null)
                                reportServerAddress = serverOption.Value;
                        }
                        reportServerCommunicator = new ReportServerCommunicator(reportServerAddress);
                    }
                }

                return reportServerCommunicator;
            }
        }

        /// <summary>
        /// Кэш справочников
        /// </summary>
        public static BaseDictionaryCache Dictionaries
        {
            get
            {
                if (SolutionType == SolutionTypes.LIMS)
                    return LisCommunicator.DictionaryCache;
                else if (SolutionType == SolutionTypes.HEM)
                    return HemCommunicator.DictionaryCache;
                else
                    return null;
            }
        }

        /// <summary>
        /// Слушатель сообщений от серверного приложения
        /// </summary>

        public static Serverlistener ServerListener
        {
            get
            {
                return serverListener.Value;
            }
        }

        /// <summary>
        /// Слушатель xml-сообщений от внешнего приложения
        /// </summary>
        public static XmlRequestlistener XmlRequestListener
        {
            get
            {
                return xmlRequestServerListener.Value;
            }
        }

        /// <summary>
        /// Диспетчер задач
        /// </summary>
        public static Taskmanager TaskManager
        {
            get
            {
                return taskManager.Value;
            }
        }

        public static SolutionTypes SolutionType { get; set; }

        public enum SolutionTypes
        {
            Unknown = 0,
            LIMS = 1,
            HEM = 2,
            UNIVERSAL = 3
        }

        public static ExchangeDB DBManager { get; set; }

        //public static Lazy<Stopwatch> Stopwatch = new Lazy<Stopwatch>(() => new Stopwatch());
    }
}