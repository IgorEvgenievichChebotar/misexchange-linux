using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class Microorganism
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public Microorganism()
        {

        }
        public Microorganism(MicroOrganismDictionaryItem microorganism)
        {
            Name = microorganism.Name;
            Code = microorganism.Code;
            Mnemonics = microorganism.Mnemonics;
            Removed = microorganism.Removed;
        }
    }

    public class MicroorganismsDictionary
    {
        public List<Microorganism> Microorganisms = new List<Microorganism>();
    }
}