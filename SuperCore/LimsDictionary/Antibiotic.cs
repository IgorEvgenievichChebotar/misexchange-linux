using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class AntibioticDictionaryItem : DictionaryItem 
    {
        public AntibioticDictionaryItem()
        {
        }

    }

    public class AntibioticDictionary : DictionaryClass<AntibioticDictionaryItem>
    {
        public AntibioticDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Antibiotic")]
        public List<AntibioticDictionaryItem> Antibiotic
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}