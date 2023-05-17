using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class BaseSample : BaseObject
    {
        private List<TargetDictionaryItem> targets = new List<TargetDictionaryItem>();
        private Boolean removed = false;
        private BiomaterialDictionaryItem biomaterial;
        private List<Work> works = new List<Work>();
        private List<PatientGroupDictionaryItem> patientGroups = new List<PatientGroupDictionaryItem>();
        private Int32 state;
        private Int32 requestState;
        private List<DefectTypeDictionaryItem> defectTypes = new List<DefectTypeDictionaryItem>();
        private Int64 timestamp;
        /// <summary>
        /// Заказанные исследования
        /// </summary>
        /// 

        public BaseSample()
        {
            UserValues = new List<UserValue>();
            DefectTypes = new List<DefectTypeDictionaryItem>();
            Microorganisms = new List<Microorganism>();
        }

        public BaseSample(String nr)
            : this()
        {
            this.InternalNr = nr;
            UserValues = new List<UserValue>();
            DefectTypes = new List<DefectTypeDictionaryItem>();
            Microorganisms = new List<Microorganism>();
        }

        private List<TargetDictionaryItem> GetTargetsFromDictionaries()
        {
            return ((List<TargetDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Target)).Where(x => targets.FirstOrDefault(y => y.Id == x.Id) != null).ToList();
        }

        [CSN("Timestamp")]
        public Int64 Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }


        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets
        {
            get { return targets; }
            set { targets = value; }
        }

        [CSN("TargetsNames")]
        [SendToServer(false)]
        public string TargetsNames
        {
            get
            {
                if (targets.Count == 0)
                    return "";

                return string.Join(",", GetTargetsFromDictionaries().Where(x => string.IsNullOrEmpty(x.Name) == false).Select(x => x.Name));
            }
        }

        [CSN("TargetsMnemonics")]
        [SendToServer(false)]
        public string TargetsMnemonics
        {
            get
            {
                if (targets.Count == 0)
                    return "";

                return string.Join(",", GetTargetsFromDictionaries().Where(x => string.IsNullOrEmpty(x.Mnemonics) == false).Select(x => x.Mnemonics));
            }
        }

        [CSN("TargetsAltNames")]
        [SendToServer(false)]
        public string TargetsAltNames
        {
            get
            {
                if (targets.Count == 0)
                    return "";

                return string.Join(",", GetTargetsFromDictionaries().Where(x => string.IsNullOrEmpty(x.EngName) == false).Select(x => x.EngName));
            }
        }
        /// <summary>
        /// Признак того, что объект "помечен как удалённый"
        /// </summary>
        /// 
        [CSN("Removed")]
        public Boolean Removed
        {
            get { return removed; }
            set { removed = value; }
        }
        /// <summary>
        /// Номер пробы
        /// </summary>
        /// 
        [CSN("InternalNr")]
        public String InternalNr { get; set; }
        /// <summary>
        /// Номер пробы в подразделении
        /// </summary>
        /// 
        [CSN("DepartmentNr")]
        public String DepartmentNr { get; set; }
        /// <summary>
        /// Биоматериал
        /// </summary>
        /// 
        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial
        {
            get
            {
                if (BiomaterialId > 0)
                {
                    return (BiomaterialDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Biomaterial, BiomaterialId);
                }
                return null;
            }
            set { biomaterial = value; BiomaterialId = value.Id; }
        }
        /// <summary>
        /// Сие пришлось сделать, поскольку сервер присылает именно такое имя
        /// </summary>
        /// 
        [CSN("BioMaterial")]
        public BiomaterialDictionaryItem BioMaterial
        {
            get
            {
                return Biomaterial;
            }
            set
            {
                Biomaterial = value;
            }
        }

        [CSN("BiomaterialName")]
        public string BiomaterialName
        {
            get { return Biomaterial == null ? "" : Biomaterial.Name; }
        }

        [CSN("BiomaterialCode")]
        public string BiomaterialCode
        {
            get { return Biomaterial == null ? "" : Biomaterial.Code; }
        }

        [CSN("BiomaterialMnemonics")]
        public string BiomaterialMnemonics
        {
            get { return Biomaterial == null ? "" : Biomaterial.Mnemonics; }
        }

        [CSN("BiomaterialComment")]
        public string BiomaterialComment
        {
            get { return Biomaterial == null ? "" : Biomaterial.Comment; }
        }

        [CSN("BiomaterialAltName")]
        public string BiomaterialAltName
        {
            get { return Biomaterial == null ? "" : Biomaterial.EngName; }
        }
        /// <summary>
        /// Id биоматериала, необходим, поскольку сервер присылает именно biomaterialId при запросе рабочего журнала
        /// </summary>
        /// 
        [CSN("BiomaterialId")]
        public Int32 BiomaterialId { get; set; }
        /// <summary>
        /// Id подразделения, необходим, поскольку сервер присылает именно departmentId при запросе рабочего журнала
        /// </summary>
        /// 
        [CSN("DepartmentId")]
        public Int32 DepartmentId { get; set; }

        /// <summary>
        /// Заказанные работы(тесты)
        /// </summary>
        /// 
        [CSN("Works")]
        public List<Work> Works
        {
            get { return works; }
            set { works = value; }
        }

        [CSN("DepartmentName")]
        public string DepartmentName
        {
            get { return Department == null ? "" : Department.Name; }
        }
        /// <summary>
        /// Подразделение, с которым связана проба
        /// </summary>
        /// 
        [CSN("Department")]
        public DepartmentDictionaryItem Department
        {
            get
            {
                if (DepartmentId > 0)
                {
                    return (DepartmentDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Department, DepartmentId);
                }
                return null;
            }
            set
            {
                DepartmentId = value.Id;
            }
        }
        /// <summary>
        /// Дата рождения пациента
        /// </summary>
        /// 
        [CSN("BirthDate")]
        [SendToServer(false)]
        public DateTime? BirthDate
        {
            get
            {
                if (BirthDay.HasValue && BirthMonth.HasValue && BirthYear.HasValue)
                    return new DateTime(BirthYear.Value, BirthMonth.Value, BirthDay.Value);
                else
                    return null;
            }
        }

        [CSN("BirthDayString")]
        public string BirthDayString
        {
            get
            {
                if (!BirthYear.HasValue)
                    return "";
                else if (!BirthMonth.HasValue)
                    return BirthYear.Value.ToString();
                else if (!BirthDay.HasValue)
                    return BirthMonth.Value.ToString("D2") + "." + BirthYear.Value.ToString();
                else
                {
                    try
                    {
                        return BirthDay.Value.ToString("D2") + "." + BirthMonth.Value.ToString("D2") + "." + BirthYear.Value.ToString();
                    }
                    catch { return ""; }
                }
            }
        }

        [CSN("BirthDay")]
        public int? BirthDay { get; set; }
        [CSN("BirthMonth")]
        public int? BirthMonth { get; set; }
        [CSN("BirthYear")]
        public int? BirthYear { get; set; }

        /// <summary>
        /// Имя пациента
        /// </summary>
        /// 
        [CSN("FirstName")]
        public String FirstName { get; set; }
        /// <summary>
        /// Фамилия пациента
        /// </summary>
        /// 
        [CSN("LastName")]
        public String LastName { get; set; }
        /// <summary>
        /// Отчество пациента
        /// </summary>
        /// 
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        /// <summary>
        /// ФИО пациента
        /// </summary>
        [SendToServer(false)]
        [CSN("Name")]
        public String Name
        {
            get { return LastName + " " + FirstName + " " + MiddleName; }
            set { }
        }
        /// <summary>
        /// Половая принадлежность пациента
        /// </summary>
        /// 
        [SendToServer(false)]
        [CSN("Sex")]
        [SendAsInt(true)]
        public SexDictionaryItem Sex { get; set; }

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
        /// <summary>
        /// Номер заявки
        /// </summary>
        /// 
        [CSN("RequestInternalNr")]
        public String RequestInternalNr { get; set; }

        [CSN("Request")]
        public ObjectRef Request { get; set; }
        /// <summary>
        /// Группы пациентов
        /// </summary>
        /// 
        [CSN("PatientGroups")]
        public List<PatientGroupDictionaryItem> PatientGroups
        {
            get { return patientGroups; }
            set { patientGroups = value; }
        }

        /// <summary>
        /// Статус пробы
        /// </summary>
        /// 
        [CSN("State")]
        public SampleStateDictionaryItem State { get; set; }

        [SendToServer(false)]
        [CSN("StateName")]
        public String StateName
        {
            get
            {
                if (State != null)
                    return State.Name;
                else
                    return "";
            }
        }


        [CSN("RequestState")]
        public Int32 RequestState
        {
            get
            {
                return requestState;
            }
            set
            {
                requestState = value;
            }
        }

        [SendToServer(false)]
        [CSN("RequestStateName")]
        public String RequestStateName
        {
            get
            {
                if (RequestState > 0)
                    return ((RequestStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestState, RequestState)).Name;
                else
                    return null;
            }
        }

        [CSN("WorksLoaded")]
        [SendToServer(false)]
        public Int32 WorksLoaded { get; set; }

        [CSN("AbnormalCount")]
        [SendToServer(false)]
        public Int32 AbnormalCount { get; set; }

        [CSN("ActivationDate")]
        public DateTime? ActivationDate { get; set; }

        [CSN("SamplingDate")]
        public DateTime? SamplingDate { get; set; }

        [DateTimeFormat(@"dd/MM/yyyy")]
        [CSN("SampleDeliveryDate")]
        public DateTime? SampleDeliveryDate { get; set; }

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

        [CSN("NormsState")]
        [SendToServer(false)]
        public String NormsState
        {
            set { }
            get
            {
                if (WorksLoaded < 1) return "Нет данных";   // id = 0
                else if (AbnormalCount > 0) return "Не норма";  // id = 2
                else return "Норма";  // id = 1
            }
        }


        [SendToServer(false)]
        [CSN("PlannedDate")]
        public String PlannedDate
        {
            get
            {
                if (PlannedDateTime.HasValue && PlannedDateTime.Value != DateTime.MinValue)
                    return PlannedDateTime.ToString();

                if (Works.Count > 0)
                {
                    DateTime Max = new DateTime(0);
                    foreach (Work work in Works)
                    {
                        if (work.PlannedDate > Max && work.PlannedDate != null)
                        {
                            Max = (DateTime)work.PlannedDate;
                        }
                    }
                    return Max.ToString();
                }

                return "";
            }
        }

        [SendToServer(false)]
        [CSN("PlannedDateTime")]
        public DateTime? PlannedDateTime { get; set; }

        [CSN("DefectTypes")]
        public List<DefectTypeDictionaryItem> DefectTypes
        {
            get { return defectTypes; }
            set { defectTypes = value; }
        }
        [CSN("Color")]
        public String Color { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }

        // Пользовательские поля 
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }

        [CSN("CustDoctorId")]
        public Int32 CustDoctorId { get; set; }

        private DoctorDictionaryItem m_CustDoctor;

        [CSN("CustDoctor")]
        public DoctorDictionaryItem CustDoctor
        {
            get
            {
                if (CustDoctorId > 0)
                {
                    return (DoctorDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Doctor, CustDoctorId);
                }
                return m_CustDoctor;
            }
            set { m_CustDoctor = value; CustDoctorId = value.Id; }
        }

        [CSN("EndDate")]
        public DateTime? EndDate { get; set; }

        [CSN("DateOfReceptionResult")]
        public DateTime? DateOfReceptionResult { get; set; }

        [CSN("AdditionalStatesText")]
        public string AdditionalStatesText { get; set; }

        [CSN("CustHospitalId")]
        public Int32 CustHospitalId { get; set; }

        private HospitalDictionaryItem m_CustHospital;
        [CSN("CustHospital")]
        public HospitalDictionaryItem CustHospital
        {
            get
            {
                if (CustHospitalId > 0)
                {
                    return (HospitalDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Hospital, CustHospitalId);
                }
                return m_CustHospital;
            }
            set { m_CustHospital = value; CustHospitalId = value.Id; }
        }

        [CSN("CustDepartmentId")]
        public Int32 CustDepartmentId { get; set; }

        private CustDepartmentDictionaryItem m_CustDepartment;

        [CSN("CustDepartment")]
        public CustDepartmentDictionaryItem CustDepartment
        {
            get
            {
                if (CustDepartmentId > 0)
                {
                    return (CustDepartmentDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.CustDepartment, CustDepartmentId);
                }
                return m_CustDepartment;
            }
            set { m_CustDepartment = value; CustDepartmentId = value.Id; }
        }

        [CSN("PatientCardNr")]
        public string PatientCardNr { get; set; }

        [CSN("PayCategoryId")]
        public Int32 PayCategoryId { get; set; }

        private PayCategoryDictionaryItem m_PayCategory;

        [CSN("PayCategory")]
        public PayCategoryDictionaryItem PayCategory
        {
            get
            {
                if (PayCategoryId > 0)
                {
                    return (PayCategoryDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.PayCategory, PayCategoryId);
                }
                return m_PayCategory;
            }
            set { m_PayCategory = value; PayCategoryId = value.Id; }
        }

        [CSN("Code")]
        public string Code { get; set; }

        [CSN("DefectNames")]
        public string DefectNames { get; set; }

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

        [CSN("Delivered")]
        public bool Delivered { get; set; }

        [CSN("PatientNr")]
        public string PatientNr { get; set; }

        [CSN("PregnancyDuration")]
        public int PregnancyDuration { get; set; }

        [CSN("PriorityHigh")]
        public bool PriorityHigh { get { return Priority == LisRequestPriorities.LIS_PRIORITY_HIGH; } }

        [CSN("DocumentState")]
        public int DocumentState { get; set; }

        [CSN("DocumentStateName")]
        public string DocumentStateName { get { return GetDocumentStateName(); } }

        private string GetDocumentStateName()
        {
            if (DocumentState == 0)
                return "";

            List<string> stateNames = new List<string>();
            if ((DocumentState & 4) == 4)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 4, "Name"));
            if ((DocumentState & 8) == 8)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 8, "Name"));
            if ((DocumentState & 16) == 16)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 16, "Name"));
            if ((DocumentState & 1) == 1)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 1, "Name"));
            if ((DocumentState & 2) == 2)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 2, "Name"));
            if ((DocumentState & 6) == 6)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 6, "Name"));
            if ((DocumentState & 7) == 7)
                stateNames.Add(ProgramContext.Dictionaries.GetDictionaryValue(LimsDictionaryNames.DocumentState, 7, "Name"));

            return String.Join(", ", stateNames);
        }

        [CSN("Priority")]
        public int Priority { get; set; }


        [CSN("CyclePeriod")]
        public CyclePeriodDictionaryItem CyclePeriod { get; set; }


        [CSN("CyclePeriodName")]
        public string CyclePeriodName
        {
            get
            {
                if (CyclePeriod != null)
                    return CyclePeriod.Name;
                else return String.Empty;
            }
        }


        [CSN("ShortName")]
        public string ShortName
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

        [CSN("Cito")]
        public bool Cito { get; set; }

        [SendToServer(false)]
        [CSN("Microorganisms")]
        public List<Microorganism> Microorganisms { get; set; }

    }

    public class CreateRequest3SampleInfo : BaseObject
    {
        public CreateRequest3SampleInfo()
        {
            Targets = new List<CreateRequest3TargetInfo>();
            Biomaterial = new BiomaterialDictionaryItem();
            Priority = 0;
        }

        public CreateRequest3SampleInfo(String nr)
            : this()
        {
            this.InternalNr = nr;
        }

        [CSN("InternalNr")]
        [DTOv2(field: "Barcode")]
        public String InternalNr { get; set; }

        [CSN("Biomaterial")]
        [DTOv2(dictionaryName: LimsDictionaryNames.Biomaterial, codeField: "BiomaterialCode", canCreate: false)]
        public BiomaterialDictionaryItem Biomaterial { get; set; }

        [CSN("Priority")]
        [DTOv2(field: "Priority")]
        public Int32? Priority { get; set; }

        [CSN("Targets")]
        [DTOv2(field: "Targets")]
        public List<CreateRequest3TargetInfo> Targets { get; set; }

        [CSN("Cito")]
        public bool? Cito
        {
            get
            {
                // Костыль, добавленный для обратной совместимости .NET-приложений со старыми серверами ЛИС. После внедрения версий ЛИС старше 2.21.23.0
                // у всех заказчиков костыль можно ликвидировать
                if (ProgramContext.Settings == null)
                    return null;
                bool? enableCito = (bool?)ProgramContext.Settings["enableCitoPropertyForTargetAndSampleInfo", false] ?? false;
                if (enableCito.Value)
                {
                    if (Priority == 1) return true;
                    else return false;
                }
                else
                    return null;
            }

            set { }
        }
    }

    public class SampleSet
    {
        private List<BaseSample> samples = new List<BaseSample>();
        [CSN("Samples")]
        public List<BaseSample> Samples
        {
            get { return samples; }
            set { samples = value; }
        }
    }

    public class RequestSampleInfo
    {
        [CSN("Department")]
        public ObjectRef Department { get; set; }

        [CSN("Request")]
        public ObjectRef Request { get; set; }

        [CSN("Sample")]
        public ObjectRef Sample { get; set; }
    }

    public class TubeBiomaterialSelectorSampleInfo
    {
        [CSN("TubeNr")]
        public String TubeNr { get; set; }

        [CSN("TubeId")]
        public Int32 TubeId { get; set; }
    }

    public class RequestSamplesInfo
    {
        public RequestSamplesInfo()
        {
            Samples = new List<RequestSampleInfo>();
        }

        [CSN("Request")]
        public ObjectRef Request { get; set; }

        [CSN("SampleDeliveryDate")]
        public DateTime? SampleDeliveryDate { get; set; }

        [CSN("Samples")]
        public List<RequestSampleInfo> Samples { get; set; }
    }

    public class SampleShortInfo : BaseObject
    {
        [CSN("BiomaterialId")]
        public Int32 BiomaterialId { get; set; }

        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial
        {
            get
            {
                if (BiomaterialId > 0)
                {
                    return (BiomaterialDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Biomaterial, BiomaterialId);
                }
                return null;
            }
            set { }
        }

        [CSN("DepartmentId")]
        public Int32 DepartmentId { get; set; }

        [CSN("Department")]
        public DepartmentDictionaryItem Department
        {
            get
            {
                if (DepartmentId > 0)
                {
                    return (DepartmentDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Department, DepartmentId);
                }
                return null;
            }
            set { }
        }

        [CSN("InternalNr")]
        public String InternalNr { get; set; }
    }

    public class SampleShortInfoSet
    {
        public SampleShortInfoSet()
        {
            Samples = new List<SampleShortInfo>();
        }

        [CSN("Samples")]
        public List<SampleShortInfo> Samples { get; set; }
    }
}
