using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class LegalEntityDictionaryItem : DictionaryItem
    {
        public LegalEntityDictionaryItem()
        {
            OrgStateCode = String.Empty;
            OrgStateName = String.Empty;
            Website = String.Empty;
            LicenseNumber = String.Empty;
            FactAddress = String.Empty;
        }

        [CSN("OrgStateCode")]
        public String OrgStateCode { get; set; }
        [CSN("OrgStateName")]
        public String OrgStateName { get; set; }
        [CSN("Website")]
        public String Website { get; set; }
        [CSN("LicenseNumber")]
        public String LicenseNumber { get; set; }
        [CSN("FactAddress")]
        public String FactAddress { get; set; }
    }

    public class LegalEntityDictionary : DictionaryClass<LegalEntityDictionaryItem>
    {
        public LegalEntityDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("LegalEntity")]
        public List<LegalEntityDictionaryItem> LegalEntity
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}