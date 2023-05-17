using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.HL7
{
    [System.Reflection.Obfuscation]
    public class HelperSettings
    {
        public String LocalTCPHost { get; set; }
        public String RemoteTCPHost { get; set; }
        public Int32 LocalTCPPortNumber { get; set; }
        public Int32 RemoteTCPPortNumber { get; set; }
        public Int32 TimeOut { get; set; }
        public String PatientCodePrefix { get; set; }
    }
}
