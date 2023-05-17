using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ru.novolabs.MisExchange.ServiceClasses
{
    class MessageSimpleLoggingInspector: IClientMessageInspector, IDispatchMessageInspector
    {
        protected const string requestStrCosnt = "request";
        protected const string responseStrConst = "response";
        protected const string clientTypeStrConst = "Client";
        protected const string serviceTypeStrConst = "Service";

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Logging(ref reply, responseStrConst);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            Logging(ref request, requestStrCosnt);
            return null;
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            Logging(ref request, requestStrCosnt, true);
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Logging(ref reply, responseStrConst, true);
        }
        protected virtual void Logging(ref System.ServiceModel.Channels.Message message, string messageType, bool isService = false)
        {
            string sideType = clientTypeStrConst;
            if (isService)
                sideType = serviceTypeStrConst;
        
            Log.WriteText("Soap {0} message {1} :\r\n{2}", sideType, messageType, message.ToString());         
        
        }
    }
}
