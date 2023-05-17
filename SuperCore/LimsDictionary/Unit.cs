using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class UnitDictionaryItem : DictionaryItem
    {
        [CSN("EngName")]
        public String EngName { get; set; }
    }

    public class UnitDictionary : DictionaryClass<UnitDictionaryItem>
    {
        public UnitDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("Unit")]
        public List<UnitDictionaryItem> Unit
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
