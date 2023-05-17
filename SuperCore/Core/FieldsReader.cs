using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.LimsBusinessObjects;

namespace ru.novolabs.SuperCore.Core
{
    public class FieldsReader
    {

        public static class ValueTypePrefix
        {
            internal const String SYSTEM = "S";
            internal const String USER = "U";
        }

        private BaseDictionaryCache Dictionaries { get; set; }

        public FieldsReader(BaseDictionaryCache DictionaryCache)
        {
            Dictionaries = DictionaryCache;
        }

        public void ReadObjectFromDictionary(Dictionary<String, String> InputData, Object Results, List<String> ErrorMsg)
        {
            foreach (KeyValuePair<String, String> Element in InputData)
            {
                String FieldName;
                String[] FieldSet = Element.Key.ToString().Split(new char[] { '_' });
                if (FieldSet.Length == 1)
                    FieldName = FieldSet[0];
                else
                    FieldName = FieldSet[1];
                PropertyInfo propInfo = FindField(Results, FieldName);

                if (propInfo != null && FieldSet[0] != ValueTypePrefix.USER)
                {
                    if (propInfo.PropertyType.Equals(typeof(String)))
                    {
                        SetString(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    } 
                    else if (propInfo.PropertyType.Equals(typeof(Int32)) || propInfo.PropertyType.Equals(typeof(Int32?)))
                    {
                        SetInt(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.Equals(typeof(Int64)) || propInfo.PropertyType.Equals(typeof(Int64?)))
                    {
                        SetLong(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.Equals(typeof(float)) || propInfo.PropertyType.Equals(typeof(float?)))
                    {
                        SetFloat(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.Equals(typeof(double)) || propInfo.PropertyType.Equals(typeof(double?)))
                    {
                        SetDouble(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.Equals(typeof(Boolean)) || propInfo.PropertyType.Equals(typeof(Boolean?)))
                    {
                        SetBoolean(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    } else if (propInfo.PropertyType.IsSubclassOf(typeof(DictionaryItem)))
                    {
                        Type pType = propInfo.PropertyType;
                        SetDictionaryItem(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.Equals(typeof(ObjectRef)))
                    {
                        SetObjectRef(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.Equals(typeof(DateTime)) || propInfo.PropertyType.Equals(typeof(DateTime?)))
                    {
                        SetDateTime(Results, propInfo, Element.Value, ErrorMsg);
                        continue;
                    }
                    else if (propInfo.PropertyType.GetInterface("IList", false) != null)
                    {
                        Type pType = propInfo.PropertyType.GetGenericArguments()[0];
                        if (pType.IsSubclassOf(typeof(DictionaryItem)))
                        {
                            Object list = Activator.CreateInstance(propInfo.PropertyType);
                            SetDictionaryLis(pType, (IList)list, Element.Value, ErrorMsg);
                            propInfo.SetValue(Results, list, null);
                            //ProgramContext.Dictionaries.GetItemByReference(pType, 
                        }
                        continue;
                    }
                }
                else
                {
                    if (((UserFieldDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserField]).Find(FieldName) != null)
                    {
                        UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.UserField, FieldName);
                        //UserDirectoryDictionaryItem userDirectory = (UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LisDictionaryNames.UserDirectory, userField.UserDirectory.Id];
                            
                        if (userField.UserDirectory != null)
                        {
                            UserDirectoryDictionaryItem userDirectory = userField.UserDirectory;
                            Int32 Id;
                            if (Int32.TryParse(Element.Value, out Id))
                            {
                                UserDictionaryValueClass userDictionaryValue = userDirectory.Find(Id);
                                if (userDictionaryValue != null)
                                {
                                    UserValue userValue = new UserValue();
                                    userValue.Reference = new ObjectRef(userDictionaryValue.Id);
                                    //userValue.Value = Element.Value;
                                    userValue.UserField = new ObjectRef(userField.Id);
                                    Type t = Results.GetType();
                                    if (t.Equals(typeof(BaseRequest)))
                                        ((BaseRequest)Results).UserValues.Add(userValue);
                                    if (t.Equals(typeof(CreateRequest3Request)))
                                        ((CreateRequest3Request)Results).UserValues.Add(userValue);
                                }
                            }
                        }
                        else
                        {
                            UserValue userValue = new UserValue();
                            userValue.UserField = new ObjectRef(userField.Id);
                            userValue.Value = Element.Value;
                            if (userField.FieldType == (int)UserFieldTypes.DATETIME)
                            {
                                userValue.Value = userValue.Value.Replace(".", "");
                            }

                            Type t = Results.GetType();
                            if (t.Equals(typeof(BaseRequest)))
                                ((BaseRequest)Results).UserValues.Add(userValue);
                            if (t.Equals(typeof(CreateRequest3Request)))
                                ((CreateRequest3Request)Results).UserValues.Add(userValue);
                        }
                    }
                    else
                        ErrorMsg.Add("Поля " + FieldName + " в объекте " + Results.ToString() + " не существует.");
                }
               
            }
            return;
        }

        private  void SetDictionaryLis(Type pType, IList list, String value, List<String> errorMsg)
        {
      
            String[] values = value.Split(',');
            foreach (String v in values)
            {
                if (v != String.Empty)
                {
                    try
                    {
                        Object obj = Dictionaries.GetItemByCode(pType, v);
                        if (obj != null)
                        {
                            list.Add(obj);
                        }
                        else
                        {
                            errorMsg.Add("Не удалось найти значение " + v + " объекта  с типом " + pType.Name + " в кэше");
                        }

                    }
                    catch
                    {
                        errorMsg.Add("Не удалось привести значение " + v + " к типу Int32");
                    }
                }
            }
            
        }

        private void SetDictionaryItem(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                Object obj = Dictionaries.GetItemByReference(propInfo.PropertyType, Int32.Parse(value));
                propInfo.SetValue(results, obj, null);
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private  void SetDouble(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                propInfo.SetValue(results, double.Parse(value), null);
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private  void SetFloat(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                propInfo.SetValue(results, float.Parse(value), null);
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private  void SetInt(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                propInfo.SetValue(results, Int32.Parse(value), null);
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private  void SetString(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                propInfo.SetValue(results, value.ToString(), null);
            }
            catch
            { 
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private void SetLong(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                propInfo.SetValue(results, Int64.Parse(value), null);
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private void SetBoolean(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                propInfo.SetValue(results, Boolean.Parse(value), null);
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private void SetUserValue(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                UserFieldDictionaryItem userField = (UserFieldDictionaryItem)((UserFieldDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserField]).Find(propInfo.Name);

                UserValue uv = new UserValue();
                uv.Code = propInfo.Name;
                uv.Value = value;

                ((List<UserValue>)results.GetFieldByAttributeName(LimsFieldNamesConst.UserValues).GetValue(results, null)).Add(uv);
            }
            catch
            {
                errorMsg.Add("Не удалось добавить элемент " + propInfo.Name + " в UserValues");
            }
        }

        private void SetDateTime(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                {
                    //((ObjectRef)propInfo.GetValue(results, null)).SetRef(dt);
                    propInfo.SetValue(results, DateTime.Parse(value), null);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private void SetObjectRef(Object results, PropertyInfo propInfo, String value, List<String> errorMsg)
        {
            try
            {
                Int32 id;
                if (Int32.TryParse(value, out id))
                {
                    ((ObjectRef)propInfo.GetValue(results, null)).SetRef(id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                errorMsg.Add("Не удалось установить значение поля " + propInfo.Name + " с типом " + propInfo.PropertyType.Name);
            }
        }

        private  PropertyInfo FindField(Object obj, string fieldName)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (propInfo.Name.Equals(fieldName))
                {
                    return propInfo;
                }
            }
            return null;
        }

        
    }

}
