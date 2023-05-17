using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// LIS-6255, LIS-6256
///
namespace ru.novolabs.SuperCore.LimsBusinessObjects.Audit {
    public class AuditJournalFilter : BaseJournalFilter {
        [CSN("Employee")]
        public EmployeeDictionaryItem Employee { get; set; }

        [CSN("LoginDate")]
        public DateTime? LoginDate { get; set; }
        [CSN("SkipLoginDate")]
        [SendToServer(false)]
        public Boolean SkipLoginDate { get; set; }
        [CSN("SessionNr")]
        public Int32? SessionNr { get; set; }

        [CSN("RevNr1")]
        public Int32? RevNr1 { get; set; }
        [CSN("RevNr2")]
        public Int32? RevNr2 { get; set; }

        [CSN("RevDateFrom")]
        public DateTime? RevDateFrom { get; set; }
        [CSN("RevDateTill")]
        public DateTime? RevDateTill { get; set; }
        [CSN("SkipRevDate")]
        [SendToServer(false)]
        public Boolean SkipRevDate { get; set; }


        [CSN("ObjectCode")]
        public String ObjectCode { get; set; }
        [CSN("ObjectType")]
        public String ObjectType { get; set; }
        [CSN("ObjectNr")]
        public Int32? ObjectNr { get; set; }

        public override void PrepareToSend()
        {
            base.PrepareToSend();
            if (SkipRevDate)
            {
                RevDateFrom = null;
                RevDateTill = null;
            }

            if (SkipLoginDate)
                LoginDate = null;
        }
    }

    public class AuditJournalSet
    {
        private List<AuditJournalItem> auditJournal = new List<AuditJournalItem>();
        [CSN("Revisions")]
        public List<AuditJournalItem> Revisions
        {
            get { return auditJournal; }
            set { auditJournal = value; }
        }
    }

    public class AuditJournalItem {
        [CSN("Login")]
        public String Login { get; set; }
        [CSN("SessionNr")]
        public Int32 SessionNr { get; set; }
        [CSN("RevNr")]
        public Int32 RevNr { get; set; }
        [CSN("RevDate")]
        public DateTime RevDate { get; set; }
        [CSN("ObjectId")]
        public Int32 ObjectId { get; set; }
    }



    public class AuditResults
    {
        private List<AuditObject> results = new List<AuditObject>();
        [CSN("Results")]
        public List<AuditObject> Results
        {
            get { return results; }
            set { results = value; }
        }
    }

    public class AuditObject
    {
        [CSN("RevType")]
        public String RevType { get; set; }
        [CSN("RealRevisionDate")]
        public DateTime RealRevisionDate { get; set; }
        [CSN("RealRevision")]
        public Int32 RealRevision { get; set; }

        private List<AuditObjectField> fields = new List<AuditObjectField>();
        [CSN("Fields")]
        public List<AuditObjectField> Fields
        {
            get { return fields; }
            set { fields = value; }
        }
    }

    public class AuditObjectField
    {
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("Kind")]
        public String Kind { get; set; } // имя класса результата
        [CSN("Type")]
        public Int32 Type { get; set; } // тип результата реальный
        private List<AuditSubField> fields = new List<AuditSubField>();
        [CSN("SubObjects")]
        public List<AuditSubField> SubObjects
        {
            get { return fields; }
            set { fields = value; }
        }

    }

    public class AuditSubField
    {
        [CSN("Rev")]
        public Int32 Rev { get; set; } // ревизия субобъекта
        [CSN("RevType")]
        public String RevType { get; set; }
        [CSN("Kind")]
        public String Kind { get; set; } // имя класса субобоъекта
        [CSN("Id")]
        public String Id { get; set; } // id
    }

}
