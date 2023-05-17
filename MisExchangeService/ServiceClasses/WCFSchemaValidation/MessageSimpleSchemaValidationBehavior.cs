using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ru.novolabs.MisExchange.ServiceClasses
{
    class MessageSimpleSchemaValidationBehavior : IEndpointBehavior
    {
        IClientMessageInspector ClientMessageInspector { get; set; }
        IDispatchMessageInspector DispatcherMessageInspector { get; set; }
        public MessageSimpleSchemaValidationBehavior(IClientMessageInspector clientMessageInspector = null, IDispatchMessageInspector dispatcherMessageInspector = null)
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
            clientRuntime.ClientMessageInspectors.Add(ClientMessageInspector);        
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            if (DispatcherMessageInspector == null)
                return;
            //endpointDispatcher.DispatchRuntime.MessageInspectors.Add(DispatcherMessageInspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            
        }
    }
}
