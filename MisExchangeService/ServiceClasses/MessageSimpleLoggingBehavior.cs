using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ru.novolabs.MisExchange.ServiceClasses
{
    class MessageSimpleLoggingBehavior : IEndpointBehavior
    {
        IClientMessageInspector ClientMessageInspector { get; set; }
        IDispatchMessageInspector DispatcherMessageInspector { get; set; }
        public MessageSimpleLoggingBehavior(IClientMessageInspector clientMessageInspector, IDispatchMessageInspector dispatcherMessageInspector)
        {
            ClientMessageInspector = clientMessageInspector;
            DispatcherMessageInspector = dispatcherMessageInspector;        
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            if (ClientMessageInspector == null)
                return;
            clientRuntime.MessageInspectors.Add(ClientMessageInspector);        
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            if (DispatcherMessageInspector == null)
                return;
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(DispatcherMessageInspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            
        }
    }
}
