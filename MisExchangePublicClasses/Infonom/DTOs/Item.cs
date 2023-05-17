using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class item
    {
        // ================ Обязательные свойства =====================================

        public String ServiceID { get; set; } // Идентификатор услуги (4 цифры с лидирующим нулём). Пример: <ServiceID>0648</ServiceID>

        // ================ НЕ Обязательные свойства =====================================

        public String CategoryID { get; set; } // Идентификатор категории услуги (2 цифры с лидирующими нулями). Пример: <CategoryID>27</CategoryID>
        public String Category { get; set; } // Категория услуги (45). Пример: <Category>ОМСМК</Category>

        // ================ НЕ Обязательные свойства, заполняющиеся в ответе по заявке =====================================================

        public String ServiceCode { get; set; } // Аббревиатура для услуги (25). Пример: <ServiceCode>26246</ServiceCode>
        public String Service { get; set; } // Наименование услуги (245). Пример: <Service>Общий белок</Service>
        public Boolean? Completed { get; set; } // Услуга выполнена?  true – да, false – нет
        public String CategoryCode { get; set; } // Аббревиатура для категории услуги (6). Пример: <CategoryCode>ОМСМК</CategoryCode>
        public String WorkplaceID { get; set; } // Рабочее место (2). Пример: <WorkplaceID>1</WorkplaceID>
        public String ExecutorID { get; set; } // Идентификатор исполнителя услуги (5). Пример: <ExecutorID>33</ExecutorID>
        public String ExecutorName { get; set; } // ФИО исполнителя услуги (50). Пример: <ExecutorName>Тусева Т. К.</ExecutorName>
    }
}