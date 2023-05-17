using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class Payment : PaymentShort
    {
        public Payment() : base()
        {
            Services = new List<PaymentService>();
            PaymentParts = new List<PaymentPart>();
        }

        [CSN("OriginalPrice")]
        public double OriginalPrice { get; set; }

        [CSN("DiscountInRub")]
        public double DiscountInRub { get; set; }

        [CSN("Amount")]
        public double Amount { get; set; }

        [CSN("TaxInRub")]
        public double TaxInRub { get; set; }

        [CSN("Services")]
        public List<PaymentService> Services { get; set; }

        [CSN("PaymentParts")]
        public List<PaymentPart> PaymentParts { get; set; }

        [CSN("OperationType")]
        public int OperationType { get; set; }

        [CSN("MainPaymentNumber")]
        public string MainPaymentNumber { get; set; }

        [CSN("PaymentReturnReason")]
        public PaymentReturnReasonDictionaryItem PaymentReturnReason { get; set; }
    }
}
