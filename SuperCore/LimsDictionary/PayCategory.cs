using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class PayCategoryDictionaryItem : DictionaryItem 
    {
        public PayCategoryDictionaryItem ()
        {
            Values = new List<PayCategoryValue>();
            NonCheckedServices = new List<ObjectRef>();
            Hospitals = new List<HospitalDictionaryItem>();
        }

        [CSN("Values")]
        public List<PayCategoryValue> Values { get; set; }
        [CSN("NonCheckedServices")]
        public List<ObjectRef> NonCheckedServices { get; set; }

        [CSN("Hospitals")]
        [JsonIgnore]
        public List<HospitalDictionaryItem> Hospitals { get; set; }

        public PayCategoryValue GetActualValue()
        {
            DateTime now = DateTime.Now;
            foreach (PayCategoryValue value in Values)
            {
                if (value.StartDate.CompareTo(now) < 0 && (value.EndDate.CompareTo(now) > 0 || value.EndDate.CompareTo(DateTime.MinValue) == 0))
                    return value;
            }
            return null;
        }
    }

    public class PayCategoryValue: BaseObject 
    {
        [CSN("StartDate")]
        public DateTime StartDate { get; set; }
        [CSN("EndDate")]
        public DateTime EndDate { get; set; }
        [CSN("Discount")]
        public float Discount { get; set; }
        [CSN("Pricelist")]
        public ObjectRef Pricelist { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("PayCategory")]
        public PayCategoryDictionaryItem PayCategory { get; set; }
    }

    public class PayCategoryDictionary : DictionaryClass<PayCategoryDictionaryItem>
    {
        public PayCategoryDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("PayCategory")]
        public List<PayCategoryDictionaryItem> PayCategory
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}