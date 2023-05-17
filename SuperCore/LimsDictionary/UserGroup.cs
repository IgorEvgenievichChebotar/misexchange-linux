using Newtonsoft.Json;
using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsDictionary
{


    public class UserGroupDictionaryItem : DictionaryItem
    {
        public UserGroupDictionaryItem()
        {
            Rights = new List<AccessRightDictionaryItem>();
            Employees = new List<EmployeeDictionaryItem>();
        }
        [CSN("Rights")]
        public List<AccessRightDictionaryItem> Rights { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        [CSN("Employees")]
        public List<EmployeeDictionaryItem> Employees { get; set; }

    }

    public class UserGroupDictionary : DictionaryClass<UserGroupDictionaryItem>
    {
        public UserGroupDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("UserGroup")]
        public List<UserGroupDictionaryItem> UserGroup
        {
            get { return Elements; }
            set { Elements = value; }
        }


    }


}
