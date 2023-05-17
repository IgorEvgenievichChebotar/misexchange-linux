using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class RejectPaymentServicesRequestRow
    {
        [CSN("Service")]
        public ObjectRef Service { get; set; }

        [CSN("Count")]
        public int Count { get; set; }
    }
}
