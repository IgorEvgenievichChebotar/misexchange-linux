using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
{
    public class UserField
    {
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Код пользовательского поля
        /// </summary>
        [StringLength(255)]
        [FieldProps(mandatory: true, maxLength: 128)]
        public String Code { get; set; }
        /// <summary>
        /// Название пользовательского поля
        /// </summary>
        [StringLength(1024)]
        [FieldProps(mandatory: false, maxLength: 128)]
        public String Name { get; set; }
        /// <summary>
        /// Значение пользовательского поля
        /// </summary>
        [StringLength(2048)]
        [FieldProps(mandatory: true, maxLength: 1024)]
        public String Value { get; set; }
    }
}