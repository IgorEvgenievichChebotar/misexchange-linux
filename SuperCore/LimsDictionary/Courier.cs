using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class CourierDictionaryItem : DictionaryItem
    {
        public CourierDictionaryItem()
        {
            Hospitals = new List<HospitalDictionaryItem>();
        }

        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("Phone")]
        public String Phone { get; set; }
        [CSN("Hospitals")]
        public List<HospitalDictionaryItem> Hospitals { get; set; }

        public override string Name
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(LastName))
                {
                    result += LastName;
                    if (!String.IsNullOrEmpty(FirstName))
                    {
                        result += " " + FirstName.Substring(0, 1) + ".";
                        if (!String.IsNullOrEmpty(MiddleName))
                            result += " " + MiddleName.Substring(0, 1) + ".";
                    }
                }
                return result;
            }
        }
    }

    public class CourierDictionary : DictionaryClass<CourierDictionaryItem>
    {
        public CourierDictionary(String DictionaryName) : base(DictionaryName) { }
        [CSN("Courier")]
        public List<CourierDictionaryItem> Courier
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
