using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    /// <summary>
    /// Запрос на исследование (OBR)
    /// </summary>
    public class TargetRequest
    {
        public TargetRequest()
        {
            //TestCodes = new TestCodes();
        }
        /// <summary>
        /// Тип сегмента (OBR)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentType;

        /// <summary>
        /// ID сегмента
        /// </summary>
        //[DataMember(Order = 1)]
        public String SegmentId;

        /// <summary>
        /// Id заказа у заказчика (МИС)
        /// </summary>
        //[DataMember(Order = 2)]
        public String TaskIdMis;

        /// <summary>
        /// Id заказа у исполнителя (ЛИС)
        /// </summary>
        //[DataMember(Order = 3)]
        public String TaskIdLis;

        /// <summary>
        /// Коды тестов
        /// </summary>
        //[DataMember(Order = 4)]
        public String TestCodes;

        /// <summary>
        /// Дата
        /// </summary>
        //[DataMember(Order = 7)]
        public DateTime? Date;

        /// <summary>
        /// Код задания: "A" или "R"
        /// </summary>
        //[DataMember(Order = 11)]
        public String TaskCode;

        /// <summary>
        /// Клинические комментарии
        /// </summary>
        //[DataMember(Order = 13)]
        public String ClinicComments;

        /// <summary>
        /// Дата доставки пробы
        /// </summary>
        //[DataMember(Order = 14)]
        public DateTime SampleDeliveryDate;

        /// <summary>
        /// Биоматериал
        /// </summary>
        //[DataMember(Order = 15)]
        public String Biomaterial;

        /// <summary>
        /// ФИО исполнителя
        /// </summary>
        //[DataMember(Order = 16)]
        public String ExecuterName;
        /// <summary>
        /// Уникальный ключ исследования
        /// </summary>
        public String PlacerKeyTest;


        /// <summary>
        /// Дата исполнителя
        /// </summary>
        //[DataMember(Order = 22)]
        public DateTime ExecutingDate;

        /// <summary>
        /// Количество и срочность
        /// </summary>
        //[DataMember(Order = 27)]
        public String CountAndCito;

        private String FieldDelimiter = "|";

        public override string ToString()
        {
            return String.Join(
                FieldDelimiter,
                SegmentType,
                SegmentId,
                TaskIdMis,
                TaskIdLis,
                TestCodes,
                FieldDelimiter,
                Date.HasValue? Date.Value.ToString("yyyyMMddHHmmss"): String.Empty,
                new String('|', 7),
                "^^",
                new String('|', 2),
                PlacerKeyTest,
                new String('|', 3),
                "F");
        }
    }

    
    public class TestCodes
    {
        //[DataMember(Order = 0)]
        public String TestCode { get; set; }
        //[DataMember(Order = 1)]
        public String Text { get; set; }
        //[DataMember(Order = 2)]
        public String CodingSystem { get; set; }
    }
}
