using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ApparatusData
    {
        public ApparatusData()
        {
            ParamValues = new List<ParamValue>();
        }

        [CSN("Apparatus")]
        [SendAsRef(true)]
        public ApparatusDictionaryItem Apparatus { get; set; }
        [CSN("ParamValues")]
        public List<ParamValue> ParamValues { get; set; }
    }

    public class ParamValue : Object
    {
        /// <summary>
        /// Ключ параметра
        /// </summary>
        [CSN("Key")]
        public String Key { get; set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        [CSN("Value")]
        public String Value { get; set; }         
    } 
}
