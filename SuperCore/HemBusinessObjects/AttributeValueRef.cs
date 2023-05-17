using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class AttributeValueRef: BaseObject
    {
        [CSN("Attribute")]
        public ObjectRef Attribute { get; set; }
        [CSN("Value")]
        public ObjectRef Value { get; set; }
        
    }
}
