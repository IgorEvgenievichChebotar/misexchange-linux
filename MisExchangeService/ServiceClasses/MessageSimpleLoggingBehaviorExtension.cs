using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;

namespace ru.novolabs.MisExchange.ServiceClasses
{
    public class MessageSimpleLoggingBehaviorExtension: BehaviorExtensionElement
    {
        private Lazy<MessageSimpleLoggingBehavior> behavior = new Lazy<MessageSimpleLoggingBehavior>(() => 
        {
            MessageSimpleLoggingInspector inspector = new MessageSimpleLoggingInspector();
            return new MessageSimpleLoggingBehavior(inspector, inspector);        
        });

        public override Type BehaviorType
        {
            get { return typeof(MessageSimpleLoggingBehavior); }
        }

        protected override object CreateBehavior()
        {
            return behavior.Value;
        }
    }
}
