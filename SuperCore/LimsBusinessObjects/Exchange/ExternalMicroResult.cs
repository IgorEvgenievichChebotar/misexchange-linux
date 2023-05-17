using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает результат выполнения микробиологического иследования
    /// длы одного микроорганизма
    /// </summary>
    [XmlType(TypeName = "MicroResult")]
    public class ExternalMicroResult
    {
        public ExternalMicroResult()
        {
            MicroOrganism = new ObjectRef();
            Antibiotics = new List<ExternalWork>();
        }
        // Код и название обнаруженного микроорганизма
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Name")]
        public String Name { get; set; }

        // Выявленное значение
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("Comments")]
        public String Comments { get; set; }

        // Ссылка на микроорганизм
        [XmlIgnore]
        [CSN("MicroOrganism")]
        public ObjectRef MicroOrganism { get; set; }
        // Флаг выявленности микроорганизма
        [CSN("Found")]
        public Boolean? Found { get; set; }

        // Результаты исследований на устойчивость к антибиотикам
        [CSN("Antibiotics")]
        public List<ExternalWork> Antibiotics { get; set; }
        [CSN("ParentWork")]
        public ExternalWork ParentWork { get; set; }

        // Значение КОЕ для микроорганизма
        [CSN("ColonyFormingUnitValue")]
        public String ColonyFormingUnitValue { get; set; }
    }
}
