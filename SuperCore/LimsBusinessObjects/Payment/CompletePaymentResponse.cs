using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class CompletePaymentResponse
    {
        [CSN("Id")]
        public int Id { get; set; }

        [CSN("State")]
        public int State { get; set; }
    }
}
