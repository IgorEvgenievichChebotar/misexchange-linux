using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.HemDictionary;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class TransfusionTemplateDictionaryItem : DictionaryItem
    {
        public TransfusionTemplateDictionaryItem()
        {
            TransfusionRequestTemplates = new List<TransfusionRequestTemplateDictionaryItem>();
            Hospitals = new List<HospitalDictionaryItem>();
            HospitalDepartments = new List<DepartmentDictionaryItem>();
            ObservationTemplates = new List<ObservationTemplate>();
        }

        [CSN("TransfusionRequestTemplates")]
        public List<TransfusionRequestTemplateDictionaryItem> TransfusionRequestTemplates { get; set; }

        [CSN("EmptyRequestEnable")]
        public Boolean EmptyRequestEnable { get; set; }

        [CSN("Hospitals")]
        public List<HospitalDictionaryItem> Hospitals { get; set; }

        [CSN("HospitalDepartments")]
        public List<DepartmentDictionaryItem> HospitalDepartments { get; set; }

        [CSN("Process")]
        public ProcessTemplateDictionaryItem Process { get; set; }

        [CSN("ObservationTemplates")]
        public List<ObservationTemplate> ObservationTemplates { get; set; }
    }

    public class ObservationTemplate : DictionaryItem
    {
        public ObservationTemplate()
        {
            PhysioIndicators = new List<Parameter>();
        }

        [CSN("Time")]
        public Int32 Time { get; set; }

        [CSN("PhysioIndicators")]
        public List<Parameter> PhysioIndicators { get; set; }


    }
}
