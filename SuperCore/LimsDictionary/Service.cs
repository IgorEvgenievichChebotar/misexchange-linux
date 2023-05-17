using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class ServiceDictionaryItem : DictionaryItem
    {
        public ServiceDictionaryItem()
        {
            Resources = new List<DictionaryItem>();
            Targets = new List<TargetDictionaryItem>();
            ServiceGroups = new List<DictionaryItem>();
            DirectoryGroups = new List<DictionaryItem>();
        }
        [CSN("Resources")]
        public List<DictionaryItem> Resources { get; set; }
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
        [CSN("Duration")]
        public Int32 Duration { get; set; }
        [CSN("WorkingDays")]
        public Int32 WorkingDays { get; set; }
        [CSN("ExportCode")]
        public String ExportCode { get; set; }
        [CSN("ServiceGroups")]
        public List<DictionaryItem> ServiceGroups { get; set; }
        [CSN("UrgentDuration")]
        public Int32 UrgentDuration { get; set; }
        [CSN("EngName")]
        public String EngName { get; set; }
        [CSN("DirectoryGroups")]
        public List<DictionaryItem> DirectoryGroups { get; set; }
    }

    public class ServiceDictionary : DictionaryClass<ServiceDictionaryItem>
    {
        public ServiceDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("ServiceShort")]
        public List<ServiceDictionaryItem> ServiceShort
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}