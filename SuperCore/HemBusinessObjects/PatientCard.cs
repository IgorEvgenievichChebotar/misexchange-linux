using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    // BLOOD-94
    public class PatientCard : BaseObject
    {
        public PatientCard()
        {
        }

        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("Date")]
        public DateTime? Date { get; set; }
        [CSN("Patient")]
        [SendAsRef(true)]
        public Donor Patient { get; set; }
        [CSN("HospitalDepartment")]
        public DepartmentDictionaryItem HospitalDepartment { get; set; }
        [CSN("Doctor")]
        public EmployeeDictionaryItem Doctor { get; set; }
        [CSN("PrimaryDiagnosis")]
        public DiagnosisDictionaryItem PrimaryDiagnosis { get; set; }
        [CSN("AdditionalDiagnosises")]
        public DiagnosisDictionaryItem AdditionalDiagnosises { get; set; }
        [CSN("Comments")]
        public String Comments { get; set; }
    }
}
