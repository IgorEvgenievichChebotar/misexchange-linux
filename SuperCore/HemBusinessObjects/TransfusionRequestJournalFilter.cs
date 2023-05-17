using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class TransfusionRequestJournalFilter : BaseJournalFilter
    {
        public TransfusionRequestJournalFilter()
        {
            States = new List<ObjectRef>();
            Doctors = new List<ObjectRef>();
            ApprovingDoctors = new List<ObjectRef>();
            Transfusiologists = new List<ObjectRef>();
            CancelDoctors = new List<ObjectRef>();
            HospitalDepartments = new List<ObjectRef>();
            RequestTypes = new List<ObjectRef>();
        }

        [CSN("Nr")]
        public String Nr { get; set; }

        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }

        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }

        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }

        [CSN("States")]
        public List<ObjectRef> States { get; set; }

        [CSN("Doctors")]
        public List<ObjectRef> Doctors { get; set; }

        [CSN("PlannedDateFrom")]
        public DateTime? PlannedDateFrom { get; set; }

        [CSN("PlannedDateTill")]
        public DateTime? PlannedDateTill { get; set; }

        [SendToServer(false)]
        [CSN("SkipPlannedDate")]
        public Boolean SkipPlannedDate { get; set; }

        [CSN("RecipientFirstName")]
        public String RecipientFirstName { get; set; }
        [CSN("RecipientLastName")]
        public String RecipientLastName { get; set; }
        [CSN("RecipientMiddleName")]
        public String RecipientMiddleName { get; set; }


        [CSN("Comment")]
        public String Comment { get; set; }

        [CSN("ApprovingDoctors")]
        public List<ObjectRef> ApprovingDoctors { get; set; }

        [CSN("ApprovedDateFrom")]
        public DateTime? ApprovedDateFrom { get; set; }
        [CSN("ApprovedDateTill")]
        public DateTime? ApprovedDateTill { get; set; }

        [CSN("ApprovedVolume")]
        public Int32 ApprovedVolume { get; set; }

        [CSN("Transfusiologists")]
        public List<ObjectRef> Transfusiologists { get; set; }

        [CSN("TransfusiologistDateFrom")]
        public DateTime? TransfusiologistDateFrom { get; set; }

        [CSN("TransfusiologistDateTill")]
        public DateTime? TransfusiologistDateTill { get; set; }

        [CSN("CancelDoctors")]
        public List<ObjectRef> CancelDoctors { get; set; }

        [CSN("CancelDateFrom")]
        public DateTime? CancelDateFrom { get; set; }

        [CSN("CancelDateTill")]
        public DateTime? CancelDateTill { get; set; }

        [CSN("HospitalDepartments")]
        public List<ObjectRef> HospitalDepartments { get; set; }

        [CSN("RequestTypes")]
        public List<ObjectRef> RequestTypes { get; set; }

        public override void PrepareToSend()
        {
            //if (ApprovingDoctors.Count == 0)
            //    ApprovingDoctors = null;
            //if (Doctors.Count == 0)
            //    Doctors = null;
            //if (CancelDoctors.Count == 0)
            //    CancelDoctors = null;
            //if (Hospitals.Count == 0)
            //    Hospitals = null;
            //if (HospitalDepartments.Count == 0)
            //    HospitalDepartments = null;
            //if (Transfusiologists.Count == 0)
            //    Transfusiologists = null;
            if (SkipDate)
            {
                DateFrom = null; DateTill = null;
            }
            if(SkipPlannedDate)
            {
                PlannedDateFrom = null;
                PlannedDateTill = null;
            }
            base.PrepareToSend();
            
        }
    }

    public class TransfusionRequestJournalRow : BaseObject
    {
        public TransfusionRequestJournalRow () 
        {
            BloodParameterValues = new List<BloodParameterValue>();
        }
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("ApproveDate")]
        public DateTime ApproveDate { get; set; }
        [CSN("CancelDate")]
        public DateTime CancelDate { get; set; }
        [CSN("PlannedDate")]
        public DateTime PlannedDate { get; set; }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("State")]
        public Int32 State { get; set; }
        [CSN("RecipientLastName")]
        public String RecipientLastName { get; set; }
        [CSN("RecipientFirstName")]
        public String RecipientFirstName { get; set; }
        [CSN("RecipientMiddleName")]
        public String RecipientMiddleName { get; set; }

        [CSN("HospitalDepartment")]
        public DepartmentDictionaryItem HospitalDepartment { get; set; }
        [CSN("Doctor")]
        public EmployeeDictionaryItem Doctor { get; set; }
        [CSN("ApprovingDoctor")]
        public EmployeeDictionaryItem ApprovingDoctor { get; set; }
        [CSN("Transfusiologist")]
        public EmployeeDictionaryItem Transfusiologist { get; set; }
        [CSN("CancelDoctor")]
        public EmployeeDictionaryItem CancelDoctor { get; set; }

        [CSN("Candidates")]
        public List<ObjectRef> Candidates { get; set; }

        [CSN("Volume")]
        public Int32 Volume { get; set; }

        [CSN("ApprovedVolume")]
        public Int32 ApprovedVolume { get; set; }

        [CSN("Classification")]
        public ProductClassificationDictionaryItem Classification { get; set; }

        [CSN("Units")]
        public Int32 Units { get; set; }

        [CSN("BloodParameterValues")]
        public List<BloodParameterValue> BloodParameterValues { get; set; }

        [CSN("Template")]
        public TransfusionRequestTemplateDictionaryItem Template { get; set; }

        [CSN("Transfusion")]
        public Transfusion Transfusion { get; set; }

        [CSN("RequestType")]
        public int RequestType { get; set; }

    }
}
