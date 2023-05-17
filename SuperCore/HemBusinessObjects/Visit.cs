using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.HemDictionary;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class VisitHeader : DonorHeader
    {
        private DateTime date = DateTime.Now;
        private DateTime startDate = DateTime.Now;
        private bool relative = false;
        private HospitalDictionaryItem senderHospital = null;
        private int donorStatus = 0;
        private String donationRecipient = null;
        private int reserveType = 0;
        private DonorCategoryDictionaryItem donorCategory = null;
        private DepartmentDictionaryItem department = null;
        private int activeStageType = 0;
        private int resultStatus = 0;
        private DateTime endDate = DateTime.Now;
        private DonationTypeDictionaryItem donationType = null;
        private int volume = 0;
        private bool needMainLabBeforeDonation = false;

        [CSN("Date")]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [CSN("StartDate")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        [CSN("Relative")]
        public bool Relative
        {
            get { return relative; }
            set { relative = value; }
        }

        [CSN("SenderHospital")]
        public HospitalDictionaryItem SenderHospital
        {
            get { return senderHospital; }
            set { senderHospital = value; }
        }

        [CSN("DonationRecipient")]
        public String DonationRecipient
        {
            get { return donationRecipient; }
            set { donationRecipient = value; }
        }

        [CSN("DonorStatus")]
        public int DonorStatus
        {
            get { return donorStatus; }
            set { donorStatus = value; }
        }

        [CSN("ReserveType")]
        public int ReserveType
        {
            get { return reserveType; }
            set { reserveType = value; }
        }

        [CSN("DonorCategory")]
        public DonorCategoryDictionaryItem DonorCategory
        {
            get { return donorCategory; }
            set { donorCategory = value; }
        }

        [CSN("Department")]
        public DepartmentDictionaryItem Department
        {
            get { return department; }
            set { department = value; }
        }

        [CSN("ActiveStageType")]
        public int ActiveStageType
        {
            get { return activeStageType; }
            set { activeStageType = value; }
        }

        [CSN("ResultStatus")]
        public int ResultStatus
        {
            get { return resultStatus; }
            set { resultStatus = value; }
        }

        [CSN("EndDate")]
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        [CSN("DonationType")]
        public DonationTypeDictionaryItem DonationType
        {
            get { return donationType; }
            set { donationType = value; }
        }

        [CSN("Volume")]
        public int Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        [CSN("NeedMainLabBeforeDonation")]
        public bool NeedMainLabBeforeDonation
        {
            get { return needMainLabBeforeDonation; }
            set { needMainLabBeforeDonation = value; }
        }
    }

    public class Visit : VisitHeader
    {
        private bool autoGenerateNr = false;
        private string comment = string.Empty;
        private List<VisitStage> stages = new List<VisitStage>();
        private string lisRequestNrPrefix = string.Empty;
        private string lisRequestNrValue = string.Empty;
        private List<ObjectRef> donorGroups = new List<ObjectRef>();
        private ObjectRef donation = new ObjectRef();
        private PaymentCategoryDictionaryItem paymentCategory = null;
        private List<StateGroupItem> groupStates = new List<StateGroupItem>();
        public string donorFullNr = string.Empty;
        

        [CSN("DonorFullNr")]
        public string DonorFullNr
        {
            get { return donorFullNr; }
            set { donorFullNr = value; }
        }

        [CSN("AutoGenerateNr")]
        public bool AutoGenerateNr
        {
            get { return autoGenerateNr; }
            set { autoGenerateNr = value; }
        }

        [CSN("Comment")]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [CSN("Stages")]
        public List<VisitStage> Stages
        {
            get { return stages; }
            set { stages = value; }
        }

        [CSN("LisRequestNrPrefix")]
        public string LisRequestNrPrefix
        {
            get { return lisRequestNrPrefix; }
            set { lisRequestNrPrefix = value; }
        }

        [CSN("LisRequestNrValue")]
        public string LisRequestNrValue
        {
            get { return lisRequestNrValue; }
            set { lisRequestNrValue = value; }
        }

        [CSN("DonorGroups")]
        public List<ObjectRef> DonorGroups
        {
            get { return donorGroups; }
            set { donorGroups = value; }
        }

        [CSN("Donation")]
        public ObjectRef Donation
        {
            get { return donation; }
            set { donation = value; }
        }

        [CSN("PaymentCategory")]
        public PaymentCategoryDictionaryItem PaymentCategory
        {
            get { return paymentCategory; }
            set { paymentCategory = value; }
        }


        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates
        {
            get
            {
                return groupStates;
            }
            set
            {
                groupStates = value;
            }
        }
        
        public void Prepare()
        {            
        }

        public string DisplayStartDate = string.Empty;
        public string EditHyperLink = string.Empty;
        public string SetNrHyperLink = string.Empty;
        public string DisplayState = string.Empty;
    }

    public class ShortVisit : Visit
    {
    }

    public class ShortVisitSet
    {
        private List<ShortVisit> result = new List<ShortVisit>();

        [CSN("Result")]
        public List<ShortVisit> Result
        {
            get { return result; }
            set { result = value; }
        }
        public void Prepare()
        {
            foreach (ShortVisit Visit in Result)
            {
                Visit.Prepare();
            }
        }
    }



    public class VisitStageDeltaClass
    {
        public string comment = string.Empty;
        public List<ParameterValue> values = new List<ParameterValue>();
        public string stageType = string.Empty;
    }

    public class VisitDelta
    {
        public int id = 0;

        public string nrValue = string.Empty;
        public bool autoGenerateNr = false;
        public ObjectRef donor = new ObjectRef();
        public ObjectRef department = new ObjectRef();
        public bool relative = false;
        public int donorStatus = 0;
        public ObjectRef donorCategory = new ObjectRef();
        public string comment = string.Empty;
        public string lisRequestNrValue = string.Empty;
        public bool needMainLabBeforeDonation = false;
        public string donationRecipient = string.Empty;

        public ObjectRef donationType = new ObjectRef();
        public ObjectRef paymentCategory = new ObjectRef();
        public int reserveType = 1;

        public int volume = 0;

        public int activeStageType = 0;
        public ObjectRef senderHospital = new ObjectRef();
        public List<VisitStageDeltaClass> modifiedStages = new List<VisitStageDeltaClass>();
        public List<ObjectRef> modifiedTargets = new List<ObjectRef>();
        public bool targetsModified = false;
    }

    public class FindVisitsRequest
    {
        private DateTime dateFrom;

        [CSN("DateFrom")]
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }
        private DateTime dateTill;

        [CSN("DateTill")]
        public DateTime DateTill
        {
            get { return dateTill; }
            set { dateTill = value; }
        }

        private List<ObjectRef> departments = new List<ObjectRef>();

        [CSN("Departments")]
        public List<ObjectRef> Departments
        {
            get { return departments; }
            set { departments = value; }
        }
        public int sendType = 2;
        public int relative = 2;
        public int needMainLabBeforeDonation = 2;

        public string donorNr = string.Empty;

        public string lastName = string.Empty;
        public string firstName = string.Empty;
        public string middleName = string.Empty;

        [CSN("DonorNr")]
        public string DonorNr
        {
            get { return donorNr; }
            set { donorNr = value; }
        }

        [CSN("LastName")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string nr = string.Empty;

        public bool emptyNr = false;
        public bool strictLastName = false;
        public ObjectRef sex = new ObjectRef(3);

        public List<ObjectRef> states = new List<ObjectRef>();
        public List<ObjectRef> activeStages = new List<ObjectRef>();
        public List<ObjectRef> donorStatuses = new List<ObjectRef>();
        public List<ObjectRef> reserveTypes = new List<ObjectRef>();
        public List<ObjectRef> senderHospitals = new List<ObjectRef>();
        public List<ObjectRef> donationTypes = new List<ObjectRef>();

        public List<ObjectRef> donorCategories = new List<ObjectRef>();
        public List<ObjectRef> paymentCategory = new List<ObjectRef>();

    }

    public class GetVisitInfo
    {
        public GetVisitInfo()
        {
        }

        public GetVisitInfo(int id)
        {
            this.id = id;
        }

        private int id = 0;
        [CSN("Id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }


    public class VisitSaveResult
    {
        public int id = 0;
        public List<string> errors = new List<string>();
    }

    public class VisitChangeStateReques
    {
        public List<ObjectRef> ids = new List<ObjectRef>();
        public int resultStatus = 2;
        public string comment = string.Empty;
        public int activeStageType = 0;
    }





    public class VisitChangeStateResult: BaseSaveResult
    {
        public List<ErrorMessage> errors = new List<ErrorMessage>();
    }

    public class VisitNrDeltaClass
    {
        public string nrValue = string.Empty;
        public int id = 0;
        public bool autoGenerateNr = false;
    }

}
