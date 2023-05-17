using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class PaymentReturnReasonDictionaryItem : DictionaryItem
    {
        public PaymentReturnReasonDictionaryItem() : base()
        {

        }
    }

    public class PaymentReturnReasonDictionary : DictionaryClass<PaymentReturnReasonDictionaryItem>
    {
        public PaymentReturnReasonDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("PaymentReturnReason")]
        public List<PaymentReturnReasonDictionaryItem> PaymentReturnReason
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
