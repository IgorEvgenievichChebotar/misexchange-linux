using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    public class Observation
    {
        public Observation()
        {
            Patient = new Patient();
        }

        // ================ Обязательные свойства ===========================================================================================

        public String ID { get; set; } // Идентификатор документа (25 цифр)
        public String SpecimenTypeID { get; set; } // Типа материала, ID по справочнику (3 цифры с лидирующим нулём). Пример: 53
        public String SpecimenTypeCode { get; set; } // Тип материала, аббревиатура (5). Пример: <SpecimenTypeCode>Б/х</SpecimenTypeCode>
        public String SpecimenType { get; set; } // Тип материала, наименование (50). Используется на этапе передачи результатов. Пример: <SpecimenType>Биохимия-сыворотка</SpecimenType>
        public String Diagnosis { get; set; } // Диагноз (20)
        public String ResultToOrderingInstitution { get; set; } // Результат выдать направившей организации? Boolean  true – да, false – нет
        public DateTime? RegDate { get; set; } // Дата регистрации в ЛИС YYYY-MM-DD[THH:MM[:SS]]. Используется на этапе передачи результатов. Пример: <RegDate>2012-11-09T16:36:56</RegDate>      

        // ================ НЕ Обязательные свойства ========================================================================================

        public DateTime? OrderDate { get; set; } // Дата заказа  DateTime  YYYY-MM-DD[THH:MM[:SS]]. Пример: <OrderDate>2012-11-09</OrderDate>
        public String OrderID { get; set; } // Идентификатор заказа 16 цифр. Пример: 100001
        public String HisOrderID { get; set; } // Идентификатор заказа в МИС 32. Пример: 10000101015551211
        public DateTime? CollectDate { get; set; } // Дата взятия материала  DateTime  YYYY-MM-DD[THH:MM[:SS]]. Пример: 2012-11-09T00:00:00
        public Byte? CustomerType { get; set; } // Типа заказчика  SmallInt  Тип заказчика: 1 – организация, 2 - пациент
        public Patient Patient { get; set; } // Информация о пациенте 
        public OrderingInstitution OrderingInstitution { get; set; } // Информация о заказчике
        public List<item> OrderInfo { get; set; } // Информация о заказе
        public ObservationReport ObservationReport { get; set; } // Результаты исследований. Используется на этапе передачи результатов
        public Boolean? Cito { get; set; } // Заказ срочный?  Boolean  true – да, false – нет

        // ================ НЕ Обязательные свойства, заполняющиеся в ответе по заявке =====================================================
        
        public String SpecimenSiteID { get; set; } // Место взятия материала, ID по справочнику (3 цифры с лидирующими нулями). Пример: <SpecimenSiteID>1</SpecimenSiteID>
        public String SpecimenSite { get; set; } // Место взятия материала (50). Пример: <SpecimenSite>---</SpecimenSite>
        public Byte? VizitID { get; set; } // Идентификатор визита. Используется на этапе передачи результатов. Пример: <VisitID>1</VisitID>
        public String Status { get; set; } // Статус материала: I, S, A, R, X, F. Используется на этапе передачи результатов. Пример: <Status>R</Status>
/*???*/         public DateTime? FinishDate { get; set; } // Дата завершения исследований YYYY-MM-DD[THH:MM[:SS]]. Используется на этапе передачи результатов. Пример: <FinishDate>2012-11-09T19:35:35</FinishDate>
/*???*/         public DateTime? ReportDate { get; set; } // Дата выдачи отчета YYYY-MM-DD[THH:MM[:SS]]. Используется на этапе передачи результатов. Пример: <ReportDate>2012-11-09T16:56:47</ReportDate>
        public String PurposeID { get; set; } // Цель исследования, ID по справочнику (3 цифры с лидирующим нулём). Пример: <PurposeID>01</PurposeID>
        public String Purpose { get; set; } // Цель исследования (25). Пример: <Purpose>---</Purpose>
    }
}
