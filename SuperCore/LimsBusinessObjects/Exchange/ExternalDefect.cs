using System;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает выявленные дефекты пробы
    /// </summary>
    [XmlType(TypeName = "Defect")]
    public class ExternalDefect
    {
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Name")]
        public String Name { get; set; }
    }
}