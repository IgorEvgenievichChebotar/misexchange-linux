using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces
{
    [Obsolete("Interface with weak-typed methods")]
    public interface IDictionaryCacheOld
    {
        IBaseDictionary this[string dictionaryName] { get; }

        object this[string dictionaryName, string elementCode, bool skipRemoved = true] { get; }

        object this[string dictionaryName, int elementId] { get; }

        DictionaryItem CreateItem(string dictionaryName, string code, string name, BaseUserSession userSession);

        DictionaryItem CreateItem(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, BaseUserSession userSession);

        DictionaryItem CreateItemNonCachedDictionary(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, BaseUserSession userSession);
    }
    public interface IDictionaryCache
    {
        T GetDictionary<T, U>() where T : DictionaryClass<U> where U : DictionaryItem;

        T GetDictionaryItem<T>(string elementCode, bool skipRemoved = true) where T : DictionaryItem;

        T GetDictionaryItem<T>(int elementId) where T : DictionaryItem;

        T CreateItem<T>(string code, string name) where T : DictionaryItem;

        T UpdateItem<T>(DictionaryItem item) where T : DictionaryItem;

        T CreateItem<T, TParent>(string code, string name, TParent parentValue)
            where T : DictionaryItem
            where TParent : DictionaryItem;

        T CreateItemNonCachedDictionary<T, TParent>(string code, string name, TParent parentValue)
            where T : DictionaryItem
            where TParent : DictionaryItem;

        List<string> DictionaryNamesList { get; }
    }
}