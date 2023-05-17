using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class HemServerResponse : BaseObject
    {
        public HemServerResponse()
        {
            Errors = new List<ErrorMessage>();
        }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }
}
