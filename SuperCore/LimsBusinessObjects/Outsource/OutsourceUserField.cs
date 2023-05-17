using System;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    [XmlType(TypeName = "UserField")]
    public class OutsourceUserField : BaseObject
    {
        public OutsourceUserField()
        {
            UserField = new ObjectRef();
        }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
        [XmlIgnore]
        [CSN("UserField")]
        public ObjectRef UserField { get; set; }
    }
}
