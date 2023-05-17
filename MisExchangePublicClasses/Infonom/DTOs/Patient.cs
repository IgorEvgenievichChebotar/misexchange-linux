using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class Patient
    {
        // ================ Обязательные свойства =====================================

        /* У нас возможны проблемы с пробелом*/ public String RegCode { get; set; } // Регистрационный номер пациента (20). Пример: <RegCode>002-754-835 31</RegCode> 
        public String FamilyName { get; set; } // Фамилия (30)

        // ================ НЕ Обязательные свойства =====================================        

        public String GivenName { get; set; } // Имя (25)
        public String MiddleName { get; set; } // Отчество (25)
        public String Gender { get; set; } // Пол (1) Пример: M, F
        public DateTime? BirthDate { get; set; } // Дата рождения  DateTime  YYYY-MM-DD[THH:MM[:SS]]. Пример: <BirthDate>1957-11-20</BirthDate>
        public String Address { get; set; } // Адрес (200). Пример: <Address>2:Москва,,Москва г,Космонавта Волкова ул,28-1--32</Address>
        public Insurance Insurance { get; set; } // Страховая информация

        // ================ НЕ Обязательные свойства, заполняющиеся в ответе по заявке =====================================================

        public String ID { get; set; } // Идентификатор пациента в ЛИС (8 цифр с лидирующими нулями). Пример: <ID>01435317</ID>  
        public String ExternalID { get; set; } // Внешний идентификатор пациента (23). Пример: <ExternalID>MDPL050C009009215534</ExternalID>
        public DateTime? RegDate { get; set; } // Дата регистрации. Пример: <RegDate>2012-10-24</RegDate>
    }
}
