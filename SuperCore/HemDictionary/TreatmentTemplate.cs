using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class TreatmentTemplateDictionaryItem : DictionaryItem
    {
        public TreatmentTemplateDictionaryItem()
        {
            Equipments = new List<ApparatusDictionaryItem>();
            PhysioIndicatorsObservationTemplates = new List<GenericObservationTemplate>();
            LabParamsObservationTemplates = new List<GenericObservationTemplate>();
            TechParamsObservationTemplates = new List<GenericObservationTemplate>();
            LiquidBalanceParamsObservationTemplates = new List<GenericObservationTemplate>();
            TotalVolumeObservationTemplates = new List<GenericObservationTemplate>();

        }

        [CSN("Equipments")]
        public List<ApparatusDictionaryItem> Equipments { get; set; }

        [CSN("DonationTemplate")]
        public DonationTemplateDictionaryItem DonationTemplate { get; set; }

        [CSN("TransfusionTemplate")]
        public TransfusionTemplateDictionaryItem TransfusionTemplate { get; set; }

        [CSN("TransfusionRequestTemplate")]
        public TransfusionRequestTemplateDictionaryItem TransfusionRequestTemplate { get; set; }

        [CSN("Process")]
        public ProcessTemplateDictionaryItem Process { get; set; }

        [CSN("PhysioIndicatorsObservationTemplates")]
        public List<GenericObservationTemplate> PhysioIndicatorsObservationTemplates { get; set; }


        [CSN("LabParamsObservationTemplates")]
        public List<GenericObservationTemplate> LabParamsObservationTemplates { get; set; }


        [CSN("TechParamsObservationTemplates")]
        public List<GenericObservationTemplate> TechParamsObservationTemplates { get; set; }


        [CSN("LiquidBalanceParamsObservationTemplates")]
        public List<GenericObservationTemplate> LiquidBalanceParamsObservationTemplates { get; set; }


        [CSN("TotalVolumeObservationTemplates")]
        public List<GenericObservationTemplate> TotalVolumeObservationTemplates { get; set; }
    }

    public class GenericObservationTemplate : BaseObject
    {
        public GenericObservationTemplate()
        {
            Indicators = new List<Parameter>();
            RequiredIndicators = new List<Parameter>();
        }

        [CSN("Time")]
        public Int32 Time { get; set; }

        [SendToServer(false)]
        [CSN("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Множество ссылок на параметры из справочника parameterGroup
        /// </summary>
        [CSN("Indicators")]
        public List<Parameter> Indicators { get; set; }

        /// <summary>
        /// Множество ссылок на обязательные для заполнения в данном наблюдении параметры из справочника parameterGroup
        /// </summary>
        [CSN("RequiredIndicators")]
        public List<Parameter> RequiredIndicators { get; set; }
    }
    
}
