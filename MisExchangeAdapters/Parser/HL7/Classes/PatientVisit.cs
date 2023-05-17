using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    /// <summary>
    /// Визит пациента
    /// </summary>
    [Obfuscation]
    public class PatientVisit
    {
        /// <summary>
        /// Тип сегмента (PV1)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentType;
        /// <summary>
        /// Sequence ID визита
        /// </summary>
        //[DataMember(Order = 1)]
        public String SeqVisitId;
        /// <summary>
        /// Класс пациента
        /// </summary>
        //[DataMember(Order = 2)]
        public String PatientClass;
        /// <summary>
        /// Отделение госпитализации
        /// </summary>
        //[DataMember(Order = 3)]
        public String Hospital;
        /// <summary>
        /// Лечащий врач
        /// </summary>
        //[DataMember(Order = 7)]
        public String Doctor;
        /// <summary>
        /// ID визита
        /// </summary>
        //[DataMember(Order = 19)]
        public String VisitNumber;
        /// <summary>
        /// Дата поступления
        /// </summary>
        //[DataMember(Order = 44)]
        public DateTime? HospitalizationDate;
        /// <summary>
        /// Дата выписки
        /// </summary>
        /// [DataMember(Order = 45)]
        public DateTime? DischargeDate;
        /// <summary>
        /// Альтернативный ID визита
        /// </summary>
        // [DataMember(Order = 50)]
        public String AlterVisitNumber;

        public override string ToString()
        {
            return String.Join(HL7SeparatorConst.DefaultFieldDelim.ToString(),
                SegmentType,
                SeqVisitId,
                PatientClass,
                Hospital,
                new String('|', 3),
                Doctor,
                new String('|', 4),
                "qMS",
                new String('|', 3),
                VisitNumber,
                new String('|', 23),
                HospitalizationDate.HasValue? HospitalizationDate.Value.ToString("yyyyMMddHHmmss"): String.Empty,
                DischargeDate.HasValue? DischargeDate.Value.ToString("yyyyMMdd"): String.Empty,
                new String('|',3),
                AlterVisitNumber);
        }
    }
}
