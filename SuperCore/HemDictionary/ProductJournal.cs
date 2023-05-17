using System;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class ProductJournalDictionaryItem : DictionaryItem
    {
        public ProductJournalDictionaryItem()
        {
            Departments = new List<DepartmentDictionaryItem>();
            UserGroups = new List<UserGroupDictionaryItem>();
            Filters = new List<ProductFilterDictionaryItem>();
        }

        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("ShowDateInterval")]
        public Boolean ShowDateInterval { get; set; }
        [CSN("DateInterval")]
        public Int32 DateInterval { get; set; }
        [CSN("Departments")]
        public List<DepartmentDictionaryItem> Departments { get; set; }
        [CSN("UserGroups")]
        public List<UserGroupDictionaryItem> UserGroups { get; set; }
        [CSN("Filters")]
        public List<ProductFilterDictionaryItem> Filters { get; set; }
        [CSN("UserFilters")]
        public List<ProductFilterDictionaryItem> UserFilters { get; set; }
    }
}
