using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class Insurance
    {
        // ================ Обязательные свойства =====================================

        public String PolicyID { get; set; } // Страховой полис (32). Пример: <PolicyID>770000 7139201157</PolicyID>

        /* В примере нет */ public String StatusID { get; set; } // Идентификатор страхового статус (2 цифры с лидирующим нулём )
        
        // ================ НЕ Обязательные свойства =====================================
        
        public Company Company { get; set; } // Страховая компания
    }
}
