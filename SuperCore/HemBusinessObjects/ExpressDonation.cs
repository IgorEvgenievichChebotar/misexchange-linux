//using System;
//using System.Collections.Generic;
//using ru.novolabs.SuperCore.HemDictionary;
//using ru.novolabs.SuperCore.DictionaryCore;


//namespace ru.novolabs.SuperCore.HemBusinessObjects
//{
//    public class ExpressDonation : BaseObject, IDonorHeader
//    {
//        // Fields
//        private DateTime birthDate = DateTime.MinValue;
//        private List<BloodParameterValue> bloodParameters = new List<BloodParameterValue>();
//        private string comments = string.Empty;
//        private DateTime denyDateFrom = DateTime.MinValue;
//        private int denyDuration = 0;
//        private int denyDurationUnit = 0;
//        private ObjectRef denySource = new ObjectRef();
//        private ObjectRef denyType = new ObjectRef();
//        private ObjectRef department = new ObjectRef();
//        private ObjectRef donation = new ObjectRef();
//        private string donationComment = string.Empty;
//        private DateTime donationDate = DateTime.MinValue;
//        private ObjectRef donationTemplate = new ObjectRef();
//        private ObjectRef donationType = new ObjectRef();
//        private bool done = false;
//        private ObjectRef donor = new ObjectRef();
//        private DonorCategoryDictionaryItem donorCategory = null;
//        private DonorCategoryDictionaryItem donorCurrentCategory = null;
//        private int donorCurrentStatus = 0;
//        private ObjectRef donorDeny = new ObjectRef();
//        private string donorNrPrefix = string.Empty;
//        private string donorNrSuffix = string.Empty;
//        private string donorNrValue = string.Empty;
//        private int donorStatus = 0;
//        private List<BloodParameterValue> expressLab = new List<BloodParameterValue>();
//        private string firstName = string.Empty;
//        private bool honored = false;
//        private string internalNr = string.Empty;
//        private string lastName = string.Empty;
//        private string middleName = string.Empty;
//        private string nrValue = string.Empty;
//        private string nrPrefix = string.Empty;
//        private ObjectRef paymentCategory = new ObjectRef();
//        private string recipientName = string.Empty;
//        private DateTime registerDate = DateTime.MinValue;
//        private bool relative = false;
//        private int reserveType = 0;
//        private int result = 0;
//        private ObjectRef senderHospital = new ObjectRef();
//        private int sex = 0;
//        private ObjectRef visit = new ObjectRef();
//        private int volume = 0;
//        private ObjectRef employee = new ObjectRef();
//        private int additiveVolume = 0;
//        private int clearVolume = 0;
//        private int systemBloodVolume = 0;
//        private int serialNumber = 0;





//        // Methods
//        public void beforeSave()
//        {
//            this.nrValue = this.nrValue.Trim();
//        }

//        public bool CheckCanExecute(ref string messages)
//        {
//            messages = string.Empty;
//            if (this.Result == 0)
//            {
//                messages = string.Format(Properties.Resources.Messages_Visit_No_Result, this.NrValue);
//                return false;
//            }
//            if (this.Result == 1)
//            {
//                if ((((this.DonationTemplate.GetRef() == 0) || (this.Volume == 0)) || (this.ExpressLab.Count == 0)) || (this.Relative && (this.RecipientName.Equals(string.Empty) || (this.SenderHospital.GetRef() == 0))))
//                {
//                    messages = string.Format(Properties.Resources.Messages_Visit_Not_AllFields, this.NrValue);

//                    return false;
//                }
//            }
//            else if ((this.Result == 2) && ((((this.DenyDateFrom == DateTime.MinValue) || (this.DenyType.GetRef() == 0)) || ((this.DenyDurationUnit == 0) || (this.DenySource.GetRef() == 0))) || (this.DenyDuration < 1)))
//            {
//                messages = string.Format(Properties.Resources.Messages_Visit_Not_AllFields, this.NrValue);
//                return false;
//            }
//            return true;
//        }

//        // Decompiling functions
//        public void ClearExpressLabIds()
//        {
//            foreach (BaseObject baseObject in this.expressLab)
//                baseObject.Id = 0;
//        }

//        public void DropIds()
//        {
//            this.Id = 0;
//            foreach (BaseObject baseObject in this.BloodParameters)
//                baseObject.Id = 0;
//            foreach (BaseObject baseObject in this.ExpressLab)
//                baseObject.Id = 0;
//        }

