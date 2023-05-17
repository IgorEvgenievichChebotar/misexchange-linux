using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class ApparatusDictionaryItem : DriverDictionaryItem
    {
        public ApparatusDictionaryItem()
        {
            OutgoingParameters = new List<ApparatusOutgoingParameter>();
        }

        [CSN("OutgoingParameters")]
        public List<ApparatusOutgoingParameter> OutgoingParameters { get; set; }
    }

    public class ApparatusOutgoingParameter : Object
    {
        [CSN("ParameterName")]
        public string ParameterName { get; set; }
    }    
}
