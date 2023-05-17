using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class DepartmentDictionaryItem : DictionaryItem 
    {
        [CSN("AllowDepartmentNr")]
        public Boolean AllowDepartmentNr { get; set; }

        [CSN("Micro")]
        public Boolean Micro { get; set; }
    }

    public class DepartmentDictionary : DictionaryClass<DepartmentDictionaryItem>
    {
        public DepartmentDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Department")]
        public List<DepartmentDictionaryItem> Department
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}