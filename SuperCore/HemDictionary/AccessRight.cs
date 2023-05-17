using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;


namespace ru.novolabs.SuperCore.HemDictionary
{
    public class AccessRightDictionaryItem: DictionaryItem
    {
        public AccessRightDictionaryItem()
        {
        }

        [CSN("Id1")]
        public Int32 Id1 { get; set; }
    }

    public class AccessRightDictionary : DictionaryClass<AccessRightDictionaryItem>
    {
        public AccessRightDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("AccessRight")]
        public List<AccessRightDictionaryItem> AccessRight
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
