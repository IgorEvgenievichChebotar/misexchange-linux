using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class CompleteRejectedPaymentRequest
    {
        public CompleteRejectedPaymentRequest(int id)
        {
            this.RejectedPayment = new ObjectRef(id);
        }
        [CSN("RejectedPayment")]
        public ObjectRef RejectedPayment { get; set; }
    }
}