//        private string GetBirthDay()
//        {
//            if (!this.birthDate.Equals(DateTime.MinValue))
//            {
//                return this.birthDate.Day.ToString();
//            }
//            return string.Empty;
//        }

//        private string GetBirthMonth()
//        {
//            if (!this.birthDate.Equals(DateTime.MinValue))
//            {
//                return this.birthDate.Month.ToString();
//            }
//            return string.Empty;
//        }

//        private string GetBirthYear()
//        {
//            if (!this.birthDate.Equals(DateTime.MinValue))
//            {
//                return this.birthDate.Year.ToString();
//            }
//            return string.Empty;
//        }

//        public virtual void Prepare() { }

//        private void SetNrValue(string value)
//        {
//            if (value.Length == 0x10)
//            {
//                value = value.Substring(1, value.Length - 1);
//            }
//            if (value.Length == 14)
//            {
//                value = value.Substring(1, value.Length - 1);
//            }
//            if (value.Length == 13)
//            {
//                value = value.Substring(7, 6);
//            }
//            if (value.Length == 15)
//            {
//                value = value.Substring(7, 6);
//            }
//            this.nrValue = value;
//        }

//        public void SetRef(Donor donor)
//        {
//            this.Donor.SetRef(donor.Id);
//            this.DonorNrPrefix = donor.NrPrefix;
//            this.DonorNrValue = donor.NrValue;
//            this.DonorNrSuffix = donor.NrSuffix;
//            this.FirstName = donor.FirstName;
//            this.LastName = donor.LastName;
//            this.MiddleName = donor.MiddleName;
//            this.Sex = donor.Sex;
//            this.BirthDate = donor.BirthDate.Value.Date;
//            this.DonorCurrentStatus = donor.DonorStatus;
//            this.DonorCurrentCategory = donor.DonorCategory;
//            if (this.BloodParameters.Count == 0)
//            {
//                this.BloodParameters = donor.BloodParameters;
//            }
//        }

//        /*
//              private int GetAdditiveVolume();
//              {
//                  object addvolume ;
//                  addvolume = (ContainerDictionaryItem)DictionaryCash.GetDictionaryItem(DictionaryNamesConst.ContainerType, );
//                  return 0;

//              }
//      */


//        [SendToServer(false)]
//        [CSN("SerialNumber")]
//        public Int32 SerialNumber
//        {
//            get { return GetSerialNumber(); }
//            set { serialNumber = value; }
//        }

//        private int GetSerialNumber()
//        {
//            int number = 0;
//            Int32.TryParse(this.DonationComment, out number);
//            return number;
//        }


//        [SendToServer(false)]
//        [CSN("SystemBloodVolume")]
//        public int SystemBloodVolume
//        {
//            get { return GetSystemBloodVolume(); }
//            set { systemBloodVolume = value; }
//        }

//        protected virtual int GetSystemBloodVolume()
//        {
//            throw new NotImplementedException();
//        }

//        [CSN("ClearVolume")]
//        [SendToServer(false)]
//        public Int32 ClearVolume
//        {
//            get { return GetClearVolume(); }
//            set { clearVolume = value; }
//        }

//        private int GetClearVolume()
//        {
//            return volume - AdditiveVolume;
//        }

//        [SendToServer(false)]
//        [CSN("AdditiveVolume")]
//        public Int32 AdditiveVolume
//        {
//            get { return GetAdditiveVolume(); }
//            set { additiveVolume = value; }
//        }

//        protected virtual int GetAdditiveVolume()
//        {
//            throw new NotImplementedException();
//        }

//        protected virtual DonationTemplateDictionaryItem GetDonationTemplate()
//        {
//            throw new NotImplementedException();
//        }

//        [CSN("ShortAge")]
//        [SendToServer(false)]
//        public string ShortAge
//        {
//            get { return GetShortAge(); }
//        }

//        private string GetShortAge()
//        {
//            DonationTemplateDictionaryItem DonTempalte = GetDonationTemplate();
//            int templateVolume = 0;
//            if (DonTempalte != null)
//            {
//                foreach (ProductTemplate pTemplate in DonTempalte.ProductTemplates)
//                {
//                    if (!pTemplate.Removed)
//                    {
//                        templateVolume = pTemplate.Volume;
//                    }
//                }
//                if (this.result == 2)
//                {
//                    return "Отвод";
//                }
//                else if (DonTempalte.ProductTemplates.Count == 0)
//                {
//                    return "Анализы";
//                }
//                else if (templateVolume > this.ClearVolume)
//                {
//                    return "Недобор";
//                }
//                else if (templateVolume < this.ClearVolume)
//                {
//                    return "Перебор";
//                }
//            }
//            return string.Empty;
//        }

