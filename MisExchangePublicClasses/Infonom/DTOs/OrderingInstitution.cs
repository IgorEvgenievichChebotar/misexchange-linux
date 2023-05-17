using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class OrderingInstitution
    {
        // ================ Обязательные свойства =====================================

        public String ID { get; set; } // Идентификатор организации 5 цифр с лидирующими нулями. Пример: <ID>00198</ID>
        public String FullName { get; set; } // Наименование полное (254) . Пример: <FullName>ГП N 555</FullName>

        // ================ НЕ Обязательные свойства =====================================
        public String Code { get; set; } // Регистрационный код (12) . Пример: <Code>0101555</Code>
    }
}
