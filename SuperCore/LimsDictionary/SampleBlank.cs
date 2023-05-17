using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    /// Представляет справочник "Бланки ответов по пробам"
    /// </summary>
    public class SampleBlankDictionary : DictionaryClass<SampleBlankDictionaryItem>
    {
        public SampleBlankDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("SampleBlank")]
        public List<SampleBlankDictionaryItem> SampleBlank
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }

    public class SampleBlankDictionaryItem : DictionaryItem
    {
        public SampleBlankDictionaryItem()
        {
            Targets = new List<TargetDictionaryItem>();
            Groups = new List<SampleBlankGroup>();
        }

        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
        [CSN("Groups")]
        public List<SampleBlankGroup> Groups { get; set; }
        [CSN("PrintForm")]
        public ObjectRef PrintForm { get; set; }
    }

    public class SampleBlankGroup : BaseObject
    {
        public SampleBlankGroup()
        {
            Targets = new List<TargetDictionaryItem>();
            Elements = new List<SampleBlankGroupElement>();
        }

        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("NameAsBlank")]
        public Boolean NameAsBlank { get; set; }
        [CSN("NameAsTarget")]
        public Boolean NameAsTarget { get; set; }
        [CSN("BandName")]
        public String BandName { get; set; }
        [CSN("Param1")]
        public String Param1 { get; set; }
        [CSN("Param2")]
        public String Param2 { get; set; }
        [CSN("Param3")]
        public String Param3 { get; set; }
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
        [CSN("Elements")]
        public List<SampleBlankGroupElement> Elements { get; set; }
    }

    public class SampleBlankGroupElement : BaseObject
    {
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
        [CSN("Caption")]
        public String Caption { get; set; }
        [CSN("Required")]
        public Boolean Required { get; set; }
        [CSN("ShowIfAbsent")]
        public Boolean ShowIfAbsent { get; set; }
        [CSN("ShowIfCancelled")]
        public Boolean ShowIfCancelled { get; set; }
        [CSN("Param1")]
        public String Param1 { get; set; }
        [CSN("Param2")]
        public String Param2 { get; set; }
        [CSN("Param3")]
        public String Param3 { get; set; }
        [CSN("TestName")]
        public String TestName { get; set; }
        [CSN("DisplayName")]
        public String DisplayName { get; set; }
        [CSN("PrintName")]
        public String PrintName { get; set; }
    }

}
