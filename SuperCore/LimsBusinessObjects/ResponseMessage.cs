using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class ResponseMessage
    {
        [CSN("Severity")]
        public int Severity { get; set; }
        [CSN("Message")]
        public String Message { get; set; }
        [CSN("SystemSeverity")]
        public int SystemSeverity { get; set; }
    }
}
