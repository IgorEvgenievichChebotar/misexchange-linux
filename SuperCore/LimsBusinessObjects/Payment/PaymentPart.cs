using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class PaymentPart
    {
        public PaymentPart() { }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("PayType")]
        public int PayType { get; set; }

        [CSN("Amount")]
        public double Amount { get; set; }
    }
}
