using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class DonorJournalFilter : BaseJournalFilter
    {
        public DonorJournalFilter()
        {            
            Departments = new List<DepartmentDictionaryItem>();
            DonorCategories = new List<DonorCategoryDictionaryItem>();
            BloodParameters = new List<BloodParameterValue>();
            IsDonor = 2; IsPatient = 2;
            HasPhone = HasMobilePhone = HasWork = HasAddress = Temp = Honored = 2;
            StrictLastName = false;
            IgnoreBoundaryConditions = false;
            Address = new AddressClass();
            Clear();
        }

        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("DonationNr")]
        public String DonationNr { get; set; }
        [CSN("NrEDC")]
        public String NrEDC { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("StrictLastName")]
        public Boolean StrictLastName { get; set; }
        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("Sex")]
        public Int32? Sex { get; set; }
        //  C.AddProp('Address', ptObject);
        [CSN("Address")]
        public AddressClass Address { get; set; }
        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }
        [CSN("Departments")]
        public List<DepartmentDictionaryItem> Departments { get; set; }
        [CSN("DonorCategories")]
        public List<DonorCategoryDictionaryItem> DonorCategories { get; set; }
        [CSN("VisitDate")]
        public DateTime? VisitDate { get; set; }
        [CSN("NextVisitDateFrom")]
        public DateTime? NextVisitDateFrom { get; set; }
        [CSN("NextVisitDateTill")]
        public DateTime? NextVisitDateTill { get; set; }
        [CSN("Temp")]
        public Int32? Temp { get; set; }
        [CSN("BirthYear")]
        public Int32? BirthYear { get; set; }
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }
        [CSN("HasPhone")]
        public Int32? HasPhone { get; set; }
        [CSN("HasMobilePhone")]
        public Int32? HasMobilePhone { get; set; }
        [CSN("HasWork")]
        public Int32? HasWork { get; set; }
        [CSN("HasAddress")]
        public Int32? HasAddress { get; set; }
        [CSN("DonationDateFrom")]
        public DateTime? DonationDateFrom { get; set; }
        [CSN("DonationDateTill")]
        public DateTime? DonationDateTill { get; set; }
        [CSN("DenyTypes")]
        public List<DenyTypeDictionaryItem> DenyTypes { get; set; }
        //[CSN("Denies")]
        //public List<ObjectRef> Denies { get; set; }
        [CSN("DonationCountFrom")]
        public Int32? DonationCountFrom { get; set; }
        [CSN("DonationCountTo")]
        public Int32? DonationCountTo { get; set; }
        [CSN("Honored")]
        public Int32? Honored { get; set; }
        //[CSN("DenyStatuses")]
        //public List<ObjectRef> DenyStatuses { get; set; }
        [CSN("IgnoreBoundaryConditions")]
        public Boolean? IgnoreBoundaryConditions { get; set; }

        [SendToServer(false)]
        [CSN("SkipDonationDate")]
        public Boolean SkipDonationDate { get; set; }

        [CSN("PersonalDonor")]
        public Donor PersonalDonor { get; set; }
        [CSN("Recipient")]
        public Donor Recipient { get; set; }

        [CSN("IsDonor")]
        public Int32 IsDonor { get; set; } // 0=нет, 1=да, 2=не важно
        [CSN("IsPatient")]
        public Int32 IsPatient { get; set; } // 0=нет, 1=да, 2=не важно
        [CSN("ExternalId")]
        public String ExternalId { get; set; } // Внешний ID донора/пациента

        public override void Clear()
        {
            Departments.Clear();
            DonorCategories.Clear();
            BloodParameters.Clear();
        }

        public override void PrepareToSend()
        {
            /*if (SkipArchiveRackCreationDate)
            {
                ArchiveRackCreationDateFrom = ArchiveRackCreationDateTill = null;
            }
            if (SkipRequestCreationDate)
            {
                RequestCreationDateFrom = RequestCreationDateTill = null;
            }*/
            if (SkipDate)
            {
                DateFrom = null; DateTill = null;
            }
            if (SkipDonationDate)
            {
                DonationDateFrom = null;
                DonationDateTill = null;
            }
            if (Sex == 0)
                Sex = 3;
            base.PrepareToSend();
        }

        [CSN("BirthDate")]
        public DateTime? BirthDate { get; set; }
    }
}
