using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;
using System;
using ru.novolabs.SuperCore.DictionaryCommon;

namespace ru.novolabs.SuperCore
{
    public class HemClientDictionaryCache : HemDictionaryCache
    {
        public override void CreateDictionaries()
        {
            base.CreateDictionaries();
            AddDictionary(typeof(DictionaryClass<EmployeeDictionaryItem>), HemDictionaryNames.Employee);            
            AddDictionary(typeof(DictionaryClass<AccessRightDictionaryItem>), HemDictionaryNames.AccessRight);
            AddDictionary(typeof(DictionaryClass<DepartmentDictionaryItem>), HemDictionaryNames.Department);
            AddDictionary(typeof(UserProfileDictionary), HemDictionaryNames.UserProfile);
            AddDictionary(typeof(UserDirectoryDictionary), HemDictionaryNames.UserDirectory);
            AddDictionary(typeof(ParameterGroupDictionary<ParameterGroup>), HemDictionaryNames.ParameterGroup);
        }
    }

    public class BloodClient : HemCommunicator
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BloodClient
        /// </summary>
        public BloodClient() : base() { }

        protected override Type DictionaryCacheType()
        {
            return typeof(HemDictionaryCache);
        }
    }
}