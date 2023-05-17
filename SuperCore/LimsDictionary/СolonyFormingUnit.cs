using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class СolonyFormingUnitDictionaryItem : DictionaryItem {

    }
    public class СolonyFormingUnitDictionary : DictionaryClass<СolonyFormingUnitDictionaryItem>
    {
        public СolonyFormingUnitDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("ColonyFormingUnit")]
        public List<СolonyFormingUnitDictionaryItem> ColonyFormingUnit
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
