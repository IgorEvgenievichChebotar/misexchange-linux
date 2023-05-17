using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает карту пациента
    /// </summary>     
    public class ExternalRequestPatientCard
    {
        public ExternalRequestPatientCard()
        {
            UserValues = new List<UserValue>();
            PayCategory = new ObjectRef();
        }

        [CSN("CardNr")]
        public String CardNr { get; set; } // Номер карты
        [XmlIgnore]
        [CSN("CardSuffix")]
        public String CardSuffix { get; set; } // Cуффикс номера карты
        [XmlIgnore]
        [CSN("Kind")]
        public Int32 Kind { get; set; } // Тип карты: 1-СТАЦИОНАРНАЯ, 2-АМБУЛАТОРНАЯ, 3-АРХИВНАЯ
        [XmlIgnore]
        [CSN("ExternalId")]
        public String ExternalId { get; set; } // уникальный внешний идентификатор карты пациента
        [XmlIgnore]
        [CSN("PayCategory")]
        public ObjectRef PayCategory { get; set; } // Ссылка на справочник "Категории оплаты"
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

        public PatientCard ToPatientCard()
        {
            PatientCard result = new PatientCard();
            result.CardNr = this.CardNr;
            result.CardSuffix = this.CardSuffix;
            result.ExternalId = this.ExternalId;
            result.Kind = this.Kind;
            result.PayCategory = this.PayCategory;
            result.UserValues.AddRange(this.UserValues);

            return result;
        }
    }
}
