using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourcePatientCard : BaseObject
    {
        public OutsourcePatientCard()
        {
            UserFields = new List<OutsourceUserField>();
            PatientCard = new ObjectRef();
        }
        [CSN("CardNr")]
        public String CardNr { get; set; }
        [CSN("UserFields")]
        public List<OutsourceUserField> UserFields { get; set; }
        [XmlIgnore]
        [CSN("PatientCard")]
        public ObjectRef PatientCard { get; set; }
    }
}