//        [SendToServer(false)]
//        [CSN("BirthDate")]
//        public DateTime? BirthDate
//        {
//            get
//            {
//                return this.birthDate;
//            }
//            set
//            {
//                this.birthDate = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("BirthDay")]
//        public string BirthDay
//        {
//            get
//            {
//                return this.GetBirthDay();
//            }
//        }

//        [SendToServer(false)]
//        [CSN("BirthMonth")]
//        public string BirthMonth
//        {
//            get
//            {
//                return this.GetBirthMonth();
//            }
//        }

//        [SendToServer(false)]
//        [CSN("BirthYear")]
//        public string BirthYear
//        {
//            get
//            {
//                return this.GetBirthYear();
//            }
//        }

//        [SendToServer(false)]
//        [CSN("BloodParameters")]
//        public List<BloodParameterValue> BloodParameters
//        {
//            get
//            {
//                return this.bloodParameters;
//            }
//            set
//            {
//                this.bloodParameters = value;
//            }
//        }

//        [CSN("Comments")]
//        public string Comments
//        {
//            get
//            {
//                return this.comments;
//            }
//            set
//            {
//                this.comments = value;
//            }
//        }

//        [CSN("DenyDateFrom")]
//        public DateTime DenyDateFrom
//        {
//            get
//            {
//                return this.denyDateFrom;
//            }
//            set
//            {
//                this.denyDateFrom = value;
//            }
//        }

//        [CSN("DenyDuration")]
//        public int DenyDuration
//        {
//            get
//            {
//                return this.denyDuration;
//            }
//            set
//            {
//                this.denyDuration = value;
//            }
//        }

//        [LinkedDictionary("denyDurationUnits", "Name")]
//        [CSN("DenyDurationUnit")]
//        public int DenyDurationUnit
//        {
//            get
//            {
//                return this.denyDurationUnit;
//            }
//            set
//            {
//                this.denyDurationUnit = value;
//            }
//        }

//        [LinkedDictionary("denySource", "Name")]
//        [CSN("DenySource")]
//        public ObjectRef DenySource
//        {
//            get
//            {
//                return this.denySource;
//            }
//            set
//            {
//                this.denySource = value;
//            }
//        }

//        [LinkedDictionary("deny", "Name")]
//        [CSN("DenyType")]
//        public ObjectRef DenyType
//        {
//            get
//            {
//                return this.denyType;
//            }
//            set
//            {
//                this.denyType = value;
//            }
//        }

//        [LinkedDictionary("department", "Name"), Mandatory(true)]
//        [CSN("Department")]
//        public ObjectRef Department
//        {
//            get
//            {
//                return this.department;
//            }
//            set
//            {
//                this.department = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DisplayBloodGroup")]
//        public string DisplayBloodGroup
//        {
//            get
//            {
//                return this.GetBloodGroup();
//            }
//        }

//        protected virtual string GetBloodGroup()
//        {
//            throw new NotImplementedException();
//        }

//        [SendToServer(false)]
//        [CSN("DisplayBloodResus")]
//        public string DisplayBloodResus
//        {
//            get
//            {
//                return this.GetResus();
//            }
//        }

//        protected virtual string GetResus()
//        {
//            throw new NotImplementedException();
//        }

//        [SendToServer(false)]
//        [CSN("Donation")]
//        public ObjectRef Donation
//        {
//            get
//            {
//                return this.donation;
//            }
//            set
//            {
//                this.donation = value;
//            }
//        }

//        [CSN("DonationComment")]
//        public string DonationComment
//        {
//            get
//            {
//                return this.donationComment;
//            }
//            set
//            {
//                this.donationComment = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonationDate")]
//        public DateTime DonationDate
//        {
//            get
//            {
//                return this.donationDate;
//            }
//            set
//            {
//                this.donationDate = value;
//            }
//        }

//        [LinkedDictionary("donationTemplate", "Name")]
//        [CSN("DonationTemplate")]
//        public ObjectRef DonationTemplate
//        {
//            get
//            {
//                return this.donationTemplate;
//            }
//            set
//            {
//                this.donationTemplate = value;
//            }
//        }

