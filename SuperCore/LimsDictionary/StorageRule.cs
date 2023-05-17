using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class StorageRuleDictionaryItem : DictionaryItem
    {
    }

    public class StorageRuleDictionary : DictionaryClass<StorageRuleDictionaryItem>
    {
        public StorageRuleDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("StorageRule")]
        public List<StorageRuleDictionaryItem> StorageRule
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
