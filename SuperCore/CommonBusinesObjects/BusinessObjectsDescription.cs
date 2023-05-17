using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.CommonBusinesObjects
{
    public class BusinessObjectsDescription
    {
        public BusinessObjectsDescription()
        {
            Objects = new List<ObjectDescription>();        
        }

        public List<ObjectDescription> Objects { get; set; }
    }

    public class ObjectDescription
    {
        public ObjectDescription()
        {
            PropertyNames = new List<string>();        
        }

        public string Name { get; set; }
        [XmlArrayItem(ElementName = "PropertyName")]
        public List<string> PropertyNames { get; set; }
    }
}