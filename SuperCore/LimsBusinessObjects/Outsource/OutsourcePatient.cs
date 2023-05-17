using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourcePatient : BaseObject
    {
        public OutsourcePatient()
        {
            UserFields = new List<OutsourceUserField>();
            PatientCard = new OutsourcePatientCard();
            Patient = new ObjectRef();        
        }

        [CSN("Code")]
        public String Code { get; set; } 
        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("BirthDay")]
        public Int32 BirthDay { get; set; }
        [CSN("BirthMonth")]
        public Int32 BirthMonth { get; set; }
        [CSN("BirthYear")]
        public Int32 BirthYear { get; set; }
        [CSN("Sex")]
        public Int32 Sex { get; set; }
        [CSN("Country")]
        public String Country { get; set; }
        [CSN("City")]
        public String City { get; set; }
        [CSN("Street")]
        public String Street { get; set; }
        [CSN("Building")]
        public String Building { get; set; }
        [CSN("Flat")]
        public Int32 Flat { get; set; }
        [CSN("InsuranceCompany")]
        public String InsuranceCompany { get; set; }
        [CSN("PolicySeries")]
        public String PolicySeries { get; set; }
        [CSN("PolicyNumber")]
        public String PolicyNumber { get; set; }
        [CSN("UserFields")]
        public List<OutsourceUserField> UserFields { get; set; }
        [CSN("PatientCard")]
        public OutsourcePatientCard PatientCard { get; set; }
        [XmlIgnore]
        [CSN("Patient")]
        public ObjectRef Patient { get; set; }
    }
}
