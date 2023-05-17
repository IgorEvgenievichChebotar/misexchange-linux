using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ru.novolabs.ExchangeDTOs
{
    /// <summary>
    /// Класс описывает заказ на выполнение исследований для одного пациента
    /// </summary>
    //[Table("Request")]
    public class Request
    {
        public Request()
        {
           // Patient = new Patient();
            Samples = new List<Sample>();
            UserFields = new List<UserField>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код (номер) заявки
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 20)] // max длина поля увеличена до 20 символов в соответствии с задачей [LIS-9371]
        public String RequestCode { get; set; }
        // Свойство убрано из класса, в дальнейшем будет использоваться код регистрационной формы, взятый из настроек обменной службы
    /*    /// <summary>
        /// Код регистрационной формы
        /// </summary>
        public String RegistrationFormCode { get; set; }*/
        /// <summary>
        /// Код ЛПУ, создавшего заявку
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 128)]
        public String HospitalCode { get; set; }
        /// <summary>
        /// Название ЛПУ, создавшего заявку
        /// </summary>
        [StringLength(1024)]
        [FieldProps(maxLength: 128)]
        public String HospitalName { get; set; }
        /// <summary>
        /// Код отделения ЛПУ, создавшего заявку
        /// </summary>
        [StringLength(255)]
        [FieldProps(maxLength: 128)]
        public String DepartmentCode { get; set; }
        /// <summary>
        /// Название отделения ЛПУ, создавшего заявку
        /// </summary>
        [StringLength(1024)]
        [FieldProps(maxLength: 128)]
        public String DepartmentName { get; set; }
        /// <summary>
        /// Код врача, назначившего исследования
        /// </summary>
        [StringLength(255)]
        [FieldProps(maxLength: 128)]
        public String DoctorCode { get; set; }
        /// <summary>
        /// Имя врача, назначившего исследования
        /// </summary>
        [StringLength(1024)]
        [FieldProps(maxLength: 128)]
        public String DoctorName { get; set; }
        /// <summary>
        /// Дата взятия биоматериала
        /// </summary>
        [FieldProps()]
        [Column(TypeName="datetime")]
        public DateTime? SamplingDate { get; set; }
        /// <summary>
        /// Дата доставки биоматериала в лабораторию
        /// </summary>
        [FieldProps()]
        [Column(TypeName = "datetime")]
        public DateTime? SampleDeliveryDate { get; set; }
        /// <summary>
        /// Только для женщин. Продолжительность беременности
        /// </summary>
        [FieldProps()]
        public Int32? PregnancyDuration { get; set; }
        /// <summary>
        /// Только для женщин. Фаза менструального цикла
        /// </summary>
        [FieldProps()]
        public Int32? CyclePeriod { get; set; }
        /// <summary>
        /// Запрет на редактирование и исполнение
        /// </summary>
        [FieldProps()]
        public Boolean? ReadOnly { get; set; }
        /// <summary>
        /// Информация о пациенте
        /// </summary>
         [FieldProps(mandatory: true)]
        public Patient Patient { get; set; }
        /// <summary>
        /// Список проб
        /// </summary>
       // [FieldProps(mandatory: true)]
        [FieldProps()]
        public List<Sample> Samples { get; set; }
        /// <summary>
        /// Пользовательские поля
        /// </summary>
        [FieldProps()]
        public List<UserField> UserFields { get; set; }
        /// <summary>
        /// Срочность Cito
        /// </summary>
        [FieldProps()]
        public Int32? Priority { get; set; }
        /// <summary>
        /// Код скидки
        /// </summary>
        [StringLength(255)]
        [FieldProps(maxLength: 16)]
        public String PayCategoryCode { get; set; }
        /// <summary>
        /// e-mail
        /// </summary>
        [StringLength(1024)]
        [FieldProps(maxLength: 128)]
        public String Email { get; set; }
        /// <summary>
        /// Статус заявки
        /// </summary>
      //  [FieldProps(maxLength: 128)]
      //  public Int32 State { get; set; }
        /// <summary>
        /// Код учреждения (филиала учреждения)
        /// </summary>
        [StringLength(255)]
        [FieldProps(maxLength: 128)]
        public String OrganizationCode { get; set; }
        /// <summary>
        /// Код учреждения исполнителя(филиала учреждения)
        /// </summary>
        [StringLength(255)]
        [FieldProps(maxLength: 128)]
        public String ExecutorOrganizationCode { get; set; }
        /// <summary>
        /// Дополнительная информация для кэширования
        /// </summary>
        [XmlIgnore]
        public string ExtraData { get; set; }
        /// <summary>
        /// Номер телефона пациента
        /// </summary>
        [StringLength(20)]
        [FieldProps(maxLength: 20)]
        public String Telephone { get; set; }
        /// <summary>
        /// Номер амбулаторной карты пациента
        /// </summary>
        [StringLength(128)]
        [FieldProps(maxLength: 128)]
        public String CardAmbulatory { get; set; }
        /// <summary>
        /// Номер стационарной карты пациента
        /// </summary>
        [StringLength(128)]
        [FieldProps(maxLength: 128)]
        public String CardStationary { get; set; }
        /// <summary>
        /// Номер карты пациента типа "дополнительная тип 1"
        /// </summary>
        [StringLength(128)]
        [FieldProps(maxLength: 128)]
        public String CardExtraType1 { get; set; }
        /// <summary>
        /// Опекун пациента
        /// </summary>
        [StringLength(1024)]
        [FieldProps(mandatory: false, maxLength: 1024)]
        public String Guardian { get; set; }
        /// <summary>
        /// идентификатор заказа в Яндекс
        /// </summary>
        [StringLength(48)]
        [FieldProps(mandatory: false, maxLength: 48)]
        public String Yandex_OrderId { get; set; }
        /// <summary>
        /// идентификатор сумки/бригады в Яндекс
        /// </summary>
        [StringLength(48)]
        [FieldProps(mandatory: false, maxLength: 48)]
        public String Yandex_BucketId { get; set; }
    }
}
