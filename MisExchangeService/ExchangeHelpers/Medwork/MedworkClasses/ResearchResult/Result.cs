using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
{
    /// <summary>
    /// Класс описывает результаты выполнения исследований по заявке
    /// </summary>
    public class Result
    {
        public Result()
        {
            SampleResults = new List<SampleResult>();
            //Patient = new Patient();
            UserFields = new List<UserField>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код (номер) заявки 
        /// </summary>
        [StringLength(255)]
        public String RequestCode { get; set; }
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
        /// Дата доставки
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? SampleDeliveryDate { get; set; }
        /// <summary>
        /// Код заказчика
        /// </summary>
        [StringLength(255)]
        public String HospitalCode { get; set; }
        /// <summary>
        /// Имя заказчика
        /// </summary>
        [StringLength(1024)]
        public String HospitalName { get; set; }
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
        /// Код учреждения (филиала лаборатории), в котором выполнялась заявка
        /// </summary>
        [StringLength(255)]
        public String OrganizationCode { get; set; }
        /// <summary>
        /// Название учреждения (филиала лаборатории), в котором выполнялась заявка
        /// </summary>
        [StringLength(1024)]
        public String OrganizationName { get; set; }
    }
}