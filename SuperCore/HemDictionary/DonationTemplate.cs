using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class DonationTemplateDictionaryItem : DictionaryItem
    {
        public DonationTemplateDictionaryItem()
        {
            ProductTemplates = new List<ProductTemplate>();
            Departments = new List<ObjectRef>();
        }

        [CSN("SystemBloodVolume")]
        public int SystemBloodVolume { get; set; }
        [CSN("ProductTemplates")]
        public List<ProductTemplate> ProductTemplates { get; set; }
        [CSN("Departments")]
        public List<ObjectRef> Departments { get; set; }
    }
}
