using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
{
    /// <summary>
    /// Класс описывает выявленные дефекты пробы
    /// </summary>
    public class Defect
    {
        [XmlIgnore]
        public long Id { get; set; }
        [StringLength(255)]
        public String Code { get; set; }
        [StringLength(1024)]
        public String Name { get; set; }
    }
}