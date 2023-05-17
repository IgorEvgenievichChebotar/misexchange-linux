using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace ru.novolabs.SuperCore.DictionaryCore
{
    public class DictionaryLoadingEventArgs : EventArgs
    {
        public DictionaryLoadingEventArgs(int progressPecentage, string dictionaryName)
        {
            ProgressPecentage = progressPecentage;
            DictionaryName = dictionaryName;
        }

        public int ProgressPecentage { get; private set; }
        public string DictionaryName { get; private set; }
    }

    public class BaseDictionaryCache
    {
        public Hashtable DictionaryList = new Hashtable();

        public string StaticPath = string.Empty;

        public event EventHandler<DictionaryLoadingEventArgs> DictionaryLoadingEvent;

        [CSN("Communicator")]
        public BaseCommunicator Communicator { get; set; }

        public void AddDictionary(Type type, string name)
        {
            if (DictionaryList.ContainsKey(name))
            {
                throw new InvalidOperationException(string.Format("Справочник [{0}] уже зарегистрирован. Исправьте реализацию метода CreateDictionaries() класса, производного от \"BaseDictionaryCache\"", name));
            }

            DictionaryList.Add(name, Activator.CreateInstance(type, new object[] { name }));
        }

        public BaseDictionaryCache()
        {
            StaticPath = string.Empty; // Определить путь к xml-файлу со статическими справочниками в наследнике
            CreateDictionaries();
        }

        public virtual void CreateDictionaries()
        { }

        /// <summary>
        /// Загружает динамические справочники с файлового кэша (при необходимости обновляет с сервера) и статические файлы из XML-файла
        /// </summary>
        /// <param name="userSession"></param>
        public void LoadDictionaries(BaseUserSession userSession)
        {
            float counter = 0;
            CacheHelper.GetCachedDictionaries(DictionaryLoadingEvent, userSession);
            foreach (DictionaryEntry entry in DictionaryList)
            {
                if (((IBaseDictionary)entry.Value).Version != 0)
                {
                    continue;
                }

                counter++;
                int progressPercentage = (int)(counter * 100) / DictionaryList.Count;
                // Если какой-либо объект подписан на событие загрузки справочника, инициируем событие
                if (DictionaryLoadingEvent != null)
                {
                    DictionaryLoadingEvent(this, new DictionaryLoadingEventArgs(progressPercentage, ((IBaseDictionary)entry.Value).Name));
                }

                LoadDictionary(entry.Value, userSession);
            }
            UpdateReferences();
            Log.WriteText("Dictionaries loaded");
            Log.WriteText("-------------------------------------------------------------------------------------------------------------");
        }
        /// <summary>
        /// Устанавливает новое значение справочника, обновляя все его элементы и версию из нового значения
        /// </summary>
        /// <param name="newValue">Новое значение справочника</param>
        private void SetDictionary(IBaseDictionary newValue)
        {
            foreach (DictionaryItem item in newValue.DictionaryElements)
            {
                ((IBaseDictionary)DictionaryList[newValue.Name]).UpdateElement(item);
            } ((IBaseDictionary)DictionaryList[newValue.Name]).Version = newValue.Version;
        }
        /// <summary>
        /// Проверка - является ли словарь статическим
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns>Truе если словарь статический. Иначе false</returns>
        public bool IsStaticDictionary(IBaseDictionary dictionary)
        {
            Type type = (dictionary).DictionaryElements.GetType().GetGenericArguments()[0];
            object[] attributes = type.GetCustomAttributes(typeof(StaticDictionary), true);
            foreach (object attribute in attributes)
            {
                if (((StaticDictionary)attribute).IsStatic)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadDictionaries(LimsUserSession userSession)
        {
            float counter = 0;
            foreach (DictionaryEntry entry in DictionaryList)
            {
                counter++;
                int progressPercentage = (int)(counter * 100) / DictionaryList.Count;
                // Если какой-либо объект подписан на событие загрузки справочника, инициируем событие
                if (DictionaryLoadingEvent != null)
                {
                    DictionaryLoadingEvent(this, new DictionaryLoadingEventArgs(progressPercentage, ((IBaseDictionary)entry.Value).Name));
                }

                LoadDictionary(entry.Value, userSession);
            }

            UpdateReferences();
            Log.WriteText("Dictionaries loaded");
            Log.WriteText("-------------------------------------------------------------------------------------------------------------");
        }


        private void UpdateObjectRefs(object parentObject, PropertyInfo propInfo, object propValue, object[] index = null)
        {
            if (propValue == null)
            {
                return;
            }
            // Если текущий объект является совместимым с классом справочного объекта
            if (propInfo.PropertyType.IsSubclassOf(typeof(DictionaryItem)))
            {
                DictionaryItem value = (DictionaryItem)propValue;
                if ((value != null) && (value.Id > 0))
                {
                    object dictionaryItem = GetItemByReference(propInfo.PropertyType, value.Id);
                    if (dictionaryItem != null)
                    {
                        propInfo.SetValue(parentObject, dictionaryItem, index);
                    }
                }
            }
            // Если текущий объект является экземпляром класса, отличного от справочного
            else
            {
                // Если текущий объект является коллекцией объектов, но не массивом
                if ((propInfo.PropertyType.GetInterface(typeof(IList<Type>).Name) != null) && (!propInfo.PropertyType.IsArray))
                {
                    IList list = (IList)propValue;
                    for (int i = 0; i < list.Count; i++)
                    {
                        UpdateObjectRefs(list, list.GetType().GetProperty("Item", new Type[] { typeof(int) }), list[i], new object[] { i });
                    }
                }
                // Если текущий объект является экземпляром единичного класса
                else
                {
                    if (!(propInfo.PropertyType.GetInterface(typeof(IDictionary<Type, Type>).Name) != null))
                    {
                        foreach (PropertyInfo propertyInfo in propValue.GetType().GetProperties())
                        {
                            if ((propertyInfo.PropertyType.IsClass) && (!propertyInfo.PropertyType.Equals(typeof(string))))
                            {
                                UpdateObjectRefs(propValue, propertyInfo, propertyInfo.GetValue(propValue, null));
                            }
                        }
                    }
                }
            }
        }

        public void UpdateReferences()
        {
            foreach (DictionaryEntry entry in DictionaryList)
            {
                Type dictionaryItemType = ((IBaseDictionary)entry.Value).DictionaryElements.GetType().GetGenericArguments()[0];
                IList elements = ((IBaseDictionary)entry.Value).DictionaryElements;
                foreach (object element in elements)
                {
                    foreach (PropertyInfo propInfo in dictionaryItemType.GetProperties())
                    {
                        if ((propInfo.PropertyType.IsClass) && (!propInfo.PropertyType.Equals(typeof(string))))
                        {
                            object propValue = propInfo.GetValue(element, null);
                            UpdateObjectRefs(element, propInfo, propValue);
                        }
                    }
                }
            }
        }

        public void LoadDictionary(object dictionary, BaseUserSession userSession)
        {
            bool loaded = false;
            Type type = ((IBaseDictionary)dictionary).DictionaryElements.GetType().GetGenericArguments()[0];
            object[] attributes = type.GetCustomAttributes(typeof(StaticDictionary), true);
            foreach (object attribute in attributes)
            {
                if (((StaticDictionary)attribute).IsStatic)
                {
                    LoadStaticDictionary(dictionary);
                    loaded = true;
                    break;
                }
            }
            if (!loaded)
            {
                LoadServerDictionary(dictionary, userSession);
            }
        }

        public void LoadDictionary(object dictionary, LimsUserSession userSession)
        {
            bool loaded = false;
            Type type = ((IBaseDictionary)dictionary).DictionaryElements.GetType().GetGenericArguments()[0];
            object[] attributes = type.GetCustomAttributes(typeof(StaticDictionary), true);
            foreach (object attribute in attributes)
            {
                if (((StaticDictionary)attribute).IsStatic)
                {
                    LoadStaticDictionary(dictionary);
                    loaded = true;
                    break;
                }
            }
            if (!loaded)
            {
                LoadServerDictionary(dictionary, userSession);
            }
        }


        private bool LoadStaticDictionary(object Dictionary)
        {
            string DictionaryName = ((IBaseDictionary)Dictionary).Name;
            Log.WriteText("Loading static dictionary " + DictionaryName);

            ObjectReader Reader = new ObjectReader();

            Reader.ReadXMLObjectFromFile(StaticPath, Dictionary, DictionaryName);

            return true;
        }

        public bool RefInList(IList List, int RefId)
        {
            foreach (ObjectRef Item in List)
            {
                if (Item.GetRef().Equals(RefId))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateDictionaies(BaseUserSession userSession)
        {
            List<DirectoryVersionInfo> versions = Communicator.DirectoryVersions(userSession);
            foreach (DictionaryEntry entry in DictionaryList)
            {
                Type type = ((IBaseDictionary)entry.Value).DictionaryElements.GetType().GetGenericArguments()[0];
                if (type.GetCustomAttributes(typeof(StaticDictionary), true).Length > 0)
                {
                    continue;
                }

                IBaseDictionary dictionary = (IBaseDictionary)entry.Value;
                int version = versions.Find(dvi => dvi.Name.Equals(dictionary.Name)).Version;
                if (version != dictionary.Version)
                {
                    LoadServerDictionary(entry.Value, userSession);
                }
            }

        }

        public bool LoadServerDictionary(object dictionary, BaseUserSession userSession)
        {
            string dictionaryName = string.Empty;
            try
            {
                dictionaryName = ((IBaseDictionary)dictionary).Name;
                Log.WriteText("Loading server dictionary " + dictionaryName);
                Communicator.GetDictionary(dictionaryName, dictionary, userSession);
                ((IBaseDictionary)dictionary).Prepare();
                ((IBaseDictionary)dictionary).Sort();
            }
            catch (Exception ex)
            {
                Log.WriteError("Can not load dictionary \"{0}\". Reason: {1}", dictionaryName, ex.ToString());
                throw ex;
            }
            return true;
        }

        public bool LoadServerDictionary(object dictionary, LimsUserSession userSession)
        {
            string dictionaryName = string.Empty;
            try
            {
                dictionaryName = ((IBaseDictionary)dictionary).Name;
                Log.WriteText("Loading server dictionary " + dictionaryName);
                Communicator.GetDictionary(dictionaryName, dictionary, userSession);
                ((IBaseDictionary)dictionary).Prepare();
                ((IBaseDictionary)dictionary).Sort();
            }
            catch (Exception ex)
            {
                Log.WriteError("Can not load dictionary \"{0}\". Reason: {1}", dictionaryName, ex.ToString());
                throw ex;
            }
            return true;
        }

        public bool RefreshDictionary(string dictionaryName, BaseUserSession userSession)
        {
            object dictionary = DictionaryList[dictionaryName];

            IList dictionaryElements = ((IBaseDictionary)dictionary).DictionaryElements;
            dictionaryElements.Clear();

            LoadDictionary(dictionary, userSession);

            return true;
        }

        public object GetDictionary(Type itemClass)
        {
            foreach (DictionaryEntry entry in DictionaryList)
            {
                Type dictionaryItemType = ((IBaseDictionary)entry.Value).DictionaryElements.GetType().GetGenericArguments()[0];
                if (dictionaryItemType.Equals(itemClass))
                {
                    return entry.Value;
                }
            }
            return null;
        }

        public object GetDictionary(string dictionaryName)
        {
            return DictionaryList[dictionaryName];
        }

        public IBaseDictionary GetIDictionary(string dictionaryName)
        {
            return (IBaseDictionary)DictionaryList[dictionaryName];
        }

        /// <summary>
        /// Устарело, т.к. получает удалённые элементы из справочника. Рекоммендуется использовать типизированную перегрузку
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <returns></returns>
        [Obsolete]
        public IList GetDictionaryElements(string dictionaryName)
        {
            IBaseDictionary dictionary = GetIDictionary(dictionaryName);
            if (dictionary != null)
            {
                return dictionary.DictionaryElements;
            }
            return null;
        }

        /// <summary>
        /// Получение не удалённых элементов справочника
        /// </summary>
        /// <typeparam name="T">Тип DictionaryItem элементов</typeparam>
        /// <param name="dictionaryName">Константа-имя справочника из класса Lims</param>
        /// <returns></returns>
        public List<T> GetDictionaryElements<T>(string dictionaryName) where T : DictionaryItem
        {
            IBaseDictionary dictionary = GetIDictionary(dictionaryName);
            List<T> result = new List<T>();
            if (dictionary != null)
            {
                foreach (T item in dictionary.DictionaryElements)
                {
                    if (!item.Removed)
                    {
                        result.Add(item);
                    }
                }
                return result;
            }
            return null;
        }

        public IList GetDictionaryElementsForFilters(string dictionaryName)
        {
            List<DictionaryItem> result = new List<DictionaryItem>();
            IBaseDictionary dictionary = GetIDictionary(dictionaryName);

            if (dictionary != null)
            {
                //result.Add(new DictionaryItem(){Id = 0, Name = ""});
                foreach (DictionaryItem item in dictionary.DictionaryElements)
                {
                    if (!item.Removed)
                    {
                        result.Add(new DictionaryItem() { Id = item.Id, Name = item.Name });
                    }
                }

                result.Sort((a, b) => a.Name.CompareTo(b.Name));
            }
            return result;
        }


        public object GetDictionaryItem(string dictionaryName, int id, bool skipRemoved = true)
        {
            IList elements = GetDictionaryElements(dictionaryName);

            if (elements != null)
            {
                foreach (object element in elements)
                {
                    if (((IBaseDictionaryItem)element).Id == id)
                    {
                        if (skipRemoved)
                        {
                            if (!((IBaseDictionaryItem)element).Removed)
                            {
                                return element;
                            }
                        }
                        else
                        {
                            return element;
                        }
                    }
                }
            }
            return null;
        }

        public object GetDictionaryItem(string dictionaryName, string code, bool skipRemoved = true)
        {
            //not good solution, but needed for compability with shared synchronization
            lock (ProgramContext.Dictionaries)
            {
                IList elements = GetDictionaryElements(dictionaryName);

                if (elements != null)
                {
                    foreach (object element in elements)
                    {
                        if (((IBaseDictionaryItem)element).Code == code)
                        {
                            if (skipRemoved)
                            {
                                if (!((IBaseDictionaryItem)element).Removed)
                                {
                                    return element;
                                }
                            }
                            else
                            {
                                return element;
                            }
                        }
                    }
                }
                return null;
            }
        }

        internal IBaseDictionaryItem GetIDictionaryItem(string dictionaryName, int id)
        {
            return (IBaseDictionaryItem)GetDictionaryItem(dictionaryName, id);
        }

        public string GetDictionaryValue(string dictionaryName, int id, string propName)
        {
            object item = GetDictionaryItem(dictionaryName, id);
            if (item != null)
            {
                try
                {
                    return item.GetType().GetProperty(propName).GetValue(item, null).ToString();
                }
                catch
                { }
            }
            return id.ToString();
        }

        public string GetDictionaryValue(PropertyInfo propInfo, object obj)
        {
            object[] attributes = propInfo.GetCustomAttributes(typeof(LinkedDictionary), true);
            object value = propInfo.GetValue(obj, null);

            foreach (object att in attributes)
            {
                return GetDictionaryValue(value, (LinkedDictionary)att);
            }
            return string.Empty; ;
        }

        public string GetDictionaryValue(object value, LinkedDictionary LinkedDictionary)
        {
            if (value == null)
            {
                return string.Empty;
            }

            int id = 0;
            if (value.GetType().Equals(typeof(int)))
            {
                id = Convert.ToInt32(value);
            }
            else if (value.GetType().GetInterface("IBaseObject") != null)
            {
                id = ((IBaseObject)value).Id;
            }
            else
            {
                return value.ToString();
            }
            return GetDictionaryValue(LinkedDictionary.DictionaryName, id, LinkedDictionary.PropertyName);
        }

        public object GetItemByReference(Type type, int objRef)
        {
            foreach (IBaseDictionary dictionary in DictionaryList.Values)
            {
                object obj = dictionary.GetByReference(type, objRef);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        public object GetItemByCode(Type type, string code, bool skipRemoved = true)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionaryByItemType(type);
            if (dictionary != null)
            {
                IList elements = dictionary.DictionaryElements;
                if (elements != null)
                {
                    foreach (object element in elements)
                    {
                        if (((IBaseDictionaryItem)element).Code == code)
                        {
                            if (skipRemoved)
                            {
                                if (!((IBaseDictionaryItem)element).Removed)
                                {
                                    return element;
                                }
                            }
                            else
                            {
                                return element;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public object GetItemByName(Type type, string name, bool skipRemoved = true)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionaryByItemType(type);
            if (dictionary != null)
            {
                IList elements = dictionary.DictionaryElements;
                if (elements != null)
                {
                    foreach (object element in elements)
                    {
                        if (((IBaseDictionaryItem)element).Name == name)
                        {
                            if (skipRemoved)
                            {
                                if (!((IBaseDictionaryItem)element).Removed)
                                {
                                    return element;
                                }
                            }
                            else
                            {
                                return element;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public object GetItemByEngName(Type type, string engName, bool skipRemoved = true)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionaryByItemType(type);
            if (dictionary != null)
            {
                IList elements = dictionary.DictionaryElements;
                if (elements != null)
                {
                    foreach (object element in elements)
                    {
                        if (((IBaseDictionaryItem)element).EngName == engName)
                        {
                            if (skipRemoved)
                            {
                                if (!((IBaseDictionaryItem)element).Removed)
                                {
                                    return element;
                                }
                            }
                            else
                            {
                                return element;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public object GetItemByAltCode(Type type, string altCode, bool skipRemoved = true)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionaryByItemType(type);
            if (dictionary != null)
            {
                IList elements = dictionary.DictionaryElements;
                if (elements != null)
                {
                    foreach (object element in elements)
                    {
                        if (((IBaseDictionaryItem)element).AlternativeCode == altCode)
                        {
                            if (skipRemoved)
                            {
                                if (!((IBaseDictionaryItem)element).Removed)
                                {
                                    return element;
                                }
                            }
                            else
                            {
                                return element;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public object GetDictionaryByItemType(Type type)
        {
            foreach (IBaseDictionary dictionary in DictionaryList.Values)
            {
                if (dictionary.DictionaryElements.GetType().GetGenericArguments()[0] == type)
                {
                    return dictionary;
                }
            }
            return null;
        }

        public object GetDictionaryItem(Type type, int id)
        {
            foreach (IBaseDictionary dictionary in DictionaryList.Values)
            {
                if (dictionary.DictionaryElements.GetType().GetGenericArguments()[0] == type)
                {
                    foreach (object o in dictionary.DictionaryElements)
                    {
                        if (((DictionaryItem)o).Id == id)
                        {
                            return o;
                        }
                    }
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает справочник по имени
        /// </summary>
        public IBaseDictionary this[string dictionaryName] => (IBaseDictionary)GetDictionary(dictionaryName);

        /// <summary>
        /// Возвращает из указанного справочника элемент с указанным кодом
        /// </summary>
        public object this[string dictionaryName, string elementCode, bool skipRemoved = true] => GetDictionaryItem(dictionaryName, elementCode, skipRemoved);

        /// <summary>
        /// Возвращает из указанного справочника элемент с указанным id 
        /// </summary>
        public object this[string dictionaryName, int elementId] => GetDictionaryItem(dictionaryName, elementId);

        /// <summary>
        /// Возвращает неудалённые элементы справочника
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetSortedNotRemoved<T>()
            where T : DictionaryItem
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionaryByItemType(typeof(T));
            if (dictionary == null)
            {
                return null;
            }
            else
            {
                List<T> list = dictionary.DictionaryElements.Cast<T>().ToList().FindAll(item => !item.Removed);
                list.Sort((x, y) => x.Name.CompareTo(y.Name));
                return list;
            }
        }

        /// <summary>
        /// Возвращает неудалённые элементы справочника
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <returns></returns>
        public List<DictionaryItem> GetSortedNotRemoved(string dictionaryName)
        {
            IList elements = GetDictionaryElements(dictionaryName);
            if (elements == null)
            {
                return null;
            }
            else
            {

                List<DictionaryItem> list = elements.Cast<DictionaryItem>().ToList().FindAll(item => !item.Removed);
                list.Sort((x, y) => x.Name.CompareTo(y.Name));
                return list;
            }
        }

        /// <summary>
        /// Ищет неудалённый элемент справочника с указанным именем
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T FindNotRemovedItemByName<T>(string name)
            where T : DictionaryItem
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionaryByItemType(typeof(T));
            if (dictionary == null)
            {
                return null;
            }
            else
            {
                return dictionary.DictionaryElements.Cast<T>().ToList().Find(item => (!item.Removed) && (item.Name == name));
            }
        }

        public DictionaryItem UpdateItem(string dictionaryName, DictionaryItem item, BaseUserSession userSession)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionary(dictionaryName);
            if (null == dictionary)
            {
                throw new Exception(string.Format("Справочник {0} не существует", dictionaryName));
            }

            DictionarySaveRequest requestObj = new DictionarySaveRequest() { Directory = dictionaryName, Element = item };
            item.Id = ProgramContext.BaseCommunicator.SaveDictionary(requestObj, userSession, out int newDictionaryVersion);
            CacheHelper.UpdateLoadedDictionaries(userSession);
            return item;
        }

        public DictionaryItem CreateItem(string dictionaryName, string code, string name, BaseUserSession userSession)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionary(dictionaryName);
            if (null == dictionary)
            {
                throw new Exception(string.Format("Справочник {0} не существует", dictionaryName));
            }

            Type itemType = dictionary.DictionaryElements.GetType().GetGenericArguments()[0];
            DictionaryItem item = (DictionaryItem)Activator.CreateInstance(itemType);
            item.Code = code;
            item.Name = name;

            DictionarySaveRequest requestObj = new DictionarySaveRequest() { Directory = dictionaryName, Element = item };
            item.Id = ProgramContext.BaseCommunicator.SaveDictionary(requestObj, userSession, out int newDictionaryVersion);
            CacheHelper.UpdateLoadedDictionaries(userSession);
            return item;
        }

        public DictionaryItem CreateItem(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, BaseUserSession userSession)
        {
            IBaseDictionary dictionary = (IBaseDictionary)GetDictionary(dictionaryName);
            if (null == dictionary)
            {
                throw new Exception(string.Format("Справочник {0} не существует", dictionaryName));
            }

            Type itemType = dictionary.DictionaryElements.GetType().GetGenericArguments()[0];
            return CreateItemImpl(dictionaryName, code, name, objectType, parentType, parentValue, userSession);
        }
        public DictionaryItem CreateItemNonCachedDictionary(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, BaseUserSession userSession)
        {
            return CreateItemImpl(dictionaryName, code, name, objectType, parentType, parentValue, userSession);
        }
        private DictionaryItem CreateItemImpl(string dictionaryName, string code, string name, Type objectType, Type parentType, object parentValue, BaseUserSession userSession)
        {
            DictionaryItem item = (DictionaryItem)Activator.CreateInstance(objectType);
            item.Code = code;
            item.Name = name;
            foreach (PropertyInfo propInfo in item.GetType().GetProperties())
            {
                if (propInfo.PropertyType.Name == parentType.Name)
                {

                    propInfo.SetValue(item, parentValue, null);
                    break;
                }
            }
            DictionarySaveRequest requestObj = new DictionarySaveRequest() { Directory = dictionaryName, Element = item };
            item.Id = ProgramContext.BaseCommunicator.SaveDictionary(requestObj, userSession, out int newDictionaryVersion);
            CacheHelper.UpdateLoadedDictionaries(userSession);
            return item;
        }
    }
}