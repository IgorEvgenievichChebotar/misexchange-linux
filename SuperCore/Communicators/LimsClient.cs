using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;

namespace ru.novolabs.SuperCore
{
    public class LimsClientDictionaryCache : LimsDictionaryCache
    {
        public override void CreateDictionaries()
        {
            base.CreateDictionaries();
            AddDictionary(typeof(EmployeeDictionary), LimsDictionaryNames.Employee);
            AddDictionary(typeof(WorkPlaceDictionary), LimsDictionaryNames.WorkPlace);
            AddDictionary(typeof(DepartmentDictionary), LimsDictionaryNames.Department);
            AddDictionary(typeof(AccessRightDictionary), LimsDictionaryNames.AccessRight);
        }
    }

    public class LimsClient : LimsCommunicator
    {
        protected override Type DictionaryCacheType()
        {
            return typeof(LimsClientDictionaryCache);
        }
    }
}