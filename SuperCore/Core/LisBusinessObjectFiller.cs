using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ru.novolabs.SuperCore.Core
{
    /// <summary>
    /// Вспомогательный класс для заплонения LIS-бизнес объектов из Supercore
    /// </summary>
    public class LisBusinessObjectFiller
    {
        public String _journalId { get; set; }
        public String _department { get; set; }

        public LisBusinessObjectFiller()
        {
            fillerData = new Dictionary<String, String>();
        }

        /// <summary>
        /// Справочник имя свойства-значение бизнес объекта
        /// </summary>
        public Dictionary<String, String> fillerData { get; set; }
        /// <summary>
        /// Список биоматериалов и назначенных исследований. Используется при сохранении заявки
        /// </summary>
        public Dictionary<String, String> biomaterials { get; set; }
        /// <summary>
        /// Заполнение полей фильтра из Supercore на основе key-value списка с названиями и значениями полей
        /// </summary>
        /// <param name="filter">Экземпляр фильтра из Supercore</param>
        public void FillObject(Object filter)
        {
            FillObjectFromDictionary(filter, fillerData);
        }
        /// <summary>
        /// Заполнение полей объекта из Supercore на основе key-value списка с названиями и значениями полей
        /// </summary>
        /// <param name="filter">Экземпляр фильтра из Supercore</param>
        /// <param name="filterFields">Список полей и их значений</param>
        public static void FillObjectFromDictionary(object filter, Dictionary<String, String> filterFields, bool skipEmptyValues = true)
        {
            foreach (PropertyInfo propInfo in filter.GetType().GetProperties())
            {
                if (!filterFields.Keys.Contains(propInfo.Name)) continue;

                string value = filterFields[propInfo.Name];
                if (String.IsNullOrWhiteSpace(value) && skipEmptyValues) continue;

                Type pType = propInfo.PropertyType;

                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    pType = pType.GetGenericArguments()[0];
                    if(string.IsNullOrEmpty(value))
                    {
                        propInfo.SetValue(filter, null, null);
                        continue;
                    }
                }

                if (pType == typeof(String))
                {
                    propInfo.SetValue(filter, value, null);
                }
                else if (pType == typeof(DateTime))
                {
                    DateTime Date = Convert.ToDateTime(value, ProgramContext.Settings.DateTimeFormatInfo);
                    propInfo.SetValue(filter, Date, null);
                }
                else if ((pType == typeof(Int32)))
                {
                    propInfo.SetValue(filter, Convert.ToInt32(value.Trim().Replace(",", "")), null);
                }
                else if ((pType == typeof(Int64)))
                {
                    propInfo.SetValue(filter, Convert.ToInt64(value.Trim().Replace(",", "")), null);
                }
                else if (pType == typeof(Boolean))
                {
                    if ("true".Equals(value.ToLower()))
                    {
                        propInfo.SetValue(filter, true, null);
                    }
                    else
                    {
                        propInfo.SetValue(filter, false, null);
                    }
                }
                else if (pType.GetInterface("IBaseObject") != null && pType != typeof(AndOrIdList))
                {
                    Object baseObject = Activator.CreateInstance(pType);
                    ((IBaseObject)baseObject).Id = Convert.ToInt32(value);
                    propInfo.SetValue(filter, baseObject, null);
                }
                else if (pType == typeof(Single))
                {
                    propInfo.SetValue(filter, Convert.ToSingle(value, ProgramContext.Settings.NumberFormatInfo), null);
                }
                else if ((pType == typeof(Double)) || (pType == typeof(float)))
                {
                    propInfo.SetValue(filter, Convert.ToDouble(value, ProgramContext.Settings.NumberFormatInfo), null);
                }
                else if (pType == typeof(AndOrIdList))
                {
                    AndOrIdList list = new AndOrIdList();
                    list.Operator = AndOrListContst.OrOperator;
                    string[] vals = value.Split(',');
                    foreach (string val in vals)
                    {
                        int intValue = 0;
                        if (int.TryParse(val, out intValue))
                        {
                            list.IdList.Add(new ObjectRef(intValue));
                            propInfo.SetValue(filter, list, null);
                        }
                    }
                }
                else if (pType.GetInterface("IList") != null)
                {
                    Type itemType = pType.GetGenericArguments()[0];
                    IList list = (IList)Activator.CreateInstance(pType);
                    String[] values = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String val in values)
                    {
                        if (itemType.Name.ToUpper() == "STRING")
                        {
                            list.Add(val);
                            continue;
                        }
                        Object item = Activator.CreateInstance(itemType);
                        if (itemType.GetInterface("IBaseObject") != null)
                            ((IBaseObject)item).Id = Convert.ToInt32(val);
                        else
                        {
                            var conv = TypeDescriptor.GetConverter(itemType);
                            item = conv.ConvertFromString(val);
                        }
                        list.Add(item);
                    }
                    propInfo.SetValue(filter, list, null);
                }

            }
            // Обработка пользовательских полей
            if (filter.GetType() == typeof(RegistrationJournalFilter))
            {
                RegistrationJournalFilter journalFilter = (RegistrationJournalFilter)filter;
                foreach (string key in filterFields.Keys)
                {
                    int userFieldId;
                    if (int.TryParse(key, out userFieldId))
                    {
                        try
                        {
                            UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.UserField, userFieldId);
                            if (userField == null)
                            {
                                Log.WriteError("Нет пользовательского поля c ID = " + userFieldId.ToString());
                                continue;
                            }
                            if (userField.FieldType == LimsUserFieldType.LIS_UERS_FIELD_TYPE_STRING)
                            {
                                string stringUserFieldValue = filterFields[key];
                                if (!String.IsNullOrEmpty(stringUserFieldValue))
                                {
                                    journalFilter.UserValues = journalFilter.UserValues ?? new List<UserValue>();
                                    UserValue userValue = new UserValue() { UserField = new ObjectRef(userFieldId) };
                                    userValue.Value = stringUserFieldValue;
                                    journalFilter.UserValues.Add(userValue);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex.ToString());
                        }
                    }
                }
            }

            if (filter.GetType().GetInterface("IJournalFilter") == null) return;

            ((IJournalFilter)filter).PrepareToSend();
        }

    }
}