using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class GetRequestPaymentsResponse
    {
        [CSN("Payments")]
        public List<Payment> Payments { get; set; }

        public GetRequestPaymentsResponse()
        {
            Payments = new List<Payment>();
        }
    }
}
