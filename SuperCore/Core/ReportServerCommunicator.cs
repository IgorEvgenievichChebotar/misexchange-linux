using ru.novolabs.SuperCore.Reporting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public class ReportServerCommunicator
    {
        private string serverAddress = null;

        public string ServerAddress
        {
            get { return serverAddress; }
            set { serverAddress = value; }
        }

        public ReportServerCommunicator(string serverAddress)
        {
            if (String.IsNullOrEmpty(serverAddress))
                throw new ArgumentNullException("serverAddress");
            Contract.EndContractBlock();

            this.serverAddress = serverAddress;
        }

        private string SendRequest(string xmlRequest)
        {
            Log.WriteText("\r\n\r\nSending request to the server [{0}]. Request-text:\r\n\r\n {1}", serverAddress, xmlRequest);

            if (!serverAddress.Contains("http://"))
                serverAddress = "http://" + serverAddress;
            string resultString = String.Empty;

            var request = (HttpWebRequest)WebRequest.Create(serverAddress);
            {
                request.Method = "POST";
                using (var requestStream = request.GetRequestStream())
                using (var sw = new StreamWriter(requestStream, Encoding.GetEncoding(1251)))
                    sw.Write(xmlRequest);

                using (var response = request.GetResponse())
                using (var responceStream = response.GetResponseStream())
                using (var reader = new StreamReader(responceStream, Encoding.GetEncoding(1251)))
                {
                    resultString = reader.ReadToEnd();
                    if (ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_ALL)
                    {
                        Log.WriteText("\r\n\r\nServer Response: \r\n\r\n {0}", resultString);
                    }
                    else
                    {
                        Log.WriteText("\r\n\r\nServer Response: \r\n\r\n Response in this case is ordinary too large. If you want to see the full Response, change loggingLevel to ALL");                     
                    }
                }
            }
            return resultString;
        }

        /// <summary>
        /// Возвращает список отчётов, доступных на сервере
        /// </summary>
        /// <returns></returns>
        public List<String> GetReportList()
        {
            var request = new XmlRequest<Object>() { RequestType = ReportServerProcessorsNames.GetReportList, Content = new Object() };
            var responce = SendRequest(request.Serialize(Encoding.GetEncoding(1251))).Deserialize<XmlRequestResponce<GetReportListResponse>>(Encoding.GetEncoding(1251));

            if ((responce.Content.Message != null) && (responce.Content.Message.Severity == (int)ErrorMessageType.Error))
                throw new ApplicationException(responce.Content.Message.Message);
            else
                return responce.Content.ReportList;
        }

        /// <summary>
        /// Возвращает описание отчёта
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public ReportDescription GetReportDescription(string reportName)
        {
            var parameters = new GetReportDescriptionParams() { ReportName = reportName };
            var request = new XmlRequest<GetReportDescriptionParams>() { RequestType = ReportServerProcessorsNames.GetReportDescription, Content = parameters };
            var responce = SendRequest(request.Serialize(Encoding.GetEncoding(1251))).Deserialize<XmlRequestResponce<GetReportDescriptionResponse>>(Encoding.GetEncoding(1251));

            if ((responce.Content.Message != null) && (responce.Content.Message.Severity == (int)ErrorMessageType.Error))
                throw new ApplicationException(responce.Content.Message.Message);
            else
                return responce.Content.ReportDescription;
        }

        /// <summary>
        /// Запускает выполнение отчёта с указанными параметрами. Возвращает список сформированных документов (имена и содержимое файлов, закодированное в base64)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<ReportingDocument> ExecuteReport(ExecuteReportParams parameters)
        {
            var request = new XmlRequest<ExecuteReportParams>() { RequestType = ReportServerProcessorsNames.ExecuteReport, Content = parameters };
            var responce = SendRequest(request.Serialize(Encoding.GetEncoding(1251))).Deserialize<XmlRequestResponce<ExecuteReportResponse>>(Encoding.GetEncoding(1251));

            if ((responce.Content.Message != null) && (responce.Content.Message.Severity == (int)ErrorMessageType.Error))
                throw new ApplicationException(responce.Content.Message.Message);
            else
                return responce.Content.Documents;
        }

        /// <summary>
        /// Возвращает для указанного отчёта элементы справочника по имени
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="dictionaryName"></param>
        /// <returns></returns>
        public List<DictionaryItemIdCaptionPair> GetDictionary(string reportName, string dictionaryName)
        {
            var request = new XmlRequest<GetDictionaryParams>() { RequestType = ReportServerProcessorsNames.GetDictionary, Content = new GetDictionaryParams() { ReportName = reportName, DictionaryName = dictionaryName } };
            var responce = SendRequest(request.Serialize(Encoding.GetEncoding(1251))).Deserialize<XmlRequestResponce<GetDictionaryResponse>>(Encoding.GetEncoding(1251));

            if ((responce.Content.Message != null) && (responce.Content.Message.Severity == (int)ErrorMessageType.Error))
                throw new ApplicationException(responce.Content.Message.Message);
            else
                return responce.Content.Items;
        }
    }
}