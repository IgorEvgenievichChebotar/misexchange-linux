using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ru.novolabs.MisExchange.ServiceClasses
{
    class MessageSimpleSchemaValidationInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        private const string requestStrCosnt = "request";
        private const string responseStrConst = "response";
        private const string clientTypeStrConst = "Client";
        private const string serviceTypeStrConst = "Service";
        public MessageSimpleSchemaValidationInspector(SimpleSchemaValidator clientRequestValidator = null, SimpleSchemaValidator clientReplyValidator = null,
            SimpleSchemaValidator serviceRequestValidator = null, SimpleSchemaValidator serviceReplyValidator = null)
        {
            ClientRequestValidator = clientRequestValidator;
            ClientReplyValidator = clientReplyValidator;
            ServiceRequestValidator = serviceRequestValidator;
            ServiceReplyValidator = serviceReplyValidator;
        }
        SimpleSchemaValidator ClientRequestValidator { get; set; }
        SimpleSchemaValidator ClientReplyValidator { get; set; }
        SimpleSchemaValidator ServiceRequestValidator { get; set; }
        SimpleSchemaValidator ServiceReplyValidator { get; set; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (ClientReplyValidator == null)
                return;
            Validate(ref reply, ClientReplyValidator, responseStrConst, clientTypeStrConst);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (ClientRequestValidator == null)
                return null;
            Validate(ref request, ClientRequestValidator, requestStrCosnt, clientTypeStrConst);
            return null;
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            if (ServiceRequestValidator == null)
                return null;
            Validate(ref request, ServiceRequestValidator, requestStrCosnt, serviceTypeStrConst);
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (ServiceReplyValidator == null)
                return;
            Validate(ref reply, ServiceReplyValidator, responseStrConst, serviceTypeStrConst);
        }
        private void Validate(ref Message message, SimpleSchemaValidator validator, string messageType, string sideType)
        {
            string errorText = validator.ValidateMessage(ref message);
            if (!String.IsNullOrEmpty(errorText))
                throw new FaultException(String.Format("Some errors occured while validation {0} {1} message:{2}\r\n", sideType, messageType, errorText));
        }
    }
    class SimpleSchemaValidator
    {
        public SimpleSchemaValidator(bool isEnabledSchemaCache, string schemaUri, string targetNamespace, bool isEnabledTraceValidation = false)
        {
            IsEnabledSchemaCache = isEnabledSchemaCache;
            SchemaUri = schemaUri;
            TargetNamespace = targetNamespace;
            IsEnabledTraceValidation = isEnabledTraceValidation;
            if (IsEnabledSchemaCache)
                SchemaSetCached = GetSchema();
        }
        private bool IsEnabledSchemaCache { get; set; }
        private bool IsEnabledTraceValidation { get; set; }
        private XmlSchemaSet SchemaSetCached { get; set; }
        private string SchemaUri { get; set; }
        private string TargetNamespace { get; set; }
        public string ValidateMessage(ref Message message)
        {
            XmlReader reader = null;
            try
            {
                XmlDocument bodyDocument = new XmlDocument();
                bodyDocument.Load(message.GetReaderAtBodyContents());
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(GetSchema());

                reader = XmlReader.Create(new XmlNodeReader(bodyDocument), settings);
                string errorStr = Validate(reader);
                if (!String.IsNullOrEmpty(errorStr))
                    return errorStr;


                Message copy = Message.CreateMessage(message.Version, null, new XmlNodeReader(bodyDocument.DocumentElement));
                copy.Headers.CopyHeadersFrom(message);
                copy.Properties.CopyProperties(message.Properties);
                message.Close();
                message = copy;
            }
            catch (Exception ex)
            {
                Log.WriteError("Error while validation:\r\n{0}", ex.ToString());
                return String.Empty;
            }
            return String.Empty;
        }
        private string Validate(XmlReader reader)
        {
            try
            {
                while (reader.Read()) { if (IsEnabledTraceValidation) { Log.WriteText("Validating Element:{0}", reader.Name); } }
                return String.Empty;
            }
            catch (XmlSchemaValidationException ex)
            {
                Log.WriteError("Message wasnt validated:\r\n{0}", ex.ToString());
                return String.Format("[Line: {0}][Pos: {1}] Element:[{2}] {3} ", ex.LineNumber, ex.LinePosition, reader.Name, ex.Message);
            }
            catch (Exception ex)
            {
                throw;
            }        
        }
        private XmlSchemaSet GetSchema()
        {
            if (IsEnabledSchemaCache && SchemaSetCached != null)
                return SchemaSetCached;
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(TargetNamespace, SchemaUri);
            return schemaSet;
        }
    
    }
}
