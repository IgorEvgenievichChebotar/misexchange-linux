using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class EmployeeRoleDictionaryItem : DictionaryItem
    {
        public EmployeeRoleDictionaryItem()
        {
            Employees = new List<EmployeeDictionaryItem>();
            Targets = new List<TargetDictionaryItem>();
        }

        /// <summary>
        /// Множество ссылок на сотрудников, которые могут выполнять эту роль
        /// </summary>
        [CSN("Employees")]
        public List<EmployeeDictionaryItem> Employees { get; set; }
        /// <summary>
        /// Множество ссылок на исследования, которые может выполнять эта роль
        /// </summary>
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
    }

    public class EmployeeRoleDictionary : DictionaryClass<EmployeeRoleDictionaryItem>
    {
        public EmployeeRoleDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("EmployeeRole")]
        public List<EmployeeRoleDictionaryItem> EmployeeRole
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}