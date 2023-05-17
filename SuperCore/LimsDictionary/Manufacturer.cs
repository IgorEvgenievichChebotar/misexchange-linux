using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class ManufacturerDictionaryItem : DictionaryItem
    {
        public ManufacturerDictionaryItem()
        {
            //
        }
    }

    public class ManufacturerDictionary : DictionaryClass<ManufacturerDictionaryItem>
    {
        public ManufacturerDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Manufacturer")]
        public List<ManufacturerDictionaryItem> Manufacturer
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}