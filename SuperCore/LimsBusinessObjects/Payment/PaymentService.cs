using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class PaymentService : CreatePaymentRequestServiceRow
    {
        public PaymentService() { }
        [CSN("Id")]
        public int Id { get; set; }

        [CSN("Tax")]
        public int Tax { get; set; }

        [CSN("TaxInRub")]
        public double TaxInRub { get; set; }

        [CSN("State")]
        public int State { get; set; }

        [CSN("RejectedCount")]
        public int RejectedCount { get; set; }

        [CSN("ServiceName")]
        [SendToServer(false)]
        public string ServiceName { get; set; }

        [CSN("Code")]
        [SendToServer(false)]
        public string Code { get; set; }

        public void InitForFrontend()
        {
            if (Service != null && Service.Targets != null && Service.Targets.Count > 0)
            {
                ServiceName = Service.Name;
                Code = Service.Code;
                int id = Service.Id;
                Service = new ServiceDictionaryItem();
                Service.Id = id;
            }
        }
    }
}
