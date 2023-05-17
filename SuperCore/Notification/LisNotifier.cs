using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace ru.novolabs.SuperCore.Notification
{
    public class LisNotifier
    {
        public bool TryNotifyLis(String xml, String serverAddress)
        {
            if (!serverAddress.Contains("http://"))
            {
                serverAddress = "http://" + serverAddress;
            }
            using (HttpClient client = new HttpClient())
            using (StringContent content = new StringContent(xml, Encoding.GetEncoding(1251)))
            {
                client.Timeout = new TimeSpan(0, 2, 0);
                try
                {
                    var response = client.PostAsync(serverAddress, content).Result;
                    string resultString = response.Content.ReadAsStringAsync().Result;
                    if (!String.IsNullOrEmpty(resultString))
                    {
                        resultString = FormatString(resultString);                  
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        Log.WriteError("LIS returned bad http request status code({0} {1}) for POST notification method:\r\n{2}\r\n", response.StatusCode.GetHashCode(), response.ReasonPhrase, resultString ?? String.Empty);
                        return false;
                    }
                    else
                    {
                        Log.WriteText("Уведомление ЛИС выполнено. Ответ сервера:\r\n{0}\r\n" ,resultString ?? String.Empty);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Log.WriteError("Ошибка уведомления ЛИС. Сообщение:\r\n{0}", e.ToString());
                    return false;
                }
            }
        }
        String FormatString(string xml)
        {
            return FormatXml(FormatWhitespaces(xml));       
        }
        string FormatWhitespaces(String xml)
        {
            return xml.Replace("#$D#$A", "\r\n");
        }
        // Быстрое форматирование XML с использованием класса XDocument
        string FormatXml(string xml)
        {
            try
            {
                return XDocument.Parse(xml).ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }
    }
}
