using ru.novolabs.ExchangeDTOs;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    [XmlType(TypeName = "UserField")]
    public class UserValue
    {
        public UserValue()
        {
            UserField = new ObjectRef();
            Reference = new ObjectRef();
            Value = String.Empty;
            Name = String.Empty;
            Code = String.Empty;
            Values = new List<ObjectRef>();
        }

        [SendToServer(false)]
        [CSN("Name")]
        public String Name { get; set; }

        [SendToServer(false)]
        [CSN("Code")]
        public String Code { get; set; }

        [XmlIgnore]
        [CSN("UserField")]
        [DTOv2(dictionaryName: LimsDictionaryNames.UserField, codeField: "Code", nameField: "Name", canCreate: false)]
        public ObjectRef UserField { get; set; }

        [CSN("Value")]
        [DTOv2(field: "Value")]
        public String Value { get; set; }

        [CSN("Reference")]
        [XmlIgnore]
        public ObjectRef Reference { get; set; } // ссылка на значение пользовательского справочника для перечислимых полей
         
        [XmlIgnore]
        [CSN("Values")]
        public List<ObjectRef> Values { get; set; } // множество ссылок на значения пользовательского справочника для полей типа "множество"
    }

    public class UserValueSet
    {
        public UserValueSet()
        {
            UserValues = new List<UserValue>();
        }

        [Unnamed]
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }
    }  
}
