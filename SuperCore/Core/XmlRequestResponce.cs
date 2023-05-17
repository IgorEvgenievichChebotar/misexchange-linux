using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore
{
    [XmlType("ServerResponce")]
    public class XmlRequestResponce<T>
        where T : class
    {
        public T Content { get; set; }
    }
}
