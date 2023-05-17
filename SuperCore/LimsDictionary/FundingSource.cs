using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class FundingSourceDictionaryItem : DictionaryItem
    {
        public FundingSourceDictionaryItem()
        {
            //
        }
        [CSN("ByDefault")]
        public Boolean ByDefault { get; set; }
    }

    public class FundingSourceDictionary : DictionaryClass<FundingSourceDictionaryItem>
    {
        public FundingSourceDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("FundingSource")]
        public List<FundingSourceDictionaryItem> FundingSource
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}