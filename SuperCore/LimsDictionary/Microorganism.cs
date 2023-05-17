using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class MicroOrganismDictionaryItem : DictionaryItem { }

    public class MicroOrganismDictionary : DictionaryClass<MicroOrganismDictionaryItem>
    {
        public MicroOrganismDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("MicroOrganism")]
        public List<MicroOrganismDictionaryItem> MicroOrganism
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
