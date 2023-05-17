using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Payment
{
    public class UpdatePaymentRequest
    {
        public UpdatePaymentRequest()
        {
            PaymentParts = new List<UpdatePaymentRequestPaymentPart>();
        }

        /// <summary>
        /// Номер платежа("счёта")
        /// </summary>
        [CSN("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Номер чека
        /// </summary>
        [CSN("CheckNumber")]
        public string CheckNumber { get; set; }
        /// <summary>
        /// Дата совершения платежа 
        /// </summary>
        [CSN("PayDate")]
        public DateTime PayDate { get; set; }
        /// <summary>
        /// Список платежей (должны отличаться видом оплаты)
        /// </summary>
        [CSN("PaymentParts")]
        public List<UpdatePaymentRequestPaymentPart> PaymentParts { get; set; }
    }

    public class UpdatePaymentRequestPaymentPart
    {
        public UpdatePaymentRequestPaymentPart() { }

        [CSN("PayType")]
        public int PayType { get; set; }

        [CSN("Amount")]
        public double Amount { get; set; }
    }
}