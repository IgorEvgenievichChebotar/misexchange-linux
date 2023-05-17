using System;
using System.Runtime.Serialization;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    [DataContract]
    public class PayCategory
    {
        [DataMember(IsRequired = true, Order = 1)]
        public String Code { get; set; }
        [DataMember(IsRequired = true, Order = 2)]
        public String Name { get; set; }
    }
}