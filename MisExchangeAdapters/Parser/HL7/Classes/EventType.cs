using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    public class EventType
    {
        /// <summary>
        /// Имя сегмента (MSA)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentName;

        /// <summary>
        /// Тип ответа(MSH) - Application Accept (AA), Application Error (AE), Application Reject (AR)
        /// </summary>
        //[DataMember(Order = 1)]
        public String MessageType;

        /// <summary>
        /// Контрольный ID сообщения
        /// </summary>
        //[DataMember(Order = 2)]
        public String MessageId;

        /// <summary>
        /// Текстовое сообщение, макс. длина - 80
        /// </summary>
        //[DataMember(Order = 3)]
        public String TextMessage;

        public override string ToString()
        {
            return String.Join(HL7SeparatorConst.DefaultFieldDelim.ToString(), SegmentName, MessageType, MessageId, TextMessage);
        }
    }


    public struct MessageTypes
    {
        public const String ApplicationAccept = "AA";
        public const String ApplicationError = "AE";
        public const String ApplicationReject = "AR";
    }
}
