using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class PricelistDictionaryItem : DictionaryItem {
            public PricelistDictionaryItem()
            {
                PayCategories = new List<DictionaryItem>();
                //DirectoryGroups = new List<DictionaryItem>();
                Hospitals = new List<HospitalPricelistItem>();
                Prices = new PricesList();
                
            }
            [CSN("PayCategories")]
        	public List<DictionaryItem> PayCategories {get; set;}
            [CSN("UrgentFactor")]
			public Double UrgentFactor  {get; set;}
            //[CSN("DirectoryGroups")]
            //public List<DictionaryItem> DirectoryGroups {get; set;}
            [XmlIgnore]
            [CSN("Hospitals")]
            public List<HospitalPricelistItem> Hospitals {get; set;}
            [CSN("Prices")]
            public PricesList Prices { get; set; }
    }

    public class PriceItem
    {
        public PriceItem()
        {
            Price = 0;
        }
        [CSN("Price")]
        public float Price { get; set; }
        [CSN("Service")]
        public ObjectRef Service { get; set; }
    }

    public class PricesList : List<PriceItem>
    {
        public new PriceItem this[Int32 serviceId]
        {
            get
            {
                foreach (PriceItem item in this)
                    if (item.Service != null && item.Service.Id == serviceId)
                        return item;
                return null;
            }
            set
            {
                this[serviceId] = value;
            }
        }
    }

    public class HospitalPricelistItem : BaseObject
    {
        [CSN("EndDate")]
        public DateTime? EndDate { get; set; }
        [CSN("StartDate")]
        public DateTime? StartDate { get; set; }
        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }
        [CSN("Discount")]
        public Double Discount { get; set; }
    }

    public class PricelistDictionary : DictionaryClass<PricelistDictionaryItem>
    {
        public PricelistDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("PricelistShort")]
        public List<PricelistDictionaryItem> PricelistShort
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
