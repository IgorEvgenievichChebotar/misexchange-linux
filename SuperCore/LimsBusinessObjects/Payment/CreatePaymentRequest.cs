using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class CreatePaymentRequest
    {

        [CSN("Request")]
        public ObjectRef Request { get; set; }
    }
}
