using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class OperationArrivalItemTemplate : BaseObject
    {
        public OperationArrivalItemTemplate()
        {
            Unit = new ObjectRef();
            Manufacturer = new ObjectRef();
        }
        [CSN("Material")]
        public MaterialDictionaryItem Material { get; set; }
        [CSN("NameInInvoice")]
        public String NameInInvoice { get; set; }
        [CSN("Qty")]
        public Int32 Qty { get; set; }
        [CSN("UnitsInBox")]
        public Int32? UnitsInBox { get; set; }
        [CSN("Unit")]
        public ObjectRef Unit { get; set; }
        [CSN("UseBoxUnit")]
        public Boolean UseBoxUnit { get; set; }
        [CSN("Price")]
        public Single Price { get; set; }
        [CSN("Cost")]
        public Single Cost { get; set; }
        [CSN("Storage")]
        public ObjectRef Storage { get; set; }
        [CSN("LotNr")]
        public String LotNr { get; set; }
        [SendToServer(false)]
        [CSN("BatchNumber")]
        public String BatchNumber { get; set; }
        [CSN("ExpireDate")]
        public DateTime ExpireDate { get; set; }
        [SendToServer(false)]
        [CSN("Manufacturer")]
        public ObjectRef Manufacturer { get; set; }
        [CSN("UnitTreeNode")]
        public ObjectRef UnitTreeNode { get; set; }
    }

    public class OperationArrivalTemplate : BaseObject
    {
        public OperationArrivalTemplate()
        {
            FromExternal = true;
            Supplier = new ObjectRef();
            FundingSource = new ObjectRef();
            Suboperations = new List<OperationArrivalItemTemplate>();
        }
        [CSN("FromExternal")]
        public Boolean FromExternal { get; set; }
        [CSN("Supplier")]
        public ObjectRef Supplier { get; set; }
        [CSN("FundingSource")]
        public ObjectRef FundingSource { get; set; }
        [CSN("InvoiceNr")]
        public String InvoiceNr { get; set; }
        [CSN("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }
        [CSN("InvoiceProformaDate")]
        public DateTime InvoiceProformaDate { get; set; }
        [CSN("InvoiceProformaNr")]
        public String InvoiceProformaNr { get; set; }
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("DocumentNr")]
        public String DocumentNr { get; set; }
        [CSN("InitialExport")]
        public Boolean InitialExport { get; set; }
        [CSN("Suboperations")]
        public List<OperationArrivalItemTemplate> Suboperations { get; set; }
    }

    public class OperationArrivalTemplateSaveResponce
    {
        public OperationArrivalTemplateSaveResponce()
        {
            Errors = new List<ErrorMessage>();
        }
        [CSN("Id")]
        public Int32 Id { get; set; }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class OperationArrivalResponce : OperationArrivalTemplateSaveResponce
    {
        //
    }
}