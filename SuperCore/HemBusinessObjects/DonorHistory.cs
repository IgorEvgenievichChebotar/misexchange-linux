using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;


namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class DonorHistory : BaseObject
    {
        private DateTime date = DateTime.MinValue;
        private ObjectRef department = new ObjectRef();
        private ObjectRef donationType = new ObjectRef();
        private ObjectRef donationTemplate = new ObjectRef();
        private int visitResultStatus = 0;
        private int donationState = 0;
        private int donationVolume = 0;
        private ObjectRef visit = new ObjectRef();
        private ObjectRef donation = new ObjectRef();

        //public void Prepare(DictionaryCashe DictionaryCash)
        //{
        //    DisplayDate = "-";
        //    DisplayDepartment = "-";
        //    DisplayType = "-";
        //    DisplayTemplate = "-";

        //    if (date != DateTime.MinValue)
        //    {
        //        DisplayDate = date.ToShortDateString();
        //    }
        //    if (department.GetRef() != 0)
        //    {
        //        DisplayDepartment = DictionaryCash.Departments.Find(department.GetRef()).name;
        //    }
        //    if (donationType.GetRef() != 0)
        //    {
        //       DisplayType = DictionaryCash.DonationType.Find(donationType.GetRef()).name;
        //    }
        //    if (donationTemplate.GetRef() != 0)
        //    {
        //        DisplayTemplate = DictionaryCash.DonationTemplate.Find(donationTemplate.GetRef()).name;
        //    }
        //}

        [CSN("Date")]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [CSN("Department")]
        [LinkedDictionary("department", "Name")]
        public ObjectRef Department
        {
            get { return department; }
            set { department = value; }
        }

        [CSN("DonationType")]
        [LinkedDictionary("donationType", "Name")]
        public ObjectRef DonationType
        {
            get { return donationType; }
            set { donationType = value; }
        }

        [CSN("DonationTemplate")]
        [LinkedDictionary("donationTemplate", "Name")]
        public ObjectRef DonationTemplate
        {
            get { return donationTemplate; }
            set { donationTemplate = value; }
        }

        [CSN("VisitResultStatus")]
        public int VisitResultStatus
        {
            get { return visitResultStatus; }
            set { visitResultStatus = value; }
        }

        [CSN("StringDonationState")]
        public string StringDonationState
        {
            get { return GetStringDonationState(); }
            set { }
        }

        [CSN("DonationState")]
        public int DonationState
        {
            get { return donationState; }
            set { donationState = value; }
        }

        [CSN("DonationVolume")]
        public int DonationVolume
        {
            get { return donationVolume; }
            set { donationVolume = value; }
        }

        [CSN("Visit")]
        public ObjectRef Visit
        {
            get { return visit; }
            set { visit = value; }
        }

        [CSN("Donation")]
        public ObjectRef Donation
        {
            get { return donation; }
            set { donation = value; }
        }

        private string GetStringDonationState()
        {
            if (donationState.Equals(4))
            {
                return "Завершена";
            }
            if (donationState.Equals(6))
            {
                return "Отменена";
            }
            return (string.Empty);
        }

    }

    public class DonorHistorySet
    {
        private List<DonorHistory> result = new List<DonorHistory>();

        [CSN("Result")]
        public List<DonorHistory> Result
        {
            get { return result; }
            set { result = value; }
        }
    }

    public class GetDonorHistoryRequest
    {
        public ObjectRef donor = new ObjectRef();

        [CSN("Donor")]
        public ObjectRef Donor
        {
            get { return donor; }
            set { donor = value; }
        }
    }
}