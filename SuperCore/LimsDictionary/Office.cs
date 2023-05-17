using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class OfficeDictionaryItem : DictionaryItem
    {
        [CSN("RequestNrTemplate")]
        public String RequestNrTemplate { get; set; }
        [CSN("Organization")]
        public OrganizationDictionaryItem Organization { get; set; }
    }

    public class OfficeDictionary : DictionaryClass<OfficeDictionaryItem>
    {
        public OfficeDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Office")]
        public List<OfficeDictionaryItem> Office
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
