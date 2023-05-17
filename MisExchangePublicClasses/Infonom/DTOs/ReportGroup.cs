using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class ReportGroup
    {
        // ================ Обязательные свойства ===========================================================================================

        public String ResTableID { get; set; } // Идентификатор вида исследования (4) (2 цифры с лидирующим нулём). Пример: <ResTableID>35</ResTableID>
        public String ResTableAlias { get; set; } // Аббревиатура вида исследования (10). Пример: <ResTableAlias>OLYMP</ResTableAlias>
        public String ResTableName { get; set; } // Наименование вида исследования (50). Пример: <ResTableName>Ан. Olympus</ResTableName>
        public String ResTableDescription { get; set; } // Описание вида исследования (100). Пример: <ResTableDescription>Биохимический анализатор Olympus</ResTableDescription>
/*????*/ public DateTime? FinishDate { get; set; } // Дата завершения исследования YYYY-MM-DD[THH:MM[:SS]]. Пример: <FinishDate>2012-11-06T18:55:35</FinishDate>
/* в примере отсутствует ??? у нас есть только одобривший врач для отдельных работ*/ public String VerifierCode { get; set; } // Код ответственного лица (20)
        public Results Results { get; set; } // Список результатов

        // ================ НЕ Обязательные свойства ========================================================================================

        public int? ReportSeq { get; set; } // Порядковый номер в отчете. Пример: <ReportSeq>86</ReportSeq>        
        public String VerifierID { get; set; } // Идентификатор ответственного лица (5). Пример: <VerifierID>33</VerifierID>
        public String VerifierName { get; set; } //  Порядковый номер в отчете. Пример: <VerifierName>Тусева Т. К.</VerifierName>
    }
}
