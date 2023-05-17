using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
{
    /// <summary>
    /// Класс описывает результат попадания в норму для одного простого исследования.
    /// Нормы вычисляются с учетом пола и возраста пациента а так же прочих параметров
    /// таких как фаза цикла, срок беременности и тд.
    /// </summary>
    public class Norm
    {
        public Norm()
        {
            Norms = "";
        }
        [XmlIgnore]
        public long Id { get; set; }
        public Double? LowLimit { get; set; }
        public Double? HighLimit { get; set; }
        public Double? CriticalLowLimit { get; set; }
        public Double? CriticalHighLimit { get; set; }
        [StringLength(2048)]
        public String Norms { get; set; }
        public String NormComment { get; set; }
        [StringLength(255)]
        public String UnitName { get; set; }
    }
}