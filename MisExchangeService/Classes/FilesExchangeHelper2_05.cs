using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.Classes
{
    public class Acknowledgment
    {
        public string RequestCode { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }

    public class StatusTypes
    {
        public const string STATUS_SUCCESS = "Success";
        public const string STATUS_ERROR = "Error";
    }
}