using System;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class DenyInfo : BaseObject
    {
        private DenyDictionaryItem deny = null;
        private DepartmentDictionaryItem department = null;
        private ObjectRef donor = new ObjectRef();
        private ObjectRef denyElement = new ObjectRef();
        private DateTime? date = DateTime.MinValue;
        private DateTime? dateFrom = DateTime.MinValue;
        private DateTime? dateTill = DateTime.MinValue;
        private DateTime? dateRemove = DateTime.MinValue;
        private DenySourceDictionaryItem source = null;


        public DenyInfo(DenyJournalItem deny)
        {
            this.Id = deny.Id;
            this.deny = deny.Deny;
            this.donor = null;
            this.denyElement = new ObjectRef(deny);
            try
            {
                this.date = new DateTime(deny.FindYear, deny.FindMonth, deny.FindDay);
            }
            catch
            {
                this.Date = DateTime.Now;
            }
            this.dateFrom = this.date;
            this.dateTill = null;
            this.dateRemove = null;
            this.Source = deny.Source;
        }

        public DenyInfo(Donor donor, DonorDeny donorDeny)
        {
          this.Id = donorDeny.Id;
          this.deny = donorDeny.Deny;
          this.department = donor.Department;
          this.donor = new ObjectRef(donor);
          this.denyElement = null;
          this.date = donorDeny.Date;
          this.dateFrom = donorDeny.DateFrom;
          this.dateTill = donorDeny.DateTill;
          this.dateRemove = donorDeny.DateRemove;
          this.source = donorDeny.Source;
        }

        [CSN("Department")]
        [LinkedDictionary("department", "ExternalCode")]
        public DepartmentDictionaryItem Department
        {
            get { return department; }
            set { department = value; }
        }

        [CSN("Source")]
        [LinkedDictionary("denySource", "ExternalCode")]
        public DenySourceDictionaryItem Source
        {
            get { return source; }
            set { source = value; }
        }

        [CSN("Deny")]
        [LinkedDictionary("deny", "ExternalCode")]
        public DenyDictionaryItem Deny
        {
            get { return deny; }
            set { deny = value; }
        }

        [CSN("Donor")]
        public ObjectRef Donor
        {
            get { if (donor != null) return donor; else return denyElement; }
            set { donor = value; }
        }

        [CSN("DenyElement")]
        public ObjectRef DenyElement
        {
            get { if (denyElement != null) return donor; else return denyElement; }
            set { denyElement = value; }
        }

        [CSN("Date")]
        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }

        [CSN("DateFrom")]
        public DateTime? DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }

        [CSN("DateTill")]
        public DateTime? DateTill
        {
            get { return dateTill; }
            set { dateTill = value; }
        }

        [CSN("DateRemove")]
        public DateTime? DateRemove
        {
            get { return dateRemove; }
            set { dateRemove = value; }
        }
    }
}
