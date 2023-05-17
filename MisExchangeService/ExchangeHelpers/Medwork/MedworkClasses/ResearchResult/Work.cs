using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
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
            //Norm = new Norm();
            Defects = new List<Defect>();
            Images = new List<string>();
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
        public String Code { get; set; }
        /// <summary>
        /// Название теста
        /// </summary>
        [StringLength(1024)]
        public String Name { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        [StringLength(1024)]
        public String Value { get; set; }
        /// <summary>
        /// Единица измерения
        /// </summary>
        [StringLength(1024)]
        public String UnitName { get; set; }
        /// <summary>
        /// Состояние работы (0 - создана, 1 - выполняется, 2 - выполнена, 3 – одобрена, 4 - отменена)
        /// </summary>
        public Int32 State { get; set; }
        /// <summary>
        /// Норма для данного теста
        /// </summary>
        public Norm Norm { get; set; }
        /// <summary>
        /// Признак попадания результата в норму
        /// </summary>
        public Int32 Normality { get; set; }
        /// <summary>
        /// Браки
        /// </summary>
        public List<Defect> Defects { get; set; }
        /// <summary>
        /// Одобривший врач (ФИО)
        /// </summary>
        [StringLength(2048)]
        public String ApprovingDoctor { get; set; }
        /// <summary>
        /// Код одобрившего врача
        /// </summary>
        [StringLength(255)]
        public String ApprovingDoctorCode { get; set; }
        /// <summary>
        /// Код анализатора, с которого получен результат
        /// </summary>
        //public string EquipmentCode { get; set; }
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
        /// Множество строк, содержащих имена файлов с графическими объектами, связанными с работой
        /// </summary>
        public List<String> Images { get; set; }
        /// <summary>
        /// Диаметр зоны задержки роста (для антибиотиков)
        /// </summary>
        public String Diameter { get; set; }
        /// <summary>
        /// Комментарии
        /// </summary>
        public String Comments { get; set; }
        /// <summary>
        /// Ранг группы тестов в справочнике "Бланки ответов по пробам"
        /// </summary>
        public Int32 GroupRank { get; set; }
        /// <summary>
        /// Ранг теста в группе в справочнике "Бланки ответов по пробам"
        /// </summary>       
        public Int32 RankInGroup { get; set; }
        /// <summary>
        /// Ранг теста при печати
        /// </summary>
        public Int32 TestRank { get; set; }
        /// <summary>
        /// Точность округления результата при печати
        /// </summary>
        public Int32? Precision { get; set; }
        /// <summary>
        /// Код группы пациентов, которая была определена при вычислении нормы
        /// </summary>
        [StringLength(255)]
        public String PatientGroupCode { get; set; } // [LIS-7210]
        /// <summary>
        /// Имя группы пациентов, котоаря была определена при вычислении нормы
        /// </summary>
        [NotMapped]
        public String PatientGroupName { get; set; } // [LIS-7210]
    }
}