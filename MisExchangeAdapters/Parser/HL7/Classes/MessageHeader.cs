using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    /// <summary>
    /// Заголовок сообщения
    /// </summary>
    public class MessageHeader
    {
        /// <summary>
        /// Имя сегмента (MSH)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentName;

        /// <summary>
        /// Разделители
        /// </summary>
        //[DataMember(Order = 1)]
        public String Delimeters;

        /// <summary>
        /// Приложение-отправитель
        /// </summary>
        //[DataMember(Order = 2)]
        public String AppSender;

        /// <summary>
        /// Оборудование-отправитель
        /// </summary>
        //[DataMember(Order = 3)]
        public String EqSender;

        /// <summary>
        /// Приложение-получатель
        /// </summary>
        //[DataMember(Order = 4)]
        public String AppReceiver;

        /// <summary>
        /// Оборудование-получатель
        /// </summary>
        //[DataMember(Order = 5)]
        public String EqReceiver;
        /// <summary>
        /// Дата/время сообщения
        /// </summary>
        //[DataMember(Order = 6)]
        public DateTime MessageDate;
        /// <summary>
        /// Защита сообщения
        /// </summary>
        //[DataMember(Order = 7)]
        public String MessageDefence; 
        //Переделать в подтип
        /// <summary>
        /// Тип сообщения 
        /// </summary>
        //[DataMember(Order = 8)]
        public String MessageType;
        /// <summary>
        /// Контрольный ID сообщения
        /// </summary>
        //[DataMember(Order = 9)]
        public String MessageId;
        /// <summary>
        /// Код исполнения
        /// </summary>
        //[DataMember(Order = 10)]
        public String ExecuteCode;
        /// <summary>
        /// Версия
        /// </summary>
        //[DataMember(Order = 11)]
        public String Version;

        private String FieldDelimiter = "|";

        public override string ToString()
        {
            return String.Join(
                FieldDelimiter,
                SegmentName,
                Delimeters,
                AppSender,
                EqSender,
                AppReceiver,
                EqReceiver,
                MessageDate.ToString("yyyyMMddHHmmss"),
                "",
                MessageType,
                MessageId,
                ExecuteCode,
                Version,
                FieldDelimiter,
                "AL",
                "NE");
        }
    }
}
