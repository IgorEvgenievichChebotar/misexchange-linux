using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class EmployeeMessage : BaseEmployeeMessage
    {
        public EmployeeMessage() { }

        [CSN("Read")]
        public bool Read { get; set; }

        [CSN("Sender")]
        public string Sender { get; set; }
    }

    public class GetEmployeeMessagesResponse
    {
        public GetEmployeeMessagesResponse()
        {
            Messages = new List<EmployeeMessage>();
        }
        [CSN("Messages")]
        public List<EmployeeMessage> Messages { get; set; }
    }
}
