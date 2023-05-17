using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает результаты выполнения исследоаний для одной пробы
    /// </summary>       
    [XmlType(TypeName = "SampleResult")]
    public class ExternalSampleResult
    {
        public ExternalSampleResult()
        {
            Defects = new List<ExternalDefect>();
            TargetResults = new List<ExternalTargetResult>();
            MicroResults = new List<ExternalMicroResult>();
            Images = new List<FileInfo>();
        }

        // Идентификатор пробирки
        [CSN("Id")]
        public int Id { get; set; }

        // Штриход пробирки
        [CSN("Barcode")]
        public String Barcode { get; set; }

        // Браки
        [CSN("Defects")]
        public List<ExternalDefect> Defects { get; set; }

        // Результат выполнения исследований
        [ClonePropName("Targets")]
        [CSN("TargetResults")]
        public List<ExternalTargetResult> TargetResults { get; set; }

        // Результат выполнения микробиологических исследований
        [CSN("MicroResults")]
        public List<ExternalMicroResult> MicroResults { get; set; }

        /// <summary>
        /// Графические объекты, связанные с пробой
        /// </summary>
        [CSN("Images")]
        public List<FileInfo> Images { get; set; }

        [CSN("Comments")]
        public String Comments { get; set; }

        /// <summary>
        /// Дата/время закрытия
        /// </summary>
        [CSN("EndDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Внутренний номер в подразделении
        /// </summary>
        [CSN("DepartmentNr")]
        public String DepartmentNr { get; set; }

        // Свойство необходимо при получении номера от сервера ЛИС
        [XmlIgnore]
        [CSN("InternalNr")]
        public String InternalNr
        {
            get { return Barcode; }
            set { Barcode = value; }
        }
        /// <summary>
        /// Код биоматериала
        /// </summary> 
        [CSN("BiomaterialCode")]
        public String BiomaterialCode { get; set; }
        [CSN("BiomaterialName")]
        public String BiomaterialName { get; set; }

        /// <summary>
        /// Дата доставки
        /// </summary>
        public DateTime? SampleDeliveryDate { get; set; }
    }

    public class GetSampleResultsParams
    {
        [CSN("Sample")]
        public List<ObjectRef> Sample { get; set; }
    }

    public class GetSamplesResultsResponce
    {
        public GetSamplesResultsResponce()
        {
            Results = new List<ExternalSampleResult>();
        }

        [Unnamed]
        [CSN("Results")]
        public List<ExternalSampleResult> Results { get; private set; }
    }
}