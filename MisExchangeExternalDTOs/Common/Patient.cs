using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    /// <summary>
    /// Класс описывает совокупность данных о пациенте
    /// </summary>       
    public class Patient
    {
        public Patient()
        {
            // PatientCard = new PatientCard();
            UserFields = new List<UserField>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Уникальный код (идентификатор)
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 20)]
        public string Code { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 64)]
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 64)]
        public string LastName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 64)]
        public string MiddleName { get; set; }
        /// <summary>
        /// День рождения
        /// </summary>
        [FieldProps(minValue: 1, maxValue: 31)]
        public Nullable<Int32> BirthDay { get; set; }
        /// <summary>
        /// Месяц рождения
        /// </summary>
        [FieldProps(minValue: 1, maxValue: 12)]
        public Nullable<Int32> BirthMonth { get; set; }
        /// <summary>
        /// Год рождения
        /// </summary>
        [FieldProps(minValue: 1890)]
        public Nullable<Int32> BirthYear { get; set; }
        /// <summary>
        /// Пол (0 - не указан, 1 - мужской, 2 - женский)
        /// </summary>
        [FieldProps(mandatory: true, minValue: 0, maxValue: 2)]
        public int Sex { get; set; }
        /// <summary>
        /// Страна проживания
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Country { get; set; }
        /// <summary>
        /// Город проживания
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string City { get; set; }
        /// <summary>
        /// Улица проживания
        /// </summary>
        [StringLength(1024)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Street { get; set; }
        /// <summary>
        /// Номер дома
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Building { get; set; }
        /// <summary>
        /// Номер квартиры
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Flat { get; set; }
        /// <summary>
        /// Название страховой компании
        /// </summary>
        [StringLength(1024)]
        [FieldProps(mandatory: false, maxLength: 1024)]
        public string InsuranceCompany { get; set; }
        /// <summary>
        /// Серия страхового полиса
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string PolicySeries { get; set; }
        /// <summary>
        /// Номер страхового полиса
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string PolicyNumber { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Telephone { get; set; }

        /// <summary>
        /// Карта пациента, по которой создаётся заявка
        /// </summary>
        [FieldProps()]
        public PatientCard PatientCard { get; set; }
        /// <summary>
        /// Пользовательские поля
        /// </summary>
        [FieldProps()]
        public List<UserField> UserFields { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Region { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Area { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Location { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressCountry { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressRegion { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressArea { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressCity { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressLocation { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressStreet { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressBuilding { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string LivingAddressFlat { get; set; }

        [StringLength(255)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public string Email { get; set; }
    }
}
