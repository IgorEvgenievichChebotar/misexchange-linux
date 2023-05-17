using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class CancelRejectedPaymentServicesRequest
    {
        public CancelRejectedPaymentServicesRequest()
        {
            this.Services = new List<ObjectRef>();
        }
        [CSN("Services")]
        public List<ObjectRef> Services { get; set; }
    }
}
