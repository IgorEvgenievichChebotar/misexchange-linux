using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class Company
    {
        // ================ Обязательные свойства =====================================

        public String ID { get; set; } // Идентификатор организация (5 цифр с лидирующими нулями). Пример: <ID>025</ID>
        public String FullName { get; set; } // Наименование полное (254). Пример: <FullName>ОТКРЫТОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО СТРАХОВАЯ КОМПАНИЯ "РОСНО-МС"</FullName>
        
        // ================ НЕ Обязательные свойства =====================================

        public String Code { get; set; } // Регистрационный код (20). Пример: <Code>1027739051460</Code>
    }
}
