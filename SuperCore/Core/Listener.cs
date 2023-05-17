using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Получает и обрабатывает сообщения от серверного приложения
    /// </summary>
    public class AppServerListener: IDisposable
    {
        HttpListener listener = new HttpListener();
        volatile Boolean needStop = false;
        Thread loop;

        public AppServerListener(String uriPrefix) : this(new String[] { uriPrefix }) { }
        public AppServerListener(String[] uriPrefixes)
        {
            foreach (String prefix in uriPrefixes)
                listener.Prefixes.Add(prefix);

            loop = new Thread(ListenLoop);
            loop.Name = "server_listen";
            loop.IsBackground = true;
        }

        public void Dispose()
        {
            Stop();
        }

        // Начинает прием запросов от сервера приложений.
        public void Run()
        {
            Log.WriteText("----- start listen -----");

            listener.Start();
            needStop = false;

            loop.Start();
        }

        void ListenLoop()
        {
            //  Log.WriteText(String.Format("Listen thread start: {0} [{1}]", Thread.CurrentThread.Name, DateTime.Now));

            while (!needStop)
            {
                try
                {
                    //IAsyncResult result = listener.BeginGetContext(ar => new Thread(Work).Start(ar), listener);
                    IAsyncResult result = listener.BeginGetContext(ar => new System.Threading.Tasks.Task(Work, ar).Start(), listener);

                    while (!result.AsyncWaitHandle.WaitOne(25))
                        if (needStop)
                            break;
                }
                catch (OutOfMemoryException ex)
                {
                    Utils.PrintProcessInfo();
                    Log.WriteError("ListenLoop exception: " + ex.ToString());
                    GC.Collect();
                    Log.WriteText("--------------------------");
                    Utils.PrintProcessInfo();
                }
            }

            // Log.WriteText(String.Format("Listen thread stop: {0} [{1}]", Thread.CurrentThread.Name, DateTime.Now));
        }

        void Work(Object obj)
        {
            if (needStop)
                return;

            try
            {

                HttpListenerContext context = listener.EndGetContext((IAsyncResult)obj);

                HttpListenerRequest request = context.Request;
                HttpListenerResponse responce = context.Response;

                responce.StatusCode = 200;

                using (Stream requestIn = request.InputStream)
                using (Stream responceOut = responce.OutputStream)
                {
                    String requestXML = null;
                    using (var reader = new StreamReader(requestIn, Encoding.GetEncoding(1251)))
                        requestXML = reader.ReadToEnd();

                    if ((ProgramContext.Settings != null) && (ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_DEBUG))
                        Log.WriteText(String.Format("Http request received from {0}. Content:\n\n {1}\n", request.RemoteEndPoint.ToString(), requestXML));

                    if (requestXML == String.Empty)
                        return;

                    using (var writer = new StreamWriter(responceOut, Encoding.GetEncoding(1251)))
                        RunProcessor(requestXML, writer);
                }
            }
            catch (OutOfMemoryException ex)
            {
                Utils.PrintProcessInfo();
                Log.WriteError("AppServerListener.Work() exception: " + ex.ToString());
                GC.Collect();
                Log.WriteText("--------------------------");
                Utils.PrintProcessInfo();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
            }
        }

        // Запускает процессор из пула процессоров, в качестве параметра передавая requestXml.
        // Процессор будет получен из атрибута phoxEvent в requestXml.
        // Если процессор во время вызова Execute не изменит состояние RequestDone на true,
        // то текст запроса будет сохранен в специальную папку.
        public virtual void RunProcessor(String requestXml, StreamWriter writer)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(ObjectReader.NormalizeXML(requestXml));

            XmlNode phoxEvent = document.SelectSingleNode(XMLConst.XML_NODE_EVENT);
            XmlNode phoxRequest = document.SelectSingleNode(XMLConst.XML_DOCTYPE_NAME);
            String callBackAddress = null;
            bool async = false;
            if (phoxRequest != null)
            {
                XmlAttribute attCallbackAddress = phoxRequest.Attributes[XMLConst.XML_Attribute_Callback_Address];
                if (attCallbackAddress != null)
                {
                    callBackAddress = attCallbackAddress.Value;
                }

                XmlAttribute attAsync = phoxRequest.Attributes[XMLConst.XML_Attribute_Async];
                async = (attAsync != null) ? attAsync.Value.ToLower().Equals(XMLConst.XML_Bool_Value_True) : false;
            }

            XmlNode content = (phoxEvent != null) ? phoxEvent.SelectSingleNode(XMLConst.XML_Node_Content) : phoxRequest.SelectSingleNode(XMLConst.XML_Node_Content);

            String processorName = (phoxEvent != null) ? phoxEvent.Attributes[XMLConst.XML_Request_Type].Value : phoxRequest.Attributes[XMLConst.XML_Request_Type].Value;

            // Пытаемся получить наиболее подходящий процессор из пула.
            Processor proc = ProcessorPool.GetProcessor(processorName);

            if (proc == null)
            {
                Log.WriteError(String.Format("Не найден обработчик события [{0}].", processorName));
                return;
            }

            using (proc)
            {
                try
                {
                    if (phoxEvent != null)
                    // Запуск обработки события
                    {
                        proc.Execute(content);
                    }
                    else
                    // Запуск обработки запроса
                    {
                        if (async)
                            proc.ExecuteAsync(content, writer, callBackAddress);
                        else
                            proc.Execute(content, writer);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Необработанная ошибка в процессоре [{0}]: {1}\n\r\t{2}", processorName, ex.Message, ex.StackTrace));
                }

                RequestErrorCompensation.CheckProcessorStatus(proc, processorName, requestXml);
            }
        }

        // Прекращает прием запросов от сервера и пытается дождаться завершения текущих задач.
        public void Stop()
        {
            lock (listener)
                needStop = true;

            loop.Join(TimeSpan.FromSeconds(30));

            if (listener.IsListening)
                listener.Stop();

            Log.WriteText("----- stop listen -----");
        }
    }
}