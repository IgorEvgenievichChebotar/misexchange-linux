using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.DictionaryCore
{
    public class DictionaryNamesMapping
    {
        public DictionaryNamesMapping()
        {
            DictionaryList = new List<DictionaryMapping>();
        }

        public List<DictionaryMapping> DictionaryList { get; set; }
    }
    
    public class StaticDictionaryElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Mnemonics { get; set; }
    }

    public class DictionaryMapping
    {
        public DictionaryMapping()
        {
            PrimaryKeyPropName = "id";
            DefaultDisplayPropName = "Name";
        }

        public String DictionaryName { get; set; }
        public String TableName { get; set; }
        public String PrimaryKeyPropName { get; set; }
        public String DefaultDisplayPropName { get; set; }
        public String QueryCustom { get; set; }
        public List<StaticDictionaryElement> Elements { get; set; }
    }
}
