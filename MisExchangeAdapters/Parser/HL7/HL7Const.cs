using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MisExchangeAdapters.Parser.HL7
{
    internal class HL7SeparatorConst
    {
        internal const char DefaultFieldDelim = '|';
        internal const char ComponentSeparator = '^';
        internal const char SubComponentSeparator = '&';
        internal const char RepetitionSeparator = '-';
        internal const char EscapeCharacter = '\\';
    }

    internal class HL7Sex
    {
        public const String Male = "M";
        public const String Female = "F";
        public const String Other = "O";
        public const String Unknown = "U";
    }

    internal class HL7BlockConst
    {
        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        internal const String MSH = "MSH";
        /// <summary>
        /// Информация о пациенте
        /// </summary>
        internal const String PID = "PID";
        /// <summary>
        /// Информация о визите пациента
        /// </summary>
        internal const String PV1 = "PV1";
        /// <summary>
        /// Комментарии
        /// </summary>
        internal const String NTE = "NTE";
        /// <summary>
        /// Блок заданий
        /// </summary>
        internal const String ORC = "ORC";
        /// <summary>
        /// Запросы на исследования
        /// </summary>
        internal const String OBR = "OBR";
        /// <summary>
        /// Результаты исследований
        /// </summary>
        internal const String OBX = "OBX";
        /// <summary>
        /// Тип события
        /// </summary>
        internal const String MSA = "MSA";
        /// <summary>
        /// Ошибка, наверн
        /// </summary>
        internal const String ERR = "ERR";
    }
}
