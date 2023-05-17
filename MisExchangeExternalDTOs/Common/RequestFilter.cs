using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    public class RequestFilter
    {
        public RequestFilter()
        {
            RequestCodes = new List<String>();
            PatientCodes = new List<String>();
            States = new List<Int32>();
            CustomStates = new List<String>();
            Targets = new List<String>();
            Hospitals = new List<String>();
            CustDepartments = new List<String>();
            Doctors = new List<String>();
            Departments = new List<String>();
        }

        [XmlArrayItem(ElementName = "Code")]
        public List<String> RequestCodes { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> PatientCodes { get; set; }
        [XmlArrayItem(ElementName = "State")]
        public List<Int32> States { get; set; } // Статусы заявок: 1 -  Регистрация; 2 - Открыта; 3 – Закрыта
        [XmlArrayItem(ElementName = "Code")]
        public List<String> CustomStates { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> Targets { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> Hospitals { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> CustDepartments { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> Doctors { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> Departments { get; set; }
    }
}
