using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class DepartmentDictionaryItem : DictionaryItem
    {
        public DepartmentDictionaryItem()
        {
            ExternalCode = String.Empty;
            NrPrefix = String.Empty;
            Classifications = new List<ProductClassificationDictionaryItem>();
        }

        [CSN("NrPrefix")]
        public string NrPrefix { get; set; }

        [CSN("Classifications")]
        public List<ProductClassificationDictionaryItem> Classifications { get; set; }

        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }

        [CSN("AutoGenerateRequestNr")]
        public Boolean AutoGenerateRequestNr { get; set; }
    }
}