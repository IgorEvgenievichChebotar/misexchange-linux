using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class AttributeDictionaryItem : DictionaryItem
    {
        public AttributeDictionaryItem()
        {
            Values = new List<AttributeValue>();
        }
        [CSN("Values")]
        public List<AttributeValue> Values { get; set; }
    }

    public class AttributeValue : RankedNamedObject
    {
        [CSN("System")]
        public Boolean System { get; set; }
        [CSN("Strict")]
        public Boolean Strict { get; set; }
       
    }

}
