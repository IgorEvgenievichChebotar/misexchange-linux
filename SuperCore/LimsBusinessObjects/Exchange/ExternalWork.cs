using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    public class ExternalWorkState
    {
        public const int New = 0;
        public const int Loaded = 1;
        public const int Done = 2;
        public const int Approved = 3;
        public const int Cancelled = 4;
    }

    /// <summary>
    /// Класс описывает результат выполнения одного простого теста
    /// </summary>
    [XmlType(TypeName = "Work")]
    public class ExternalWork
    {
        public ExternalWork()
        {
            Norm = new ExternalNorm();
            Defects = new List<ExternalDefect>();
            Images = new List<FileInfo>();
        }

        // Идентификатор работы
        [XmlIgnore]
        [CSN("Id")]
        public int Id { get; set; }
        /// <summary>
        /// Код теста
        /// </summary>
        [CSN("Code")]
        public string Code { get; set; }
        /// <summary>
        /// Название теста
        /// </summary>
        [CSN("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Короткое название теста
        /// </summary>
        [CSN("Mnemonics")]
        public string Mnemonics { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        [CSN("Value")]
        public string Value { get; set; }
        /// <summary>
        /// Единица измерения
        /// </summary>
        [CSN("UnitName")]
        public string UnitName { get; set; }
        /// <summary>
        /// Состояние работы (0 - создана, 1 - выполняется, 2 - выполнена, 3 – одобрена, 4 - отменена)
        /// </summary>
        [CSN("State")]
        public int State { get; set; }
        // Закомментировал это большое зло, как неуместное в средствах доступа к свойству. Любые "подгонки" этого свойства должны осуществляться в другом месте
        //{
        //    get { return state; }
        //    set { state = value - 1; } // Статусы работ, описанные в протоколе интеграции на 1 меньше принятых в ЛИС 
        //}
        /// <summary>
        /// Норма для данного теста
        /// </summary>
        [CSN("Norm")]
        public ExternalNorm Norm { get; set; }
        /// <summary>
        /// Признак попадания результата в норму
        /// </summary>
        [CSN("Normality")]
        public int Normality { get; set; }
        /// <summary>
        /// Браки
        /// </summary>
        [CSN("Defects")]
        public List<ExternalDefect> Defects { get; set; }
        /// <summary>
        /// Одобривший врач
        /// </summary>
        [CSN("ApprovingDoctor")]
        public string ApprovingDoctor { get; set; }
        /// <summary>
        /// Одобривший врач (код)
        /// </summary>
        [CSN("ApprovingDoctorCode")]
        public string ApprovingDoctorCode { get; set; }
        /// <summary>
        /// Одобривший врач (СНИЛС)
        /// </summary>
        [CSN("ApprovingDoctorSnils")]
        public string ApprovingDoctorSnils { get; set; }
        /// <summary>
        /// Код анализатора, с которого получен результат
        /// </summary>
        [CSN("EquipmentCode")]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// Имя анализатора, с которого получен результат
        /// </summary>
        [CSN("EquipmentName")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        [CSN("CreateDate")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Дата одобрения
        /// </summary>
        [CSN("ApproveDate")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// Дата модификации
        /// </summary>
        [CSN("ModifyDate")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// Графические объекты, связанные с работой
        /// </summary>
        [CSN("Images")]
        public List<FileInfo> Images { get; set; }
        /// <summary>
        /// Комментарии
        /// </summary>
        [CSN("Comments")]
        public string Comments { get; set; }
        /// <summary>
        /// Диаметр задержки роста (для теста на антибиотик)
        /// </summary>
        [CSN("Diameter")]
        public string Diameter { get; set; }
        /// <summary>
        /// Ранг группы тестов в справочнике "Бланки ответов по пробам"
        /// </summary>
        [CSN("GroupRank")]
        public int GroupRank { get; set; }
        /// <summary>
        /// Ранг теста в группе в справочнике "Бланки ответов по пробам". Однако при печати на бумажном бланке этот ранг не используется, а используется свойство TestRank
        /// </summary>       
        [CSN("RankInGroup")]
        public int RankInGroup { get; set; }
        /// <summary>
        /// Ранг теста при печати. Берётся из справочника "test", поле "Rank". Это же поле используется при сортировке тестов в карте пробы
        /// </summary>
        [CSN("TestRank")]
        public int TestRank { get; set; }
        /// <summary>
        /// Точность округления результата при печати
        /// </summary>
        [CSN("Precision")]
        public string Precision { get; set; }
        /// <summary>
        /// Код группы пациентов, которая была определена при вычислении нормы
        /// </summary>
        [CSN("PatientGroupCode")]
        public string PatientGroupCode { get; set; } // [LIS-7210]
        /// <summary>
        /// Источник результата
        /// </summary>
        [CSN("ResultSource")]
        public int? ResultSource { get; set; } // [LIS-9457]
        /// <summary>
        /// Код анализатора
        /// </summary>
        [CSN("AnalyzerCode")]
        public string AnalyzerCode { get; set; } // [LIS-9457]
        /// <summary>
        /// Имя анализатора
        /// </summary>
        [CSN("AnalyzerName")]
        public string AnalyzerName { get; set; } // [LIS-9457]
        /// <summary>
        /// Округление результата до знака
        /// </summary>
        [CSN("Format")]
        public string Format { get; set; } // [LIS-9996]
    }
}