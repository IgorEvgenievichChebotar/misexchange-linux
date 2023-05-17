using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System.Xml.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class HospitalDictionaryItem : DictionaryItem
    {
        public HospitalDictionaryItem()
        {
            Pricelists = new List<HospitalPricelists>();
            HospitalServicePrice = new Dictionary<String, HospitalServicePrice>();
            CustDepartments = new List<CustDepartmentDictionaryItem>();
            PayCategories = new List<PayCategoryDictionaryItem>();
        }

        public HospitalDictionaryItem(Int32 Id) : this()
        {
            this.Id = Id;
        }

        [CSN("ExternalCode")]
        [SendToServer(false)]
        public new string ExternalCode { get; set; }


        [CSN("Removed")]
#if v129
        [SendToServer(false)]
#endif
        new public bool Removed { get { return base.Removed; } set { base.Removed = value; } }


        [CSN("Pricelists")]
        [SendToServer(false)]
        [JsonIgnore]
        public List<HospitalPricelists> Pricelists { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [SendToServer(false)]
        [CSN("HospitalServicePrice")]
        public Dictionary<String, HospitalServicePrice> HospitalServicePrice { get; set; }
        
        [CSN("CustDepartments")]
        [JsonIgnore]
        [XmlIgnore]
        public List<CustDepartmentDictionaryItem> CustDepartments { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("PayCategories")]
        public List<PayCategoryDictionaryItem> PayCategories { get; set; }
        
        public HospitalPricelists GetLastPriceList()
        {
            DateTime now = DateTime.Now;
            if(Pricelists != null)
                foreach (HospitalPricelists pricelist in Pricelists)
                {
                    if (pricelist.StartDate != null && ((DateTime)pricelist.StartDate).CompareTo(now) < 0)
                        if (pricelist.EndDate == null || ((((DateTime)pricelist.EndDate).CompareTo(now) > 0) || ((DateTime)pricelist.EndDate).CompareTo(DateTime.MinValue) == 0))
                            return pricelist;
                }
            return null;
        }

        [CSN("DefaultQuota")]
        public ObjectRef DefaultQuota { get; set; }

        [CSN("DefaultQuotaDisplayName")]
        public string DefaultQuotaDisplayName { get; set; }

        [CSN("SuggestCreatePayment")]
        public bool SuggestCreatePayment { get; set; }

        public void UpdatePrices(ServicePrice prices)
        {
            try
            {
                if (prices == null) return;

                HospitalServicePrice.Clear();
                if (this.Pricelists == null) return;
                HospitalPricelists pricelist = this.Pricelists.Find(pl => (pl.StartDate != null) && (pl.EndDate == null));
                if (pricelist == null) return;

                foreach (ServicePriceItem price in prices.Elements)
                {

                    if (price.Pricelist.Id == pricelist.Pricelist.Id)
                        if (!price.Service.Removed && (price.Service.Targets.Count > 0) && !price.Service.Targets[0].Removed)
                        {
                            if (!HospitalServicePrice.ContainsKey(price.Service.Targets[0].Code))
                                HospitalServicePrice.Add(price.Service.Targets[0].Code, new HospitalServicePrice(price.Price, price.UrgentFactor, price.Pricelist.Id));
                        }
                }
            }
            catch (Exception ex)
            {
                Log.WriteText("Error while updating prices at Hospital: " + this.Name + " Id: " + this.Id);
                Log.WriteText(ex.Message);
                Log.WriteText(ex.StackTrace);
            }
        }
    }

    public class HospitalDictionary : DictionaryClass<HospitalDictionaryItem>
    {
        public HospitalDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Hospital")]
        public List<HospitalDictionaryItem> Hospital
        {
            get { return Elements; }
            set { Elements = value; }
        }

        public void UpdatePrices(ServicePrice prices)
        {
            foreach (HospitalDictionaryItem hospital in this.Elements)
            {
                hospital.UpdatePrices(prices);
            }
        }
    }

    public class HospitalPricelists : BaseObject
    {
        [CSN("Pricelist")]
        public PricelistDictionaryItem Pricelist { get; set; }
        public String PricelistName;
        [CSN("EndDate")]
        public DateTime? EndDate {get; set;}
        [CSN("StartDate")]
        public DateTime? StartDate { get; set; }
        [CSN("Discount")]
        public Double? Discount { get; set; }
    }

    public class HospitalServicePrice
    {
        [CSN("Price")]
        public Double Price { get; set; }
        [CSN("UrgentFactor")]
        public Double UrgentFactor { get; set; }
        [SendToServer(false)]
        [CSN("Pricelist")]
        public ObjectRef Pricelist { get; set; }
        public HospitalServicePrice(Double price, Double urgentFactor, int priceListId)
        {
            Price = price;
            UrgentFactor = urgentFactor;
            Pricelist = new ObjectRef(priceListId);
        }
    }
}