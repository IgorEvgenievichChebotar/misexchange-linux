using ru.novolabs.SuperCore.DictionaryCommon;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает совокупность данных о пациенте
    /// </summary>       
    public class ExternalRequestPatient
    {
        public ExternalRequestPatient()
        {
            PatientCard = new ExternalRequestPatientCard();
            UserValues = new List<UserValue>();
        }
        /// <summary>
        /// Код пациента
        /// </summary>
        [CSN("Code")]
        public string Code { get; set; }
        [CSN("FirstName")]
        // Персональные данные пациента
        public string FirstName { get; set; }
        [CSN("LastName")]
        public string LastName { get; set; }
        [CSN("MiddleName")]
        public string MiddleName { get; set; }
        [CSN("BirthDay")]
        public int? BirthDay { get; set; }
        [CSN("BirthMonth")]
        public int? BirthMonth { get; set; }
        [CSN("BirthYear")]
        public int? BirthYear { get; set; }
        [CSN("Sex")]
        [SendAsInt(true)]
        public SexDictionaryItem Sex { get; set; }
        // Адрес пациента
        [CSN("Country")]
        public string Country { get; set; }
        [CSN("Region")]
        public string Region { get; set; }
        [CSN("Area")]
        public string Area { get; set; }
        [CSN("City")]
        public string City { get; set; }
        [CSN("Location")]
        public string Location { get; set; }
        [CSN("Street")]
        public string Street { get; set; }
        [CSN("Building")]
        public string Building { get; set; }
        [CSN("Flat")]
        public string Flat { get; set; }
        [CSN("LivingAddressCountry")]
        public string LivingAddressCountry { get; set; }
        [CSN("LivingAddressRegion")]
        public string LivingAddressRegion { get; set; }
        [CSN("LivingAddressArea")]
        public string LivingAddressArea { get; set; }
        [CSN("LivingAddressCity")]
        public string LivingAddressCity { get; set; }
        [CSN("LivingAddressLocation")]
        public string LivingAddressLocation { get; set; }
        [CSN("LivingAddressStreet")]
        public string LivingAddressStreet { get; set; }
        [CSN("LivingAddressBuilding")]
        public string LivingAddressBuilding { get; set; }
        [CSN("LivingAddressFlat")]
        public string LivingAddressFlat { get; set; }
        //  Данные о страховке
        [CSN("InsuranceCompany")]
        public string InsuranceCompany { get; set; }
        [CSN("PolicySeries")]
        public string PolicySeries { get; set; }
        [CSN("PolicyNumber")]
        public string PolicyNumber { get; set; }
        // Карта пациента, по которой создаётся заявка
        [CSN("PatientCard")]
        public ExternalRequestPatientCard PatientCard { get; set; }
        // Пользовательские поля. Свойство необходимо для считывания данных из внешней системы
        [SendToServer(false)]
        [CSN("UserFields")]
        public List<UserValue> UserFields
        {
            get => UserValues;
            set => UserValues = value;
        }

        // Пользовательские поля. Свойство необходимо для отправки серверу ЛИС
        [XmlIgnore]
        [CSN("UserValues")]
        public List<UserValue> UserValues { get; set; }
    }
}