//        [Mandatory(true)]
//        [CSN("DonationType")]
//        public ObjectRef DonationType
//        {
//            get
//            {
//                return this.donationType;
//            }
//            set
//            {
//                this.donationType = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("Done")]
//        public bool Done
//        {
//            get
//            {
//                return this.done;
//            }
//            set
//            {
//                this.done = value;
//            }
//        }

//        [Mandatory(true)]
//        [CSN("Donor")]
//        public ObjectRef Donor
//        {
//            get
//            {
//                return this.donor;
//            }
//            set
//            {
//                this.donor = value;
//            }
//        }

//        [Mandatory(true)]
//        [CSN("DonorCategory")]
//        public DonorCategoryDictionaryItem DonorCategory
//        {
//            get
//            {
//                return this.donorCategory;
//            }
//            set
//            {
//                this.donorCategory = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorCurrentCategory")]
//        public DonorCategoryDictionaryItem DonorCurrentCategory
//        {
//            get
//            {
//                return this.donorCurrentCategory;
//            }
//            set
//            {
//                this.donorCurrentCategory = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorCurrentStatus")]
//        public int DonorCurrentStatus
//        {
//            get
//            {
//                return this.donorCurrentStatus;
//            }
//            set
//            {
//                this.donorCurrentStatus = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorDeny")]
//        public ObjectRef DonorDeny
//        {
//            get
//            {
//                return this.donorDeny;
//            }
//            set
//            {
//                this.donorDeny = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorNr")]
//        public string DonorNr
//        {
//            get
//            {
//                return (this.donorNrPrefix + this.donorNrValue + this.donorNrSuffix);
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorNrPrefix")]
//        public string DonorNrPrefix
//        {
//            get
//            {
//                return this.donorNrPrefix;
//            }
//            set
//            {
//                this.donorNrPrefix = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorNrSuffix")]
//        public string DonorNrSuffix
//        {
//            get
//            {
//                return this.donorNrSuffix;
//            }
//            set
//            {
//                this.donorNrSuffix = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("DonorNrValue")]
//        public string DonorNrValue
//        {
//            get
//            {
//                return this.donorNrValue;
//            }
//            set
//            {
//                this.donorNrValue = value;
//            }
//        }

//        [Mandatory(true)]
//        [CSN("DonorStatus")]
//        public int DonorStatus
//        {
//            get
//            {
//                return this.donorStatus;
//            }
//            set
//            {
//                this.donorStatus = value;
//            }
//        }

//        [CSN("ExpressLab")]
//        public List<BloodParameterValue> ExpressLab
//        {
//            get
//            {
//                return this.expressLab;
//            }
//            set
//            {
//                this.expressLab = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("FirstName")]
//        public string FirstName
//        {
//            get
//            {
//                return this.firstName;
//            }
//            set
//            {
//                this.firstName = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("FullName")]
//        public string FullName
//        {
//            get
//            {
//                return (this.LastName + " " + this.FirstName + " " + this.MiddleName);
//            }
//        }

//        [SendToServer(false)]
//        [CSN("ShortName")]
//        public string ShortName
//        {
//            get { return GetDonorShortName(LastName, FirstName, MiddleName); }
//            set { }
//        }

//        private string GetDonorShortName(string LastName, string FirstName, string MiddleName)
//        {
//            if ((LastName.Length == 0) && (FirstName.Length == 0) && (MiddleName.Length == 0))
//                return string.Empty;
//            return LastName + " " + FirstName.Substring(0, 1).ToUpper() + " " + MiddleName.Substring(0, 1).ToUpper();
//        }


//        [SendToServer(false)]
//        [CSN("Honored")]
//        public bool Honored
//        {
//            get
//            {
//                return this.honored;
//            }
//            set
//            {
//                this.honored = value;
//            }
//        }

//        [CSN("InternalNr")]
//        public string InternalNr
//        {
//            get
//            {
//                return this.internalNr;
//            }
//            set
//            {
//                this.internalNr = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("LastName")]
//        public string LastName
//        {
//            get
//            {
//                return this.lastName;
//            }
//            set
//            {
//                this.lastName = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("MiddleName")]
//        public string MiddleName
//        {
//            get
//            {
//                return this.middleName;
//            }
//            set
//            {
//                this.middleName = value;
//            }
//        }

//        [Mandatory(true)]
//        [CSN("NrValue")]
//        public string NrValue
//        {
//            get
//            {
//                return this.nrValue;
//            }
//            set
//            {
//                this.SetNrValue(value);
//            }
//        }

