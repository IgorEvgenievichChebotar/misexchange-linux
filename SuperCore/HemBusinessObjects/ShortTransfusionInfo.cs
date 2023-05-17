using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    /// <summary>
    /// Объект, описывающий связь донора с реципиентом
    /// </summary>
    public class ShortTransfusionInfo
    {
        /// <summary>
        /// id трансфузии
        /// </summary>
        [CSN("Transfusion")]
        public Int32 Transfusion { get; set; }
        /// <summary>
        /// id донора
        /// </summary>
        [CSN("Donor")]
        public Int32 Donor { get; set; }
        /// <summary>
        /// id реципиента
        /// </summary>
        [CSN("Recipient")]
        public Int32 Recipient { get; set; }
        /// <summary>
        /// Дата переливания
        /// </summary>
        [CSN("Date")]
        public DateTime Date { get; set; }
        /// <summary>
        /// Имя донора
        /// </summary>
        [CSN("DonorFirstName")]
        public String DonorFirstName { get; set; }
        /// <summary>
        /// Фамилия донора
        /// </summary>
        [CSN("DonorLastName")]
        public String DonorLastName { get; set; }
        /// <summary>
        /// Отчество донора
        /// </summary>
        [CSN("DonorMiddleName")]
        public String DonorMiddleName { get; set; }
        /// <summary>
        /// Номер донора
        /// </summary>
        [CSN("DonorNr")]
        public String DonorNr { get; set; }
        /// <summary>
        /// Тип связи (BLOOD-153)
        /// </summary>
        [CSN("Type")]
        public Int32 Type { get; set; }
    }

    /// <summary>
    /// Запрос на историю донаций
    /// </summary>
    public class DonorTransfusionHistoryRequest
    {
        /// <summary>
        /// Ссылка на донора
        /// </summary>
        [CSN("Donor")]
        public ObjectRef Donor { get; set; }
        /// <summary>
        /// Тип связей
        /// </summary>
        [CSN("RelationTypes")]
        public List<Int32> RelationTypes { get; set; }
    }
}
