using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class Target
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }
        [XmlAttribute] public Boolean RequiresAdditionalTube { get; set; }
        public List<Biomaterial> Biomaterials { get; set; } = new List<Biomaterial>();
        public List<Test> Tests { get; set; } = new List<Test>(); 
    }

    public class TargetsDictionary
    {
        public List<Target> Targets = new List<Target>();
    }
}
