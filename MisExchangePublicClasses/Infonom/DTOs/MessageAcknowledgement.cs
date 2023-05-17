using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public struct MessageAcknowledgementStatus
    {
        public static string Successful = "D";
        public static string Error = "E";
    }
    
    public class MessageAcknowledgement
    {
        public MessageAcknowledgement()
        {
            AckDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            Status = MessageAcknowledgementStatus.Successful;
        }

        public string MessageType { get; set; } // Тип сообщения. Значения: ORD – для сообщения о заказе, REP – для сообщения с результатами, SVC – для сообщения с типами заказов и услуг 
        public string MD5 { get; set; } // Контрольная сумма исследования  
        public string Status { get; set; } // Статус доставки. D – доставлено, E – ошибка
        public string ErrorText { get; set; } // Текст ошибки, если Status = E. Max 254
        public DateTime AckDate { get; set; } // Дата подтверждения
    }
}