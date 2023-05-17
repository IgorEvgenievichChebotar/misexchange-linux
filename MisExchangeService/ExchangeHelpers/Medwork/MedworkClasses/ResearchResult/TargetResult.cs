using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
{
    /// <summary>
    /// Класс описывает результат выполнения одного сложного исследоания
    /// состоящего из набора простых тестов
    /// </summary>
    public class TargetResult
    {
        public TargetResult()
        {
            Works = new List<Work>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        [StringLength(255)]
        public String Code { get; set; }
        [StringLength(1024)]
        public String Name { get; set; }
        public String Comments { get; set; }
        [StringLength(1024)]
        public String GroupDescription { get; set; }
        [StringLength(1024)]
        public String LabName { get; set; }
        public List<Work> Works { get; set; }
    }
}