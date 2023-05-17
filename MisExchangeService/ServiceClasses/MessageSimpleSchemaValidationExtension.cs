using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;

namespace ru.novolabs.MisExchange.ServiceClasses
{
    public class MessageSimpleSchemaValidationExtension : BehaviorExtensionElement
    {
        private const string IsEnabledSchemaCacheAttributeName = "IsEnabledSchemaCache";
        private const string SchemaUriAttributeName = "SchemaUri";
        private const string TargetNamespaceAttributeName = "TargetNamespace";
        private const string IsEnabledTraceValidationAttributeName = "IsEnabledTraceValidation";

        private MessageSimpleSchemaValidationBehavior behavior = null;
        [ConfigurationProperty(IsEnabledSchemaCacheAttributeName,IsRequired=false,DefaultValue=true)]
        public bool IsEnabledSchemaCache
        {
            get { return (bool)base[IsEnabledSchemaCacheAttributeName]; }
            set { base[IsEnabledSchemaCacheAttributeName] = value; }
        }
        [ConfigurationProperty(SchemaUriAttributeName,IsRequired=true)]
        public string SchemaUri
        {
            get { return (string)base[SchemaUriAttributeName]; }
            set { base[SchemaUriAttributeName] = value; }
        }
        [ConfigurationProperty(TargetNamespaceAttributeName,IsRequired=true)]
        public string TargetNamespace
        {
            get { return (string)base[TargetNamespaceAttributeName]; }
            set { base[TargetNamespaceAttributeName] = value; }
        }
        [ConfigurationProperty(IsEnabledTraceValidationAttributeName, IsRequired=false, DefaultValue=false)]
        public bool IsEnabledTraceValidation
        {
            get { return (bool)base[IsEnabledTraceValidationAttributeName]; }
            set { base[IsEnabledTraceValidationAttributeName] = value; }        
        }
        public override Type BehaviorType
        {
            get { return typeof(MessageSimpleSchemaValidationBehavior); }
        }

        protected override object CreateBehavior()
        {
            if (behavior == null)
            {
                SimpleSchemaValidator validator = new SimpleSchemaValidator(IsEnabledSchemaCache, SchemaUri, TargetNamespace, IsEnabledTraceValidation);
                MessageSimpleSchemaValidationInspector inspector = new MessageSimpleSchemaValidationInspector(validator, validator, validator, validator);
                behavior = new MessageSimpleSchemaValidationBehavior(inspector, inspector);
            }
            return behavior;
        }
    }
}
