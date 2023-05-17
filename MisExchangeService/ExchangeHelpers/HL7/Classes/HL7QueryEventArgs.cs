using ru.novolabs.ExchangeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.HL7.Classes
{
    public class HL7QueryEventArgs: EventArgs
    {
        public Request Request { get; set; }
    }
}
