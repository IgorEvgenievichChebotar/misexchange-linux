using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.HemBusinessObjects;
using System.Reflection;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.DictionaryCommon;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class WorkJournalFilter : BaseJournalFilter
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

        public WorkJournalFilter()
        {
            Clear();
        }

        public WorkJournalFilter(JournalFilterSettings journalFilterSettings)
        {
            Clear();
            InitDefaults(journalFilterSettings);
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
        [CSN("Sex")]
        public Int32? Sex { get; set; }
        [CSN("States")]
        public List<Int32> States { get; set; }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        [CSN("Priority")]
        public Int32? Priority { get; set; }
        [SendToServer(false)]
        [CSN("Equipments")]
        public AndOrIdList Equipments { get; set; }
        [SendToServer(false)]
        [CSN("Targets")]
        public AndOrIdList Targets { get; set; }
        [CSN("Biomaterials")]
        public List<ObjectRef> Biomaterials { get; set; }
        [SendToServer(false)]
        [CSN("Worklists")]
        public AndOrIdList Worklists { get; set; }
      //  [SendToServer(false)]
      //  [CSN("TestFilter")]
      //  public AndOrIdList TestFilter { get; set; }
        //[CSN("ProfileFilter")]
        //public List<ObjectRef> ProfileFilter { get; set; }
        [SendToServer(false)]
        [CSN("WorkStates")]
        public AndOrFieldList WorkStates { get; set; }
        [CSN("Department")]
        public ObjectRef Department { get; set; }
        [CSN("Tests")]
        public List<ObjectRef> Tests { get; set; }
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        [CSN("CustDepartments")]
        public List<ObjectRef> CustDepartments { get; set; }
        [CSN("Doctors")]
        public List<ObjectRef> Doctors { get; set; }
        [CSN("EmptyPayCategory")]
        public Boolean? EmptyPayCategory { get; set; }
        [CSN("PayCategories")]
        public List<ObjectRef> PayCategories { get; set; }
        [CSN("EndDateFrom")]
        public DateTime? EndDateFrom { get; set; }
        [CSN("EndDateTill")]
        public DateTime? EndDateTill { get; set; }
        [CSN("LastTimestamp")]
        public Int64? LastTimestamp { get; set; }
        [SendToServer(false)]
        [CSN("Normality")]
        public AndOrFieldList Normality { get; set; }
        [CSN("MarkPlanDeviation")]
        public Boolean? MarkPlanDeviation { get; set; }
        [CSN("OnlyDelayed")]
        public Boolean? OnlyDelayed { get; set; }
        [CSN("DefectState")]
        public Int32? DefectState { get; set; }
        [CSN("DepartmentNr")]
        public String DepartmentNr { get; set; }
        [CSN("DepartmentNrExist")]
        public Int32? DepartmentNrExist { get; set; }
        [CSN("WorkPrevValue")]
        public Int32? WorkPrevValue { get; set; }
        [CSN("LookupDepartmentNrs")]
        public String LookupDepartmentNrs { get; set; }
        [CSN("Organizations")]
        public List<ObjectRef> Organizations { get; set; }
        [CSN("Removed")]
        public Int32? Removed { get; set; }
        [CSN("BiomaterialDelivered")]
        public Int32? BiomaterialDelivered { get; set; }
        [CSN("SampleStates")]
        public List<Int32> SampleStates { get; set; }

        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }
        [SendToServer(false)]
        [CSN("EquipmentsList")]
        public List<ObjectRef> EquipmentsList { get; set; }

        [SendToServer(false)]
        [CSN("MarkCito")]
        public Boolean MarkCito { get; set; }

        [CSN("Targets")]
        public AndOrIdList TargetsList { get; set; }
        [SendToServer(false)]
        [CSN("WorklistsList")]
        public List<ObjectRef> WorklistsList { get; set; }

        [CSN("TestFilter")]
        public AndOrIdList TestFilterList { get; set; }
        [SendToServer(false)]
        [CSN("SkipEndDates")]
        public Boolean SkipEndDates { get; set; }
        [SendToServer(false)]
        [CSN("NormalityList")]
        public List<ObjectRef> NormalityList { get; set; }
        [SendToServer(false)]
        [CSN("BirthDateString")]
        public String BirthDateString { get; set; }
        [CSN("SampleDeliveryDateFrom")]
        public DateTime? SampleDeliveryDateFrom { get; set; }
        [CSN("SampleDeliveryDateTill")]
        public DateTime? SampleDeliveryDateTill { get; set; }
        [SendToServer(false)]
        [CSN("SkipSampleDeliveryDate")]
        public Boolean SkipSampleDeliveryDate { get; set; }
        [CSN("ExecutorOrganizations")]
        public List<ObjectRef> ExecutorOrganizations { get; set; }

        public override void PrepareToSend()
        {
            base.PrepareToSend();

            if (SkipDate)
            {
                DateFrom = null;
                DateTill = null;
            }

            if (SkipEndDates)
            {
                EndDateFrom = null;
                EndDateTill = null;
            }

            if (SkipSampleDeliveryDate)
            {
                SampleDeliveryDateFrom = null;
                SampleDeliveryDateTill = null;
            }

            BirthDay = (BirthDay == null) || (BirthDay == 0) ? null : BirthDay;
            BirthMonth = (BirthMonth == null) || (BirthMonth == 0) ? null : BirthMonth;
            BirthYear = (BirthYear == null) || (BirthYear == 0) ? null : BirthYear;
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
            Sex = 3;
            States = new List<Int32>();
            Nr = string.Empty;
            DateFrom = DateTime.Now;
            DateTill = DateTime.Now;
            Priority = null;
            DefectState = LIS_HAS_DEFECT_NONE;
            EmptyPayCategory = null;
            /*EndDateFrom = DateTime.Now;
            EndDateTill = DateTime.Now; */
            MarkPlanDeviation = null;
            OnlyDelayed = null;
            Equipments = new AndOrIdList();
            Targets = new AndOrIdList();
            Biomaterials = new List<ObjectRef>();
            Worklists = new AndOrIdList();
            //TestFilter = new AndOrIdList();
            TestFilterList = new AndOrIdList();
            //ProfileFilter = null;
            WorkStates = new AndOrFieldList();
            Department = new ObjectRef();
            Tests = new List<ObjectRef>();
            Hospitals = new List<ObjectRef>();
            CustDepartments = new List<ObjectRef>();
            Doctors = new List<ObjectRef>();
            PayCategories = new List<ObjectRef>();
            Normality = new AndOrFieldList();
            OnlyDelayed = null;
            DefectState = null;
            DepartmentNr = string.Empty;
            DepartmentNrExist = 2;
            WorkPrevValue = 2;
            BirthDateString = string.Empty;
            LookupDepartmentNrs = string.Empty;
            Organizations = new List<ObjectRef>();
            Removed = 2;
            BiomaterialDelivered = 2;
            SkipEndDates = true;
            SkipSampleDeliveryDate = true;
        }
    }
}
