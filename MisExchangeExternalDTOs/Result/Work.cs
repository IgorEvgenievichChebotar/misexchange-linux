using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    public struct WorkStateForMis
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
    public class Work
    {
        public Work()
        {
            Defects = new List<Defect>();
            Images = new List<FileInfo>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Id from server
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        public int LisWorkId { get; set; }
        /// <summary>
        /// Код теста
        /// </summary>
        [StringLength(255)]
        public string Code { get; set; }
        /// <summary>
        /// Название теста
        /// </summary>
        [StringLength(1024)]
        public string Name { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        [StringLength(1024)]
        public string Value { get; set; }
        /// <summary>
        /// Единица измерения
        /// </summary>
        [StringLength(1024)]
        public string UnitName { get; set; }
        /// <summary>
        /// Состояние работы (0 - создана, 1 - выполняется, 2 - выполнена, 3 – одобрена, 4 - отменена)
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// Норма для данного теста
        /// </summary>
        public Norm Norm { get; set; }
        /// <summary>
        /// Признак попадания результата в норму
        /// Константы нормальности в ЛИС:
        /// 0- Норма не определилась, 1-  Качественный тест в норме, 2-Качественный тест не в норме, 3- Значение ниже критической нормы,
        /// 4-Значение ниже нормы, 5- Значение в норме, 6- Значение выше нормы, 7-Значение выше критической нормы.
        /// </summary>
        public int Normality { get; set; }
        /// <summary>
        /// Браки
        /// </summary>
        public List<Defect> Defects { get; set; }
        /// <summary>
        /// Одобривший врач (ФИО)
        /// </summary>
        [StringLength(2048)]
        public string ApprovingDoctor { get; set; }
        /// <summary>
        /// Код одобрившего врача
        /// </summary>
        [StringLength(255)]
        public string ApprovingDoctorCode { get; set; }
        /// <summary>
        /// СНИЛС одобрившего врача
        /// </summary>
        [StringLength(255)]
        public string ApprovingDoctorSnils { get; set; }
        /// <summary>
        /// Код анализатора, с которого получен результат
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// Имя анализатора, с которого получен результат
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Дата одобрения
        /// </summary>
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// Дата модификации
        /// </summary>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// Графические объекты, связанные с работой
        /// </summary>
        public List<FileInfo> Images { get; set; }
        /// <summary>
        /// Комментарии
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Диаметр зоны задержки роста (для антибиотиков)
        /// </summary>
        public string Diameter { get; set; }
        /// <summary>
        /// Ранг группы тестов в справочнике "Бланки ответов по пробам"
        /// </summary>
        public int? GroupRank { get; set; }
        /// <summary>
        /// Ранг теста в группе в справочнике "Бланки ответов по пробам"
        /// </summary>       
        public int? RankInGroup { get; set; }
        /// <summary>
        /// Ранг теста при печати
        /// </summary>
        public int? TestRank { get; set; }
        /// <summary>
        /// Точность округления результата при печати
        /// </summary>
        public int? Precision { get; set; }
        /// <summary>
        /// Код группы пациентов, которая была определена при вычислении нормы
        /// </summary>
        [StringLength(255)]
        public string PatientGroupCode { get; set; } // [LIS-7210]
        /// <summary>
        /// Имя группы пациентов, котоаря была определена при вычислении нормы
        /// </summary>
        [NotMapped]
        public string PatientGroupName { get; set; } // [LIS-7210]
        /// <summary>
        /// Источник результата
        ///<summary>
        public int? ResultSource { get; set; } // [LIS-9457]
        /// <summary>
        /// Код анализатора
        ///<summary>
        public string AnalyzerCode { get; set; } // [LIS-9457]
        /// <summary>
        /// Имя анализатора
        ///<summary>
        public string AnalyzerName { get; set; } // [LIS-9457]
        public bool IsInNorm()
        {
            if (Normality == 0 || Normality == 1 || Normality == 5)
            {
                return true;
            }

            return false;
        }
    }
}