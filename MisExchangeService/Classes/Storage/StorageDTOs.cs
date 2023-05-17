using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes.Storage
{
    public class ReceiptOperation
    {
        public ReceiptOperation()
        {
            Resources = new List<Resource>();
        }

        public DateTime? OperationDate { get; set; }
        public List<Resource> Resources { get; set; }
    }

    public class ExpenditureInfo
    {
        public ExpenditureInfo()
        {
            Elements = new List<ExpenditureElement>();
        }

        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public List<ExpenditureElement> Elements { get; set; }
    }

    public class ExpenditureElement
    {
        public DateTime CompletionDate { get; set; }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public string HospitalDepartmentCode { get; set; }
        public string HospitalDepartmentName { get; set; }
        public string LabDepartmentCode { get; set; }
        public string LabDepartmentName { get; set; }
        public string FundingSourceCode { get; set; }
        public string FundingSourceName { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }        
        public string ResourceCode { get; set; }
        public string ResourceName { get; set; }
        public string ResourceUnitCode { get; set; }
        public string ResourceUnitName { get; set; }
        public string ResourceQuantity { get; set; }
    }

    public class Resource
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string LotNumber { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? Quantity { get; set; }
        public string UnitCode { get; set; }
        public float? Price { get; set; }
        public string FundingSourceCode { get; set; }
        public string ManufacturerCode { get; set; }
        public string SupplierCode { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? ContractDate { get; set; }
        public string WaybillNumber { get; set; }
        public DateTime? WaybillDate { get; set; }

    }

    public class Acknowledgment
    {
        public string FileName { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}