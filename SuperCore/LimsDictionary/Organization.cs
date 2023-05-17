using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class OrganizationDictionaryItem : DictionaryItem
    {
        [CSN("RequestNrTemplate")]
        public String RequestNrTemplate { get; set; }

        [CSN("Targets")]
        [JsonIgnore]
        [XmlIgnore]
        public List<TargetDictionaryItem> Targets { get; set; }

        public OrganizationDictionaryItem()
        {
            CertificateSearchCriterion = String.Empty;
            CertificatePin = String.Empty;
            Targets = new List<TargetDictionaryItem>();
        }

        [CSN("CertificateSearchCriterion")]
        public String CertificateSearchCriterion { get; set; }
        [CSN("CertificatePin")]
        public String CertificatePin { get; set; }
    }

    public class OrganizationDictionary : DictionaryClass<OrganizationDictionaryItem>
    {
        public OrganizationDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Organization")]
        public List<OrganizationDictionaryItem> Organization
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}