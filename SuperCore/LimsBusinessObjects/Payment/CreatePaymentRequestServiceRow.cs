using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class CreatePaymentRequestServiceRow : CreatePaymentRequestServiceRowBase
    {
        [CSN("Service")]
        public ServiceDictionaryItem Service { get; set; }
    }

    public class CreatePaymentRequestServiceRowBase
    {
        [CSN("OriginalPrice")]
        public double OriginalPrice { get; set; }

        [CSN("FinalPrice")]
        public double FinalPrice { get; set; }

        [CSN("DiscountInRub")]
        public double DiscountInRub { get; set; }

        [CSN("Quantity")]
        public int Quantity { get; set; }

        [CSN("Amount")]
        public double Amount { get; set; }
    }

}
