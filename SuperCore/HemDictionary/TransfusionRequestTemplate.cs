using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class TransfusionRequestTemplateDictionaryItem : DictionaryItem
    {
        public TransfusionRequestTemplateDictionaryItem()
        {
            Hospitals = new List<HospitalDictionaryItem>();
            HospitalDepartments = new List<DepartmentDictionaryItem>();
            ProductClassifications = new List<ProductClassificationDictionaryItem>();
            RequiredBloodParams = new List<BloodParameterItem>();
            RequiredAnamnesisParams = new List<Parameter>();
            RequiredPhysioIndicators = new List<Parameter>();
            RequiredLabParams = new List<Parameter>();
            ApprovingDoctors = new List<EmployeeDictionaryItem>();
        }

        [CSN("Hospitals")]
        public List<HospitalDictionaryItem> Hospitals { get; set; }

        [CSN("HospitalDepartments")]
        public List<DepartmentDictionaryItem> HospitalDepartments { get; set; }

        [CSN("ProductClassification")]
        public ProductClassificationDictionaryItem ProductClassification { get; set; }

        [CSN("ProductClassifications")]
        public List<ProductClassificationDictionaryItem> ProductClassifications { get; set; }

        [CSN("RequiredBloodParams")]
        public List<BloodParameterItem> RequiredBloodParams { get; set; }

        [CSN("RequiredAnamnesisParams")]
        public List<Parameter> RequiredAnamnesisParams { get; set; }

        [CSN("RequiredPhysioIndicators")]
        public List<Parameter> RequiredPhysioIndicators { get; set; }

        [CSN("RequiredLabParams")]
        public List<Parameter> RequiredLabParams { get; set; }

        [CSN("RecipientMandatory")]
        public Boolean RecipientMandatory { get; set; }

        [CSN("DepartmentHeadApproval")]
        public Boolean DepartmentHeadApproval { get; set; }

        [CSN("ApprovingDoctors")]
        public List<EmployeeDictionaryItem> ApprovingDoctors { get; set; }

        [CSN("TransferProcessTemplate")]
        public ProcessTemplateDictionaryItem TransferProcessTemplate { get; set; }

        [CSN("ReceptProcessTemplate")]
        public ProcessTemplateDictionaryItem ReceptProcessTemplate { get; set; }
        
        
    }



}
