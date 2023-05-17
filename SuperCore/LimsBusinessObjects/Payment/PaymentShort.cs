using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class PaymentShort
    {
        public PaymentShort()
        {
            
        }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("PayDate")]
        public DateTime? PayDate { get; set; }

        [CSN("Number")]
        public string Number { get; set; }

        [CSN("CheckNumber")]
        public string CheckNumber { get; set; }

        [CSN("State")]
        public int State { get; set; }
    }
}
