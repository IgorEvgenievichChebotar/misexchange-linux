using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class TreatmentRequestTemplateDictionaryItem : DictionaryItem
    {
        public TreatmentRequestTemplateDictionaryItem()
        {
            TreatmentTemplates = new List<TreatmentTemplateDictionaryItem>();
            RequiredBloodParameters = new List<BloodParameterItem>();
            RequiredLabParams = new List<Parameter>();
            RequiredPhysioIndicators = new List<Parameter>();
        }

        [CSN("TreatmentTemplates")]
        public List<TreatmentTemplateDictionaryItem> TreatmentTemplates { get; set; }

        [CSN("RequiredBloodParameters")]
        public List<BloodParameterItem> RequiredBloodParameters { get; set; }

        [CSN("RequiredLabParams")]
        public List<Parameter> RequiredLabParams { get; set; }

        [CSN("RequiredPhysioIndicators")]
        public List<Parameter> RequiredPhysioIndicators { get; set; }

    }
}
