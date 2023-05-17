using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class DonationHeader : DonorHeader
    {
        public DonationHeader()
        {
            Department = new DepartmentDictionaryItem();
            Visit = new ObjectRef();
            DonationType = new DonationTypeDictionaryItem();
            ActiveDeny = new DonorDeny();
        }

        [CSN("StartDate")]
        public DateTime? StartDate { get; set; }
        [CSN("EndDate")]
        public DateTime? EndDate { get; set; }
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
        [CSN("Visit")]
        public ObjectRef Visit { get; set; }
        [CSN("DonationType")]
        public DonationTypeDictionaryItem DonationType { get; set; }
        [CSN("RecommendedVolume")]
        public Int32 RecommendedVolume { get; set; }
        [CSN("State")]
        public Int32 State { get; set; }
        [CSN("ActiveDeny")]
        public DonorDeny ActiveDeny { get; set; }
        [CSN("MainLabState")]
        public Int32 MainLabState { get; set; }
        [CSN("MainLabStateDone")]
        public Boolean MainLabStateDone { get; set; }
        [CSN("Volume")]
        public Int32 Volume { get; set; }
    }

    public class Donation : DonationHeader
    {
        public Donation()
        {
            Template = new DonationTemplateDictionaryItem();
            MainLabStage = new VisitStage();
            VisitDepartment = new DepartmentDictionaryItem();
            VisitSenderHospital = new HospitalDictionaryItem();
            VisitDonorCategory = new DonorCategoryDictionaryItem();
            PaymentCategory = new PaymentCategoryDictionaryItem();
            GroupStates = new List<StateGroupItem>();
        }

        [CSN("Comment")]
        public String Comment { get; set; }
        [CSN("Template")]
        public DonationTemplateDictionaryItem Template { get; set; }
        [CSN("MainLabStage")]
        public VisitStage MainLabStage { get; set; }
        [CSN("ProductBloodVolume")]
        public int ProductBloodVolume { get; set; }
        [CSN("MainLabBloodVolume")]
        public int MainLabBloodVolume { get; set; }
        [CSN("ReturnBloodVolume")]
        public int ReturnBloodVolume { get; set; }
        //[CSN("ClientProducts")]
        //public List<ClientOutputProduct> ClientProducts { get; set; }
        //[CSN("Products")]
        //public List<OutputProduct> Products { get; set; }
        [CSN("VisitStartDate")]
        public DateTime? VisitStartDate { get; set; }
        [CSN("VisitDepartment")]
        public DepartmentDictionaryItem VisitDepartment { get; set; }
        [CSN("VisitDonorStatus")]
        public Int32 VisitDonorStatus { get; set; }
        [CSN("VisitReserveType")]
        public Int32 VisitReserveType { get; set; }
        [CSN("VisitSenderHospital")]
        public HospitalDictionaryItem VisitSenderHospital { get; set; }
        [CSN("VisitRelative")]
        public Boolean VisitRelative { get; set; }
        [CSN("VisitDonationRecipient")]
        public String VisitDonationRecipient { get; set; }
        [CSN("VisitDonorCategory")]
        public DonorCategoryDictionaryItem VisitDonorCategory { get; set; }
        [CSN("DonorGroups")]
        public List<ObjectRef> DonorGroups { get; set; }
        [CSN("PaymentCategory")]
        public PaymentCategoryDictionaryItem PaymentCategory { get; set; }


        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates { get; set; }
    }
}