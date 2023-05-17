using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class PatientJournalFilter : BaseJournalFilter
    {
        public PatientJournalFilter()
        {
            Clear();
            InitFields();
            JournalUserFields = new List<UserFieldDictionaryItem>();
        }

        public PatientJournalFilter(JournalFilterSettings journalFilterSettings)
        {
            Clear();
            InitFields();
            InitDefaults(journalFilterSettings);
            JournalUserFields = new List<UserFieldDictionaryItem>();
        }

        private void InitFields()
        {
            FirstName = "";
            LastName = "";
            MiddleName = "";
            Sex = 3;
            Persistent = 2;
        }

        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }

        [CSN("PatientIndex")]
        public String PatientIndex { get; set; }
        [CSN("PatientCardNr")]
        public String PatientCardNr { get; set; }
        [CSN("StrictLastName")]
        public Boolean StrictLastName { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Sex")]
        public Int32 Sex { get; set; }
        [CSN("BirthYear")]
        public String BirthYear { get; set; }
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        [CSN("CustDepartments")]
        public List<ObjectRef> CustDepartments { get; set; }
        [CSN("Doctors")]
        public List<ObjectRef> Doctors { get; set; }
        [CSN("EmptyPayCategory")]
        public Boolean EmptyPayCategory { get; set; }
        [CSN("PayCategories")]
        public List<ObjectRef> PayCategories { get; set; }
        [CSN("RequestDateFrom")]
        public DateTime? RequestDateFrom { get; set; }
        [CSN("RequestDateTill")]
        public DateTime? RequestDateTill { get; set; }

        [SendToServer(false)]
        [CSN("SkipRequestDate")]
        public Boolean SkipRequestDate { get; set; }

        [CSN("Persistent")]
        public Int32 Persistent { get; set; }
        [CSN("UseExternalSystems")]
        public Boolean UseExternalSystems { get; set; }
        [CSN("CalcRequestsCount")]
        public Boolean CalcRequestsCount { get; set; }

        [CSN("JournalUserFields")]
        public List<UserFieldDictionaryItem> JournalUserFields { get; set; }
    }
}