//        [SendToServer(false)]
//        [CSN("NrPrefix")]
//        public string NrPrefix
//        {
//            get { return this.nrPrefix; }
//            set { this.nrPrefix = value; }
//        }

//        [SendToServer(false)]
//        [CSN("Nr")]
//        public string Nr
//        {
//            get { return 0 + NrPrefix + NrValue; }
//            set { }
//        }

//        [Mandatory(true)]
//        [CSN("PaymentCategory")]
//        public ObjectRef PaymentCategory
//        {
//            get
//            {
//                return this.paymentCategory;
//            }
//            set
//            {
//                this.paymentCategory = value;
//            }
//        }

//        [CSN("RecipientName")]
//        public string RecipientName
//        {
//            get
//            {
//                return this.recipientName;
//            }
//            set
//            {
//                this.recipientName = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("RegisterDate")]
//        public DateTime RegisterDate
//        {
//            get
//            {
//                return this.registerDate;
//            }
//            set
//            {
//                this.registerDate = value;
//            }
//        }

//        [CSN("Relative")]
//        public bool Relative
//        {
//            get
//            {
//                return this.relative;
//            }
//            set
//            {
//                this.relative = value;
//            }
//        }

//        [Mandatory(true)]
//        [CSN("ReserveType")]
//        public int ReserveType
//        {
//            get
//            {
//                return this.reserveType;
//            }
//            set
//            {
//                this.reserveType = value;
//            }
//        }

//        [LinkedDictionary("expressDonationResult", "Name")]
//        [CSN("Result")]
//        public int Result
//        {
//            get
//            {
//                return this.result;
//            }
//            set
//            {
//                this.result = value;
//            }
//        }

//        [LinkedDictionary("hospital", "Name")]
//        [CSN("SenderHospital")]
//        public ObjectRef SenderHospital
//        {
//            get
//            {
//                return this.senderHospital;
//            }
//            set
//            {
//                this.senderHospital = value;
//            }
//        }

//        [SendToServer(false), LinkedDictionary("sex", "Name")]
//        [CSN("Sex")]
//        public int Sex
//        {
//            get
//            {
//                return this.sex;
//            }
//            set
//            {
//                this.sex = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("Visit")]
//        public ObjectRef Visit
//        {
//            get
//            {
//                return this.visit;
//            }
//            set
//            {
//                this.visit = value;
//            }
//        }

//        [SendToServer(false)]
//        [CSN("Employee")]
//        public ObjectRef Employee
//        {
//            get
//            {
//                return this.employee;
//            }
//            set
//            {
//                this.employee = value;
//            }
//        }

//        [CSN("Volume")]
//        public int Volume
//        {
//            get
//            {
//                return this.volume;
//            }
//            set
//            {
//                this.volume = value;
//            }
//        }
//    }



//    public class ExpressDonationSet
//    {
//        private List<ExpressDonation> result = new List<ExpressDonation>();

//        [CSN("Result")]
//        public List<ExpressDonation> Result
//        {
//            get { return result; }
//            set { result = value; }
//        }

//        public void Prepare()
//        {
//            foreach (ExpressDonation expressDonation in result)
//            {
//                expressDonation.Prepare();
//            }
//        }

//        public ExpressDonation FindExpressDonation(int id)
//        {
//            foreach (ExpressDonation expressDonation in result)
//            {
//                if (expressDonation.Id == id)
//                {
//                    return expressDonation;
//                }
//            }
//            return null;
//        }

//        public void GetUniq(ExpressDonationSet MasterDonations)
//        {
//            foreach (ExpressDonation masterDnation in MasterDonations.Result)
//            {
//                foreach (ExpressDonation donation in result)
//                {
//                    if (donation.InternalNr.Equals(masterDnation.InternalNr))
//                    {
//                        Result.Remove(donation);
//                        break;
//                    }
//                }
//            }
//        }
//    }


//    public class GetExpressDonationInfoRequest
//    {
//        public GetExpressDonationInfoRequest()
//        {
//        }

//        private int id = 0;

//        [CSN("Id")]
//        public int Id
//        {
//            get { return id; }
//            set { id = value; }
//        }
//    }

//    public class ExecExpressDonationInfoRequest
//    {
//        public ExecExpressDonationInfoRequest(int expressDonationId)
//        {
//            id = expressDonationId;
//        }

//        private int id = 0;

//        [CSN("Id")]
//        public int Id
//        {
//            get { return id; }
//            set { id = value; }
//        }
//    }

