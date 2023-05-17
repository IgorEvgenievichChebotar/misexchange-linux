using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    [XmlType(TypeName = "Target")]
    public class OutsourceTarget : BaseObject
    {
        public OutsourceTarget()
        {
            Works = new List<OutsourceWork>();
            RequiredTests = new List<ObjectRef>();
            MicrobiologyWorks = new List<OutsourceMicrobiologyWork>();
        }

        [CSN("Cancel")]
        public Boolean Cancel { get; set; }
        [CSN("Priority")]
        public Int32 Priority { get; set; }
        [CSN("ReadOnly")]
        public Boolean ReadOnly { get; set; }
        [CSN("Comments")]
        public String Comments { get; set; }
        [CSN("Works")]
        public List<OutsourceWork> Works { get; set; }
        [SendToServer(false)]
        [CSN("RequiredTests")]
        public List<ObjectRef> RequiredTests { get; set; }
        [CSN("Target")]
        public ObjectRef Target { get; set; }
        [CSN("MicrobiologyWorks")]
        public List<OutsourceMicrobiologyWork> MicrobiologyWorks { get; set; }

        // Дополнительные поля
        [SendToServer(false)]
        [CSN("Code")]
        public String Code { get; set; } // Свойство необходимо при преобразовании класса в класс ExternalRequestTarget

    }
}
