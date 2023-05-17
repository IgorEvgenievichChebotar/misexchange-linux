using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает результат выполнения одного сложного исследоания
    /// состоящего из набора простых тестов
    /// </summary>    
    [XmlType(TypeName = "TargetResult")]
    public class ExternalTargetResult
    {
        public ExternalTargetResult()
        {
            Works = new List<ExternalWork>();
            Parents = new List<ObjectRef>();
        }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Name")]
        public String Name { get; set; }

        // Результат выполнения простых тестов
        [CSN("Works")]
        public List<ExternalWork> Works { get; set; }

        [CSN("Comments")]
        public String Comments { get; set; }

        [CSN("Parents")]
        public List<ObjectRef> Parents { get; set; }
    }
}
