using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class DefectType
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public DefectType()
        {

        }
        public DefectType(DefectTypeDictionaryItem defectType)
        {
            Name = defectType.Name;
            Code = defectType.Code;
            Mnemonics = defectType.Mnemonics;
            Removed = defectType.Removed;
        }
    }

    public class DefectTypesDictionary
    {
        public List<DefectType> DefectTypes = new List<DefectType>();
    }
}