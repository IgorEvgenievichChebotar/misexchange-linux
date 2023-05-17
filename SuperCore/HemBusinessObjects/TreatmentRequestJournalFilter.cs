using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class TreatmentRequestJournalFilter : BaseJournalFilter
    {
        public TreatmentRequestJournalFilter()
        {
            Statuses = new List<int>();
            Doctors = new List<EmployeeDictionaryItem>();
            HospitalDepartments = new List<DepartmentDictionaryItem>();
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

        [CSN("RequestedDateFrom")]
        public DateTime? RequestedDateFrom { get; set; }

        [CSN("RequestedDateTill")]
        public DateTime? RequestedDateTill { get; set; }

        [CSN("Statuses")]
        public List<Int32> Statuses { get; set; }

        [CSN("Doctors")]
        public List<EmployeeDictionaryItem> Doctors { get; set; }

        [CSN("HospitalDepartments")]
        public List<DepartmentDictionaryItem> HospitalDepartments { get; set; }

        [CSN("TreatmentTemplates")]
        public List<TreatmentTemplateDictionaryItem> TreatmentTemplates { get; set; }

        [CSN("PlannedDateFrom")]
        public DateTime? PlannedDateFrom { get; set; }

        [CSN("PlannedDateTill")]
        public DateTime? PlannedDateTill { get; set; }

        [SendToServer(false)]
        [CSN("SkipPlannedDate")]
        public Boolean SkipPlannedDate { get; set; }

        [SendToServer(false)]
        [CSN("SkipRequestedDate")]
        public Boolean SkipRequestedDate { get; set; }

        public override void PrepareToSend()
        {
            if (SkipPlannedDate)
            {
                PlannedDateFrom = PlannedDateTill = null;
            }

            if (SkipRequestedDate)
            {
                RequestedDateFrom = RequestedDateTill = null;
            }

            base.PrepareToSend();
        }
    }
}
