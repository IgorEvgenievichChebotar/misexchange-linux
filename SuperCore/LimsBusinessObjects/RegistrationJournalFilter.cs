using ru.novolabs.SuperCore.CommonBusinesObjects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class RegistrationJournalFilter : BaseRegistrationJournalFilter
    {
        public RegistrationJournalFilter()
        {
            Clear();
        }

        public RegistrationJournalFilter(JournalFilterSettings journalFilterSettings) : base(journalFilterSettings) { }

        public override void PrepareToSend()
        {
            base.PrepareToSend();

            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                if ((propInfo.PropertyType.Equals(typeof(DateTime?))) || (propInfo.PropertyType.Equals(typeof(DateTime))))
                {
                    if (propInfo.GetValue(this, null) != null)
                    {
                        if (propInfo.Name.EndsWith("From"))
                        {
                            if (null != propInfo.GetValue(this, null))
                                propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).StartOfTheDay(), null);
                        }

                        if (propInfo.Name.EndsWith("Till"))
                        {
                            if (null != propInfo.GetValue(this, null))
                                propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).EndOfTheDay(), null);
                        }
                    }
                }
            }

            if (SkipLastModificationDate)
            {
                LastModificationDateFrom = null;
                LastModificationDateTill = null;
            }

            if (SkipRegistrationDate)
            {
                RegistrationDateFrom = null;
                RegistrationDateTill = null;
            }

            if (SkipSampleDeliveryDate)
            {
                SampleDeliveryDateFrom = null;
                SampleDeliveryDateTill = null;
            }

            if (SkipPayDate)
            {
                PayDateFrom = null;
                PayDateTill = null;
            }
        }

        public override void Clear()
        {
            base.Clear();

            States = new List<ObjectRef>();
            SampleStates = new List<ObjectRef>();
            NormsStates = new List<ObjectRef>();
            CustomStates = new List<ObjectRef>();
            RequestForms = new List<ObjectRef>();
            PayCategories = new List<ObjectRef>();
            CustDepartments = new List<ObjectRef>();
            Doctors = new List<ObjectRef>();
            Hospitals = new List<ObjectRef>();
            Targets = new AndOrIdList();
            TargetsList = new AndOrIdList();
            Departments = new AndOrIdList();
            InternalNrs = new List<String>();
            PatientCodes = new List<String>();
            BiomaterialDelivered = ObjectDeliveredState.Any;
            Removed = 1;

            SkipLastModificationDate = true;
            SkipRegistrationDate = false;
            SkipDate = false;
            SkipSampleDeliveryDate = false;
            SkipEndDate = true;

            LastModificationDateFrom = DateTime.Now;
            LastModificationDateTill = DateTime.Now;
            RegistrationDateFrom = DateTime.Now;
            RegistrationDateTill = DateTime.Now;
            SampleDeliveryDateFrom = DateTime.Now;
            SampleDeliveryDateTill = DateTime.Now;
            Organizations = new List<ObjectRef>();
            TargetGroups = new List<ObjectRef>();
            PaymentStates = new List<int>();
            SkipPayDate = true;
        }
        [CSN("States")]
        public List<ObjectRef> States { get; set; }
        [CSN("SampleStates")]
        public List<ObjectRef> SampleStates { get; set; }
        [CSN("NormsStates")]
        public List<ObjectRef> NormsStates { get; set; }
        [CSN("CustomStates")]
        public List<ObjectRef> CustomStates { get; set; }
        [CSN("RequestForms")]
        public List<ObjectRef> RequestForms { get; set; }
        [CSN("PayCategories")]
        public List<ObjectRef> PayCategories { get; set; }
        [CSN("CustDepartments")]
        public List<ObjectRef> CustDepartments { get; set; }
        [CSN("Doctors")]
        public List<ObjectRef> Doctors { get; set; }
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        [CSN("Targets")]
        public AndOrIdList Targets { get; set; }
        [CSN("Targets")]
        public AndOrIdList TargetsList { get; set; }
        [CSN("Departments")]
        public AndOrIdList Departments { get; set; }
        [CSN("InternalNrs")]
        public List<String> InternalNrs { get; set; }
        [CSN("PatientCodes")]
        public List<String> PatientCodes { get; set; }
        [CSN("BiomaterialDelivered")]
        public Int32? BiomaterialDelivered { get; set; }
        [CSN("LastTimestamp")]
        public Int64? LastTimestamp { get; set; }
        [CSN("LastModificationDateFrom")]
        public DateTime? LastModificationDateFrom { get; set; }
        [CSN("LastModificationDateTill")]
        public DateTime? LastModificationDateTill { get; set; }
        [CSN("RegistrationDateFrom")]
        public DateTime? RegistrationDateFrom { get; set; }
        [CSN("RegistrationDateTill")]
        public DateTime? RegistrationDateTill { get; set; }
        [CSN("Organizations")]
        public List<ObjectRef> Organizations { get; set; }
        [CSN("Removed")]
        public int? Removed { get; set; }
        [CSN("Patient")]
        public ObjectRef Patient { get; set; }

        [SendToServer(false)]
        [CSN("SkipLastModificationDate")]
        public Boolean SkipLastModificationDate { get; set; }
        [SendToServer(false)]
        [CSN("SkipRegistrationDate")]
        public Boolean SkipRegistrationDate { get; set; }
        [SendToServer(false)]
        [CSN("SkipSampleDeliveryDate")]
        public Boolean SkipSampleDeliveryDate { get; set; }
        [SendToServer(false)]
        [CSN("MarkCito")]
        public Boolean MarkCito { get; set; }
        [CSN("TargetGroups")]
        public List<ObjectRef> TargetGroups { get; set; }
        [CSN("CheckNumber")]
        public string CheckNumber { get; set; }
        [CSN("PayDateFrom")]
        public DateTime? PayDateFrom { get; set; }
        [CSN("PayDateTill")]
        public DateTime? PayDateTill { get; set; }
        [CSN("PaymentStates")]
        public List<int> PaymentStates { get; set; }
        [SendToServer(false)]
        [CSN("SkipPayDate")]
        public Boolean SkipPayDate { get; set; }
    }

    public class BaseRegistrationJournalFilter : BaseJournalFilter
    {
        public const Int32 LIS_DELIVERED_FALSE = 0;
        public const Int32 LIS_DELIVERED_TRUE = 1;
        public const Int32 LIS_DELIVERED_ANY = 2;

        public const Int32 LIS_PATIENT_PRESENT_NO = 0;
        public const Int32 LIS_PATIENT_PRESENT_YES = 1;
        public const Int32 LIS_PATIENT_PRESENT_ALL = 2;

        public const Int32 LIS_HAS_DEFECT_NONE = 0;
        public const Int32 LIS_HAS_DEFECT_TRUE = 1;
        public const Int32 LIS_HAS_DEFECT_FALSE = 2;


        public BaseRegistrationJournalFilter()
        {
            Clear();
        }

        public BaseRegistrationJournalFilter(JournalFilterSettings journalFilterSettings)
        {
            Clear();
            InitDefaults(journalFilterSettings);
        }

        public override void PrepareToSend()
        {
            base.PrepareToSend();


            if (SkipEndDate)
            {
                EndDateFrom = null;
                EndDateTill = null;
            }
        }

        public override void Clear()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            MiddleName = string.Empty;
            PatientNr = string.Empty;
            PatientCardNr = string.Empty;
            BirthDay = null;
            BirthMonth = null;
            BirthYear = null;
            Sex = null;
            PatientPresent = LIS_PATIENT_PRESENT_ALL;
            Nr = string.Empty;
            SampleDeliveryDateFrom = null;
            SampleDeliveryDateTill = null;
            Priority = null;
            Delivered = LIS_DELIVERED_ANY;
            DefectState = LIS_HAS_DEFECT_NONE;
            Printed = null;
            OriginalSent = null;
            CopySent = null;
            EmptyPayCategory = null;
            /*EndDateFrom = DateTime.Now;
            EndDateTill = DateTime.Now; */
            BillNr = string.Empty;
            Billed = null;
            MarkPlanDeviation = null;
            OnlyDelayed = null;

        }

        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("PatientNr")]
        public String PatientNr { get; set; }
        [CSN("PatientCardNr")]
        public String PatientCardNr { get; set; }
        [CSN("BirthDay")]
        public Int32? BirthDay { get; set; }
        [CSN("BirthMonth")]
        public Int32? BirthMonth { get; set; }
        [CSN("BirthYear")]
        public Int32? BirthYear { get; set; }
        [SendToServer(false)]
        [CSN("BirthDate")]
        public DateTime? BirthDate
        {
            get 
            {
                if (BirthYear != null && BirthMonth != null && BirthDay != null)
                    return new DateTime((Int32)BirthYear, (Int32)BirthMonth, (Int32)BirthDay);
                else
                    return null;
            }
            set {
                if (value != null)
                {
                    BirthDay = value.Value.Day;
                    BirthMonth = value.Value.Month;
                    BirthYear = value.Value.Year;
                }
            }
        }
        [CSN("Sex")]
        public Int32? Sex { get; set; }
        [CSN("PatientPresent")]
        public Int32? PatientPresent { get; set; }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("SampleDeliveryDateFrom")]
        public DateTime? SampleDeliveryDateFrom { get; set; }
        [CSN("SampleDeliveryDateTill")]
        public DateTime? SampleDeliveryDateTill { get; set; }
        [CSN("Priority")]
        public Int32? Priority { get; set; }
        [CSN("Delivered")]
        public Int32? Delivered { get; set; }
        [CSN("DefectState")]
        public Int32? DefectState { get; set; }
        [CSN("Printed")]
        public Int32? Printed { get; set; }
        [CSN("OriginalSent")]
        public Int32? OriginalSent { get; set; }
        [CSN("CopySent")]
        public Int32? CopySent { get; set; }
        [CSN("EmptyPayCategory")]
        public Boolean? EmptyPayCategory { get; set; }
        [CSN("EndDateFrom")]
        public DateTime? EndDateFrom { get; set; }
        [CSN("EndDateTill")]
        public DateTime? EndDateTill { get; set; }
        [CSN("BillNr")]
        public String BillNr { get; set; }
        [CSN("Billed")]
        public Int32? Billed { get; set; }
        [CSN("MarkPlanDeviation")]
        public Boolean? MarkPlanDeviation { get; set; }
        [CSN("OnlyDelayed")]
        public Boolean? OnlyDelayed { get; set; }

        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }
        [SendToServer(false)]
        [CSN("SkipEndDate")]
        public Boolean SkipEndDate { get; set; }

        [CSN("ExecutorOrganizations")]
        public List<ObjectRef> ExecutorOrganizations { get; set; }
        
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }
    }
}
