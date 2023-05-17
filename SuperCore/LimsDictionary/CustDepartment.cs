using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class CustDepartmentDictionaryItem : DictionaryItem 
    {
        public CustDepartmentDictionaryItem()
        {
            Doctors = new List<DoctorDictionaryItem>();
        }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        [CSN("Doctors")]
        public List<DoctorDictionaryItem> Doctors { get; set; }
    }

    public class CustDepartmentDictionary : DictionaryClass<CustDepartmentDictionaryItem>
    {
        public CustDepartmentDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("CustDepartment")]
        public List<CustDepartmentDictionaryItem> CustDepartment
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}