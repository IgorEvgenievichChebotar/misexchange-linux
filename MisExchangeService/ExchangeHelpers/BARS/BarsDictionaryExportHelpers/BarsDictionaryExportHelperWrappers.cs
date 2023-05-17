using LisServiceClients.BarsNomenclature;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.BARS
{
    public class NomenclatureElement
    {
        public NomenclatureElement()
        { }
        public NomenclatureElement(string code, string dictionaryName, bool isRemoved)
        {
            var dictElements = ((IEnumerable<DictionaryItem>)ProgramContext.Dictionaries[dictionaryName].DictionaryElements).Where(d => d.Removed == isRemoved && d.Code == code);
            if (dictElements.Count() > 0)
                Id = dictElements.First().Id;
            else
                Id = -1;
            Code = code;
            var dictionaryElement = (DictionaryItem)ProgramContext.Dictionaries[dictionaryName, code];
            if (dictionaryElement != null)
                Version = dictionaryElement.Version;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public Nullable<DateTime> Version { get; set; }
    }

    public class TestWrapper : Test
    {
        public TestWrapper()
        { }
        public TestWrapper(Test test)
        {
            Id = ((TestDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Test, test.research_code]).Id;
            research_code = test.research_code;
            research_name = test.research_name;        
        }

        [XmlElement(Order = 2)]
        public int Id { get; set; }
    }
    public class BioWrapper : Bio
    {
        public BioWrapper()
        { }
        public BioWrapper(Bio bio)
        {
            Id = ((BiomaterialDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial, bio.bio_code]).Id;
            bio_code = bio.bio_code;
            bio_name = bio.bio_name;       
        }

        [XmlElement(Order = 2)]
        public int Id { get; set; }
    }
}
