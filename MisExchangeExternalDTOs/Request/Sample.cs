using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    /// <summary>
    /// Класс описывает заказ на выполнение набора исследований для одной пробы
    /// </summary>
    public class Sample
    {
        public Sample()
        {
            Targets = new List<Target>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Штриход пробирки
        /// </summary>
        [StringLength(255)]
        [FieldProps(maxLength: 16)]
        public String Barcode { get; set; }
        /// <summary>
        /// Код биоматериала
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 128)]
        public String BiomaterialCode { get; set; }
        /// <summary>
        /// Приоритет исполнения. (0 - обычный, 1 - срочно)
        /// </summary>
        [FieldProps(minValue: 0, maxValue: 1)]
        public Int32? Priority { get; set; }
        /// <summary>
        /// Список исследований
        /// </summary>
        [FieldProps(mandatory: true)]
        public List<Target> Targets { get; set; }
    }
}