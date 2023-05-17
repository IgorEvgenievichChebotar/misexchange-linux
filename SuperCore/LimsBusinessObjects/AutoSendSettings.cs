using System;
using System.Reflection;

namespace ru.novolabs.lims.drivers
{
    [Obfuscation]
    public class AutoSendSettings
    {
        public Int32 Department { get; set; }
        public Int32 Equipment { get; set; }
        public Int32 MaxSamplesPerSession { get; set; }
        public Int32 QueryPeriod { get; set; }
        public String WorklistCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTill { get; set; }

        public AutoSendSettings()
        {
            Department = 0;
            Equipment = 0;
            MaxSamplesPerSession = 0;
            QueryPeriod = 3600;
            WorklistCode = null;
        }
    }
}