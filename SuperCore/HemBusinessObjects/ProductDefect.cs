using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ProductDefect
    {
        public ProductDefect()
        {
            ProductType = new ProductTypeDictionaryItem();
            ContainerType = new ContainerTypeDictionaryItem();
            AdditiveType = new AdditiveTypeDictionaryItem();
            ContainerNr = String.Empty;
            Product = new ObjectRef();
            UserSet = new ObjectRef();
            UserRemove = new ObjectRef();
            Comment = String.Empty;
            Parameter = new ObjectRef();
            BloodParameters = new List<BloodParameterValue>();
        }

        [CSN("NrPrefix")]
        public String NrPrefix { get; set; }
        [CSN("NrValue")]
        public String NrValue { get; set; }
        [CSN("NrSuffix")]
        public String NrSuffix { get; set; }
        [CSN("Nr")]
        public String Nr
        {
            get
            {
                if (!String.IsNullOrEmpty(NrValue))
                    return String.Concat(NrPrefix, NrValue, NrSuffix);
                else
                    return String.Empty;
            }
        }        

        [CSN("ProductDate")]
        public DateTime ProductDate { get; set; }
        [CSN("ProductType")]
        public ProductTypeDictionaryItem ProductType { get; set; }
        [CSN("Volume")]
        public Int32 Volume { get; set; }
        [CSN("ContainerType")]
        public ContainerTypeDictionaryItem ContainerType { get; set; }
        [CSN("AdditiveType")]
        public AdditiveTypeDictionaryItem AdditiveType { get; set; }
        [CSN("ContainerNr")]
        public String ContainerNr { get; set; }
        [CSN("ProductRemoved")]
        public Boolean ProductRemoved { get; set; }
        [CSN("ProductExpireDate")]
        public DateTime? ProductExpireDate { get; set; }

        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("Product")]
        public ObjectRef Product { get; set; }
        [CSN("Defect")]
        public DefectDictionaryItem Defect { get; set; }
        [CSN("Removed")]
        public Boolean Removed { get; set; }
        [CSN("DateRemove")]
        public DateTime? DateRemove { get; set; }
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }
        [CSN("UserSet")]
        public ObjectRef UserSet { get; set; }
        [CSN("UserRemove")]
        public ObjectRef UserRemove { get; set; }

        [CSN("Comment")]
        public string Comment { get; set; }
        [CSN("Parameter")]
        public ObjectRef Parameter { get; set; }
    }

    public class DefectsSet
    {
        private List<ProductDefect> result = new List<ProductDefect>();
        [CSN("Result")]
        public List<ProductDefect> Result
        {
            get { return result; }
            set { result = value; }
        }
    }

    public class GetDefectInfoRequest
    {
        public GetDefectInfoRequest(int productId)
        {
            product = new ObjectRef(productId);
        }
        private ObjectRef product = new ObjectRef();
        [CSN("Product")]
        public ObjectRef Product
        {
            get { return product; }
            set { product = value; }
        }
    }

    public class SaveDefectRequest
    {
        public SaveDefectRequest(int productId, int defectId)
        {
            product = new ObjectRef(productId);
            defect = new ObjectRef(defectId);
        }

        private ObjectRef product = new ObjectRef();
        private ObjectRef defect = new ObjectRef();
        private string comment = string.Empty;

        [CSN("Product")]
        public ObjectRef Product
        {
            get { return product; }
            set { product = value; }
        }
        [CSN("Defect")]
        public ObjectRef Defect
        {
            get { return defect; }
            set { defect = value; }
        }
    }

    public class RemoveDefectsRequest
    {
        private List<ObjectRef> ids = new List<ObjectRef>();

        [CSN("Ids")]
        public List<ObjectRef> Ids
        {
            get { return ids; }
            set { ids = value; }
        }
    }
}
