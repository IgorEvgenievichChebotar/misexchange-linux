using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    /// <summary>
    /// Класс описывает результат выполнения микробиологического иследования
    /// длы одного микроорганизма
    /// </summary>
    public class MicroResult
    {
        public MicroResult()
        {
            Antibiotics = new List<Work>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код обнаруженного микроорганизма
        /// </summary>
        [StringLength(255)]
        public String Code { get; set; }
        /// <summary>
        /// Название обнаруженного микроорганизма
        /// </summary>
        [StringLength(1024)]
        public String Name { get; set; }
        /// <summary>
        /// Выявленное значение
        /// </summary>
        [StringLength(1024)]
        public String Value { get; set; }
        /// <summary>
        /// Коментарии
        /// </summary>
        public String Comments { get; set; }
        /// <summary>
        /// Результаты исследований на устойчивость к антибиотикам
        /// </summary>
        public List<Work> Antibiotics { get; set; }
        /// <summary>
        /// Был ли найден микроорганизм
        /// </summary>
        public bool? Found { get; set; }
        /// <summary>
        /// Ссылка на работу, породившую данный микроорганизм
        /// </summary>
        public Work ParentWork { get; set; }

    }
}