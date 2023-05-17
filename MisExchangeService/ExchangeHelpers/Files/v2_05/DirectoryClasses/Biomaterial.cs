using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class Biomaterial
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public Biomaterial()
        {

        }
        public Biomaterial(BiomaterialDictionaryItem biom)
        {
            Name = biom.Name;
            Code = biom.Code;
            Mnemonics = biom.Mnemonics;
            Removed = biom.Removed;
        }
    }

    public class BiomaterialsDictionary
    {
        public List<Biomaterial> Biomaterials = new List<Biomaterial>();
    }
}