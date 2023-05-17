using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ProductJournalFilter : BaseJournalFilter
    {
        public ProductJournalFilter()
        {
            ContainerTypes = new List<ContainerTypeDictionaryItem>();
            AdditiveTypes = new List<AdditiveTypeDictionaryItem>();
            ProductTypes = new List<ProductTypeDictionaryItem>();
            Classifications = new List<ProductClassificationDictionaryItem>();
            Departments = new List<DepartmentDictionaryItem>();
            StorageDepartments = new List<DepartmentDictionaryItem>();
            Filters = new List<ProductFilterDictionaryItem>();
            
        }

        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }
        //[CSN("DonorId")]
        //public ObjectRef DonorId { get; set; }
        //[CSN("DonationId")]
        //public ObjectRef DonationId { get; set; }
        //[CSN("ParentId")]
        //public ObjectRef ParentId { get; set; }
        //[CSN("ChildId")]
        //public ObjectRef ChildId { get; set; }
        [CSN("Nr")]
        public String Nr { get; set; }
        [CSN("Filters")]
        public List<ProductFilterDictionaryItem> Filters { get; set; }
        [SendToServer(false)]
        [CSN("ServerFilters")]
        public List<ProductFilterDictionaryItem> ServerFilters {
            get { return Filters; }
            set { Filters = value; }
        }
        [CSN("ContainerTypes")]
        public List<ContainerTypeDictionaryItem> ContainerTypes { get; set; }
        [CSN("ContainerNr")]
        public String ContainerNr { get; set; }
        [CSN("Removed")]
        public Int32? Removed { get; set; }
        [CSN("InProcess")]
        public Int32? InProcess { get; set; }
        [CSN("AdditiveTypes")]
        public List<AdditiveTypeDictionaryItem> AdditiveTypes { get; set; }
        [CSN("ProductTypes")]
        public List<ProductTypeDictionaryItem> ProductTypes { get; set; }
        //[CSN("BloodParameters")]
        //public List<BloodParameterValue> ???? BloodParameters { get; set; }
        [CSN("Classifications")]
        public List<ProductClassificationDictionaryItem> Classifications { get; set; }
        [CSN("Departments")]
        public List<DepartmentDictionaryItem> Departments { get; set; }
        [CSN("StorageDepartments")]
        public List<DepartmentDictionaryItem> StorageDepartments { get; set; }
        [CSN("StorageCode")]
        public String StorageCode { get; set; }
        [CSN("HasInvitation")]
        public Int32? HasInvitation { get; set; }

        public override void Clear()
        {

        }

        public override void PrepareToSend()
        {
            if (SkipDate)
            {
                DateFrom = DateTill = null;
            }
            base.PrepareToSend();
        }
    }
}