using System;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class ObservationResult
    {
        // ================ Обязательные свойства ===========================================================================================

        public Int32? SeqNo { get; set; } // Порядковый номер строки в пределах группы результатов
        public String MeasurementName { get; set; } // Наименование измерения (50). Пример: <MeasurementName> Креатинин</MeasurementName>
        public String ResultText { get; set; } // Полный текст значения измерения. Текст результата содержит значение результата, название единицы измерения и референтный диапазон. Пример: <ResultText>   95.20 мкмоль/л   (58.00-96.00)</ResultText>        
        public String OperatorID { get; set; } // Идентификатор исполнителя (5). Пример: <OperatorID>33</OperatorID>
        public String OperatorCode { get; set; } // Код исполнителя (20). Пример: <OperatorCode>218</OperatorCode>
        public String OperatorName { get; set; } // ФИО исполнителя (50). Пример: <OperatorName>Тусева Т. К.</OperatorName>
        public DateTime? FinishDate { get; set; } // Дата завершения измерения YYYY-MM-DD[THH:MM[:SS]]. Пример: <FinishDate>2012-11-06T18:55:35</FinishDate>

        // ================ НЕ Обязательные свойства ========================================================================================

        public String ResultValue { get; set; } // Значение измерения (25). Пример: <ResultValue>           95.20</ResultValue>
        public String NormText { get; set; } // Текстовая форма референтного интервала (30). Пример: <NormText>(58.00-96.00)</NormText>
        public String Unit { get; set; } // Единицы измерения (20). Пример: <Unit>мкмоль/л</Unit>
        public String NormMin { get; set; } // Минимальное значение референтного интервала Numeric(15,5). Пример: <NormMin>58.00000</NormMin>
        public String NormMax { get; set; } // Максимальное значение референтного интервала Numeric(15,5). Пример: <NormMax>96.00000</NormMax>
    }
}