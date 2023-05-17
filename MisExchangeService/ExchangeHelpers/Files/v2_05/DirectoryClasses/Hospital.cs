using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class Hospital
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public Hospital()
        {

        }
        public Hospital(HospitalDictionaryItem hospital)
        {
            Name = hospital.Name;
            Code = hospital.Code;
            Mnemonics = hospital.Mnemonics;
            Removed = hospital.Removed;
        }
    }

    public class HospitalsDictionary
    {
        public List<Hospital> Hospitals = new List<Hospital>();
    }
}
