using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class UnitTreeNodeDictionaryItem : DictionaryItem
    {
        public UnitTreeNodeDictionaryItem()
        {
            //
        }
    }

    public class UnitTreeNodeDictionary : DictionaryClass<UnitTreeNodeDictionaryItem>
    {
        public UnitTreeNodeDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("UnitTreeNode")]
        public List<UnitTreeNodeDictionaryItem> UnitTreeNode
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}