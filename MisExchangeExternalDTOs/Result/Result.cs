using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    /// <summary>
    /// Класс описывает результаты выполнения исследований по заявке
    /// </summary>
    public class Result
    {
        public Result()
        {
            SampleResults = new List<SampleResult>();
            UserFields = new List<UserField>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код (номер) заявки 
        /// </summary>
        [StringLength(255)]
        public string RequestCode { get; set; }
        /// <summary>
        /// Информация о пациенте
        /// </summary>
        public Patient Patient { get; set; }
        /// <summary>
        /// Результаты исследований для образцов
        /// </summary>
        public List<SampleResult> SampleResults { get; set; }
        /// <summary>
        /// Пользовательские поля
        /// </summary>
        public List<UserField> UserFields { get; set; }
        /// <summary>
        /// Дата регистрации заявки
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? RegistrationDate { get; set; }
        /// <summary>
        /// Дата доставки
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? SampleDeliveryDate { get; set; }
        /// <summary>
        /// Код заказчика
        /// </summary>
        [StringLength(255)]
        public string HospitalCode { get; set; }
        /// <summary>
        /// Имя заказчика
        /// </summary>
        [StringLength(1024)]
        public string HospitalName { get; set; }
        /// <summary>
        /// Код категории оплаты
        /// </summary>
        [StringLength(255)]
        public string PayCategoryCode { get; set; }
        /// <summary>
        /// Именование категории оплаты
        /// </summary>
        [StringLength(1024)]
        public string PayCategoryName { get; set; }
        /// <summary>
        /// Состояние заявки: 1 - Регистрация, 2 - Открыта, 3 - Закрыта, 4 - Удалена
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// Дополнительная информация для кэширования
        /// </summary>
        [XmlIgnore]
        public string ExtraData { get; set; }
        /// <summary>
        /// Код учреждения (филиала лаборатории), от имени которого была создана заявка
        /// </summary>
        [StringLength(255)]
        public string OrganizationCode { get; set; }
        /// <summary>
        /// Название учреждения (филиала лаборатории), от имени которого была создана заявка
        /// </summary>
        [StringLength(1024)]
        public string OrganizationName { get; set; }
        /// <summary>
        /// ОГРН лаборатории
        /// </summary>
        [StringLength(1024)]
        public string Ogrn { get; set; }
        /// <summary>
        /// e-mail пациента
        /// </summary>
        [StringLength(1024)]
        public string Email { get; set; }
        /// <summary>
        /// Номер телефона пациента
        /// </summary>
        [StringLength(20)]
        public string Telephone { get; set; }
        /// <summary>
        /// Код отделения заказчика 
        /// </summary>
        [StringLength(255)]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// Название отделения заказчика
        /// </summary>
        [StringLength(1024)]
        public string DepartmentName { get; set; }
        /// <summary>
        /// Код врача
        /// </summary>
        [StringLength(255)]
        public string DoctorCode { get; set; }
        /// <summary>
        /// ФИО врача
        /// </summary>
        [StringLength(1024)]
        public string DoctorName { get; set; }
        /// <summary>
        /// Дата взятия проб
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? SamplingDate { get; set; }
        /// <summary>
        /// Срочность Cito
        /// </summary>
        public int? Priority { get; set; }
        /// <summary>
        /// Код юридического лица
        /// </summary>
        public string LegalCode { get; set; }
        /// <summary>
        /// Код учреждения-исполнителя заявки
        /// </summary>
        [StringLength(255)]
        public string ExecutorOrganizationCode { get; set; }
        /// <summary>
        /// Название учреждения-исполнителя заявки
        /// </summary>
        [StringLength(1024)]
        public string ExecutorOrganizationName { get; set; }
        /// <summary>
        /// Номер амбулаторной карты пациента
        /// </summary>
        [StringLength(255)]
        public string CardAmbulatory { get; set; }
        /// <summary>
        /// Вес пациента в килограммах
        /// </summary>
        public float? PatientWeightInKg { get; set; }
    }

    public static class ResultExtension
    {
        public static bool HasDefects(this Result result)
        {
            return result.SampleResults.SelectMany(sr => sr.Defects).Any();
        }

        public static string GetAllDefectsText(this Result result)
        {
            return string.Join("; ", result.SampleResults.SelectMany(sr => sr.Defects).Select(d => d.Name));
        }
    }
}