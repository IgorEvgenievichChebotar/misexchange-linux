using System;
using System.IO;

namespace ru.novolabs.SuperCore
{
    // Занимается компенсацией ошибок процессоров, сохраняя запросы на диск.
    public static class RequestErrorCompensation
    {
        const String FILE_EXT = ".xml";

        static Object checkLocker = new Object();
        static Object processLocker = new Object();

        public static String DefaultFolder { get; set; }

        // Проверяет статус RequestDone процессора. Если статус не выставлен, то сохраняет запрос requestXml
        // в папку по умолчанию.
        public static void CheckProcessorStatus(Processor processor, String processorName, String requestXml)
        {
            lock (checkLocker)
                if (!processor.RequestDone)
                {
                    try
                    {
                        String tmpName = String.Format(@"{0}\{1}{2:yyMMddHHmmss}", DefaultFolder, processorName, DateTime.Now);

                        String fileName = String.Format("{0}{1}", tmpName, FILE_EXT);

                        Int32 iter = 1;
                        while (File.Exists(fileName))
                            fileName = String.Format("{0}({1}){2}", tmpName, ++iter, FILE_EXT);

                        Directory.CreateDirectory(DefaultFolder);
                        File.WriteAllText(fileName, requestXml);
                        Log.WriteText(String.Format("Текст запроса для процессора {0} был сохранен для повторной обработки в файл {1}, т.к. процессор не изменил статус RequestDone.", processorName, fileName));
                    }
                    catch
                    {
                        Log.WriteError(String.Format("Не удалось сохранить текст запроса для процессора {0}. ", processorName));
                    }

                }
        }

        // Проверяет папку по умолчанию. Если есть файлы с запросами, пытается передать их повторно на обработку.
        // После передачи на обработку - удаляет файл.
        public static void Process()
        {
            lock (processLocker)
            {
                DirectoryInfo dir = new DirectoryInfo(DefaultFolder);

                foreach (FileInfo fileInfo in dir.GetFiles(String.Format("*{0}", FILE_EXT)))
                {
                    Log.WriteText("Обработка файла " + fileInfo.Name + "\r\n");
                    try
                    {
                        String requestXml;

                        using (FileStream fs = fileInfo.OpenRead())
                        using (StreamReader sr = new StreamReader(fs))
                            requestXml = sr.ReadToEnd();

                        using (var ms = new MemoryStream())
                        using (var writer = new StreamWriter(ms))
                        {
                            if (ProgramContext.ServerListener.IsInitialized)
                                ProgramContext.ServerListener.RunProcessor(requestXml, writer);
                            else
                                ProgramContext.XmlRequestListener.RunProcessor(requestXml, writer);
                        }

                        fileInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Случилась беда в RequestErrorCompensation.Process с сообщением: {0}.", ex.Message));
                    }
                }
            }
        }
    }
}