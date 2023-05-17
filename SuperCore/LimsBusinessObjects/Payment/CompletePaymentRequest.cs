using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class CompletePaymentRequest
    {
        [CSN("Id")]
        public int Id { get; set; }

        [CSN("PaymentParts")]
        public List<PaymentPart> PaymentParts { get; set; }
    }
}
