using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;


namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
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
