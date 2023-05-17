using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    
    public class ServicePriceItem: BaseObject
    {
        [CSN("timestamp")]
		public Int32 timestamp {get; set;}
        [CSN("Service")]
        public ServiceDictionaryItem Service { get; set; }
        [CSN("Price")]
		public Double Price {get; set;}
        [CSN("Pricelist")]
        public PricelistDictionaryItem Pricelist { get; set; }

        [CSN("UrgentFactor")]
        public Double UrgentFactor { get; set; }
    }

    public class ServicePrice : BaseObject
    {
        public ServicePrice()
        {
            Elements = new List<ServicePriceItem>();
        }

        [Unnamed]
        [CSN("Elements")]
        public List<ServicePriceItem> Elements { get; set; }

    }
    
    public class ServicePriceRequest
    {
        public ServicePriceRequest(Int32 timestamp)
        {
            LastTimestamp = timestamp;
        }

        public ServicePriceRequest()
        {
            LastTimestamp = 0;
        }
        [CSN("LastTimestamp")]
        public Int32 LastTimestamp { get; set; }
    }
}
