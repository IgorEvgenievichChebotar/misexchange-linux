using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    public class Test
    {
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код теста
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 128)]
        public String Code { get; set; }
    }
}
