using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class StorageOperation: BaseObject
    {
        public StorageOperation()
        {
        
        }
        [CSN("Contractor")]
        public ContractorDictionaryItem Contractor { get; set; }
        [CSN("OperationType")]
        public StorageOperationTypeDictionaryItem OperationType { get; set; }
        [CSN("InvoiceItems")]
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
    public class InvoiceItem : BaseObject
    {
        public InvoiceItem()
        {
            Consignment = new Consigment();
        }
       
        [CSN("Consignment")]
        public Consigment Consignment { get; set; }
        [CSN("Delta")]
        public Int32 Delta { get; set; }
    
    }
    public class Consigment : BaseObject
    {
        public Consigment()
        {
 
        
        }
        [CSN("Storage")]
        public StorageDictionaryItem Storage { get; set; }
        [CSN("GoodsType")]
        public GoodsTypeDictionaryItem GoodsType { get; set; }
        [CSN("Manufacturer")]
        public ManufacturerDictionaryItem Manufacturer { get; set; }
        [CSN("SerialNr")]
        public String SerialNr { get; set; }
        [CSN("Quantity")]
        public Int32 Quantity { get; set; }
        [CSN("ProductionDate")]
        public DateTime ProductionDate { get; set; }
        [CSN("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }    
    }
}
