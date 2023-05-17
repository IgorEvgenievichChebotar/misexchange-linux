using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult
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
        }

        [XmlIgnore]
        public long Id { get; set; }
        /// <summary>
        /// Штриход пробирки
        /// </summary>
        [StringLength(255)]
        public String Barcode { get; set; }
        /// <summary>
        /// Код биоматериала
        /// </summary> 
        [StringLength(255)]
        public String BiomaterialCode { get; set; }
        /// <summary>
        /// Название биоматериала
        /// </summary> 
        [StringLength(255)]
        public String BiomaterialName { get; set; }
        /// <summary>
        /// Дата/время закрытия
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Внутренний номер в подразделении
        /// </summary>
        public String DepartmentNr { get; set; }
        /// <summary>
        /// Комментарии
        /// </summary>
        public String Comments { get; set; }
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
    }
}