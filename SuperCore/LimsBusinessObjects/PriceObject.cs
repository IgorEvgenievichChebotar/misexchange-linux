using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class PriceObject
    {
        public PriceObject()
        {
            Target = new ObjectRef();
            Price = 0;
        }
        [CSN("Target")]
        public ObjectRef Target { get; set; }

        [CSN("Price")]
        public double Price { get; set; }
    }

    public class PriceSet
    {
        public PriceSet()
        {
            Prices = new List<PriceObject>();
        }

        [CSN("Prices")]
        public List<PriceObject> Prices { get; set; }
    }

    public class TargetObject : TargetPrices
    {
        public TargetObject() : base()
        {
            Target = new TargetDictionaryItem();
        }
        [CSN("Target")]
        public TargetDictionaryItem Target { get; set; }
    }

    public class TargetPrices
    {
        public TargetPrices()
        {
            Price = 0;
            OriginalPrice = 0;
            DiscountInRub = 0;
        }

        [CSN("Price")]
        public double Price { get; set; }

        [CSN("OriginalPrice")]
        public double OriginalPrice { get; set; }

        [CSN("DiscountInRub")]
        public double DiscountInRub { get; set; }
    }

    public class TargetsSet
    {
        public TargetsSet()
        {
            Targets = new List<TargetObject>();
        }

        [CSN("Targets")]
        public List<TargetObject> Targets { get; set; }
    }
}
