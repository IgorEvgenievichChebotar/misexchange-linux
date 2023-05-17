using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class Suboperation : BaseObject
    {
        public Suboperation()
        {
            Operation = new Operation();
            SubBatch = new BaseObject();
        }
        [CSN("Qty")]
        public Int32 Qty { get; set; }
        [CSN("UseBoxUnit")]
        public Boolean UseBoxUnit { get; set; }
        [CSN("ExportDate")]
        public DateTime ExportDate { get; set; }
        [CSN("SubBatch")]
        public BaseObject SubBatch { get; set; }
        [CSN("Operation")]
        public Operation Operation { get; set; }
        [CSN("ArrivalIndex")]
        public Int32 ArrivalIndex { get; set; }
    }

    public class Operation : BaseObject
    {
        public Operation()
        {
            Suboperations = new List<Suboperation>();
            Author = new EmployeeDictionaryItem();
        }
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("DocumentNr")]
        public String DocumentNr { get; set; }
        [CSN("Author")]
        public EmployeeDictionaryItem Author { get; set; }
        [CSN("Type")]
        public Int32 Type { get; set; }
        [CSN("Suboperations")]
        public List<Suboperation> Suboperations { get; set; }
        [CSN("InitialExport")]
        public Boolean InitialExport { get; set; }
    }

    [LinkToJava(JavaLinkClasses.TYPE_ARRIVAL)]
    public class OperationArrival : Operation
    {
        public OperationArrival()
        {
            Template = new OperationArrivalTemplate();
        }

        public String InvoiceNr;
        public DateTime InvoiceDate;
        public DateTime InvoiceProformaDate;
        public String InvoiceProformaNr;
        public OperationArrivalTemplate Template;
    }

    [LinkToJava(JavaLinkClasses.TYPE_EXPENSE)]
    public class OperationExpense : Operation { }

    [LinkToJava(JavaLinkClasses.TYPE_RETIREMENT)]
    public class OperationRetirement : Operation
    {
        [CSN("Description")]
        public String Description { get; set; }
    }

    public class OperationInfoRequest
    {
        public OperationInfoRequest()
        {
            Operation = new ObjectRef();
        }
        [CSN("Operation")]
        public ObjectRef Operation { get; set; }
    }
}