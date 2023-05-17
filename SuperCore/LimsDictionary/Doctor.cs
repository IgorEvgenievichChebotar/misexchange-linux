using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class DoctorDictionaryItem : DictionaryItem 
    {
        public DoctorDictionaryItem()
        {
        }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("CustDepartment")]
        public CustDepartmentDictionaryItem CustDepartment { get; set; }

        [XmlIgnore]
        [CSN("Employee")]
        [JsonIgnore]
        public EmployeeDictionaryItem Employee { get; set; }

    }

    public class DoctorDictionary : DictionaryClass<DoctorDictionaryItem>
    {
        public DoctorDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Doctor")]
        public List<DoctorDictionaryItem> Doctor
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}