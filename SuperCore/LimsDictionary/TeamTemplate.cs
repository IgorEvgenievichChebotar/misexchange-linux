using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    /// <summary>
    /// Класс, описывающий члена бригады (смены) сотрудников
    /// </summary>
    public class TeamMember : BaseObject
    {
        /// <summary>
        /// Cсылка на роль сотрудника
        /// </summary>
        [CSN("Role")]
        public EmployeeRoleDictionaryItem Role { get; set; }

        /// <summary>
        /// Cсылка на сотрудника
        /// </summary>
        [CSN("Employee")]
        public EmployeeDictionaryItem Employee { get; set; }

        
        // Вычисляемые свойства. Серверу не отправляются
        [SendToServer(false)]
        [CSN("RoleName")]
        public string RoleName 
        { 
            get { return (Role != null) && (Role.Id > 0) ? Role.Name : String.Empty; }
            set 
            {
                if ((Role == null) || (Role.Name != value))
                    Role = ProgramContext.Dictionaries.FindNotRemovedItemByName<EmployeeRoleDictionaryItem>(value);
            }
        }

        [SendToServer(false)]
        [CSN("EmployeeName")]
        public string EmployeeName 
        { 
            get { return (Employee != null) && (Employee.Id > 0) ? Employee.Name : String.Empty; }
            set
            {
                if ((Employee == null) || (Employee.Name != value))
                    Employee = ProgramContext.Dictionaries.FindNotRemovedItemByName<EmployeeDictionaryItem>(value);
            }
        }

        [SendToServer(false)]
        [CSN("Checked")]
        public bool Checked { get; set; }
    }

    /// <summary>
    /// Класс, описывающий шаблон бригады (смены) сотрудников
    /// </summary>
    [OldSaveMethod]
    public class TeamTemplateDictionaryItem : DictionaryItem 
    {
        public TeamTemplateDictionaryItem()
        {
            Members = new List<TeamMember>();
        }

        /// <summary>
        /// Члены бригады (смены) сотрудников
        /// </summary>
        [CSN("Members")]
        public List<TeamMember> Members { get; set; }
    }

    public class TeamTemplateDictionary : DictionaryClass<TeamTemplateDictionaryItem>
    {
        public TeamTemplateDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("TeamTemplate")]
        public List<TeamTemplateDictionaryItem> TeamTemplate
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}