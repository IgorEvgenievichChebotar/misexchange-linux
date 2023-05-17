using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    [XmlType(TypeName = "Sample")]
    public class OutsourceSample : BaseObject
    {
        public OutsourceSample()
        {
            Targets = new List<OutsourceTarget>();
            Defects = new List<OutsourceDefectInfo>();        
        }
        [CSN("Barcode")]     
        public String Barcode { get; set; }
        [CSN("Priority")]
        public Int32 Priority { get; set; }
        [CSN("Comments")]
        public String Comments { get; set; }
        [CSN("Biomaterial")]
        public ObjectRef Biomaterial { get; set; }
        [CSN("Targets")]
        public List<OutsourceTarget> Targets { get; set; }
        [CSN("Defects")]
        public List<OutsourceDefectInfo> Defects { get; set; }
                
        // Дополнительные поля
        [SendToServer(false)]
        [CSN("BiomaterialCode")]
        public String BiomaterialCode { get; set; } // Свойство необходимо при преобразовании класса в класс ExternalRequest

        [SendToServer(false)]
        [CSN("ContainerCode")]
        public String ContainerCode { get; set; } // Свойство необходимо при преобразовании класса в класс ExternalRequest

    }
}
