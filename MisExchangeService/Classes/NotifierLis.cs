using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ru.novolabs.MisExchange.Classes
{
    class NotifierLis:INotifierLis
    {
        public void NotifyLisServer(ExternalResult result)
        {
            try
            {
                string xmlTemplate = "";
                string xmlTemplateFile = Path.Combine(Application.StartupPath, "notificationTemplate.xml");
                try
                {
                    if (File.Exists(xmlTemplateFile))
                        xmlTemplate = File.ReadAllText(xmlTemplateFile);
                }
                catch { }

                if (String.IsNullOrEmpty(xmlTemplate))
                    return;

                String xml = ProcessStringTemplate(xmlTemplate, result);
                NotifyLis(xml);
            }
            catch (Exception ex)
            {
                Log.WriteError("Не удалось оповестить сервер. Ошибка: {0}", ex.ToString());
            }
        }

        private void NotifyLis(String Xml)
        {
            Log.WriteText("NotifyLis");
            Log.WriteText("Notification text:\r\n\r\n{0}", Xml);
            String serverAddress = ProgramContext.LisCommunicator.GetServerAddress();

            try
            {
                HttpWebRequest request;
                WebResponse responce;
                Stream requestStream;
                Stream resultStream;
                Byte[] sendBytes;
                String resultString = String.Empty;
                StreamReader reader;

                if (!serverAddress.Contains("http://"))
                {
                    serverAddress = "http://" + serverAddress;
                }

                request = (HttpWebRequest)HttpWebRequest.Create(serverAddress);
                request.Method = "POST";
                request.Timeout = 120000;
                request.ProtocolVersion = HttpVersion.Version10;
                request.UserAgent = "Mozilla/3.0";
                request.Accept = "text/html, */*";
                request.ContentType = "text/html";
                request.Proxy = null;

                requestStream = request.GetRequestStream();
                sendBytes = Encoding.GetEncoding(1251).GetBytes(Xml);
                requestStream.Write(sendBytes, 0, sendBytes.Length);
                requestStream.Close();

                responce = request.GetResponse();

                resultStream = responce.GetResponseStream();

                reader = new StreamReader(resultStream, Encoding.GetEncoding(1251));
                resultString = reader.ReadToEnd();
                reader.Close();
                resultStream.Close();
                responce.Close();
                Log.WriteText("Уведомление ЛИС выполнено. Ответ сервера: " + resultString);
            }
            catch (Exception e)
            {
                Log.WriteError("Ошибка уведомления ЛИС. Сообщение: " + e.ToString());
            }
        }

        private string ProcessStringTemplate(String xmlTemplate, ExternalResult result)
        {
            if (String.IsNullOrEmpty(xmlTemplate))
                return String.Empty;

            xmlTemplate = xmlTemplate.Replace("%YYYY%", DateTime.Now.Year.ToString("D4"));
            xmlTemplate = xmlTemplate.Replace("%MM%", DateTime.Now.Month.ToString("D2"));
            xmlTemplate = xmlTemplate.Replace("%DD%", DateTime.Now.Day.ToString("D2"));
            xmlTemplate = xmlTemplate.Replace("%HH%", DateTime.Now.Hour.ToString("D2"));
            xmlTemplate = xmlTemplate.Replace("%mm%", DateTime.Now.Minute.ToString("D2"));
            xmlTemplate = xmlTemplate.Replace("%ss%", DateTime.Now.Second.ToString("D2"));
            xmlTemplate = xmlTemplate.Replace("%nnn%", DateTime.Now.Millisecond.ToString("D3"));

            xmlTemplate = xmlTemplate.Replace("%requestId%", result.Id.ToString());

            return xmlTemplate;
        }
    }
}
