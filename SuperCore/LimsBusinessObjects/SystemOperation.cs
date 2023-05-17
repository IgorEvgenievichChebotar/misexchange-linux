using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class SystemSuboperation : BaseObject
    {
        [CSN("MaterialId")]
        public Int32 MaterialId { get; set; }
        [CSN("Qty")]
        public Int32 Qty { get; set; }
        [CSN("MeasureId")]
        public Int32 MeasureId { get; set; }
    }

    public class SystemOperation : BaseObject
    {
        public SystemOperation()
        {
            Suboperations = new List<SystemSuboperation>();
        }
        [CSN("OperationType")]
        public Int32 OperationType { get; set; }
        [CSN("DocumentNr")]
        public String DocumentNr { get; set; }
        [CSN("AuthorName")]
        public String AuthorName { get; set; }
        [CSN("OperationDate")]
        public DateTime OperationDate { get; set; }
        [CSN("Suboperations")]
        public List<SystemSuboperation> Suboperations { get; set; }
    }

    public class SystemOperationInfoRequest
    {
        public SystemOperationInfoRequest()
        {
            Operation = new ObjectRef();
        }
        [CSN("Operation")]
        public ObjectRef Operation { get; set; }
    }
}
