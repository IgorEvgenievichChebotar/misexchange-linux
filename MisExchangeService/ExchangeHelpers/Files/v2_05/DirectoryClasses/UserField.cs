using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class UserField
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public UserField()
        {

        }
        public UserField(UserFieldDictionaryItem userField)
        {
            Name = userField.Name;
            Code = userField.Code;
            Mnemonics = userField.Mnemonics;
            Removed = userField.Removed;
        }
    }

    public class UserFieldsDictionary
    {
        public List<UserField> UserFields = new List<UserField>();
    }
}