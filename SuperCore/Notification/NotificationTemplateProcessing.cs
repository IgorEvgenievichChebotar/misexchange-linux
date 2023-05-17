using ru.novolabs.SuperCore.Reporting;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.Notification
{
    public static class NotificationTemplateProcessing
    {
        public static string ProcessStringTemplate(String text, List<ReportParamValue> paramValues)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            text = text.Replace("%YYYY%", DateTime.Now.Year.ToString("D4"));
            text = text.Replace("%MM%", DateTime.Now.Month.ToString("D2"));
            text = text.Replace("%DD%", DateTime.Now.Day.ToString("D2"));
            text = text.Replace("%HH%", DateTime.Now.Hour.ToString("D2"));
            text = text.Replace("%mm%", DateTime.Now.Minute.ToString("D2"));
            text = text.Replace("%ss%", DateTime.Now.Second.ToString("D2"));
            text = text.Replace("%nnn%", DateTime.Now.Millisecond.ToString("D3"));

            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (ReportParamValue paramValue in paramValues)
            {
                if (text.IndexOf("%" + paramValue.Name + "%") != -1)
                {
                    if (!values.ContainsKey("%" + paramValue.Name + "%"))
                        values.Add("%" + paramValue.Name + "%", paramValue.Value);
                    else
                        values["%" + paramValue.Name + "%"] += paramValue.Value;
                }
            }

            foreach (KeyValuePair<string, string> keyValue in values)
                text = text.Replace(keyValue.Key, keyValue.Value);

            return text;
        }
        public static  string ProcessStringTemplateFromMisExchange(String xmlTemplate, string requestId, string state)
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

            xmlTemplate = xmlTemplate.Replace("%requestId%", requestId);
            xmlTemplate = xmlTemplate.Replace("%state%", state);

            return xmlTemplate;
        }
    }
}
