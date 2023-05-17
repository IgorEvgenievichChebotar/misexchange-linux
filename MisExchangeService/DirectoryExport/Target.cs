using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    [DataContract]
    public class Target
    {
        public Target()
        {
            Biomaterials = new List<Biomaterial>();
            Tests = new List<Test>();
        }

        [DataMember(IsRequired = true, Order = 1)]
        public String Code { get; set; }
        [DataMember(IsRequired = true, Order = 2)]
        public String Name { get; set; }
        [DataMember(IsRequired = true, Order = 3)]
        public List<Biomaterial> Biomaterials { get; set; }
        [DataMember(IsRequired = true, Order = 4)]
        public List<Test> Tests { get; set; }
        [DataMember(IsRequired = true, Order = 5)]
        public Boolean RequiresAdditionalTube { get; set; }
    }
}