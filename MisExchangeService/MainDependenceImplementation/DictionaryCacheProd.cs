using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    [Obsolete("Implementation of interface with weak-typed methods")]
    class DictionaryCacheProdOld : IDictionaryCacheOld
    {
        public SuperCore.DictionaryCore.IBaseDictionary this[string dictionaryName]
        {
            get { return ProgramContext.LisCommunicator.DictionaryCache[dictionaryName]; }
        }

        public object this[string dictionaryName, string elementCode, bool skipRemoved = true]
        {
            get { return ProgramContext.LisCommunicator.DictionaryCache[dictionaryName, elementCode, skipRemoved]; }
        }

        public object this[string dictionaryName, int elementId]
        {
            get { return ProgramContext.LisCommunicator.DictionaryCache[dictionaryName, elementId]; }
        }

        public SuperCore.DictionaryCore.DictionaryItem CreateItem(string dictionaryName, string code, string name, SuperCore.Core.BaseUserSession userSession)
        {
            return ProgramContext.LisCommunicator.DictionaryCache.CreateItem(dictionaryName, code, name, userSession);
        }

        public SuperCore.DictionaryCore.DictionaryItem CreateItem(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, SuperCore.Core.BaseUserSession userSession)
        {
            return ProgramContext.LisCommunicator.DictionaryCache.CreateItem(dictionaryName, code, name, objectType, parentType, parentValue, userSession);
        }

        public SuperCore.DictionaryCore.DictionaryItem CreateItemNonCachedDictionary(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, SuperCore.Core.BaseUserSession userSession)
        {
            return ProgramContext.LisCommunicator.DictionaryCache.CreateItemNonCachedDictionary(dictionaryName, code, name, objectType, parentType, parentValue, userSession);
        }
    }
    class DictionaryCacheProd : IDictionaryCache
    {
        static Dictionary<Type, string> _dictionaryMap = new Dictionary<Type, string>();
        static List<string> _dictionariesNames = new List<string>();

        static DictionaryCacheProd()
        {
            DictionaryInternalMapperBuilder.BuildMapping(_dictionaryMap);
            _dictionariesNames = _dictionaryMap.Values.ToList();
        }

        public T GetDictionary<T, U>()
            where T : SuperCore.DictionaryCore.DictionaryClass<U>
            where U : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(U));
            return (T)ProgramContext.LisCommunicator.DictionaryCache[dictionaryName];
        }
        public T GetDictionaryItem<T>(string elementCode, bool skipRemoved = true) where T : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(T));
            return (T)ProgramContext.LisCommunicator.DictionaryCache[dictionaryName, elementCode, skipRemoved];
        }

        public T GetDictionaryItem<T>(int elementId) where T : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(T));
            return (T)ProgramContext.LisCommunicator.DictionaryCache[dictionaryName, elementId];
        }

        public T CreateItem<T>(string code, string name) where T : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(T));
            return (T)ProgramContext.LisCommunicator.DictionaryCache.CreateItem(dictionaryName, code, name, ProgramContext.LisCommunicator.LimsUserSession);
        }

        public T UpdateItem<T>(SuperCore.DictionaryCore.DictionaryItem item) where T : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(T));
            return (T)ProgramContext.LisCommunicator.DictionaryCache.UpdateItem(dictionaryName, item, ProgramContext.LisCommunicator.LimsUserSession);
        }

        public T CreateItem<T, TParent>(string code, string name, TParent parentValue)
            where T : SuperCore.DictionaryCore.DictionaryItem
            where TParent : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(T));
            return (T)ProgramContext.LisCommunicator.DictionaryCache.CreateItem(dictionaryName, code, name, typeof(T), typeof(TParent), parentValue, ProgramContext.LisCommunicator.LimsUserSession);
        }

        public T CreateItemNonCachedDictionary<T, TParent>(string code, string name, TParent parentValue)
            where T : SuperCore.DictionaryCore.DictionaryItem
            where TParent : SuperCore.DictionaryCore.DictionaryItem
        {
            string dictionaryName = GetDicitonaryName(typeof(T));
            return (T)ProgramContext.LisCommunicator.DictionaryCache.CreateItemNonCachedDictionary(dictionaryName, code, name, typeof(T), typeof(TParent), parentValue, ProgramContext.LisCommunicator.LimsUserSession);
        }
        private string GetDicitonaryName(Type type)
        {
            if (!_dictionaryMap.ContainsKey(type))
                throw new KeyNotFoundException(String.Format("There is not found any dictionary for dicitonaryItmeType [{0}]. Plesase, register the type", type.Name));
            return _dictionaryMap[type];
        }

        public List<string> DictionaryNamesList
        {
            get { return _dictionariesNames; }
        }
    }
    static class DictionaryInternalMapperBuilder
    {
        public static void BuildMapping(Dictionary<Type, string> mappingDictionary)
        {
            mappingDictionary.Add(typeof(BiomaterialDictionaryItem), LimsDictionaryNames.Biomaterial);
            mappingDictionary.Add(typeof(CustDepartmentDictionaryItem), LimsDictionaryNames.CustDepartment);
            mappingDictionary.Add(typeof(DepartmentDictionaryItem), LimsDictionaryNames.Department);
            mappingDictionary.Add(typeof(DoctorDictionaryItem), LimsDictionaryNames.Doctor);
            mappingDictionary.Add(typeof(EmployeeDictionaryItem), LimsDictionaryNames.Employee);
            mappingDictionary.Add(typeof(EquipmentDictionaryItem), LimsDictionaryNames.Equipment);
            mappingDictionary.Add(typeof(ExternalSystemDictionaryItem), LimsDictionaryNames.ExternalSystem);
            mappingDictionary.Add(typeof(HospitalDictionaryItem), LimsDictionaryNames.Hospital);
            mappingDictionary.Add(typeof(MicroOrganismDictionaryItem), LimsDictionaryNames.MicroOrganism);
            mappingDictionary.Add(typeof(PayCategoryDictionaryItem), LimsDictionaryNames.PayCategory);
            mappingDictionary.Add(typeof(RequestCustomStateDictionaryItem), LimsDictionaryNames.RequestCustomState);
            mappingDictionary.Add(typeof(RequestFormDictionaryItem), LimsDictionaryNames.RequestForm);
            mappingDictionary.Add(typeof(SampleBlankDictionaryItem), LimsDictionaryNames.SampleBlank);
            mappingDictionary.Add(typeof(ServiceDictionaryItem), LimsDictionaryNames.ServiceShort);
            mappingDictionary.Add(typeof(TargetDictionaryItem), LimsDictionaryNames.Target);
            mappingDictionary.Add(typeof(TestDictionaryItem), LimsDictionaryNames.Test);
            mappingDictionary.Add(typeof(PatientGroupDictionaryItem), LimsDictionaryNames.PatientGroup);
            mappingDictionary.Add(typeof(UserDirectoryDictionaryItem), LimsDictionaryNames.UserDirectory);
            mappingDictionary.Add(typeof(UserFieldDictionaryItem), LimsDictionaryNames.UserField);
            mappingDictionary.Add(typeof(UnitDictionaryItem), LimsDictionaryNames.Unit);
            mappingDictionary.Add(typeof(DefectTypeDictionaryItem), LimsDictionaryNames.DefectType);
            mappingDictionary.Add(typeof(OrganizationDictionaryItem), LimsDictionaryNames.Organization);
            mappingDictionary.Add(typeof(SexDictionaryItem), LimsDictionaryNames.Sex);
            mappingDictionary.Add(typeof(UserDictionaryValue), LimsDictionaryNames.UserDirectoryValue);
            mappingDictionary.Add(typeof(FundingSourceDictionaryItem), LimsDictionaryNames.FundingSource);
            mappingDictionary.Add(typeof(SupplierDictionaryItem), LimsDictionaryNames.Supplier);
            mappingDictionary.Add(typeof(ManufacturerDictionaryItem), LimsDictionaryNames.Manufacturer);
            mappingDictionary.Add(typeof(MaterialUnitDictionaryItem), LimsDictionaryNames.MaterialUnit);
            mappingDictionary.Add(typeof(MaterialDictionaryItem), LimsDictionaryNames.Material);
            mappingDictionary.Add(typeof(UnitTreeNodeDictionaryItem), LimsDictionaryNames.UnitTreeNode);
        }
    }
}
