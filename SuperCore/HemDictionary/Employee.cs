using System;
using System.Linq;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.DictionaryCommon;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class AccessAddress
    {
        public string ip = string.Empty;
    }

    public class EmployeeDictionaryItem : DictionaryItem, IEmployee
    {
        public EmployeeDictionaryItem()
        {
            Login = String.Empty;
            Password = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            MiddleName = String.Empty;
            Profession = String.Empty;
            DepartmentAccess = new List<DepartmentDictionaryItem>();
            UserGroups = new List<UserGroupDictionaryItem>();
            Rights = new List<AccessRightDictionaryItem>();
        }

        public Boolean HasRight(Int32 right)
        {
            //return Rights.Exists(r => r.Id1 == right) || UserGroups.Exists(g => g.Rights.Exists(r => r.Id1 == right));
            return Rights.Exists(r => r.Id1 == right) || UserGroups.Exists(g => g.Rights.Exists(r => r.Id == right));
        }

        /// <summary>
        /// Возвращает результат проверки - входит ли пользователь в любую из групп пользователей
        /// </summary>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        public bool IsInAnyUserGroup(List<UserGroupDictionaryItem> userGroups)
        {
            return this.UserGroups.Intersect(userGroups, new DictionaryItemEqualityComparer()).Count() > 0;
        }

        /// <summary>
        /// Возвращает результат проверки - входит ли пользователь в любую из групп пользователей
        /// </summary>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        public bool IsInAnyUserGroup(List<ObjectRef> userGroups)
        {
            foreach (ObjectRef userGroup in userGroups)
            {
                if (this.UserGroups.Exists(ug => ug.Id == userGroup.Id))
                    return true;                
            }
            return false;
        }

        /// <summary>
        /// Возвращает результат проверки - имеет ли пользователь доступ к любому из подразделений(филиалов)
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public bool HasAccessToAnyDepartment(List<DepartmentDictionaryItem> departments)
        {
            return this.DepartmentAccess.Intersect(departments, new DictionaryItemEqualityComparer()).Count() > 0;
        }

        /// <summary>
        /// Возвращает результат проверки - имеет ли пользователь доступ к любому из подразделений(филиалов)
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public bool HasAccessToAnyDepartment(List<ObjectRef> departments)
        {
            foreach (ObjectRef department in departments)
            {
                if (this.DepartmentAccess.Exists(dep => dep.Id == department.Id))
                    return true;
            }
            return false;
        }

        private string getShortName()
        {
            String result = String.Empty;

            if (!String.IsNullOrEmpty(LastName))
                result = LastName + " ";

            if (!String.IsNullOrEmpty(FirstName))
                result += FirstName.Substring(0, 1) + ".";

            if (!String.IsNullOrEmpty(MiddleName))
                result += MiddleName.Substring(0, 1) + ".";

            return result;
        }

        [CSN("Login")]
        public String Login { get; set; }
        [CSN("Password")]
        public String Password { get; set; }
        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("Profession")]
        public String Profession { get; set; }
        [CSN("AccessAllDepartments")]
        public Boolean AccessAllDepartments { get; set; }
        [CSN("DepartmentAccess")]
        public List<DepartmentDictionaryItem> DepartmentAccess { get; set; }      
        [CSN("HomeDepartment")]
        public DepartmentDictionaryItem HomeDepartment { get; set; }
        [CSN("UserGroups")]
        public List<UserGroupDictionaryItem> UserGroups { get; set; }
        [CSN("UserProfile")]
        public UserProfileDictionaryItem UserProfile { get; set; }
        [CSN("Rights")]
        public List<AccessRightDictionaryItem> Rights { get; set; }
        [SendToServer(false)]
        [CSN("ShortName")]
        public String ShortName { get { return getShortName(); } }

        [CSN("Name")]
        public String Name { get { return getShortName(); } }

        [CSN("IsDoctor")]
        public Boolean IsDoctor { get; set; }
    }

    public class EmployeeDictionaryClass<Class> : DictionaryClass<Class> where Class : DictionaryItem
    {

        public EmployeeDictionaryClass(string DictionaryName)
            : base(DictionaryName)
        {
            name = DictionaryName;                        
        }

        protected override int Compare(Class a, Class b)
        {
            Object ObjA = (Object)a;
            Object ObjB = (Object)b;
            return ((EmployeeDictionaryItem)(ObjA)).LastName.CompareTo(((EmployeeDictionaryItem)(ObjB)).LastName);
        }

    }
}
