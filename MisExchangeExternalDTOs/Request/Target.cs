using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    public class Target
    {
        public Target()
        {
            Tests = new List<Test>();        
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код исследования
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 128)]
        public String Code { get; set; }
        /// <summary>
        /// Приоритет исполнения. (0 - обычный, 1 - срочно)
        /// </summary>
        [FieldProps()]
        public Int32? Priority { get; set; }
        /// <summary>
        /// Запрет на редактирование и исполнение
        /// </summary>
        [FieldProps()]
        public Boolean? ReadOnly { get; set; }
        /// <summary>
        /// Список уточняющих тестов
        /// </summary>
        [FieldProps()]
        public List<Test> Tests { get; set; }
    }
}
