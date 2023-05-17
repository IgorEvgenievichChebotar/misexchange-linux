using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace MisExchangeAdapters.Parser.HL7.Classes
{
    [Obfuscation]
    public class TaskBlock
    {
        /// <summary>
        /// Тип сегмента (ORC)
        /// </summary>
        //[DataMember(Order = 0)]
        public String SegmentType;
        /// <summary>
        /// Тип задания (NW - новое, XE - редактирование, CA - отмена)
        /// </summary>
        //[DataMember(Order = 1)]
        public String TaskType;
        /// <summary>
        /// ID задания
        /// </summary>
        //[DataMember(Order = 2)]
        public String TaskId;
        /// <summary>
        /// Назначивший врач
        /// </summary>
        //[DataMember(Order = 3)]
        public String Doctor;
    }
}
