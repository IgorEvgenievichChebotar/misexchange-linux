using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class Test
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public Test()
        {

        }

        public Test(TestDictionaryItem test)
        {
            Name = test.Name;
            Code = test.Code;
            Mnemonics = test.Mnemonics;
            Removed = test.Removed;
        }
    }

    public class TestsDictionary
    {
        public List<Test> Tests = new List<Test>();
    }
}