//    public class ExpressDonationSaveList
//    {
//        // Fields
//        private List<Donor> donors = new List<Donor>();
//        private List<ExpressDonation> expressDonations = new List<ExpressDonation>();

//        // Methods
//        public ExpressDonation FindDonation(int id)
//        {
//            foreach (ExpressDonation donation in this.ExpressDonations)
//            {
//                if (donation.Id.Equals(id))
//                {
//                    return donation;
//                }
//            }
//            return null;
//        }

//        public ExpressDonation FindDonationByDonorId(int donorId)
//        {
//            foreach (ExpressDonation donation in this.ExpressDonations)
//            {
//                if (donation.Donor.GetRef().Equals(donorId))
//                {
//                    return donation;
//                }
//            }
//            return null;
//        }

//        // Properties
//        [CSN("Donors")]
//        public List<Donor> Donors
//        {
//            get
//            {
//                return this.donors;
//            }
//            set
//            {
//                this.donors = value;
//            }
//        }

//        [CSN("ExpressDonations")]
//        public List<ExpressDonation> ExpressDonations
//        {
//            get
//            {
//                return this.expressDonations;
//            }
//            set
//            {
//                this.expressDonations = value;
//            }
//        }
//    }

//    public class FindExpressDonationRequest
//    {
//        public FindExpressDonationRequest()
//        {
//            RegisterDateFrom = DateTime.Today;
//            RegisterDateTill = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
//        }

//        public void BeforeSave()
//        {
//            RegisterDateFrom = RegisterDateFrom.Date;
//            RegisterDateTill = RegisterDateTill.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
//        }

//        private string donorNr = string.Empty;
//        private string lastName = string.Empty;
//        private string firstName = string.Empty;
//        private string middleName = string.Empty;
//        private bool strictLastName = false;
//        private List<ObjectRef> departments = new List<ObjectRef>();
//        private DateTime registerDateFrom = DateTime.MinValue;
//        private DateTime registerDateTill = DateTime.MinValue;
//        private DateTime donationDateFrom = DateTime.MinValue;
//        private DateTime donationDateTill = DateTime.MinValue;
//        private int? done = null;

//        private List<int> results = new List<int>();
//        private string internalNr = string.Empty;
//        private bool newOnly = false;

//        [SendToServer(false)]
//        [CSN("NewOnly")]
//        public bool NewOnly
//        {
//            get { return newOnly; }
//            set { newOnly = value; }
//        }

//        [CSN("DonorNr")]
//        public string DonorNr
//        {
//            get { return donorNr; }
//            set { donorNr = value; }
//        }

//        [CSN("LastName")]
//        public string LastName
//        {
//            get { return lastName; }
//            set { lastName = value; }
//        }

//        [CSN("FirstName")]
//        public string FirstName
//        {
//            get { return firstName; }
//            set { firstName = value; }
//        }

//        [CSN("MiddleName")]
//        public string MiddleName
//        {
//            get { return middleName; }
//            set { middleName = value; }
//        }

//        [CSN("StrictLastName")]
//        public bool StrictLastName
//        {
//            get { return strictLastName; }
//            set { strictLastName = value; }
//        }

//        [CSN("Departments")]
//        public List<ObjectRef> Departments
//        {
//            get { return departments; }
//            set { departments = value; }
//        }

//        [CSN("RegisterDateFrom")]
//        public DateTime RegisterDateFrom
//        {
//            get { return registerDateFrom; }
//            set { registerDateFrom = value; }
//        }

//        [CSN("RegisterDateTill")]
//        public DateTime RegisterDateTill
//        {
//            get { return registerDateTill; }
//            set { registerDateTill = value; }
//        }

//        [CSN("DonationDateFrom")]
//        public DateTime DonationDateFrom
//        {
//            get { return donationDateFrom; }
//            set { donationDateFrom = value; }
//        }

//        [CSN("DonationDateTill")]
//        public DateTime DonationDateTill
//        {
//            get { return donationDateTill; }
//            set { donationDateTill = value; }
//        }

//        [CSN("Done")]
//        public int? Done
//        {
//            get { return done; }
//            set { done = value; }
//        }

//        [CSN("Results")]
//        public List<int> Results
//        {
//            get { return results; }
//            set { results = value; }
//        }

//        [CSN("InternalNr")]
//        public string InternalNr
//        {
//            get { return internalNr; }
//            set { internalNr = value; }
//        }
//   }
//}