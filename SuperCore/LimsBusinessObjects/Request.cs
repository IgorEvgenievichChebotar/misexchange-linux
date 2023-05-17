using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.ExchangeDTOs;
using System.Reflection;
using ru.novolabs.SuperCore.DictionaryCommon;
using System.Linq;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsBusinessObjects.Payment;
using Newtonsoft.Json;

public struct ObjectDeliveredState
{
    public const int False = 0;
    public const int True = 1;
    public const int Any = 2;
}

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class BaseRequest : BaseObject
    {
        public BaseRequest()
        {
            Country = String.Empty;
            City = String.Empty;
            Street = String.Empty;
            Building = String.Empty;
            Flat = String.Empty;
            InsuranceCompany = String.Empty;
            PolicySeries = String.Empty;
            PolicyNumber = String.Empty;
            UserValues = new List<UserValue>();
            Patient = new ObjectRef();
            PatientCard = new ObjectRef();
            Outsourcers = new List<Int32>();
            ReadOnly = false;
            AdditionalStates = new List<ObjectRef>();
        }

        // Разработчики суровы и использовали Очень не юзерфрендли подход ....
        // Обычно, если нам надо завести поле в подобном классе, мы это делаем классическим образом так:
        // [CSN("MyVariable")]
        // public bool? MyVariable { get; set; }
        // Однако, setter может вызвать Exception типа StackOverflow, в случае, когда мы будем
        // Пытаться приравнять струкутуру старого формата (без нашей MyVariable) к объекту структуры нового формата (с нашей MyVariable)
        // Или даже если мы просто приравняем на этапе инициализации нашу переменную в кокнструкторе к чему-то
        // РЕШЕНИЕ для такого случая обычно выглядит так:
        //
        // bool _myVariable = false; // создаётся буферное приватное поле для хранения реальных значений
        // 
        // [CSN("MyVariable")]
        // public bool? MyVariable
        // get { return _myVariable; }
        // set { _myVariable = value; }
        //
        // ВАЖНО: В нашем проекте _myVariable назвали просто myVariable, что сильно
        // усложняет чтение кода, Ибо и само public поле класса названо MyVariable
        // Собственно ниже имеем буферные приватные поля, а уже под ними публичные поля класса, использующие буферные, Будте внимательны....

        // Системные х-ски.
        String internalNr = String.Empty;
        String externalNr = String.Empty;
        Int32 source = 0;
        Int32? priority = 10;
        Boolean? delivered = false;
        RequestFormDictionaryItem requestForm;
        Boolean printed = false;
        Boolean originalSent = false;
        Boolean copySent = false;
        ObjectRef customState = new ObjectRef();
        RequestStateDictionaryItem stateName = new RequestStateDictionaryItem();
        RequestStateDictionaryItem state = new RequestStateDictionaryItem();
        Boolean hasAttachment = false;
        Boolean isWebRequest = false;


        // Данные пациента.
        private String code = String.Empty;
        private String firstName = String.Empty;
        private String middleName = String.Empty;
        private String lastName = String.Empty;
        private SexDictionaryItem sex;
        private String email = String.Empty;
        // Ссылки на пациента и карту пациента
        [CSN("Patient")]
        public ObjectRef Patient { get; set; }
        [CSN("PatientCard")]
        public ObjectRef PatientCard { get; set; }

        // Параметры, используемые при расчёте норм.
        private DateTime? samplingDate = null;
        private DateTime? sampleDeliveryDate = null;
        private DateTime? endDate = null;
        private Int32? pregnancyDuration;
        private Int32? cyclePeriod;
        private string cyclePeriodName;

        // Связи с заказчиком и плательщиком.
        private HospitalDictionaryItem custHospital;
        private Int32 custHospitalId;
        private CustDepartmentDictionaryItem custDepartment;
        private DoctorDictionaryItem custDoctor;
        private PayCategoryDictionaryItem payCategory;

        private Int64 timestamp;
        
        // Исследования
        private List<TargetDictionaryItem> targets = new List<TargetDictionaryItem>();
        // Биоматериалы
        private List<BiomaterialDictionaryItem> biomaterials = new List<BiomaterialDictionaryItem>();
        private List<PaymentShort> paymentOperations = new List<PaymentShort>(); //LIS-8180

        [CSN("PaymentOperations")]
        public List<PaymentShort> PaymentOperations
        {
            get { return paymentOperations; }
            set { paymentOperations = value; }
        }

        [CSN("Biomaterials")]
        public List<BiomaterialDictionaryItem> Biomaterials
        {
            get { return biomaterials; }
            set { biomaterials = value; }
        }

        [CSN("EndDate")]
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        // Пробы
        private List<BaseSample> samples = new List<BaseSample>();

        [CSN("InternalNr")]
        public String InternalNr
        {
            get { return internalNr; }
            set { internalNr = value; }
        }

        // 1032 // IsWebRequest = request.IsWebRequest;
        [CSN("IsWebRequest")]
        //public Boolean IsWebRequest { get; set; }
        public bool IsWebRequest
        {
            get { return isWebRequest; }
            set { isWebRequest = value; }
        }

        [SendToServer(false)]
        [CSN("ExternalNr")]
        public String ExternalNr
        {
            get { return externalNr; }
            set { externalNr = value; }
        }

        [SendToServer(false)]
        [CSN("Source")]
        public Int32 Source
        {
            get { return source; }
            set { source = value; }
        }

        [CSN("Priority")]
        public Int32? Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        [CSN("Cito")]
        public Boolean Cito
        {
            get
            {
                if (priority == 10) return false;
                else return true;
            }
            set
            {
                if (value)
                    priority = 20;
                else
                    priority = 10;
            }
        }

        [SendToServer(false)]
        [CSN("PriorityHigh")]
        public String PriorityHigh
        {
            get
            {
                if (Cito)
                    return "Да";
                else
                    return "";
            }
        }

        [CSN("Delivered")]
        public Boolean? Delivered
        {
            get { return delivered; }
            set { delivered = value; }
        }        

        [CSN("DefectState")]
        public DefectStateDictionaryItem DefectState { get; set; }

        [CSN("DefectStateName")]
        public string DefectStateName
        {
            get
            {
                if (DefectState != null)
                    return DefectState.Name;
                else return String.Empty;
            }
        }
        [CSN("HasAttachment")]
        public Boolean HasAttachment
        {
            get { return hasAttachment; }
            set { hasAttachment = value; }
        }
        

        [CSN("RequestForm")]
        [JsonIgnore]
        public RequestFormDictionaryItem RequestForm
        {
            get { return requestForm; }
            set { requestForm = value; }
        }
        [CSN("Printed")]
        public Boolean Printed
        {
            get { return printed; }
            set { printed = value; }
        }
        [CSN("OriginalSent")]
        public Boolean OriginalSent
        {
            get { return originalSent; }
            set { originalSent = value; }
        }
        [CSN("CopySent")]
        public Boolean CopySent
        {
            get { return copySent; }
            set { copySent = value; }
        }
/*        [CSN("CustomState")]
        public ObjectRef CustomState
        {
            get { return customState; }
            set { customState = value; }
        } */


        [SendToServer(false)]
        [CSN("State")]
        public RequestStateDictionaryItem State
        {
            get 
            {
                if (Removed)
                    return (RequestStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestState, 4);
                return state;
            }
            set { state = value; }
        }

        [SendToServer(false)]
        [LinkedDictionary(LimsDictionaryNames.RequestState, "Name")]
        [CSN("StateName")]
        public RequestStateDictionaryItem StateName
        {
            get
            {
                // Очень злая заглушка!!!!
                if (Removed)
                    return (RequestStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestState, 4);
                if (state.Id > 0)
                {
                    return (RequestStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestState, state.Id);
                }
                return stateName;
            }
            set { stateName = value; }
        }
        [CSN("Samples")]
        [JsonIgnore]
        public List<BaseSample> Samples
        {
            get { return samples; }
            set { samples = value; }
        }
        [CSN("Targets")]
        [JsonIgnore]
        public List<TargetDictionaryItem> Targets
        {
            get { return targets; }
            set { targets = value; }
        }
        [CSN("Code")]
        public String Code
        {
            get { return code; }
            set { code = value; }
        }
        [CSN("FirstName")]
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [CSN("MiddleName")]
        public String MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }
        [CSN("LastName")]
        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [CSN("Email")]
        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        [CSN("Retailer")]
        public HospitalDictionaryItem Retailer { get; set; }

        [CSN("RetailerPayCategory")]
        public PayCategoryDictionaryItem RetailerPayCategory { get; set; }

        [SendToServer(false)]
        [CSN("Name")]
        public String Name
        {
            get
            {
                return lastName + " " + firstName + " " + middleName;
            }
        }

        [SendToServer(false)]
        [CSN("LongFullName")]
        public String LongFullName
        {
            get
            {
                return LastName + " " + FirstName + " " + MiddleName;
            }
        }
        [SendToServer(false)]
        [CSN("ShortFullName")]
        public String ShortFullName
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(LastName))
                {
                    result += LastName;
                    if (!String.IsNullOrEmpty(FirstName))
                    {
                        result += " " + FirstName.Substring(0, 1) + ".";
                        if (!String.IsNullOrEmpty(MiddleName))
                            result += " " + MiddleName.Substring(0, 1) + ".";
                    }
                }
                return result;
            }
        }

        [CSN("BirthDay")]
        public Int32? BirthDay { get; set; }
        [CSN("BirthMonth")]
        public Int32? BirthMonth { get; set; }
        [CSN("BirthYear")]
        public Int32? BirthYear { get; set; }

        [SendToServer(false)]
        [CSN("BirthDate")]
        public String BirthDate
        {
            get
            {
                return (BirthDay != null ? BirthDay.Value.ToString("D2") : "") + "." +
                    (BirthMonth != null ? BirthMonth.Value.ToString("D2") : "") + "." +
                    (BirthYear != null ? BirthYear.Value.ToString("D4") : "");
            }
            set
            {
                String date = value;
                String[] nums = date.Split(new Char[] { '.' });
                if (date != String.Empty && date != ".." && nums.Length == 3)
                {
                    BirthDay = Int32.Parse(nums[0]);
                    BirthMonth = Int32.Parse(nums[1]);
                    BirthYear = Int32.Parse(nums[2]);
                }
                else
                {
                    BirthDay = BirthMonth = BirthYear = null;
                }

            }
        }
        [CSN("BirthDayString")]
        [SendToServer(false)]
        public String BirthDayString
        {
            get
            {
                return BirthDate;
            }
        }

        [LinkedDictionary(LimsDictionaryNames.Sex, DictionaryPropertyConst.Name)]
        [CSN("Sex")]
        [SendAsInt(true)]
        public SexDictionaryItem Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        [SendToServer(false)]
        [CSN("SexName")]
        public String SexName
        {
            get {
                if (Sex == null) return "Не указан";
                switch (sex.Id)
                {

                    default:
                    case 0:
                        return "Не указан";
                    case 1:
                    case 2:
                        return ((SexDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Sex, sex.Id]).Name;
                }
            }
        }

        [DateTimeFormat(@"dd/MM/yyyy")]
        [CSN("SamplingDate")]
        public DateTime? SamplingDate
        {
            get { return samplingDate; }
            set { samplingDate = value; }
        }

        [DateTimeFormat(@"dd/MM/yyyy")]
        [CSN("SampleDeliveryDate")]
        public DateTime? SampleDeliveryDate
        {
            get
            {
                return sampleDeliveryDate;
            }
            set { sampleDeliveryDate = value; }
        }

        [CSN("Timestamp")]
        public Int64 Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        [CSN("PregnancyDuration")]
        public Int32? PregnancyDuration
        {
            get { return pregnancyDuration; }
            set { pregnancyDuration = value; }
        }
        [CSN("CyclePeriod")]
        public Int32? CyclePeriod
        {
            get { return cyclePeriod; }
            set { cyclePeriod = value; }
        }

        [CSN("CyclePeriodName")]
        [SendToServer(false)]
        public string CyclePeriodName
        {
            get 
            {
                if (CyclePeriod.HasValue == false || CyclePeriod.Value == 0)
                    return string.Empty;

                var cyclePer = ((CyclePeriodDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.CyclePeriod, CyclePeriod.Value));
                if(cyclePer != null)
                    return cyclePer.Name;

                return cyclePeriodName ?? "";
            }
            set
            {
                cyclePeriodName = value;
            }
        }

        [CSN("CustHospitalId")]
        public Int32 CustHospitalId
        {
            get { return custHospitalId; }
            set { custHospitalId = value; }
        }

        [LinkedDictionary(LimsDictionaryNames.Hospital, DictionaryPropertyConst.Fullname)]
        [CSN("CustHospital")]
        public HospitalDictionaryItem CustHospital
        {
            get
            {
                // Очень злая заглушка!!!!

                if (custHospitalId > 0)
                {
                    return (HospitalDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem("hospital", custHospitalId);
                }
                return custHospital;
            }
            set 
            { 
                custHospital = value;
                if (value != null)
                    custHospitalId = value.Id;
            }
        }

        [LinkedDictionary(LimsDictionaryNames.CustDepartment, DictionaryPropertyConst.Name)]
        [CSN("CustDepartment")]
        public CustDepartmentDictionaryItem CustDepartment
        {
            get { return custDepartment; }
            set { custDepartment = value; }
        }
        [LinkedDictionary(LimsDictionaryNames.Doctor, DictionaryPropertyConst.Fullname)]
        [CSN("CustDoctor")]
        public DoctorDictionaryItem CustDoctor
        {
            get { return custDoctor; }
            set { custDoctor = value; }
        }
        [CSN("PayCategory")]
        public PayCategoryDictionaryItem PayCategory
        {
            get { return payCategory; }
            set { payCategory = value; }
        }
        [CSN("Removed")]
        public Boolean Removed { get; set; }

        [CSN("Telephone")]
        public String Telephone { get; set; }


        [CSN("WorksLoaded")]
        [SendToServer(false)]
        public Int32 WorksLoaded { get; set; }

        [CSN("AbnormalCount")]
        [SendToServer(false)]
        public Int32 AbnormalCount { get; set; }

        [CSN("SampleState")]
        [SendToServer(false)]
        public String SampleState
        {
            set { }
            get
             {
                 return ((DictionaryClass<BiomaterialStateDictionaryItem>)ProgramContext.Dictionaries[LimsDictionaryNames.BiomaterialStateEx]).Elements.FirstOrDefault(x => x.Id == SampleStateValue).Name;
            }
        }

        [CSN("SampleStateValue")]
        public int SampleStateValue { get; set; }

        [CSN("DocumentStateName")]
        [SendToServer(false)]
        public String DocumentStateName
        {
            set { }
            get
            {
                // DocumentStateDictionaryItem docState = (DocumentStateDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.DocumentState, elementId: DocumentState];
                // Общий механизм статических справочников не подходит, т.к. "Статус документа" - кумулятивное свойство. Необходимо использовать битовые маски

                List<string> stateNames = new List<string>();
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_PRINTED) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_PRINTED)
                    stateNames.Add("Распечатан");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_REPRINTED) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_REPRINTED)
                    stateNames.Add("Распеч. повт.");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EMAIL_SENT) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EMAIL_SENT)
                    stateNames.Add("Отправлен");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EMAIL_RESENT) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EMAIL_RESENT)
                    stateNames.Add("Отпр. повт.");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EMAIL_NOT_SENT) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EMAIL_NOT_SENT)
                    stateNames.Add("Не отправ.");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_SMS_SENT) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_SMS_SENT)
                    stateNames.Add("Отправлена СМС");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_SMS_ERROR) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_SMS_ERROR)
                    stateNames.Add("Ошибка СМС");
                if ((DocumentState & DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EXTERNAL_SYSTEM_SENT) == DocumentSates.LIS_REQUEST_DOCUMENT_MASK_EXTERNAL_SYSTEM_SENT)
                    stateNames.Add("Отправлен в МИС");

                return String.Join("; ", stateNames);
            }
        }

        [CSN("documentState")]
        public int DocumentState { get; set; }

        [CSN("NormsState")]
        [SendToServer(false)]
        public String NormsState
        {
            set { }
            get
            {
                if (WorksLoaded < 1) return ((DictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.NormalityState, 0)).Name; // Нет данных
                else if (AbnormalCount > 0) return ((DictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.NormalityState, 2)).Name;  // Не норма
                else return ((DictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.NormalityState, 1)).Name;  // Норма
            }
        }

        // Данные о пациенте
        [CSN("PatientCardNr")]
        public String PatientCardNr { get; set; }
        /// <summary>
        /// Адрес регистрации: Страна
        /// </summary>
        [CSN("Country")]
        public String Country { get; set; }
        /// <summary>
        /// Адрес регистрации: Название региона(области)
        /// </summary>
        [CSN("Region")]
        public String Region { get; set; }
        /// <summary>
        /// Адрес регистрации: Район области
        /// </summary>
        [CSN("Area")]
        public String Area { get; set; }
        /// <summary>
        /// Адрес регистрации: Город
        /// </summary>
        [CSN("City")]
        public String City { get; set; }
        /// <summary>
        /// Адрес регистрации: Населённый пункт
        /// </summary>
        [CSN("Location")]
        public String Location { get; set; }
        /// <summary>
        /// Адрес регистрации: Улица
        /// </summary>
        [CSN("Street")]
        public String Street { get; set; }
        /// <summary>
        /// Адрес регистрации: Дом
        /// </summary>
        [CSN("Building")]
        public String Building { get; set; }
        [CSN("Flat")]
        public String Flat { get; set; }
        //  Данные о страховке
        [CSN("InsuranceCompany")]
        public String InsuranceCompany { get; set; }
        [CSN("PolicySeries")]
        public String PolicySeries { get; set; }
        [CSN("PolicyNumber")]
        public String PolicyNumber { get; set; }
        [CSN("CustHospitalCode")]
        public String CustHospitalCode { get; set; }
        [CSN("CustHospitalName")]
        public String CustHospitalName { get; set; }
        [CSN("CustDepartmentCode")]
        public String CustDepartmentCode { get; set; }
        [CSN("CustDepartmentName")]
        public String CustDepartmentName { get; set; }
        [CSN("CustDoctorCode")]
        public String CustDoctorCode { get; set; }
        [CSN("CustDoctorName")]
        public String CustDoctorName { get; set; }
        // Пользовательские поля 
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }
        [CSN("ExternalSystem")]
        public ExternalSystemDictionaryItem ExternalSystem { get; set; }


        [CSN("Outsourcers")]
        public List<Int32> Outsourcers { get; set; }

        [SendToServer(false)]
        [CSN("Outsource")]
        public String Outsource
        {
            get
            {
                if (Outsourcers != null)
                {
                    String result = "";
                    List<OutsourcerDictionaryItem> ouss = new List<OutsourcerDictionaryItem>();
                    foreach (Int32 outsourcer in Outsourcers)
                    {
                        OutsourcerDictionaryItem os = (OutsourcerDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.OutsourcerNew, outsourcer];
                        if (os != null)
                            ouss.Add(os);
                    }
                    ouss.Sort((a, b) => (a.Name.CompareTo(b.Name)));
                    foreach (OutsourcerDictionaryItem os in ouss)
                    {
                        if (result != "")
                            result += ", ";
                        result = result + os.Name; 
                    }
                    return result;
                }
                else
                    return null;
            }
        }

        //Заглушки из-за толстого клиента. RAAAAAGE

        [CSN("RequestFormId")]
        [SendToServer(false)]
        public Int32 RequestFormId
        {
            get
            {
                if (RequestForm != null)
                    return RequestForm.Id;
                else
                    return 0;
            }
            set
            {
                RequestFormDictionaryItem requestForm = (RequestFormDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestForm, value);
                RequestForm = requestForm;
            }
        }

        [CSN("PayCategoryId")]
        [SendToServer(false)]
        public Int32 PayCategoryId
        {
            get
            {
                if (PayCategory != null)
                    return PayCategory.Id;
                else
                    return 0;
            }
            set
            {
                PayCategoryDictionaryItem payCategory = (PayCategoryDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.PayCategory, value);
                PayCategory = payCategory;
            }
        }

        [CSN("Organization")]
        public OrganizationDictionaryItem Organization { get; set; }
        [CSN("OrganizationId")]
        [SendToServer(false)]
        public Int32 OrganizationId
        {
            get
            {
                if (Organization != null)
                    return Organization.Id;
                else
                    return 0;
            }
            set
            {
                OrganizationDictionaryItem organization = (OrganizationDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Organization, value);
                Organization = organization;
            }
        }

        [CSN("CustDepartmentId")]
        [SendToServer(false)]
        public Int32 CustDepartmentId
        {
            get
            {
                if (CustDepartment != null)
                    return CustDepartment.Id;
                else
                    return 0;
            }
            set
            {
                CustDepartmentDictionaryItem custDepartment = (CustDepartmentDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.CustDepartment, value);
                CustDepartment = custDepartment;
            }
        }

        [CSN("CustDoctorId")]
        [SendToServer(false)]
        public Int32 CustDoctorId
        {
            get
            {
                if (CustDoctor != null)
                    return CustDoctor.Id;
                else
                    return 0;
            }
            set
            {
                CustDoctor = (DoctorDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Doctor, value);
            }
        }

        [SendToServer(false)]
        [CSN("TargetsText")]
        public String TargetsText
        {
            get
            {
                String result = "";
                foreach (TargetDictionaryItem target in targets)
                {
                    TargetDictionaryItem tar = (TargetDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Target, target.Id];
                    if (tar != null)
                        result += tar.Mnemonics + "; ";
                }
                return result;
            }
        }

        [CSN("PaymentState")]
        public PaymentStateDictionaryItem PaymentState { get; set; }

        [CSN("PaymentStateColor")]
        [SendToServer(false)]
        public string PaymentStateColor
        {
            get
            {
                if (!string.IsNullOrEmpty(PaymentState.Color))
                {
                    return PaymentState.Color;
                }
                return "";
            }
            set
            {
                PaymentState.Color = value;
            }
        }

        [SendToServer(false)]
        [CSN("PaymentStateName")]
        public string PaymentStateName
        {
            get
            {
                return PaymentState == null ? "" : PaymentState.Name;
            }
            set
            {
                PaymentState.Name = value;
            }
        }

        [CSN("Cost")]
        public float Cost { get; set; }

        [CSN("PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        [CSN("ReadOnly")]
        public Boolean ReadOnly { get; set; }

        [CSN("AdditionalStates")]
        public List<ObjectRef> AdditionalStates { get; set; }

        [CSN("AdditionalStatesText")]
        [SendToServer(false)]
        public string AdditionalStatesText
        {
            get
            {
                String result = "";
                foreach (ObjectRef addStateId in AdditionalStates)
                {
                    RequestCustomStateDictionaryItem state = (RequestCustomStateDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.RequestCustomState, addStateId.Id];
                    if (state != null)
                        result += state.Mnemonics + "; ";
                }
                return result;
            }

        }

        [CSN("RegistrationDate")]
        public DateTime? RegistrationDate { get; set; }

        [CSN("Quota")]
        public ObjectRef Quota { get; set; }

        [CSN("QuotaDisplayName")]
        public string QuotaDisplayName { get; set; }

        [CSN("ExecutorOrganization")]
        public OrganizationDictionaryItem ExecutorOrganization { get; set; }
        /// <summary>
        /// Адрес проживания: Страна
        /// </summary>
        [CSN("LivingAddressCountry")]
        public String LivingAddressCountry { get; set; }
        /// <summary>
        /// Адрес проживания: Название региона(области)
        /// </summary>
        [CSN("LivingAddressRegion")]
        public String LivingAddressRegion { get; set; }
        /// <summary>
        /// Адрес проживания: Район
        /// </summary>
        [CSN("LivingAddressArea")]
        public String LivingAddressArea { get; set; }
        /// <summary>
        /// Адрес проживания: Город
        /// </summary>
        [CSN("LivingAddressCity")]
        public String LivingAddressCity { get; set; }
        /// <summary>
        /// Адрес проживания: Населённый пункт
        /// </summary>
        [CSN("LivingAddressLocation")]
        public String LivingAddressLocation { get; set; }
        /// <summary>
        /// Адрес проживания: Улица
        /// </summary>
        [CSN("LivingAddressStreet")]
        public String LivingAddressStreet { get; set; }
        /// <summary>
        /// Адрес проживания: Дом
        /// </summary>
        [CSN("LivingAddressBuilding")]
        public String LivingAddressBuilding { get; set; }
        /// <summary>
        /// Адрес проживания: Квартира
        /// </summary>
        [CSN("LivingAddressFlat")]
        public String LivingAddressFlat { get; set; }
        /// <summary>
        /// Номер амбулаторной карты пациента
        /// </summary>
        [CSN("CardAmbulatory")]
        public String CardAmbulatory { get; set; }
        /// <summary>
        /// Номер стационарной карты пациента
        /// </summary>
        [CSN("CardStationary")]
        public String CardStationary { get; set; }
        /// <summary>
        /// Номер карты пациента типа "дополнительная тип 1"
        /// </summary>
        [CSN("CardExtraType1")]
        public String CardExtraType1 { get; set; }

    } // public class BaseRequest : BaseObject

    public class CreateRequest3Request : BaseObject
    {
        public CreateRequest3Request()
        {
            Request = new ObjectRef();
            InternalNr = String.Empty;
            CustHospital = new HospitalDictionaryItem();
            CustDepartment = new CustDepartmentDictionaryItem();
            CustDoctor = new DoctorDictionaryItem();
            Patient = new ObjectRef();
            PatientCard = new ObjectRef();
            PayCategory = new PayCategoryDictionaryItem();
            Retailer = new HospitalDictionaryItem();
            RetailerPayCategory = new PayCategoryDictionaryItem();
            UserValues = new List<UserValue>();
            Samples = new List<CreateRequest3SampleInfo>();
            BatchNrs = new List<string>();
            Priority = 10;
            PaymentOperations = new List<PaymentShort>();
        }


        public CreateRequest3Request(BaseRequest request)
        {
            if (request != null)
            {
                Id = request.Id;
                BirthDate = request.BirthDate;
                Building = request.Building;
                CardAmbulatory = request.CardAmbulatory;
                CardExtraType1 = request.CardExtraType1;
                CardStationary = request.CardStationary;
                CardNr = request.PatientCardNr;
                Cito = request.Cito;
                City = request.City;
                Code = request.Code;
                Country = request.Country;
                CustDepartment = request.CustDepartment;
                CustDoctor =  request.CustDoctor == null ? null : (DoctorDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Doctor, request.CustDoctor.Id];
                CustHospital = request.CustHospital;
                CyclePeriod = request.CyclePeriod;
                Email = request.Email;
                FirstName = request.FirstName;
                Flat = request.Flat;
                InsuranceCompany = request.InsuranceCompany;
                InternalNr = request.InternalNr;
                IsWebRequest = request.IsWebRequest;
                LastName = request.LastName;
                MiddleName = request.MiddleName;
                Organization = request.Organization;
                Patient = request.Patient;
                PatientCard = request.PatientCard;
                PayCategory = request.PayCategory;
                PolicyNumber = request.PolicyNumber;
                PolicySeries = request.PolicySeries;
                PregnancyDuration = request.PregnancyDuration;
                Priority = request.Priority;
                Request = new ObjectRef(request.Id);
                RequestForm = request.RequestForm;
                Retailer = request.Retailer;
                RetailerPayCategory = request.RetailerPayCategory;
                SampleDeliveryDate = request.SampleDeliveryDate;
                SamplingDate = request.SamplingDate;
                Sex = request.Sex;
                Street = request.Street;
                Source = request.Source;
                Telephone = request.Telephone;
                UserValues = request.UserValues;
                Samples = new List<CreateRequest3SampleInfo>();
                Readonly = request.ReadOnly;
                RequestsInfoResponse tir = request as RequestsInfoResponse;
                foreach (BaseSample sample in request.Samples)
                {
                    CreateRequest3SampleInfo sampleX = new CreateRequest3SampleInfo();
                    sampleX.Id = sample.Id;
                    sampleX.InternalNr = sample.InternalNr;
                    sampleX.Biomaterial = sample.Biomaterial;
                    foreach (TargetDictionaryItem target in sample.Targets)
                    {
                        CreateRequest3TargetInfo targetX = new CreateRequest3TargetInfo();
                        targetX.Target = target;
                        sampleX.Targets.Add(targetX);
                        if(tir != null)
                        {
                            var td = tir.TargetsDetails.FirstOrDefault(x => x.Sample.Id == sample.Id && x.Target.Id == target.Id);
                            if (td == null)
                            { 
                                // TargetDetails может записаться в другую пробу с таким-же биоматериалом. Ищем его там
                                foreach (BaseSample sameSample in request.Samples.Where(x => x != sample && x.Biomaterial.Id == sample.Biomaterial.Id))
                                {
                                    td = tir.TargetsDetails.FirstOrDefault(x => x.Sample.Id == sameSample.Id && x.Target.Id == target.Id);
                                    if (td != null)
                                        break;
                                }
                            }
                            if(td != null)
                            {
                                targetX.OriginalPrice = td.OriginalPrice;
                                targetX.Price = td.Price;
                                targetX.DiscountInRub = td.DiscountInRub;
                                targetX.Cito = td.Cito;
                                targetX.State = td.State;
                            }
                        }
                    }
                    Samples.Add(sampleX);
                }
                Quota = request.Quota;
                QuotaDisplayName = request.QuotaDisplayName;
                Removed = request.Removed;
                PaymentOperations = request.PaymentOperations;
                PaymentState = request.PaymentState;
                ExecutorOrganization = request.ExecutorOrganization;
            }
        }
        /// <summary>
        /// Метод заполнения данных заявки одноимёнными данными пациента
        /// </summary>
        /// <param name="patient">Экземпляр класса с данными пациента</param>
        public void FillPatientField(Patient patient, bool dontSetRefference = false)
        {
            if (patient == null)
                return;
            if (dontSetRefference == false)
                this.Patient = new ObjectRef(patient.Id);
            this.FirstName = patient.FirstName;
            this.LastName = patient.LastName;
            this.MiddleName = patient.MiddleName;
            this.BirthDay = patient.BirthDay == 0 ? null : patient.BirthDay;
            this.BirthMonth = patient.BirthMonth == 0 ? null : patient.BirthMonth;
            this.BirthYear = patient.BirthYear == 0 ? null : patient.BirthYear;
            this.Email = patient.Email;
            this.Country = patient.Country;
            this.Region = patient.Region;
            this.Area = patient.Area;
            this.City = patient.City;
            this.Location = patient.Location;
            this.Street = patient.Street;
            this.Building = patient.Building;
            this.Flat = patient.Flat;
            this.Code = patient.Code;
            this.DummyPatient = patient.Dummy;            
            this.InsuranceCompany = patient.InsuranceCompany;
            this.PolicyNumber = patient.PolicyNumber;
            this.PolicySeries = patient.PolicySeries;
            this.Sex = patient.Sex;            
            this.Telephone = patient.Telephone;
            this.LivingAddressCountry = patient.LivingAddressCountry;
            this.LivingAddressRegion = patient.LivingAddressRegion;
            this.LivingAddressArea = patient.LivingAddressArea;
            this.LivingAddressCity = patient.LivingAddressCity;
            this.LivingAddressLocation = patient.LivingAddressLocation;
            this.LivingAddressStreet = patient.LivingAddressStreet;
            this.LivingAddressBuilding = patient.LivingAddressBuilding;
            this.LivingAddressFlat = patient.LivingAddressFlat;

            foreach (var userVal in UserValues)
            {
                foreach (var patUserVal in patient.UserValues)
                {
                    if (Id == 0)
                    {
                        if (userVal.Name == patUserVal.Name)
                        {
                            userVal.Reference = new ObjectRef(patUserVal.Reference.Id);
                            userVal.Value = patUserVal.Value;
                            userVal.UserField = new ObjectRef(patUserVal.UserField.Id);
                            userVal.Code = patUserVal.Code;
                        }
                        else if (userVal.UserField.Id == patUserVal.UserField.Id)
                        {
                            userVal.Reference = new ObjectRef(patUserVal.Reference.Id);
                            userVal.Value = patUserVal.Value;
                            userVal.UserField = new ObjectRef(patUserVal.UserField.Id);
                        }
                    }

                }
            }
            // Если по непонятной причине в заявке на данный момент ещё нет пользовательских полей, скопированных из пациента, то копируем их
            patient.UserValues.ForEach(uv =>
            {
                if (!this.UserValues.Exists(uvr => uvr.UserField.Id == uv.UserField.Id))
                {
                    this.UserValues.Add(new UserValue()
                    {
                        UserField = uv.UserField,
                        Value = uv.Value,
                        Reference = uv.Reference,
                        Values = uv.Values
                    });
                }
            });
        }

        [CSN("BatchNrs")]
        public List<String> BatchNrs { get; set; }

        private HospitalDictionaryItem custHospital;

        [CSN("Request")] 
        public ObjectRef Request { get; set; }

        [CSN("InternalNr")]
        [DTOv2(field: "RequestCode")]
        public String InternalNr { get; set; }



        [CSN("CustHospital")]
        [DTOv2(dictionaryName: LimsDictionaryNames.Hospital, codeField: "HospitalCode", nameField: "HospitalName", canCreate: true)]
        public HospitalDictionaryItem CustHospital {
            get
            {
                // Очень злая заглушка!!!!

                if (CustHospitalId > 0)
                {
                    return (HospitalDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem("hospital", CustHospitalId);
                }
                return custHospital;
            }
            set {
                custHospital = value;
            }
        }


        [CSN("CustDepartment")]
        [DTOv2(dictionaryName: LimsDictionaryNames.CustDepartment, codeField: "DepartmentCode", nameField: "DepartmentName", canCreate: true, parentName: "CustHospital")]
        public CustDepartmentDictionaryItem CustDepartment { get; set; }

        [CSN("Organization")]
        [DTOv2(dictionaryName: LimsDictionaryNames.Organization, codeField: "OrganizationCode", nameField: "OrganizationName", canCreate: false)]
        public OrganizationDictionaryItem Organization { get; set; }

        [CSN("CustDoctor")]
        [DTOv2(dictionaryName: LimsDictionaryNames.Doctor, codeField: "DoctorCode", nameField: "DoctorName", canCreate: true, parentName: "CustDepartment")]
        public DoctorDictionaryItem CustDoctor { get; set; }

        [CSN("RequestForm")] 
        [JsonIgnore]
        public RequestFormDictionaryItem RequestForm { get; set; }

        [CSN("SamplingDate")]
        [DTOv2(field: "SamplingDate")]
        public virtual DateTime? SamplingDate { get; set; }

        [CSN("SampleDeliveryDate")]
        [DTOv2(field: "SampleDeliveryDate")]
        public virtual DateTime? SampleDeliveryDate { get; set; }

        [CSN("PregnancyDuration")]
        [DTOv2(field: "PregnancyDuration")]
        public Int32? PregnancyDuration { get; set; }

        [CSN("CyclePeriod")]
        [DTOv2(field: "CyclePeriod")]
        public Int32? CyclePeriod { get; set; }

        [CSN("Readonly")]
        [DTOv2(field: "ReadOnly")]
        public Boolean Readonly { get; set; }

        [CSN("Priority")] 
        public Int32? Priority { get; set; }

        [CSN("Patient")]        
        public ObjectRef Patient { get; set; }

        [CSN("Code")]
        [DTOv2(field: "Patient.Code")]
        public String Code { get; set; }

        [CSN("FirstName")]
        [DTOv2(field: "Patient.FirstName")]
        public String FirstName { get; set; }

        [CSN("LastName")]
        [DTOv2(field: "Patient.LastName")]
        public String LastName { get; set; }

        [CSN("MiddleName")]
        [DTOv2(field: "Patient.MiddleName")]
        public String MiddleName { get; set; }

        [GuiIgnore]
        [CSN("BirthDay")]
        [DTOv2(field: "Patient.BirthDay")]
        public Int32? BirthDay { get; set; }

        [GuiIgnore]
        [CSN("BirthMonth")]
        [DTOv2(field: "Patient.BirthMonth")]
        public Int32? BirthMonth { get; set; }

        [GuiIgnore]
        [CSN("BirthYear")]
        [DTOv2(field: "Patient.BirthYear")]
        public Int32? BirthYear { get; set; }
        
        [CSN("Sex")]
        [DTOv2(field: "Patient.Sex")]
        [SendAsInt(true)]
        public SexDictionaryItem Sex { get; set; }

        [CSN("Country")]
        [DTOv2(field: "Patient.Country")]
        public String Country { get; set; }
        /// <summary>
        /// Адрес регистрации: Название региона(области)
        /// </summary>
        [CSN("Region")]
        public String Region { get; set; }
        /// <summary>
        /// Адрес регистрации: Район области
        /// </summary>
        [CSN("Area")]
        public String Area { get; set; }
        [CSN("City")]
        [DTOv2(field: "Patient.City")]
        public String City { get; set; }
        /// <summary>
        /// Адрес регистрации: Населённый пункт
        /// </summary>
        [CSN("Location")]
        public String Location { get; set; }

        [CSN("Street")]
        [DTOv2(field: "Patient.Street")]
        public String Street { get; set; }

        [CSN("Building")]
        [DTOv2(field: "Patient.Building")]
        public String Building { get; set; }

        [CSN("Flat")]
        [DTOv2(field: "Patient.Flat")]
        public String Flat { get; set; }

        [CSN("InsuranceCompany")]
        [DTOv2(field: "Patient.InsuranceCompany")]
        public String InsuranceCompany { get; set; }

        [CSN("PolicySeries")]
        [DTOv2(field: "Patient.PolicySeries")]
        public String PolicySeries { get; set; }

        [CSN("PolicyNumber")]
        [DTOv2(field: "Patient.PolicyNumber")]
        public String PolicyNumber { get; set; }

        [CSN("Email")] 
        [DTOv2(field: "Email")]
        public String Email { get; set; }

        [CSN("Telephone")] 
        public String Telephone { get; set; }

        [CSN("PatientCard")] 
        public ObjectRef PatientCard { get; set; }        

        [CSN("CardNr")]
        [DTOv2(field: "Patient.PatientCard.CardNr")]
        public String CardNr { get; set; }

        [CSN("CardSuffix")] 
        public String CardSuffix { get; set; }

        [CSN("Kind")] 
        public Int32? Kind { get; set; }

        [CSN("ExternalId")] 
        public String ExternalId { get; set; }

        [CSN("PayCategory")]
        [DTOv2(dictionaryName: LimsDictionaryNames.PayCategory, codeField: "PayCategoryCode", canCreate: false, parentName: "CustHospital")]
        public PayCategoryDictionaryItem PayCategory { get; set; }

        [CSN("Retailer")] 
        public HospitalDictionaryItem Retailer { get; set; }

        [CSN("RetailerPayCategory")] 
        public PayCategoryDictionaryItem RetailerPayCategory { get; set; }

        [CSN("Samples")]
        [DTOv2(field: "Samples")]
        [JsonIgnore]
        public List<CreateRequest3SampleInfo> Samples { get; set; }

        [CSN("UserValues")]
        [DTOv2(field: "UserFields")]
        public List<UserValue> UserValues { get; set; }

        [SendToServer(false)]
        [CSN("CustHospitalId")]
        public Int32 CustHospitalId { get; set; }

        [SendToServer(false)]
        [CSN("BirthDate")]
        public String BirthDate
        {
            get
            {
                return (BirthDay != null ? BirthDay.Value.ToString("D2") : "") + "." +
                    (BirthMonth != null ? BirthMonth.Value.ToString("D2") : "") + "." +
                    (BirthYear != null ? BirthYear.Value.ToString("D4") : "");
            }
            set
            {
                String date = value;
                String[] nums = date.Split(new Char[] { '.' });
                if (date != String.Empty && date != ".." && nums.Length == 3)
                {
                    if (nums[0] != String.Empty)
                        BirthDay = Int32.Parse(nums[0]);
                    if (nums[1] != String.Empty)
                        BirthMonth = Int32.Parse(nums[1]);
                    if (nums[2] != String.Empty)
                        BirthYear = Int32.Parse(nums[2]);
                }
                else
                {
                    BirthDay = BirthMonth = BirthYear = null;
                }
            }
        }

        [SendToServer(false)]
        [CSN("Cito")]
        public Boolean Cito
        {
            get
            {
                if (Priority == 10) return false;
                else return true;
            }
            set
            {
                if (value)
                    Priority = 20;
                else
                    Priority = 10;
            }
        }

        [SendToServer(false)]
        [CSN("DummyPatient")]
        public Boolean DummyPatient { get; set; }

        [SendToServer(false)]
        [CSN("PaymentState")]
        public PaymentStateDictionaryItem PaymentState { get; set; }

        [SendToServer(false)]
        [CSN("Cost")]
        public float Cost { get; set; }

        [SendToServer(false)]
        [CSN("PaymentDate")]
        public DateTime? PaymentDate { get; set; }

        [SendToServer(false)]
        [JsonIgnore]
        [CSN("CurrentUser")]
        public EmployeeDictionaryItem CurrentUser
        {
            //get { return ProgramContext.LisCommunicator.LimsUserSession.User; }
            get;
            set; 
        }

        [CSN("Quota")]
        public ObjectRef Quota { get; set; }

        [CSN("QuotaDisplayName")]
        [SendToServer(false)]
        public string QuotaDisplayName { get; set; }

        [CSN("QuotaExceeded")]
        [SendToServer(false)]
        public bool QuotaExceeded { get; set; }

        [CSN("Removed")]
        [SendToServer(false)]
        [XmlIgnore]
        public bool Removed { get; set; }

        [CSN("PaymentOperations")]
        [SendToServer(false)]
        public List<PaymentShort> PaymentOperations { get; set; }

        /// <summary>
        /// Номер амбулаторной карты пациента
        /// </summary>
        [CSN("CardAmbulatory")]
        public String CardAmbulatory { get; set; }
        /// <summary>
        /// Номер стационарной карты пациента
        /// </summary>
        [CSN("CardStationary")]
        public String CardStationary { get; set; }
        /// <summary>
        /// Номер карты пациента типа "дополнительная тип 1"
        /// </summary>
        [CSN("CardExtraType1")]
        public String CardExtraType1 { get; set; }
        /// <summary>
        /// Флаг копирования заявки
        /// </summary>
        [CSN("IsCopy")]
        public bool? IsCopy { get; set; }

        /// <summary>
        /// Флаг метка того, что заявка сохранена через Веб2
        /// </summary>
        [CSN("IsWebRequest")]
        public bool? IsWebRequest { get; set; }



        [CSN("ExecutorOrganization")]
        public OrganizationDictionaryItem ExecutorOrganization { get; set; }
        /// <summary>
        /// Опекун пациента
        /// </summary>
        [CSN("Guardian")]
        [DTOv2(field: "Guardian")]
        public String Guardian { get; set; }
        /// <summary>
        /// идентификатор заказа в Яндекс
        /// </summary>
        [CSN("Yandex_OrderId")]
        [DTOv2(field: "Yandex_OrderId")]
        public String Yandex_OrderId { get; set; }
        /// <summary>
        /// идентификатор сумки/бригады в Яндекс
        /// </summary>
        [CSN("Yandex_BucketId")]
        [DTOv2(field: "Yandex_BucketId")]
        public String Yandex_BucketId { get; set; }
        /// <summary>
        /// Адрес проживания: Страна
        /// </summary>
       [CSN("LivingAddressCountry")]
        public String LivingAddressCountry { get; set; }
        /// <summary>
        /// Адрес проживания: Название региона(области)
        /// </summary>
        [CSN("LivingAddressRegion")]
        public String LivingAddressRegion { get; set; }
        /// <summary>
        /// Адрес проживания: Район
        /// </summary>
        [CSN("LivingAddressArea")]
        public String LivingAddressArea { get; set; }
        /// <summary>
        /// Адрес проживания: Город
        /// </summary>
        [CSN("LivingAddressCity")]
        public String LivingAddressCity { get; set; }
        /// <summary>
        /// Адрес проживания: Населённый пункт
        /// </summary>
        [CSN("LivingAddressLocation")]
        public String LivingAddressLocation { get; set; }
        /// <summary>
        /// Адрес проживания: Улица
        /// </summary>
        [CSN("LivingAddressStreet")]
        public String LivingAddressStreet { get; set; }
        /// <summary>
        /// Адрес проживания: Дом
        /// </summary>
        [CSN("LivingAddressBuilding")]
        public String LivingAddressBuilding { get; set; }
        /// <summary>
        /// Адрес проживания: Квартира
        /// </summary>
        [CSN("LivingAddressFlat")]
        public String LivingAddressFlat { get; set; }
        /// <summary>
        /// Идентификатор источника создания заявки(МИС, пакетная, сканер, ...) // Источник заявки
        /// </summary>
        [SendToServer(false)]
        [CSN("Source")] 
        public Int32 Source { get; set; }

        //
        // Добавлять новые поля нужно именно сюда и после всех полей, что выше этого коммента
        //

    }

    public class RequestBatchNr
    {
        String internalNr = String.Empty;
        [CSN("InternalNr")]
        public String InternalNr
        {
            get { return internalNr; }
            set { internalNr = value; }
        }
    }

    public class RequestSaveParams
    {
        Boolean createWorklists = false;
        BaseRequest request = null;
        List<RequestBatchNr> batchNrs = new List<RequestBatchNr>();

        [CSN("CreateWorklists")]
        public Boolean CreateWorklists
        {
            get { return createWorklists; }
            set { createWorklists = value; }
        }

        [CSN("Request")]
        public BaseRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        [CSN("BatchNrs")]
        public List<RequestBatchNr> BatchNrs
        {
            get { return batchNrs; }
            set { batchNrs = value; }
        }
    }

    public class RequestId
    {
        public RequestId()
        {
            Id = 0;
            InternalNr = String.Empty;
        }
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("InternalNr")]
        public String InternalNr { get; set; }
    }

    public class RequestSaveResponce
    {
        public RequestSaveResponce()
        {
            Ids = new List<RequestId>();
            Errors = new List<ErrorMessage>();
        }
        [CSN("Ids")]
        public List<RequestId> Ids { get; set; }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }



    public class FindRequestByContainerNrRequest
    {
        String containerNr = String.Empty;
        [CSN("ContainerNr")]
        public String ContainerNr
        {
            get { return containerNr; }
            set { containerNr = value; }
        }
    }

    public class RequestSet
    {
        private List<BaseRequest> request = new List<BaseRequest>();
        [CSN("Request")]
        public List<BaseRequest> Request
        {
            get { return request; }
            set { request = value; }
        }
    }

    public class BaseRequestFilter
    {
        public const Int32 LIS_DELIVERED_FALSE = 0;
        public const Int32 LIS_DELIVERED_TRUE = 1;
        public const Int32 LIS_DELIVERED_ANY = 2;

        public const Int32 LIS_PATIENT_PRESENT_NO = 0;
        public const Int32 LIS_PATIENT_PRESENT_YES = 1;
        public const Int32 LIS_PATIENT_PRESENT_ALL = 2;

        public const Int32 LIS_HAS_DEFECT_NONE = 0;
        public const Int32 LIS_HAS_DEFECT_TRUE = 1;
        public const Int32 LIS_HAS_DEFECT_FALSE = 2;

        String firstName = String.Empty;
        String lastName = String.Empty;
        String middleName = String.Empty;
        String patientNr = String.Empty;
        String patientCardNr = String.Empty;
        Int32 birthDay;
        Int32 birthMonth;
        Int32 birthYear;
        Int32 sex = SexConst.LIS_GENDER_ALL;
        Int32 patientPresent = LIS_PATIENT_PRESENT_ALL;
        String nr = String.Empty;
        DateTime dateFrom = DateTime.Today;
        DateTime dateTill = DateTime.Today;
        DateTime? endDateFrom;
        DateTime? endDateTill;
        String billNr = String.Empty;
        Int32 billed = 2;
        Boolean skipDate = false;
        Boolean markPlanDeviation = false;
        Boolean onlyDelayed = false;
        Int32 priority = 0;
        Int32 delivered = LIS_DELIVERED_ANY;
        Int32 defectState = LIS_HAS_DEFECT_NONE;
        Int32 printed = 2;
        Int32 originalSent = 2;
        Int32 copySent = 2;
        Boolean emptyPayCategory = false;
        [CSN("FirstName")]
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [CSN("LastName")]
        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [CSN("MiddleName")]
        public String MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }
        [CSN("PatientNr")]
        public String PatientNr
        {
            get { return patientNr; }
            set { patientNr = value; }
        }
        [CSN("PatientCardNr")]
        public String PatientCardNr
        {
            get { return patientCardNr; }
            set { patientCardNr = value; }
        }
        [CSN("BirthDay")]
        public Int32 BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }
        [CSN("BirthMonth")]
        public Int32 BirthMonth
        {
            get { return birthMonth; }
            set { birthMonth = value; }
        }
        [CSN("BirthYear")]
        public Int32 BirthYear
        {
            get { return birthYear; }
            set { birthYear = value; }
        }
        [CSN("Sex")]
        public Int32 Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        [CSN("PatientPresent")]
        public Int32 PatientPresent
        {
            get { return patientPresent; }
            set { patientPresent = value; }
        }
        [CSN("Nr")]
        public String Nr
        {
            get { return nr; }
            set { nr = value; }
        }
        [CSN("DateFrom")]
        public DateTime DateFrom
        {
            get { return SkipDate ? new DateTime() : dateFrom; }
            set { dateFrom = value; }
        }
        [CSN("DateTill")]
        public DateTime DateTill
        {
            get { return SkipDate ? new DateTime() : dateTill; }
            set { dateTill = value; }
        }
        [CSN("Priority")]
        public Int32 Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        [CSN("Delivered")]
        public Int32 Delivered
        {
            get { return delivered; }
            set { delivered = value; }
        }
        [CSN("DefectState")]
        public Int32 DefectState
        {
            get { return defectState; }
            set { defectState = value; }
        }
        [CSN("Printed")]
        public Int32 Printed
        {
            get { return printed; }
            set { printed = value; }
        }
        [CSN("OriginalSent")]
        public Int32 OriginalSent
        {
            get { return originalSent; }
            set { originalSent = value; }
        }
        [CSN("CopySent")]
        public Int32 CopySent
        {
            get { return copySent; }
            set { copySent = value; }
        }
        [CSN("EmptyPayCategory")]
        public Boolean EmptyPayCategory
        {
            get { return emptyPayCategory; }
            set { emptyPayCategory = value; }
        }
        [CSN("EndDateFrom")]
        public DateTime? EndDateFrom
        {
            get { return endDateFrom; }
            set { endDateFrom = value; }
        }
        [CSN("EndDateTill")]
        public DateTime? EndDateTill
        {
            get { return endDateTill; }
            set { endDateTill = value; }
        }
        [CSN("BillNr")]
        public String BillNr
        {
            get { return billNr; }
            set { billNr = value; }
        }
        [CSN("Billed")]
        public Int32 Billed
        {
            get { return billed; }
            set { billed = value; }
        }
        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate
        {
            get { return skipDate; }
            set { skipDate = value; }
        }
        [CSN("MarkPlanDeviation")]
        public Boolean MarkPlanDeviation
        {
            get { return markPlanDeviation; }
            set { markPlanDeviation = value; }
        }
        [CSN("OnlyDelayed")]
        public Boolean OnlyDelayed
        {
            get { return onlyDelayed; }
            set { onlyDelayed = value; }
        }
    }

    public class RequestFilter : BaseRequestFilter
    {
        public RequestFilter()
        {
            States = new List<ObjectRef>();
            CustomStates = new List<ObjectRef>();
            RequestForms = new List<ObjectRef>();
            PayCategories = new List<ObjectRef>();
            CustDepartments = new List<ObjectRef>();
            Doctors = new List<ObjectRef>();
            Hospitals = new List<ObjectRef>();
            Targets = new AndOrIdList();
            Departments = new AndOrIdList();
            InternalNrs = new List<String>();
            PatientCodes = new List<String>();
        }
        [CSN("States")]
        public List<ObjectRef> States { get; set; }
        [CSN("CustomStates")]
        public List<ObjectRef> CustomStates { get; set; }
        [CSN("RequestForms")]
        public List<ObjectRef> RequestForms { get; set; }
        [CSN("PayCategories")]
        public List<ObjectRef> PayCategories { get; set; }
        [CSN("CustDepartments")]
        public List<ObjectRef> CustDepartments { get; set; }
        [CSN("Doctors")]
        public List<ObjectRef> Doctors { get; set; }
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        [CSN("Targets")]
        public AndOrIdList Targets { get; set; }
        [CSN("Departments")]
        public AndOrIdList Departments { get; set; }
        [CSN("InternalNrs")]
        public List<String> InternalNrs { get; set; }
        [CSN("PatientCodes")]
        public List<String> PatientCodes { get; set; }
    }



    public class CheckRequestNrsParamsRequest
    {
        public CheckRequestNrsParamsRequest()
        {
            Nrs = new List<RequestBatchNr>();
        }
        [CSN("Nrs")]
        public List<RequestBatchNr> Nrs { get; set; }
    }

    public class CheckRequestNrsParamsResponce
    {
        public CheckRequestNrsParamsResponce()
        {
            Nrs = new List<RequestBatchNr>();
        }

        [Unnamed]
        [CSN("Nrs")]
        public List<RequestBatchNr> Nrs { get; set; }
    }

    public class CheckRequestXXXSNrsParamsResponce
    {
        public CheckRequestXXXSNrsParamsResponce()
        {
            Nr = "";
            Errors = new List<ErrorMessage>();
        }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class RemoveRequestsRequest
    {
        public RemoveRequestsRequest()
        {
            Requests = new List<ObjectRef>();
        }
        [CSN("Requests")]
        public List<ObjectRef> Requests { get; set; }
    }

    public class RequestSamplesParams
    {
        public RequestSamplesParams()
        {
            Request = new ObjectRef();
        }
        [CSN("Request")]
        public ObjectRef Request { get; set; }
    }

    public class RequestSamplesResponce
    {
        public RequestSamplesResponce()
        {
            Samples = new List<BaseSample>();
        }
        [CSN("Samples")]
        public List<BaseSample> Samples { get; set; }
    }

    public class NrsList
    {
        public NrsList()
        {
            Nrs = new List<string>();
            Errors = new List<ErrorMessage>();
        }
        [SendToServer(false)]
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
        [CSN("Nrs")]
        public List<String> Nrs { get; set; }
    }

    public class PatientHistoryRequest
    {
        public PatientHistoryRequest()
        {
            Sample = new ObjectRef();
            Patient = new ObjectRef();
        }
        [CSN("Sample")]
        public ObjectRef Sample { get; set; }

        [CSN("Patient")]
        public ObjectRef Patient { get; set; }
    }

    public class PatientHistoryResponse
    {
        public PatientHistoryResponse()
        {
            Result = new List<PatientHistoryResponseListItem>();
        }

        [Unnamed]
        [CSN("Result")]
        public List<PatientHistoryResponseListItem> Result { get; set; }

        public class PatientHistoryResponseListItem
        {
            public PatientHistoryResponseListItem()
            {
                Works = new List<Work>();
            }

            [DateTimeFormat(@"dd/MM/yyyy")]
            [CSN("Date")]
            public DateTime? Date { get; set; }

            [CSN("Works")]
            public List<Work> Works { get; set; }
        }
    }

    public class PatientQuickSearchRequest
    {
        [CSN("Code")]
        public string Code { get; set; }

        [CSN("LastName")]
        public string LastName { get; set; }

        [CSN("FirstName")]
        public string FirstName { get; set; }

        [CSN("MiddleName")]
        public string MiddleName { get; set; }

        [CSN("BirthYear")]
        public int BirthYear { get; set; }
    }

    public class PatientQuickSearchResponse
    {
        public PatientQuickSearchResponse()
        {
            Patients = new List<Patient>();
        }
        [CSN("Patients")]
        public List<Patient> Patients { get; set; }
    }

    public class GenerateRequestTempPasswordResponse
    {
        public GenerateRequestTempPasswordResponse() {}

        [CSN("TempPassword")]
        public string TempPassword { get; set; }
    }

    public class GetRequestTempPasswordResponse
    {
        public GetRequestTempPasswordResponse() { }
        [CSN("TempPassword")]
        public string TempPassword { get; set; }

        [CSN("IssueDate")]
        public DateTime IssueDate { get; set; }
    }

    public class GetPatientLkRequestResponse : LkResponse
    {
        public GetPatientLkRequestResponse() : base()
        {
            SampleResults = new List<ExternalSampleResult>();
        }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("SampleResults")]
        public List<ExternalSampleResult> SampleResults { get; private set; }

        [CSN("OfficeName")]
        public string OfficeName { get; set; }

        [CSN("OfficeAddress")]
        public string OfficeAddress { get; set; }

        [CSN("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }
    }

    public class LkResponse
    {
        public LkResponse() { }

        [CSN("Patient")]
        public Patient Patient { get; set; }
    }

    public class GetPatientLkAllRequestsResponse : LkResponse
    {
        public GetPatientLkAllRequestsResponse(): base()
        {
            RequestsRows = new List<RequestShortRow>();
        }
        [CSN("RequestsRows")]
        public List<RequestShortRow> RequestsRows { get; set; }
    }

    public class RequestShortRow
    {
        public RequestShortRow() { }

        [CSN("Nr")]
        public string Nr { get; set; }
        [CSN("State")]
        public int State { get; set; }
        [CSN("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }
    }

    public class CreateEmployeesMessageRequest : BaseObject
    {
        public CreateEmployeesMessageRequest() { Employees = new List<ObjectRef>(); }

        public CreateEmployeesMessageRequest(List<ObjectRef> employees, string message, string subject)
        {
            this.Employees = employees;
            this.Message = message.Replace("\n", "\r\n");
            this.Subject = subject;
            this.Attachments = new List<int>();
        }

        [CSN("Subject")]
        public string Subject { get; set; }

        [CSN("Message")]
        public string Message { get; set; }

        [CSN("Employees")]
        public List<ObjectRef> Employees { get; set; }

        [CSN("Attachments")]
        public List<int> Attachments { get; set; }
    }

    public class GetEmployeeMessageTextRequest
    {
        [CSN("Id")]
        public int Id { get; set; }
        [CSN("Sent")]
        public bool Sent { get; set; }

        public GetEmployeeMessageTextRequest() { }
    }

    public class GetEmployeesNewMessagesRequest
    {
        public GetEmployeesNewMessagesRequest()
        {
            Employees = new List<ObjectRef>();
        }

        [CSN("Employees")]
        public List<ObjectRef> Employees { get; set; }
    }

    public class GetEmployeesNewMessagesResponse
    {
        public GetEmployeesNewMessagesResponse()
        {
            EmployeesMessages = new List<GetEmployeesNewMessagesResponseRow>();
        }

        [CSN("EmployeesMessages")]
        public List<GetEmployeesNewMessagesResponseRow> EmployeesMessages { get; set; }
    }

    public class GetEmployeesNewMessagesResponseRow : GetEmployeeMessagesResponse
    {
        public GetEmployeesNewMessagesResponseRow() { }

        [CSN("EmployeeId")]
        public int EmployeeId { get; set; }
    }

    public class GetSamplesTargetinfoResponse
    {
        public GetSamplesTargetinfoResponse() 
        {
            Samples = new List<GetSamplesTargetinfoResponseRow>();
        }

        [CSN("Samples")]
        public List<GetSamplesTargetinfoResponseRow> Samples { get; set; }
    }

    public class GetSamplesTargetinfoResponseRow
    {
        public GetSamplesTargetinfoResponseRow()
        {
            TargetDetails = new List<TargetDetails>();
        }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("TargetDetails")]
        public List<TargetDetails> TargetDetails { get; set; }
    }

    public class GetSamplesTargetinfoRequest
    {
        public GetSamplesTargetinfoRequest()
        {
            Samples = new List<ObjectRef>();
        }
        [CSN("Samples")]
        public List<ObjectRef> Samples { get; set; }
    }

    public class TargetDetails
    {
        public TargetDetails() { }

        [CSN("Target")]
        public TargetDictionaryItem Target { get; set; }

        [CSN("TargetName")]
        [SendToServer(false)]
        public string TargetName
        {
            get
            {
                return Target.Name;
            }
        }

        [CSN("State")]
        [SendToServer(false)]
        public int State { get; set; }

        [CSN("StateName")]
        [SendToServer(false)]
        public string StateName
        {
            get
            {
                var state = (TargetInfoStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.TargetInfoState, State);
                if (state != null)
                {
                    return state.Name;
                }
                return "";
            }
        }

        [CSN("StateColor")]
        [SendToServer(false)]
        public string StateColor
        {
            get
            {
                var color = (TargetInfoStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.TargetInfoState, State);
                if (color != null)
                {
                    return color.Color;
                }
                return "";
            }
        }

        [CSN("PlannedDate")]
        public DateTime PlannedDate { get; set; }

        [CSN("Cito")]
        public bool Cito { get; set; }

        [CSN("OriginalPrice")]
        [SendToServer(false)]
        public double OriginalPrice { get; set; }

        [CSN("Price")]
        [SendToServer(false)]
        public double Price { get; set; }

        [CSN("DiscountInRub")]
        [SendToServer(false)]
        public double DiscountInRub { get; set; }

        [CSN("Sample")]
        [SendToServer(false)]
        public ObjectRef Sample { get; set; }

    }

    public class RequestsInfoResponse : BaseRequest
    {
        public RequestsInfoResponse() : base()
        {
            TargetsDetails = new List<TargetDetails>();
        }
        [CSN("TargetsDetails")]
        public List<TargetDetails> TargetsDetails { get; set; }
    }
}