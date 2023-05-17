using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    /// <summary>
    /// Результат исследований (OBX)
    /// </summary>
    public class TargetResult
    {
        /// <summary>
        /// Тип сегмента (OBX)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentType;
        /// <summary>
        /// ID сегмента
        /// </summary>
        //[DataMember(Order = 1)]
        public String SegmentID;
        /// <summary>
        /// Тип результата
        /// </summary>
        //[DataMember(Order = 2)]
        public String ResultType;
        /// <summary>
        /// Код теста
        /// </summary>
        //[DataMember(Order = 3)]
        public String TestCode;
        /// <summary>
        /// Значение
        /// </summary>
        //[DataMember(Order = 5)]
        public String Value;
        /// <summary>
        /// Единицы измерения
        /// </summary>
        //[DataMember(Order = 6)]
        public String UnitType;
        /// <summary>
        /// Референсные значения
        /// </summary>
        //[DataMember(Order = 7)]
        public String ReferenceValue;
        /// <summary>
        /// Флаг
        /// </summary>
        //[DataMember(Order = 8)]
        public String Flag;
        /// <summary>
        /// Статус результата
        /// </summary>
        //[DataMember(Order = 11)]
        public String ResultState;
        /// <summary>
        /// Дата выполнения
        /// </summary>
        //[DataMember(Order = 14)]
        public DateTime? ExecutionDate;

        private String FieldDelimiter = "|";
                        
        public override string ToString()
        {
            return String.Join(
                FieldDelimiter,
                SegmentType,
                SegmentID,
                ResultType,
                TestCode,
                "",
                Value,
                UnitType,
                ReferenceValue,
                new String('|', 5),
                ExecutionDate.HasValue? ExecutionDate.Value.ToString("yyyyMMddHHmmss"): String.Empty);
        }

    }
}
