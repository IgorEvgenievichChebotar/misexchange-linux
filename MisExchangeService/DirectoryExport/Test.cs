using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    [DataContract]
    public class Test
    {
        [DataMember(IsRequired = true, Order = 1)]
        public String Code { get; set; }
        [DataMember(IsRequired = true, Order = 2)]
        public String Name { get; set; }
    }
}
