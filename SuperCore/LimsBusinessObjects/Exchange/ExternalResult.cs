using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает результаты выполнения исследований по заявке
    /// </summary>
    [XmlType(TypeName = "Result")]
    public class ExternalResult : BaseObject
    {
        public ExternalResult()
        {
            SampleResults = new List<ExternalSampleResult>();
            Patient = new ExternalRequestPatient();
            UserValues = new List<UserValue>();
        }

        /// <summary>
        /// Номер заявки 
        /// </summary>
        [CSN("RequestCode")]
        public String RequestCode { get; set; }
        /// <summary>
        /// Дата регистрации заявки
        /// </summary>
        [CSN("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// Дата доставки биоматериала в лабораторию
        /// </summary>
        [CSN("SampleDeliveryDate")]
        public DateTime SampleDeliveryDate { get; set; }
        [CSN("HospitalCode")]
        public String HospitalCode { get; set; }
        [CSN("HospitalName")]
        public String HospitalName { get; set; }

        // Данные о пациенте
        [ClonePropName("Patient")]
        [CSN("Patient")]
        public ExternalRequestPatient Patient { get; set; }

        /// <summary>
        /// Результаты исследований для образцов
        /// </summary>

        [ClonePropName("Samples")]
        [CSN("SampleResults")]
        public List<ExternalSampleResult> SampleResults { get; set; }

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

        [XmlIgnore]
        [CSN("InternalNr")]
        public String InternalNr
        {
            get { return RequestCode; }
            set { RequestCode = value; }
        }

        // Список ошибок
        [SendToServer(false)]
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }

        /// <summary>
        /// Состояние 
        /// </summary>
        [CSN("State")]
        public Int32 State { get; set; }

        /// <summary>
        /// Код учреждения (филиала лаборатории), в котором выполнялась заявка
        /// </summary>
        [CSN("OrganizationCode")]
        public String OrganizationCode { get; set; }
        /// <summary>
        /// Название учреждения (филиала лаборатории), в котором выполнялась заявка
        /// </summary>
        [CSN("OrganizationName")]
        public String OrganizationName { get; set; }
        /// <summary>
        /// ОГРН учреждения
        /// </summary>
        [CSN("Ogrn")]
        public String Ogrn { get; set; }
        /// <summary>
        /// e-mail пациента
        /// </summary>
        [CSN("Email")]
        public String Email { get; set; }
        /// <summary>
        /// Номер телефона пациента
        /// </summary>
        [CSN("Telephone")]
        public String Telephone { get; set; }

        /// <summary>
        /// Дата взятия пробы
        /// </summary>
        [CSN("SamplingDate")]
        public DateTime SamplingDate { get; set; }
        [CSN("DepartmentCode")]
        public String DepartmentCode { get; set; }
        [CSN("DepartmentName")]
        public String DepartmentName { get; set; }
        [CSN("DoctorCode")]
        public String DoctorCode { get; set; }
        [CSN("DoctorName")]
        public String DoctorName { get; set; }
        [CSN("PayCategoryCode")]
        public String PayCategoryCode { get; set; }
        [CSN("PayCategoryName")]
        public String PayCategoryName { get; set; }

        /// <summary>
        /// Срочность Cito
        /// </summary>
        [CSN("Priority")]
        public Int32 Priority { get; set; }
        /// <summary>
        /// Код юридического лица
        /// </summary>
        [CSN("LegalCode")]
        public string LegalCode { get; set; }
        /// <summary>
        /// Код учреждения-исполнителя заявки
        /// </summary>
        [CSN("ExecutorOrganizationCode")]
        public String ExecutorOrganizationCode { get; set; }
        /// <summary>
        /// Название учреждения-исполнителя заявки
        /// </summary>
        [CSN("ExecutorOrganizationName")]
        public String ExecutorOrganizationName { get; set; }
        /// <summary>
        /// Номер амбулаторной карты пациента
        /// </summary>
        [CSN("CardAmbulatory")]
        public String CardAmbulatory { get; set; }
        /// <summary>
        /// Вес пациента в килограммах
        /// </summary>
        public float? PatientWeightInKg { get; set; }
    }

    public class GetRequestResultsParams
    {
        public GetRequestResultsParams()
        {
            Requests = new List<ObjectRef>();
        }

        [CSN("Requests")]
        public List<ObjectRef> Requests { get; set; }
    }

    public class GetRequestResultsResponce
    {
        public GetRequestResultsResponce()
        {
            Results = new List<ExternalResult>();
        }

        [Unnamed]
        [CSN("Results")]
        public List<ExternalResult> Results { get; private set; }
    }
}