using Newtonsoft.Json;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class PatientCard: BaseObject
    {
        public PatientCard()
        {
            UserValues = new List<UserValue>();
            PayCategory = new ObjectRef();
        }
        [CSN("CardNr")]
        public String CardNr { get; set; } // Номер карты
        [CSN("CardSuffix")]
        public String CardSuffix { get; set; } // Cуффикс номера карты
        [CSN("Kind")]
        public Int32 Kind { get; set; } // Тип карты: 1-СТАЦИОНАРНАЯ, 2-АМБУЛАТОРНАЯ, 3-АРХИВНАЯ
        [CSN("ExternalId")]
        public String ExternalId { get; set; } // уникальный внешний идентификатор карты пациента
        [CSN("PayCategory")]
        public ObjectRef PayCategory { get; set; } // Ссылка на справочник "Категории оплаты"
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }
    } 
    
    public class Patient: BaseObject
    {
        public Patient()
        {
            Code = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            MiddleName = String.Empty;
            BirthDay = 0;
            BirthMonth = 0;
            BirthYear = 0;
            Sex = new SexDictionaryItem() { Id = SexConst.LIS_GENDER_NONE };
            Persistent = true;
            SendEmail = false;
       //     CustHospital = new ObjectRef();
       //     CustDepartment = new ObjectRef();
       //     CustDoctor = new ObjectRef();
       //     PayCategory = new ObjectRef();

            PatientCards = new List<PatientCard>();
            UserValues = new List<UserValue>();
            CurrentUser = new EmployeeDictionaryItem();
        }

        public Patient(Patient createFrom)
        {
            if (createFrom == null)
                return;
            Code = createFrom.Code;
            FirstName = createFrom.FirstName;
            LastName = createFrom.LastName;
            MiddleName = createFrom.MiddleName;
            BirthDay = createFrom.BirthDay;
            BirthMonth = createFrom.BirthMonth;
            BirthYear = createFrom.BirthMonth;
            Sex = createFrom.Sex;
            Persistent = createFrom.Persistent;
            Removed = createFrom.Removed;
            Country = createFrom.Country;
            City = createFrom.City;
            Street = createFrom.Street;
            Building = createFrom.Building;
            Flat = createFrom.Flat;
            InsuranceCompany = createFrom.InsuranceCompany;
            PolicySeries = createFrom.PolicySeries;
            PolicyNumber = createFrom.PolicyNumber;
            UserValues = createFrom.UserValues;
            CustHospital = createFrom.CustHospital;
            CustDepartment = createFrom.CustDepartment;
            PayCategory = createFrom.PayCategory;
            ExternalSystem = createFrom.ExternalSystem;
            Telephone = createFrom.Telephone;
            Email = createFrom.Email;
            SendEmail = createFrom.SendEmail;
        }

        [CSN("Code")]
        public String Code { get; set; }
        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [GuiIgnore]
        [CSN("BirthDay")]
        public Int32? BirthDay { get; set; }
        [GuiIgnore]
        [CSN("BirthMonth")]
        public Int32? BirthMonth { get; set; }
        [GuiIgnore]
        [CSN("BirthYear")]
        public Int32? BirthYear { get; set; }
        [CSN("Sex")]
        [SendAsInt(true)]
        public SexDictionaryItem Sex { get; set; }
        [CSN("Persistent")]
        public Boolean Persistent { get; set; }
        [SendToServer(false)]
        [CSN("Removed")]
        public Boolean Removed { get; set; }
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
        /// <summary>
        /// Адрес регистрации: Квартира
        /// </summary>
        [CSN("Flat")]
        public String Flat { get; set; }
        //  Данные о страховке
        [CSN("InsuranceCompany")]
        public String InsuranceCompany { get; set; }
        [CSN("PolicySeries")]
        public String PolicySeries { get; set; }
        [CSN("PolicyNumber")]
        public String PolicyNumber { get; set; }
        [CSN("PatientCards")]
        [JsonIgnore]
        public List<PatientCard> PatientCards { get; set; }
        [CSN("PatientNr")]
        public String PatientNr { get; set; }
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }
        [CSN("FindByCode")]
        public bool? FindByCode { get; set; }

        [CSN("CustHospital")]
        public HospitalDictionaryItem CustHospital { get; set; }
        [CSN("CustDepartment")]
        public CustDepartmentDictionaryItem CustDepartment { get; set; }
        [CSN("PayCategory")]
        public PayCategoryDictionaryItem PayCategory { get; set; }
        [CSN("ExternalSystem")]
        public ExternalSystemDictionaryItem ExternalSystem { get; set; }
        [CSN("Telephone")]
        public String Telephone { get; set; }
        [CSN("Email")]
        public String Email { get; set; }
        [CSN("SendEmail")]
        public Boolean SendEmail { get; set; }
        

        [SendToServer(false)]
        [CSN("CustHospitalId")]
        public Int32 CustHospitalId { get; set; }
        [SendToServer(false)]
        [CSN("CustDepartmentId")]
        public Int32 CustDepartmentId { get; set; }
        [SendToServer(false)]
        [CSN("CustDoctorId")]
        public Int32 CustDoctorId { get; set; }
        [CSN("CustDoctor")]
        public DoctorDictionaryItem CustDoctor { get; set; }
        [SendToServer(false)]
        [CSN("PayCategoryId")]
        public Int32 PayCategoryId { get; set; }
        [SendToServer(false)]
        [CSN("ExternalSystemId")]
        public Int32 ExternalSystemId { get; set; }

        [SendToServer(false)]
        [CSN("RequestsCount")]
        public Int32 RequestsCount { get; set; }

        //Вычисляемые поля
        [SendToServer(false)]
        [CSN("Name")]
        public String Name
        {
            get {
                String result = LastName;
                if (FirstName != "")
                    result += " " + FirstName;
                if (MiddleName != "")
                    result += " " + MiddleName;
                return result.ToUpper(); 
            }
        }

        [SendToServer(false)]
        [CSN("ShortName")]
        public String ShortName
        {
            get
            {
                String result = LastName.ToUpper();
                if (FirstName != "")
                    result += " " + FirstName[0].ToString().ToUpper() + ".";
                if (MiddleName != "")
                    result += " " + MiddleName[0].ToString().ToUpper() + ".";
                return result;
            }
        }

        [SendToServer(false)]
        [CSN("SexName")]
        public String SexName
        {
            get
            {
                if (Sex == null) return "Не указан";
                switch (Sex.Id)
                {

                    default:
                    case 0:
                        return "Не указан";
                    case 1:
                    case 2:
                        return ((SexDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Sex, Sex.Id]).Name;
                }
            }
        }

        [SendToServer(false)]
        [CSN("IsMale")]
        public Boolean IsMale
        {
            get
            {
                if (Sex.Id == SexConst.LIS_GENDER_MALE)
                    return true;
                return false;
            }
        }

        [SendToServer(false)]
        [CSN("IsFemale")]
        public Boolean IsFemale
        {
            get
            {
                if (Sex.Id == SexConst.LIS_GENDER_FEMALE)
                    return true;
                return false;
            }
        }

        [SendToServer(false)]
        [CSN("IsSexEmpty")]
        public Boolean IsSexEmpty
        {
            get
            {
                if (Sex.Id == SexConst.LIS_GENDER_NONE)
                    return true;
                return false;
            }
        }

        [SendToServer(false)]
        [CSN("FullName")]
        public string FullName
        {
            get { return LastName + ' ' + FirstName + ' ' + MiddleName; }
            set { setFullName(value); }
        }

        private void setFullName(string value)
        {
            String name1 = null;
            String name2 = null;
            String name3 = null;

            value = value.Trim();
            Int32 pos = value.LastIndexOf(' ');
            if (pos > 0)
            {
                name3 = value.Substring(pos + 1);
                value = value.Substring(0, pos).Trim();
                pos = value.LastIndexOf(' ');
                if (pos > 0)
                {
                    name2 = value.Substring(pos + 1);
                    name1 = value.Substring(0, pos).Trim();
                }
                else name2 = value;
            }
            if (null != name1) LastName = name1;
            if (null != name2) FirstName = name2;
            if (null != name3) MiddleName = name3;
        }


        [SendToServer(false)]
        [CSN("BirthDate")]
        public String BirthDate
        {
            get
            {
                return BirthDateString;
            }
            set
            {
                String date = value;
                String[] nums = date.Split(new Char[] { '.' });
                if (date != String.Empty && nums.Length == 3)
                {
                    if (nums[0] != String.Empty)
                    {
                        int days = 0;
                        if (int.TryParse(nums[0], out days))
                            BirthDay = days;
                    }
                    if (nums[1] != String.Empty)
                    {
                        int month = 0;
                        if (int.TryParse(nums[1], out month))
                            BirthMonth = month;
                    }
                    if (nums[2] != String.Empty)
                    {
                        int year = 0;
                        if (int.TryParse(nums[2], out year))
                            BirthYear = year;
                    }
                }
                else
                {
                    BirthDay = BirthMonth = BirthYear = null;
                }

            }
        }

        [SendToServer(false)]
        [CSN("BirthDateString")]
        public String BirthDateString
        {
            get
            {
                String result = "";
                if (BirthYear == null)
                    result = "";
                else
                    if (BirthMonth == null || BirthMonth == 0)
                        result = BirthYear.ToString();
                    else
                        if (BirthDay == null || BirthDay == 0)
                        {
                            result = BirthMonth.ToString();
                            if (result.Length == 1)
                                result = "0" + result;
                            result = result + "." + BirthYear.ToString();
                        }
                        else
                        {
                            DateTime date;
                            if (BirthYear != 0)
                            {
                                date = new DateTime((Int32)BirthYear, (Int32)BirthMonth, (Int32)BirthDay);
                            }
                            else
                                date = DateTime.MinValue;
                            result = date.ToString("dd.MM.yyyy");
                        }
                return result;
            }
        }

        [SendToServer(false)]
        [CSN("Dummy")]
        public Boolean Dummy { get; set; }

        // Добавлено для возможности доствавать ЛПУ, отделения и врачей для фильтрации в контролах
        [SendToServer(false)]
        [JsonIgnore]
        [CSN("CurrentUser")]
        public EmployeeDictionaryItem CurrentUser { get; set; }

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
    }

    public class PatientCardId
    {
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("CardNr")]
        public String CardNr { get; set; }
    }

    public class PatientResponce
    {
        public PatientResponce()
        {
            PatientCards = new List<PatientCardId>();
        }

        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("PatientCards")]
        public List<PatientCardId> PatientCards { get; set; }
    }
}
