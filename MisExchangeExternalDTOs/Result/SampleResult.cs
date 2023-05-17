using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.ExchangeDTOs
{
    /// <summary>
    /// Класс описывает результаты выполнения исследоаний для одной пробы
    /// </summary>  
    public class SampleResult
    {
        public SampleResult()
        {
            Defects = new List<Defect>();
            TargetResults = new List<TargetResult>();
            MicroResults = new List<MicroResult>();
            Images = new List<FileInfo>();
        }
        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Штриход пробирки
        /// </summary>
        [StringLength(255)]
        public string Barcode { get; set; }
        /// <summary>
        /// Код биоматериала
        /// </summary> 
        [StringLength(255)]
        public string BiomaterialCode { get; set; }
        /// <summary>
        /// Комментарии
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Множество браков пробы
        /// </summary>
        public List<Defect> Defects { get; set; }
        /// <summary>
        /// Результаты выполнения исследований
        /// </summary>
        public List<TargetResult> TargetResults { get; set; }
        /// <summary>
        /// Результаты выполнения микробиологических исследований
        /// </summary>
        public List<MicroResult> MicroResults { get; set; }
        /// <summary>
        /// Графические объекты, связанные с пробой
        /// </summary>
        public List<FileInfo> Images { get; set; }
        /// <summary>
        /// Дата/время закрытия
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Внутренний номер в подразделении
        /// </summary>
        public string DepartmentNr { get; set; }
        /// <summary>
        /// Дата/время взятия биоматериала
        /// </summary>
        public DateTime? SamplingDate { get; set; }
        /// <summary>
        /// Дата доставки
        /// </summary>
        public DateTime? SampleDeliveryDate { get; set; }
    }
}