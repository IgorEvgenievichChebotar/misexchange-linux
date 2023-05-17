using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class TreatmentJournalFilter : BaseJournalFilter
    {
        public TreatmentJournalFilter()
        {
            Statuses = new List<int>();
            OperatingRooms = new List<OperatingRoomDictionaryItem>();
            Diagnoses = new List<DiagnosisDictionaryItem>();
            Transfusiologists = new List<EmployeeDictionaryItem>();
            TreatmentTemplates = new List<TreatmentTemplateDictionaryItem>();
        }

        [CSN("Nr")]
        public String Nr { get; set; }

        [CSN("PatientFirstName")]
        public String PatientFirstName { get; set; }

        [CSN("PatientLastName")]
        public String PatientLastName { get; set; }

        [CSN("PatientMiddleName")]
        public String PatientMiddleName { get; set; }

        [CSN("Patient")]
        public ObjectRef Patient { get; set; }

        [CSN("Statuses")]
        public List<Int32> Statuses { get; set; }

        [CSN("OperatingRooms")]
        public List<OperatingRoomDictionaryItem> OperatingRooms { get; set; }

        [CSN("Diagnoses")]
        public List<DiagnosisDictionaryItem> Diagnoses { get; set; }

        [CSN("TreatmentTemplates")]
        public List<TreatmentTemplateDictionaryItem> TreatmentTemplates { get; set; }

        [CSN("Transfusiologists")]
        public List<EmployeeDictionaryItem> Transfusiologists { get; set; }

        [CSN("CreationDateFrom")]
        public DateTime? CreationDateFrom { get; set; }

        [CSN("CreationDateTill")]
        public DateTime? CreationDateTill { get; set; }

        [CSN("StartDateFrom")]
        public DateTime? StartDateFrom { get; set; }

        [CSN("StartDateTill")]
        public DateTime? StartDateTill { get; set; }

        [CSN("FinishDateFrom")]
        public DateTime? FinishDateFrom { get; set; }

        [CSN("FinishDateTill")]
        public DateTime? FinishDateTill { get; set; }

        [SendToServer(false)]
        [CSN("SkipCreationDate")]
        public Boolean SkipCreationDate { get; set; }

        [SendToServer(false)]
        [CSN("SkipStartDate")]
        public Boolean SkipStartDate { get; set; }

        [SendToServer(false)]
        [CSN("SkipFinishDate")]
        public Boolean SkipFinishDate { get; set; }

        public override void PrepareToSend()
        {
            if (SkipCreationDate)
            {
                CreationDateFrom = CreationDateTill = null;
            }

            if (SkipStartDate)
            {
                StartDateFrom = StartDateTill = null;
            }

            if (SkipFinishDate)
            {
                FinishDateFrom = FinishDateTill = null;
            }

            base.PrepareToSend();
        }
    }
}
