using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class TransfusionJournalFilter : BaseJournalFilter
    {
        public TransfusionJournalFilter()
        {
            HospitalDepartments = new List<DepartmentDictionaryItem>();
            Doctors = new List<EmployeeDictionaryItem>();
            Results = new List<TransfusionResultDictionaryItem>();
        }

        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        [CSN("TransfusionDateTill")]
        public DateTime? TransfusionDateTill { get; set; }
        [CSN("TransfusionDateFrom")]
        public DateTime? TransfusionDateFrom { get; set; }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("HospitalDepartments")]
        public List<DepartmentDictionaryItem> HospitalDepartments { get; set; }
        [CSN("Doctors")]
        public List<EmployeeDictionaryItem> Doctors { get; set; }
        [CSN("RecipientFirstName")]
        public String RecipientFirstName { get; set; }
        [CSN("RecipientLastName")]
        public String RecipientLastName { get; set; }
        [CSN("RecipientMiddleName")]
        public String RecipientMiddleName { get; set; }
        [CSN("RecipientNr")]
        public String RecipientNr { get; set; }
        [CSN("RecipientCardNr")]
        public String RecipientCardNr { get; set; }
        [CSN("DonorFirstName")]
        public String DonorFirstName { get; set; }
        [CSN("DonorLastName")]
        public String DonorLastName { get; set; }
        [CSN("DonorMiddleName")]
        public String DonorMiddleName { get; set; }
        [CSN("DonorNr")]
        public String DonorNr { get; set; }
        [CSN("ProductNr")]
        public String ProductNr { get; set; }
        [CSN("Results")]
        public List<TransfusionResultDictionaryItem> Results { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }

        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }

        [SendToServer(false)]
        [CSN("SkipTransfusionDate")]
        public Boolean SkipTransfusionDate { get; set; }

        [SendAsRef(true)]
        [CSN("Recipient")]
        public Donor Recipient { get; set; }
        //[CSN("Recipient")]
        //public ObjectRef Recipient { get; set; }

        public override void PrepareToSend()
        {
            if (SkipDate)
            {
                DateTill = DateFrom = null;
            }

            if (SkipTransfusionDate)
            {
                TransfusionDateTill = TransfusionDateFrom = null;
            }
            
            base.PrepareToSend();

            //foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            //{
            //    if ((propInfo.PropertyType.Equals(typeof(DateTime?))) || (propInfo.PropertyType.Equals(typeof(DateTime))))
            //    {
            //        if (propInfo.GetValue(this, null) != null)
            //        {
            //            if (propInfo.Name.EndsWith("From"))
            //            {
            //                if (null != propInfo.GetValue(this, null))
            //                    propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).StartOfTheDay(), null);
            //            }

            //            if (propInfo.Name.EndsWith("Till"))
            //            {
            //                if (null != propInfo.GetValue(this, null))
            //                    propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).EndOfTheDay(), null);
            //            }
            //        }
            //    }
            //}
        }

    }

    public class TransfusionJournalRow : BaseObject
    {
        public TransfusionJournalRow()
        {
            HospitalDepartments = new List<DepartmentDictionaryItem>();
            Recipient = new Donor();
            Donor = new Donor();
        }

        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("HospitalDepartments")]
        public List<DepartmentDictionaryItem> HospitalDepartments { get; set; }
        [CSN("Doctor")]
        public EmployeeDictionaryItem Doctor { get; set; }
        [CSN("RecipientFirstName")]
        public String RecipientFirstName { get; set; }
        [CSN("RecipientLastName")]
        public String RecipientLastName { get; set; }
        [CSN("RecipientMiddleName")]
        public String RecipientMiddleName { get; set; }
        [CSN("RecipientNr")]
        public String RecipientNr { get; set; }
        [CSN("RecipientCardNr")]
        public String RecipientCardNr { get; set; }
        [CSN("DonorFirstName")]
        public String DonorFirstName { get; set; }
        [CSN("DonorLastName")]
        public String DonorLastName { get; set; }
        [CSN("DonorMiddleName")]
        public String DonorMiddleName { get; set; }
        [CSN("DonorNr")]
        public String DonorNr { get; set; }
        [CSN("ProductNr")]
        public String ProductNr { get; set; }
        [CSN("Result")]
        public TransfusionResultDictionaryItem Result { get; set; }
        [CSN("Volume")]
        public Int32 Volume { get; set; }
        [CSN("UnitType")]
        public ProductUnitTypeDictionaryItem UnitType { get; set; }
        //[CSN("ProductClassification")]
        //public ProductClassificationDictionaryItem ProductClassification { get; set; }
        [CSN("Donor")]
        public Donor Donor { get; set; }
        [CSN("Recipient")]
        public Donor Recipient { get; set; }

        [CSN("Status")]
        public Int32 Status { get; set; }
    }
}
