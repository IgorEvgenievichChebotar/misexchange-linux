using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class MaterialUnitDictionaryItem : DictionaryItem
    {
        public MaterialUnitDictionaryItem()
        {
            //
        }
    }

    public class MaterialUnitDictionary : DictionaryClass<MaterialUnitDictionaryItem>
    {
        public MaterialUnitDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("MaterialUnit")]
        public List<MaterialUnitDictionaryItem> MaterialUnit
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}