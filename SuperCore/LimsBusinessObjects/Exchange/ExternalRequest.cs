using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;

/// Набор классов для описания заявки на выполнение исследований

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает заказ на выполнение исследований для одного пациента
    /// </summary>
    ///   

    [XmlType(TypeName = "Request")]
    public class ExternalRequest : BaseObject
    {
        public ExternalRequest()
        {
            Patient = new ExternalRequestPatient();
            Samples = new List<ExternalRequestSample>();
            UserValues = new List<UserValue>();
            Priority = LisRequestPriorities.LIS_PRIORITY_LOW;
            Errors = new List<ErrorMessage>();
            Warnings = new List<ErrorMessage>();
        }

        // Код заявки
        [SendToServer(false)]
        [CSN("RequestCode")]
        public String RequestCode { get; set; }
        // Код регистрационной формы
        [SendToServer(false)]
        [CSN("RegistrationFormCode")]
        public String RegistrationFormCode { get; set; }
        [CSN("RequestForm")]
        public ObjectRef RequestForm { get; set; }

        // Источник заявки
        [CSN("HospitalCode")]
        public String HospitalCode { get; set; }
        [CSN("HospitalName")]
        public String HospitalName { get; set; }
        [SendToServer(false)]
        [CSN("DepartmentCode")]
        public String DepartmentCode { get; set; }
        [SendToServer(false)]
        [CSN("DepartmentName")]
        public String DepartmentName { get; set; }
        [CSN("DoctorCode")]
        public String DoctorCode { get; set; }
        [CSN("DoctorName")]
        public String DoctorName { get; set; }

        // Дата взятия образцов
        [CSN("SamplingDate")]
        public DateTime SamplingDate { get; set; }
        // Дата доставки биоматериала в лабораторию 
        [CSN("SampleDeliveryDate")]
        public DateTime? SampleDeliveryDate { get; set; }

        // Состояние пациента    
        [CSN("PregnancyDuration")]
        public Int32? PregnancyDuration { get; set; }
        [CSN("CyclePeriod")]
        public Int32? CyclePeriod { get; set; }

        // Запрет на редактирование и исполнение
        [CSN("Readonly")]
        public Boolean Readonly { get; set; }

        // Пациент   
        [CSN("Patient")]
        public ExternalRequestPatient Patient { get; set; }

        // Список проб
        [CSN("Samples")]
        public List<ExternalRequestSample> Samples { get; set; }

        // Пользовательские поля. Свойство необходимо для считывания данных из внешней системы
        [SendToServer(false)]
        [CSN("UserFields")]
        public List<UserValue> UserFields
        {
            get { return UserValues; }
            set { UserValues = value; }
        }

        // Пользовательские поля. Свойство необходимо для отправки серверу ЛИС
        [XmlIgnore]
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }

        // Список ошибок, возникших при сохранении заявки сервером ЛИС. Используется только при помещении файла заявки в папку с ошибками
        [SendToServer(false)]
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
        // Список предупреждений, возникших при сохранении заявки сервером ЛИС. Используется только при помещении файла заявки в папку с ошибками
        [SendToServer(false)]
        [CSN("Warnings")]
        public List<ErrorMessage> Warnings { get; set; }

        // Свойства, отображённые на свойства заявки, необходимые для отправки серверу ЛИС
        [XmlIgnore]
        [CSN("InternalNr")]
        public String InternalNr
        {
            get { return RequestCode; }
        }
        [XmlIgnore]
        [CSN("CustDepartmentCode")]
        public String CustDepartmentCode
        {
            get { return DepartmentCode; }
        }
        [XmlIgnore]
        [CSN("CustDepartmentName")]
        public String CustDepartmentName
        {
            get { return DepartmentName; }
        }
        // Приоритет(срочность) выполнения заявки
        [XmlIgnore]
        [CSN("Priority")]
        public int Priority { get; set; }

        // Дополнительная информация
        [SendToServer(false)]
        [XmlIgnore]
        [CSN("ExtraInfo")]
        public Object ExtraInfo { get; set; }

        public BaseRequest ToBaseRequest()
        {
            BaseRequest result = new BaseRequest();

            // Формируем данные о пациенте для сохранения в ЛИС
            Patient patientData = new Patient();

            patientData.Code = result.Code = (!String.IsNullOrEmpty(this.Patient.Code)) ? this.Patient.Code.ToUpper() : null;
            patientData.FirstName = result.FirstName = this.Patient.FirstName;
            patientData.MiddleName = result.MiddleName = this.Patient.MiddleName;
            patientData.LastName = result.LastName = this.Patient.LastName;

            // Кроме инициализации объекта "patientData" копируем значения одноимённых свойств в заявку
            patientData.BirthDay = result.BirthDay = this.Patient.BirthDay;
            patientData.BirthMonth = result.BirthMonth = this.Patient.BirthMonth;
            patientData.BirthYear = result.BirthYear = this.Patient.BirthYear;


            patientData.BirthDay = result.BirthDay = this.Patient.BirthDay;
            patientData.BirthMonth = result.BirthMonth = this.Patient.BirthMonth;
            patientData.BirthYear = result.BirthYear = this.Patient.BirthYear;
            patientData.Sex = result.Sex = this.Patient.Sex;
            patientData.Country = result.Country = this.Patient.Country;
            patientData.City = result.City = this.Patient.City;
            patientData.Street = result.Street = this.Patient.Street;
            patientData.Building = result.Building = this.Patient.Building;
            patientData.Flat = result.Flat = this.Patient.Flat;
            patientData.InsuranceCompany = result.InsuranceCompany = this.Patient.InsuranceCompany;
            patientData.PolicySeries = result.PolicySeries = this.Patient.PolicySeries;
            patientData.PolicyNumber = result.PolicyNumber = this.Patient.PolicyNumber;
            // Формируем данные о карте пациента для сохранения в ЛИС
            PatientCard patientCard = this.Patient.PatientCard.ToPatientCard();
            patientData.PatientCards.Add(patientCard);
            patientData.UserValues.AddRange(this.Patient.UserValues);

            // Cохраняем/изменяем пациента и его карту, проставляем ссылку на них в заявке
            List<PatientCardId> cardIds;
            // Параметр отключает проверку уникальности кода пациента при неуказанном id. Необходим для автоматического обновления данных пациента и его карты из обменной службы
            // Если параметр не присылается (например из клиента), то сервер вернёт ошибку "LIS-814 Код пациента должен быть уникальным"
            patientData.FindByCode = true;
            Int32 Id = ProgramContext.LisCommunicator.SavePatient(patientData, out cardIds, null);
            result.Patient = new ObjectRef(Id);
            PatientCardId cardId = cardIds.Find(cid => cid.CardNr == this.Patient.PatientCard.CardNr);
            if (cardId != null)
                result.PatientCard = new ObjectRef(cardId.Id);

            String externalSystemCode = (String)ProgramContext.Settings["externalSystemCode"];
            var externalSystem = ProgramContext.Dictionaries[LimsDictionaryNames.ExternalSystem, externalSystemCode] as ExternalSystemDictionaryItem;
            if (externalSystem == null)
            {
                throw new ApplicationException("{3D312213-C539-43DD-AEB6-767CF5029B23}. ExternalSystem not defined");
            }
            result.ExternalSystem = new ExternalSystemDictionaryItem();
            result.ExternalSystem.Id = externalSystem.Id;
            result.Id = this.Id;
            result.InternalNr = this.RequestCode;
            result.Priority = this.Priority;
            result.Delivered = false;
            var requestForm = ProgramContext.Dictionaries[LimsDictionaryNames.RequestForm, this.RegistrationFormCode] as RequestFormDictionaryItem;
            if (requestForm != null)
                result.RequestForm = requestForm;
            else
                throw new ApplicationException(String.Format("Регистрационная форма не найдена по коду [{0}]. Невозможно продолжить создание заявки", RegistrationFormCode));
            result.Printed = false;
            result.OriginalSent = false;
            result.CopySent = false;
            result.SamplingDate = this.SamplingDate;
            result.SampleDeliveryDate = this.SampleDeliveryDate;
            result.PregnancyDuration = this.PregnancyDuration;
            result.CyclePeriod = this.CyclePeriod;

            result.CustHospitalCode = this.HospitalCode;
            result.CustHospitalName = this.HospitalName;
            result.CustDepartmentCode = this.DepartmentCode;
            result.CustDepartmentName = this.DepartmentName;
            result.CustDoctorCode = this.DoctorCode;
            result.CustDoctorName = this.DoctorName;

            // Заполняем список ID исследований по кодам
            foreach (ExternalRequestSample sample in this.Samples)
            {
                if ((sample.Biomaterial != null) && (sample.Biomaterial.Id > 0) && !result.Biomaterials.Exists(b => b.Id == sample.Biomaterial.Id))
                {
                    var bm = ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial, sample.Biomaterial.Id] as BiomaterialDictionaryItem;
                    result.Biomaterials.Add(bm);
                }

                foreach (ExternalRequestTarget target in sample.Targets)
                {
                    var trg = ProgramContext.Dictionaries[LimsDictionaryNames.Target, target.Code] as TargetDictionaryItem;
                    if (trg != null)
                        result.Targets.Add(trg);
                }
            }

            // Копируем все пользовательские поля в заявку
            result.UserValues.AddRange(this.UserValues);
            result.UserValues.AddRange(this.Patient.UserValues);
            result.UserValues.AddRange(this.Patient.PatientCard.UserValues);

            return result;
        }
    }

    public class ExternalRequestCheckException : ApplicationException { }

    public class ExternalRequestResponce
    {
        public ExternalRequestResponce()
        {
            Errors = new List<ErrorMessage>();
        }
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }
}