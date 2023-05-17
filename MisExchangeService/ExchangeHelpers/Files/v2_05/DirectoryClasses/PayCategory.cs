using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses
{
    public class PayCategory
    {
        [XmlAttribute] public String Name { get; set; }
        [XmlAttribute] public String Code { get; set; }
        [XmlAttribute] public String Mnemonics { get; set; }
        [XmlAttribute] public Boolean Removed { get; set; }

        public PayCategory()
        {

        }
        public PayCategory(PayCategoryDictionaryItem payCategory)
        {
            Name = payCategory.Name;
            Code = payCategory.Code;
            Mnemonics = payCategory.Mnemonics;
            Removed = payCategory.Removed;
        }
    }

    public class PayCategoriesDictionary
    {
        public List<PayCategory> PayCategories = new List<PayCategory>();
    }
}
