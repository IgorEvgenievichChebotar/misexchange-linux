using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.Reporting;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Получает и обрабатывает запросы (в Xml-формате) от внешнего приложения
    /// </summary>
    /*public*/
    internal class XmlRequestListener : AppServerListener
    {
        public XmlRequestListener(String uriPrefix) : base(new String[] { uriPrefix }) { }
        public XmlRequestListener(String[] uriPrefixes) : base(uriPrefixes) { }

        /// <summary>
        /// Запускает процессор из пула процессоров, передавая в аргументе содержимое запроса. Процессор будет получен с использованием элемента RequestType.
        /// Если процессор во время вызова Execute не изменит состояние RequestDone на true, то текст запроса будет сохранен в специальную папку.
        /// </summary>
        /// <param name="requestXml"></param>
        /// <param name="writer"></param>
        public override void RunProcessor(String requestXml, StreamWriter writer)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(ObjectReader.NormalizeXML(requestXml));

            XmlNode serverRequestNode = document.SelectSingleNode(XMLConst.XML_Node_ServerRequest);

            XmlNode requestTypeNode = serverRequestNode.SelectSingleNode(XMLConst.XML_Node_RequestType);
            string requestType = requestTypeNode.InnerText;

            XmlNode asyncNode = serverRequestNode.SelectSingleNode(XMLConst.XML_Node_Async);
            bool async = false;
            if (asyncNode != null)
                async = asyncNode.InnerText != null ? bool.Parse(asyncNode.InnerText) : false;

            XmlNode callbackAddressNode = serverRequestNode.SelectSingleNode(XMLConst.XML_Node_CallbackAddress);
            String callbackAddress = null;
            if (callbackAddressNode != null)
                callbackAddress = callbackAddressNode.InnerText;

            // Пытаемся получить наиболее подходящий процессор из пула.
            Processor processor = ProcessorPool.GetProcessor(requestType);
            if (processor == null)
            {
                Log.WriteError(String.Format("Не найден обработчик запроса [{0}].", requestType));
                return;
            }

            Object[] attrs = processor.GetType().GetCustomAttributes(typeof(ContentType), false);//  processorType.GetCustomAttributes(typeof(ProcessorName), false);
            if (attrs.Length == 0)
                throw new ApplicationException("ContentType attribute not found");
            Type contentType = ((ContentType)attrs[0]).Type;

            XmlNode contentNode = serverRequestNode.SelectSingleNode(XMLConst.XML_Node_ContentUpperCase);
            var sb = new StringBuilder(contentNode.OuterXml.Length);
            using (var wr = XmlWriter.Create(sb))
            {
                wr.WriteStartElement(contentType.Name);
                wr.WriteString("{0}");
                wr.WriteEndElement();
            }
            string contentXml = String.Format(sb.ToString(), contentNode.InnerXml);
            Object content = SerializeHelper.Deserialize(contentType, contentXml, Encoding.UTF8);

            using (processor)
            {
                try
                {
                    // Запуск обработки запроса
                    {
                        if (async)
                            processor.ExecuteAsync(content, writer, callbackAddress);
                        else
                            processor.Execute(content, writer);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Необработанная ошибка в процессоре [{0}]: {1}\n\r\t{2}", requestType, ex.Message, ex.StackTrace));
                }

                RequestErrorCompensation.CheckProcessorStatus(processor, requestType, requestXml);
            }
        }
    }
}