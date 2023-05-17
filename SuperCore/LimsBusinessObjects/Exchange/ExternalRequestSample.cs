using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает заказ на выполнение набора исследований из одной пробы
    /// </summary>
    [XmlType(TypeName = "Sample")]
    public class ExternalRequestSample : BaseObject
    {
        public ExternalRequestSample()
        {
            Targets = new List<ExternalRequestTarget>();
            Priority = ExternalPriorities.PRIORITY_LOW;
            Barcode = String.Empty;
        }
        // Код биоматериала (опционально)
        [XmlIgnore]
        [CSN("Biomaterial")]
        public ObjectRef Biomaterial { get; set; } // Ссылка на элемент справочника "Биоматериалы"
        // Код биоматериала (опционально)
        [SendToServer(false)]
        [CSN("BiomaterialCode")]
        public String BiomaterialCode { get; set; }

        // Штриход пробирки
        [SendToServer(false)]
        [CSN("Barcode")]
        public String Barcode { get; set; }

        // Приоритет исполнения
        [SendToServer(false)]
        [CSN("Priority")]
        public Int32 Priority { get; set; }

        // Список исследований
        [CSN("Targets")]
        public List<ExternalRequestTarget> Targets { get; set; }

        // Свойства, отображённые на свойства пробы, необходимые для отправки серверу ЛИС
        [XmlIgnore]
        [CSN("internalNr")]
        public String internalNr { get { return Barcode; } }
    }
}
