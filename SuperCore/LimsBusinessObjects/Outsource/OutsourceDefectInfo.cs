using System;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourceDefectInfo : BaseObject
    {
        public OutsourceDefectInfo() { }

        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Defect")]
        public ObjectRef Defect { get; set; }

        [SendToServer(false)]
        [CSN("DefectCode")]
        public String DefectCode { get; set; } // Свойство, необходимое для подстановки ссылки на defectMapping по коду, пришедшему от аутсорсера
    }
}