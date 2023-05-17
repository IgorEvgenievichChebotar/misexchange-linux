using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class PatientGroupDictionaryItem : DictionaryItem
    {
        [CSN("EngName")]
        public String EngName { get; set; }
        [CSN("AgeStart")]
        public float AgeStart { get; set; }
        [CSN("AgeEnd")]
        public float AgeEnd { get; set; }
        [CSN("CyclePeriod")]
        public Int32? CyclePeriod { get; set; }
        [CSN("AgeUnit")]
        public Int32 AgeUnit { get; set; }
        [CSN("Sex")]
        public Int32 Sex { get; set; }
        [CSN("PregnancyStart")]
        public Int32? PregnancyStart { get; set; }
        [CSN("PregnancyEnd")]
        public Int32? PregnancyEnd { get; set; }
    }

    public class PatientGroupDictionary : DictionaryClass<PatientGroupDictionaryItem>
    {
        public PatientGroupDictionary() { }
        public PatientGroupDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("PatientGroup")]
        public List<PatientGroupDictionaryItem> PatientGroup
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
