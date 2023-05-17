using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
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
        public string Code { get; set; }
        [StringLength(1024)]
        public string Name { get; set; }
        public string Comments { get; set; }
        public List<Work> Works { get; set; }
    }
}