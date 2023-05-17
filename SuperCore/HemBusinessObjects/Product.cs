using System;
using System.Collections.Generic;
using System.Linq;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ProductShortData : BaseObject
    {
        public ProductShortData()
        {
            NrPrefix = String.Empty;
            NrValue = String.Empty;
            NrSuffix = String.Empty;
            ProductTemplate = new ObjectRef();
            ProductType = new ProductTypeDictionaryItem();
            ContainerType = new ContainerTypeDictionaryItem();
            ContainerManufacturer = new ManufacturerDictionaryItem();
            ContainerNr = String.Empty;
            State = new ProductSateDictionaryItem();
            StoreDepartment = new DepartmentDictionaryItem();
            Storage = new StorageDictionaryItem();
            DonationDate = DateTime.MinValue;
            ExpireDate = DateTime.MinValue;
            QuarantineStartDate = DateTime.MinValue;
            QuarantineFinishDate = DateTime.MinValue;
            FirstAnalysisDate = DateTime.MinValue;
            LastAnalysisDate = DateTime.MinValue;
            Comment = String.Empty;
            ActiveProduction = new ObjectRef();
            ActiveProcessTemplate = new ProcessTemplateDictionaryItem();
            QuarantineTemplate = new QuarantineTemplateDictionaryItem();
            ActiveDefect = new ProductDefect();
            ProductionInputTemplate = new ProductionTemplateDictionaryItem();
            BloodParameters = new List<BloodParameterValue>();
            ProductPrice = new ObjectRef();
            Tariff = new ProductTariffDictionaryItem();
            Recipient = new ObjectRef();
            RecipientFullName = String.Empty;
            AltNr = String.Empty;
            Donor = new ObjectRef();
            DonorFullName = String.Empty;
            DonorShortName = String.Empty;
            RecipientName = String.Empty;
            Attributes = new List<AttributeValueRef>();
        }

        [CSN("NrPrefix")]
        public String NrPrefix { get; set; }
        [CSN("NrValue")]
        public String NrValue { get; set; }
        [CSN("NrSuffix")]
        public String NrSuffix { get; set; }
        [CSN("Nr")]
        public String Nr
        {
            get
            {
                if (!String.IsNullOrEmpty(NrValue))
                    return String.Concat(NrPrefix, NrValue, NrSuffix);
                else
                    return String.Empty;
            }
        }
        [CSN("AltNr")]
        public String AltNr { get; set; }
        [CSN("ProductType")]
        public ProductTypeDictionaryItem ProductType { get; set; }
        [CSN("Volume")]
        public Int32 Volume { get; set; }
        [CSN("DoseCount")]
        public Int32 DoseCount { get; set; }
        [CSN("CellCount")]
        public Int32 CellCount { get; set; }
        [CSN("CellsInDose")]
        public Int32 CellsInDose { get; set; }
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
        [CSN("DonationDate")]
        public DateTime? DonationDate { get; set; }
        [CSN("ContainerType")]
        public ContainerTypeDictionaryItem ContainerType { get; set; }
        [CSN("ContainerVolume")]
        public Int32 ContainerVolume { get; set; }
        [CSN("AdditiveType")]
        public AdditiveTypeDictionaryItem AdditiveType { get; set; }
        [CSN("AdditiveVolume")]
        public Int32 AdditiveVolume { get; set; }
        [CSN("ContainerManufacturer")]
        public ManufacturerDictionaryItem ContainerManufacturer { get; set; }
        [CSN("ContainerNr")]
        public String ContainerNr { get; set; }
        [CSN("Storage")]
        public StorageDictionaryItem Storage { get; set; }
        [CSN("StoreDepartment")]
        public DepartmentDictionaryItem StoreDepartment { get; set; }
        [CSN("Relative")]
        public Boolean Relative { get; set; }
        [CSN("RecipientName")]
        public String RecipientName { get; set; }
        [CSN("RecipientFullName")]
        public String RecipientFullName { get; set; }
        [CSN("RecipientBloodGroup")]
        public Int32 RecipientBloodGroup { get; set; }
        [CSN("RecipientRh")]
        public Int32 RecipientRh { get; set; }
        [CSN("State")]
        public ProductSateDictionaryItem State { get; set; }
        [CSN("Tariff")]
        public ProductTariffDictionaryItem Tariff { get; set; }
        [CSN("ActiveProduction")]
        public ObjectRef ActiveProduction { get; set; }

        // Нужно как-то переделать два свойства ActiveProcessRef и ActiveProcess.
        // ActiveProcess присылается сервером как reference, однако на форму будет выводиться вычисляемое строковое свойство (в Delphi-клиенте это 2 свойства: ActiveProcessId и ActiveProcess)
        //[CSN("ActiveProcess")]
        //public ProcessTemplateDictionaryItem ActiveProcessRef { get; set;}
        //[CSN("ActiveProcess")]
        //public String ActiveProcess
        //{
        //    get
        //    {
        //          if ((ActiveProduction != null) && (ActiveProduction.Id > 0))
        //              return "Производство";
        //        else if ((ActiveProcessRef != null) && (ActiveProcessRef.Id > 0))
        //              return ActiveProcessRef.Name;
        //          else
        //              return String.Empty;
        //    }
        //}
        [CSN("TargetHospital")]
        public HospitalDictionaryItem TargetHospital { get; set; }
        [CSN("FirstAnalysisDate")]
        public DateTime? FirstAnalysisDate { get; set; }
        [CSN("LastAnalysisDate")]
        public DateTime? LastAnalysisDate { get; set; }
        [CSN("QuarantineStartDate")]
        public DateTime? QuarantineStartDate { get; set; }
        [CSN("QuarantineFinishDate")]
        public DateTime? QuarantineFinishDate { get; set; }
        [CSN("ExpireDate")]
        public DateTime? ExpireDate { get; set; }
        [CSN("Classification")]
        public ProductClassificationDictionaryItem Classification { get; set; }
        [CSN("Recipient")]
        public ObjectRef Recipient { get; set; }        
        /// <summary>
        /// Шаблон продукта не является самостоятельным справочником (по крайней мере его нет в Delphi-клиенте).
        /// Вместо этого конкретные шаблоны продуктов описываются в справочниках "Шаблоны донаций" и "Шаблоны производства".
        /// В этих справочниках имеются вложенные списки шаблонов выходных продуктов
        /// </summary>
        [CSN("ProductTemplate")]
        public ObjectRef ProductTemplate { get; set; }
        [CSN("ActiveProcessTemplate")]
        public ProcessTemplateDictionaryItem ActiveProcessTemplate { get; set; }
        [CSN("QuarantineTemplate")]
        public QuarantineTemplateDictionaryItem QuarantineTemplate { get; set; }
        [CSN("Removed")]
        public Boolean Removed { get; set; }
        [CSN("Active")]
        public Boolean Active { get; set; }
        [CSN("ParentVolume")]
        public Int32 ParentVolume { get; set; }
        [CSN("Donor")]
        public ObjectRef Donor { get; set; }
        [CSN("DonorShortName")]
        public String DonorShortName { get; set; }
        [CSN("DonorFullName")]
        public String DonorFullName { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }
        [CSN("ProductionInputTemplate")]
        public ProductionTemplateDictionaryItem ProductionInputTemplate { get; set; }
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }
        [CSN("DoseMode")]
        public Int32 DoseMode { get; set; }
        [CSN("ProductPrice")]
        public ObjectRef ProductPrice { get; set; }
        [CSN("ActiveDefect")]
        public ProductDefect ActiveDefect { get; set; }
        [CSN("Attributes")]
        public List<AttributeValueRef> Attributes { get; set; }
        

        //============================== Дополнительные свойства ==========================        
        //
        // Вычисляемые в Delphi-клиенте
        //
        //[CSN("Sum")]
        //public Double Sum { get; set; }
        //[CSN("Price")]
        //public Double Price { get; set; }
        //[CSN("UnitType")]
        //public ObjectRef UnitType { get; set; }
        //[CSN("Amount")]
        //public String Amount { get; set; }
        //[CSN("FormattedCellCount")]
        //public String FormattedCellCount { get; set; }
        //[CSN("FormattedCellsInDose")]
        //public String FormattedCellsInDose { get; set; }

        [CSN("TargetHospitalExternalCode")]
        public String TargetHospitalExternalCode { get { return (this.TargetHospital != null) ? this.TargetHospital.ExternalCode : null; } }
        [CSN("DepartmentExternalCode")]
        public String DepartmentExternalCode { get { return (this.Department != null) ? this.Department.ExternalCode : null; } }
        [CSN("ClassificationExternalCode")]
        public String ClassificationExternalCode { get { return (this.Classification != null) ? this.Classification.ExternalCode : null; } }
        [CSN("ActiveDefectExternalCode")]
        public String ActiveDefectExternalCode { get { return (this.ActiveDefect.Defect != null) ? this.ActiveDefect.Defect.ExternalCode : null; } }

        [CSN("HospitalDepartment")]
        public DepartmentDictionaryItem HospitalDepartment { get; set; }

        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }
    }

    public class Product : ProductShortData
    {
        private ProcessInfo lastProcessInfo;

        public Product()
        {
            Donors = new List<Donor>();
            Donations = new List<Donation>();
            Parents = new List<Product>();
            Children = new List<Product>();
            ProcessesInfo = new List<ProcessInfo>();
            GroupStates = new List<StateGroupItem>();
        }

        [CSN("Donors")]
        public List<Donor> Donors { get; set; }
        [CSN("Donations")]
        public List<Donation> Donations { get; set; }
        [CSN("Parents")]
        public List<Product> Parents { get; set; }
        [CSN("Children")]
        public List<Product> Children { get; set; }
        [CSN("ProcessesInfo")]
        public List<ProcessInfo> ProcessesInfo { get; set; }
        [CSN("LastProcessInfo")]
        public ProcessInfo LastProcessInfo
        {
            get
            {
                if (lastProcessInfo != null)
                    return lastProcessInfo;

                var pInfos =
                    from pi in ProcessesInfo
                    where pi.EndDate == ProcessesInfo.Max(x => x.EndDate)
                    select pi;

                var procInfos = pInfos.ToArray<ProcessInfo>();

                lastProcessInfo = (procInfos.Length == 1) ? procInfos[0] : null;
                return lastProcessInfo;
            }
        }


        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates { get; set; }
    }
}