using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class ExternalSystemTestMapping : BaseObject
    {
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Test")]
		public TestDictionaryItem Test { get; set; }
        [CSN("TargetCode")]
		public String TargetCode { get; set; }
    }

    [OldSaveMethod]
    public class ExternalSystemDictionaryItem : DictionaryItem
    {
        public ExternalSystemDictionaryItem()
        {
            Tests = new List<ExternalSystemTestMapping>();
        }

        [CSN("ExportAllWorks")]
        public Boolean ExportAllWorks { get; set; }
        [CSN("Tests")]
        public List<ExternalSystemTestMapping> Tests { get; set; }
    }

    public class ExternalSystemDictionary : DictionaryClass<ExternalSystemDictionaryItem>
    {
        public ExternalSystemDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("ExternalSystem")]
        public List<ExternalSystemDictionaryItem> ExternalSystem
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}