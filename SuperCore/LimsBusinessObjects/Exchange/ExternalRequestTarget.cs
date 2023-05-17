using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает заказ на выполнение одного сложного исследования
    /// </summary>

    [XmlType(TypeName = "Target")]
    public class ExternalRequestTarget
    {
        public ExternalRequestTarget()
        {
            Tests = new List<ObjectRef>();
        }
        // Id Исследования
        [CSN("Target")]
        public ObjectRef Target { get; set; }
        // Код исследования
        [SendToServer(false)]
        [CSN("Code")]
        public String Code { get; set; }
        // Признак отмены исследования
        [CSN("Cancel")]
        public Boolean Cancel { get; set; }
        // Приоритет исполнения
        [SendToServer(false)]
        [CSN("Priority")]
        public Int32 Priority { get; set; }
        // Запрет на редактирование и исполнение
        [SendToServer(false)]
        [CSN("ReadOnly")]
        public Boolean ReadOnly { get; set; }
        [CSN("Tests")]
        public List<ObjectRef> Tests { get; set; }
    }
}
