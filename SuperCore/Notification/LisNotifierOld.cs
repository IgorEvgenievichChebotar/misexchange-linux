using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ru.novolabs.SuperCore.Notification
{
    public static class LisNotifierOld
    {
        public static void NotifyLis(String xml, String serverAddress)
        {
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
                sendBytes = Encoding.GetEncoding(1251).GetBytes(xml);
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
    }
}
