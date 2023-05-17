using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class RejectPaymentServicesRequest
    {
        public RejectPaymentServicesRequest()
        {
            this.Services = new List<RejectPaymentServicesRequestRow>();
        }
        [CSN("Services")]
        public List<RejectPaymentServicesRequestRow> Services { get; set; }

        [CSN("PaymentReturnReason")]
        public PaymentReturnReasonDictionaryItem PaymentReturnReason { get; set; }
    }
}
