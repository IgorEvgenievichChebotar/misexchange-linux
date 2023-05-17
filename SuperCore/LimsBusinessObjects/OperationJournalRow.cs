using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class OperationJournalRow : BaseObject
    {
        public DateTime? Date { get; set; }
        public float Cost { get; set; }
        public string LotNr { get; set; }
        public string DocumentNr { get; set; }
        public int UnitsInBox { get; set; }
        public SupplierDictionaryItem Supplier { get; set; }
        public ObjectRef Batch { get; set; }
        public ObjectRef Storage { get; set; }
        public string InvoiceNr { get; set; }
        public int Qty { get; set; }
        public bool UseBoxUnit { get; set; }
        public MaterialDictionaryItem Material { get; set; }
        public float Price { get; set; }
        public string NameInInvoice { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceProformaDate { get; set; }
        public UnitTreeNodeDictionaryItem UnitTreeNode { get; set; }
        public ObjectRef User { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int DocumentId { get; set; }
        public int OperationType { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ArrivalDocumentNr { get; set; }
        public HospitalDictionaryItem Hospital { get; set; }
        public CustDepartmentDictionaryItem HospitalDepartment { get; set; }
        public DepartmentDictionaryItem Department { get; set; }
        public FundingSourceDictionaryItem FundingSource { get; set; }
        public TestDictionaryItem Test { get; set; }
    }
}