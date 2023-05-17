using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourceWork : BaseObject
    {
        public OutsourceWork()
        {
            Norm = new OutsourceNorm();
            Defects = new List<OutsourceDefectInfo>();
            Work = new ObjectRef();
            Test = new ObjectRef();        
        }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("UnitName")]
        public String UnitName { get; set; }
        [CSN("State")]
        public String State { get; set; }
        [CSN("ApprovingDoctor")]
        public String ApprovingDoctor { get; set; }
        [CSN("CreateDate")]
        public DateTime? CreateDate { get; set; }
        [CSN("ApproveDate")]
        public DateTime? ApproveDate { get; set; }
        [CSN("ModifyDate")]
        public DateTime? ModifyDate { get; set; }
        [CSN("ResultDate")]
        public DateTime? ResultDate { get; set; }
        [CSN("SendDate")]
        public DateTime? SendDate { get; set; }
        [CSN("Comments")]
        public String Comments { get; set; }
        [CSN("Cancel")]
        public Boolean? Cancel { get; set; }
        [CSN("Norm")]
        public OutsourceNorm Norm { get; set; }
        [CSN("Defects")]
        public List<OutsourceDefectInfo> Defects { get; set; }
        [CSN("Work")]
        public ObjectRef Work { get; set; }
        [CSN("Test")]
        public ObjectRef Test { get; set; }
    }
}
