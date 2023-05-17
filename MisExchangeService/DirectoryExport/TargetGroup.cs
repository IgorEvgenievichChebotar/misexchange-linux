using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    [DataContract]
    public class TargetGroup
    {        
        public TargetGroup()
        {
            Targets = new List<Target>();
        }

        [DataMember(IsRequired = true, Order = 1)]
        public String Code { get; set; }
        [DataMember(IsRequired = true, Order = 2)]
        public String Name { get; set; }

        [DataMember(Order = 3)]
        public List<TargetGroup> SubGroups { get; set; }
        [DataMember(IsRequired = true, Order = 4)]
        public List<Target> Targets { get; set; }
    }
}
