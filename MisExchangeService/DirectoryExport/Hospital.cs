using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    [DataContract]
    public class Hospital
    {
        public Hospital()
        {
            Departments = new List<Department>();
        }

        [DataMember(IsRequired = true, Order = 1)]
        public String Code { get; set; }
        [DataMember(IsRequired = true, Order = 2)]
        public String Name { get; set; }

        [DataMember(Order = 3)]
        public List<Department> Departments { get; set; }
    }
}
