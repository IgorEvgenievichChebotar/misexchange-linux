using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public struct DelphiDateTime
    {
        public static DateTime MinValue = new DateTime(1899, 12, 30, 00, 00, 00, 00);
    }
    
    public static class DateTimeHelper
    {   
        public static DateTime StartOfTheDay(this DateTime dt)
        {
            return dt.Date;
        }

        public static DateTime? StartOfTheDay(this DateTime? dt)
        {
            if (dt != null)
                return dt.Value.Date;
            else
                return null;
        }

        public static DateTime EndOfTheDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        public static DateTime? EndOfTheDay(this DateTime? dt)
        {
            if (dt != null)
                return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, 23, 59, 59, 999);
            else
                return null;
        }

        public static Boolean IsMultilingualMinValue(this DateTime dt)
        { 
            if ((dt == DateTime.MinValue) || (dt == DelphiDateTime.MinValue))
                return true;
            else
                return false;
        }

        public static string DelphiDateTimeFormat_To_dotnetTimeFormat(string format)
        {
            // Наиболее часто используемые Delphi-форматы
            //RsShortestDateFormat = 'dd.mm.yy';
            //RsShortDateFormat = 'dd.mm.yyyy';
            //RsLongDateFormat = 'dd.mm.yy hh:nn';

            string result = format;
            // Delphi -> dotnet
            //   mm   -> MM  - month
            //   hh   -> HH  - hours in 24-hour format
            //   nn   -> mm  - minutes
            return format.Replace("mm", "MM").Replace("hh", "HH").Replace("nn", "mm");
        }
    }
}
