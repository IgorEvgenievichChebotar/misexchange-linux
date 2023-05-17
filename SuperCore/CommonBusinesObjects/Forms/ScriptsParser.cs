using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.HemBusinessObjects;
using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ru
{

    public static class UserParamNames
    {
        public static String BloodParameters = "BloodParameters";
        public static String UserValues = "UserValues";
        public static String Attributes = "Attributes";
        public static String Parameters = "Parameters";
    }
    
    public class ScriptParser
    {
        //Разделители
        private static String[] delimeters = new String[] { ">=", "<=", "!=", "=", "<", ">" };
        //Сама форма
        private WorkingFormsLayout form;
        //Значения полей формы, полученные от клиента
        public ValuesDictionary values = new ValuesDictionary();
        //Список скриптов, определяющих наполнение справочника поля
        public Dictionary<String, List<Lexem>> filter_lexems = new Dictionary<String, List<Lexem>>();
        //Список скриптов, определяющих видимость поля
        public LexemList visibility_lexems = new LexemList();

        public ScriptParser(WorkingFormsLayout Form)
        {
            form = Form;
        }

        /// <summary>
        /// Упдейтим внутренний мир поля
        /// </summary>
        /// <param name="FieldName">Имя этого конкретного поля</param>
        /// <returns></returns>
        public List<Object> GetDictionaryField(String FieldName, String DictionaryName)
        {
            if (null == ProgramContext.Dictionaries)
                throw new ApplicationException("Кэш справочников не создан");
            if (String.IsNullOrEmpty(DictionaryName))
                throw new ApplicationException(String.Format("Не указан справочник для элемента управления [{0}]", FieldName));
            object dict = ProgramContext.Dictionaries[DictionaryName];
            if (null == dict)
                throw new ApplicationException(String.Format("Справочник [{0}] не найден", DictionaryName));

            List<Object> result = new List<Object>();
            List<Lexem> lexems = null;
            if (filter_lexems.ContainsKey(FieldName))
                lexems = filter_lexems[FieldName];
            if (lexems != null)
            {
                foreach (Lexem lex in lexems)
                {
                    if (lex.LeftValue.Count != 0 && lex.RightValue.Count != 0)
                    {
                        Object RightRes = null;
                        Type t = null;
                        switch (form.BuisnessObjectType)
                        {
                            case ObjectType.Patient:
                                t = typeof(Patient);
                                break;
                            case ObjectType.Request:
                                t = typeof(CreateRequest3Request);
                                break;
                            case ObjectType.Sample:
                                t = typeof(BaseSample);
                                break;
                            case ObjectType.Hem_Donor:
                                t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Donor);
                                break;
                            case ObjectType.Hem_Product:
                                t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Product);
                                break;
                                RightRes = GetValue(t, lex.RightValue[0], lex);
                        }
                        IList Elements = ProgramContext.Dictionaries.GetDictionaryElements(DictionaryName);
                        foreach (Object element in Elements)
                        {
                            Object LeftRes = lex.GetLeftValue(element);
                            if (LeftRes != null && RightRes != null)
                                if (lex.GetValue(LeftRes, RightRes))
                                    result.Add(element);
                        }
                    }
                }
            }
            if (result.Count == 0)
            {
                List<DictionaryItem> Elements = (List<DictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElementsForFilters(DictionaryName);
                foreach (DictionaryItem element in Elements)
                {
                    result.Add(element);
                }
            }
            return result;
        }

        /// <summary>
        /// Определить видимость поля
        /// </summary>
        /// <param name="FieldName">Имя этого конкретного поля</param>
        /// <returns></returns>
        public Boolean GetFieldVisibility(String FieldName, String UserFieldKind = "")
        {
            List<Lexem> lexems;
            if (UserFieldKind != "")
                lexems = visibility_lexems[FieldName, Int32.Parse(UserFieldKind)];
            else
                lexems = visibility_lexems[FieldName];

            Object RightRes = null;
            Object LeftRes = null;
           
            Type t = null;
            switch (form.BuisnessObjectType)
            {
                case ObjectType.Patient:
                    t = typeof(Patient);
                    break;
                case ObjectType.Request:
                    t = typeof(CreateRequest3Request);
                    break;
                case ObjectType.Sample:
                    t = typeof(BaseSample);
                    break;
                case ObjectType.Hem_Donor:
                    t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Donor);
                    break;
                case ObjectType.Hem_Product:
                    t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Product);
                    break;
            }
            bool result = false;
            foreach (Lexem lex in lexems)
            {
                LeftRes = GetValue(t, lex.LeftValue[0], lex, false);
                RightRes = GetValue(t, lex.RightValue[0], lex);
                if (LeftRes == null || RightRes == null)
                {
                    if (lex.LogicalCondition == "||")
                        result = result || false;
                    else
                        result = false; //anyway false
                }
                else
                {
                    bool val = lex.GetValue(LeftRes, RightRes);
                    if (lex.LogicalCondition == "||")
                        result = result || val;
                    else
                        result = result && val;
                }
            }
            return result;
        }

        /// <summary>
        /// Узнаем значение поля
        /// </summary>
        /// <param name="ParentType">Тип объекта, который содержит поле (Например, заявка)</param>
        /// <param name="ObjectName">Имя поля, содержащего объект</param>
        /// <param name="lex">Соответствующая лексема</param>
        /// <param name="right">Стоит ли поле справа от знака в лексеме (в противном случае - слева)</param>
        /// <returns></returns>
        private Object GetValue(Type ParentType, String ObjectName, Lexem lex, Boolean right = true)
        {

            if ((ObjectName[0] == '"' && ObjectName.Last() == '"') || (ObjectName[0] == '\'' && ObjectName.Last() == '\''))
                return ObjectName.Substring(1, ObjectName.Length - 2);

            Object item;
            //if (lex.UserField)
            //{
            Type t = GetObjectType(ParentType, ObjectName);
            if (t == null)
            {
                UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserField, ObjectName];
                if (userField != null)
                {
                    switch (userField.FieldType)
                    {
                        case (Int32)UserFieldTypes.BOOLEAN:
                            return Boolean.Parse(values[ObjectName]);
                        case (Int32)UserFieldTypes.DATETIME:
                            return DateTime.Parse(values[ObjectName]);
                        case (Int32)UserFieldTypes.NUMERIC:
                            return Int32.Parse(values[ObjectName]);
                        case (Int32)UserFieldTypes.STRING:
                            return values[ObjectName];
                        case (Int32)UserFieldTypes.ENUMERATION:
                            ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem userDirectory = (ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserDirectory, userField.UserDirectory.Id];
                            Int32 Id = Int32.Parse(values[ObjectName]);
                            return userDirectory.Values.Find(ud => ud.Id == Id);
                        case (Int32)UserFieldTypes.SET:
                            //TODO: UserValue из сетов?
                            break;
                    }
                    //}
                }
            }
            

            Int32 res;
            if (Int32.TryParse(ObjectName, out res))
                return res;
            
            
            Object val = right ? values[lex.RightValue[0], form] : values[lex.LeftValue[0], form];
            
            
            if (val.GetType().IsSubclassOf(typeof(DictionaryItem)) || val.GetType().IsAssignableFrom(typeof(DictionaryItem)))
            {
                item = val;
            }
            else
            {
                if (t.IsSubclassOf(typeof(DictionaryItem)))
                {
                    Int32 id = Int32.Parse(val.ToString());
                    item = ProgramContext.Dictionaries.GetDictionaryItem(t, id);

                    //item = ((IBaseDictionary)dictionary).DictionaryElements.;

                }
                else
                {
                    if (t == typeof(Int32))
                    {
                        if (val.ToString() == "")
                            item = 0;
                        else
                            item = Int32.Parse(val.ToString());
                    }
                    else
                        item = val;
                }
            }
            Object value = null;
            if (right)
            {

                value = lex.GetRightValue(item);

            }
            else
            {
                value = lex.GetLeftValue(item);
            }
            return value;

        }

        private Type GetObjectType(Type ParentType, String ObjectName)
        {
            PropertyInfo propInfo = ParentType.GetProperty(ObjectName);
            if (propInfo == null)
                return null;
            else
                return propInfo.PropertyType;
        }

        public void ParseForm()
        {
            foreach (WorkingForm page in form.Pages)
            {
                foreach (ControlField control in page.Controls)
                {
                    
                    if (control.UserParameterCode != null && control.UserParameterCode != "")
                    {
                        filter_lexems.Add(control.UserParameterCode, ParseScript(control.FilterCondition, true));
                        Int32 UserFieldKind = 0;
                        switch (control.FieldType)
                        {
                            case NlsControlKinds.UserValue:
                                UserFieldKind = (int)UserFieldKinds.UserField;
                                break;
                            case NlsControlKinds.BloodParameter:
                                UserFieldKind = (int)UserFieldKinds.BloodParameter;
                                break;
                            case NlsControlKinds.AttrValue:
                                UserFieldKind = (int)UserFieldKinds.Attribute;
                                break;
                        }
                        //if (control.VisibleCondition != "")
                            visibility_lexems[control.UserParameterCode, UserFieldKind] = ParseScript(control.VisibleCondition, true, UserFieldKind);
                    }
                    else
                    {
                        //if (control.FilterCondition != "")
                            filter_lexems.Add(control.PropertyName, ParseScript(control.FilterCondition));
                        //visibility_lexems.Add(control.PropertyName, ParseScript(control.VisibleCondition));
                        //if (control.VisibleCondition != "")
                            visibility_lexems[control.PropertyName] = ParseScript(control.VisibleCondition);
                    }
                }
            }
        }


        

        public List<Lexem> ParseScript(String expr, Boolean UserField = false, Int32 UserFieldKind = 0)
        {
            List<Lexem> result = new List<Lexem>();
            string[] conditions = expr.Split(new string[] { "&&", "||" }, StringSplitOptions.RemoveEmptyEntries);
            char[] logicalCond = expr.Where(x => x == '&' || x == '|').ToArray();
            for (int i = 0; i < conditions.Length; ++i)
            {
                String sign = delimeters.FirstOrDefault(x => conditions[i].Contains(x));
                if (sign == null || sign == "")
                    return null;
                String[] values = conditions[i].Split(new String[] { sign }, StringSplitOptions.RemoveEmptyEntries);
                Lexem lex = new Lexem(values, sign, UserField);
                lex.UserFieldKind = UserFieldKind;
                if (i != 0)
                    lex.LogicalCondition = logicalCond[i - 1].ToString();
                result.Add(lex);
            }
            return result;
        }

        public List<String> GetRelatedFilterFields(String FieldName)
        {
            List<String> result = new List<String>();
            foreach (KeyValuePair<String, List<Lexem>> kp in filter_lexems)
            {
                List<Lexem> lexems = kp.Value;
                foreach(Lexem lex in lexems)
                    if (lex.RightValue.Count > 0)
                        if (lex.RightValue[0] == FieldName)
                        {
                            if (!result.Contains(kp.Key))
                                result.Add(kp.Key);
                        }
            }
            return result;
        }

        public List<String> GetRelatedVisibleFields(String FieldName)
        {
            List<String> result = new List<String>();
            foreach (KeyValuePair<ValuesIndexer, List<Lexem>> kp in visibility_lexems)
            {
                List<Lexem> lexems = kp.Value;
                foreach (Lexem lex in lexems)
                    if (lex.RightValue.Count > 0)
                    if (lex.RightValue[0] == FieldName)
                    {
                        if (!result.Contains(kp.Key.value))
                            result.Add(kp.Key.value);
                    }
            }
            return result;
        }


        /// <summary>
        /// Получить результат в виде конечного объекта
        /// </summary>
        /// <param name="ObjectType">Тип конечного объекта</param>
        /// <returns></returns>
        public Object GetResult(Type ObjectType)
        {
            Object obj = ObjectType.CreateInstance();
            foreach (ValuesIndexer index in values.Keys)
            {
                String key = index.value;
                PropertyInfo propInfo = obj.GetType().GetProperty(key);
                if (propInfo != null && propInfo.CanWrite && !index.isUserField)
                {
                    Type propType = propInfo.PropertyType;
                    Object value = propType.CreateInstance();
                    String str_value = values[key];

                    if ((propType == typeof(Int32)) || propType == typeof(Int32?))
                    {
                        Int32 val;
                        if (Int32.TryParse(str_value, out val))
                        {
                            value = val;
                        }
                    }
                    else
                        if ((propType == typeof(DateTime)) || propType == typeof(DateTime?))
                        {
                            value = DateTime.Parse(str_value);
                        }
                        else
                            if ((propType == typeof(float)) || propType == typeof(float?))
                            {
                                float val;
                                if (float.TryParse(str_value, out val))
                                    value = val;
                            }
                            else
                                if (propType.IsSubclassOf(typeof(DictionaryItem)))
                                {
                                    Int32 id;
                                    if (Int32.TryParse(str_value, out id))
                                    {
                                        ((DictionaryItem)value).Id = id;
                                    }
                                }
                                else if (propType == typeof(Boolean))
                                {
                                    Boolean b;
                                    if (Boolean.TryParse(str_value, out b))
                                    {
                                        value = b;
                                    }
                                }
                                else
                                    if (propType == typeof(String))
                                    {
                                        value = str_value;
                                    }
                    
                    propInfo.SetValue(obj, value, null);
                }
            }
            return obj;
        }

        /// <summary>
        /// Получить результат в виде конечного объекта
        /// </summary>
        /// <param name="obj">Конечный объект</param>
        /// <returns></returns>
        public Object GetResult(Object obj)
        {
            if (obj == null)
                return null;
            foreach (ValuesIndexer index in values.Keys)
            {
                String key = index.value;
                PropertyInfo propInfo = obj.GetType().GetProperty(key);
                if (propInfo != null && !index.isUserField)
                {
                    Type propType = propInfo.PropertyType;
                    Object value = propType.CreateInstance();
                    String str_value = values[key];

                    if (value is IList && propType.IsGenericType)
                    {
                        Type ItemType = propType.GetGenericArguments()[0];
                        List<String> vals = str_value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (ItemType.IsSubclassOf(typeof(DictionaryItem)))
                        {
                            //List<DictionaryItem> res = new List<DictionaryItem>();
                            Object res = (IList)typeof(List<>).MakeGenericType(ItemType).GetConstructor(Type.EmptyTypes).Invoke(null);
                            foreach (String val in vals)
                            {
                                Int32 int_val;
                                if (Int32.TryParse(val, out int_val))
                                {
                                    Object Item = Activator.CreateInstance(ItemType);
                                    Item.GetType().GetCustomProperty("Id").SetValue(Item, int_val, null);
                                    res.GetType().GetMethod("Add").Invoke(res, new[] { Item });
                                }
                            }
                            value = res;
                        }
                        else
                            if (ItemType == typeof(ObjectRef))
                            {
                                List<ObjectRef> res = new List<ObjectRef>();
                                foreach (String val in vals)
                                {
                                    Int32 int_val;
                                    if (Int32.TryParse(val, out int_val))
                                    {
                                        res.Add(new ObjectRef() { Id = int_val });
                                    }
                                }
                                value = res;
                            }
                    }
                    else
                        if ((propType == typeof(Int32)) || propType == typeof(Int32?))
                        {
                            Int32 val;
                            if (Int32.TryParse(str_value, out val))
                            {
                                value = val;
                            }
                        }
                        else
                            if ((propType == typeof(DateTime)) || propType == typeof(DateTime?))
                            {
                                if (str_value != "")
                                    value = DateTime.Parse(str_value);
                                else
                                    value = null;
                            }
                            else
                                if ((propType == typeof(float)) || propType == typeof(float?))
                                {
                                    float val;
                                    if (float.TryParse(str_value, out val))
                                        value = val;
                                }
                                else
                                    if (propType.IsSubclassOf(typeof(DictionaryItem)))
                                    {
                                        Int32 id;
                                        if (Int32.TryParse(str_value, out id))
                                        {
                                            ((DictionaryItem)value).Id = id;
                                        }
                                    }
                                    else
                                        if (propType == typeof(ObjectRef))
                                        {
                                            Int32 id;
                                            if (Int32.TryParse(str_value, out id))
                                            {
                                                value = new ObjectRef(id);
                                            }
                                        }
                                        else if (propType == typeof(Boolean))
                                        {
                                            Boolean b;
                                            if (Boolean.TryParse(str_value, out b))
                                            {
                                                value = b;
                                            }
                                        }
                                        else
                                            if (propType == typeof(String))
                                            {
                                                value = str_value;
                                            }
                    if (propInfo.CanWrite && propInfo.GetSetMethod(true).IsPublic) 
                        propInfo.SetValue(obj, value, null);
                }
                else
                {
                    //User parameters
                    #region BloodParameter
                    if (obj.GetType().GetCustomProperty(UserParamNames.BloodParameters) != null)
                    {
                        BloodParameterItem param = ((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterByCode(key);
                        if (param != null)
                        {
                            List<BloodParameterValue> bloodParams = (List<BloodParameterValue>)obj.GetType().GetCustomProperty(UserParamNames.BloodParameters).GetValue(obj, null);

                            BloodParameterValue paramValue = new BloodParameterValue();
                            //paramValue.Parameter = new ObjectRef(param.Id);
                            paramValue.Parameter = new Parameter() { Id = param.Id };
                            ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem userDirectory = param.UserDirectory;
                            switch (param.FieldType)
                            {
                                case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                                    
                                    if (userDirectory != null)
                                    {
                                        String id = values[key];
                                        userDirectory = (ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                                        foreach (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue val in userDirectory.Values)
                                        {
                                            if (val.Id.ToString().Equals(id))
                                            {
                                                paramValue.Reference = (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue)((ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionary)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory]).GetByReference(typeof(ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue), val.Id);
                                                    //new ObjectRef(val.Id);
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case BloodParameterFieldType.HEM_FIELD_TYPE_STRING:
                                case BloodParameterFieldType.HEM_FIELD_TYPE_NUMERIC:
                                case BloodParameterFieldType.HEM_FIELD_TYPE_BOOLEAN:
                                case BloodParameterFieldType.HEM_FIELD_TYPE_DATETIME:
                                    paramValue.Value = values[key];
                                    break;
                                case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                                    if (userDirectory != null)
                                    {
                                        List<String> ids = values[key].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        userDirectory = (ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                                        foreach (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue val in userDirectory.Values)
                                        {
                                            foreach (String id in ids)
                                            {
                                                if (val.Id.ToString().Equals(id))
                                                {
                                                    paramValue.Values.Add(new ObjectRef(val.Id));
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }

                            bloodParams.Add(paramValue);
                            obj.GetType().GetCustomProperty(UserParamNames.BloodParameters).SetValue(obj, bloodParams, null);
                        }

                    }
                    #endregion
                    else
                        #region UserValues
                        if (obj.GetType().GetCustomProperty(UserParamNames.UserValues) != null)
                        {
                            UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserField, key];
                            if (userField != null)
                            {
                                List<UserValue> userValues = (List<UserValue>)obj.GetType().GetCustomProperty(UserParamNames.UserValues).GetValue(obj, null);
                                UserValue userValue = new UserValue();
                                userValue.UserField = new ObjectRef(userField.Id);
                                ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem userDirectory = userField.UserDirectory;
                                switch (userField.FieldType)
                                {

                                    case (Int32)UserFieldTypes.DATETIME:
                                        userValue.Value = values[key, true].Replace(".", "");
                                        break;
                                    case (Int32)UserFieldTypes.BOOLEAN:
                                    case (Int32)UserFieldTypes.NUMERIC:
                                    case (Int32)UserFieldTypes.STRING:
                                        userValue.Value = values[key, true];
                                        break;
                                    case (Int32)UserFieldTypes.ENUMERATION:
                                        if (userDirectory != null)
                                        {
                                            Int32 Id = Int32.Parse(values[key, true]);
                                            userDirectory = (ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserDirectory, userDirectory.Id];
                                            foreach (ru.novolabs.SuperCore.LimsDictionary.UserDictionaryValue val in userDirectory.Values)
                                            {
                                                if (val.Id == Id)
                                                {
                                                    userValue.Reference = new ObjectRef(val.Id);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    case (Int32)UserFieldTypes.SET:
                                        if (userDirectory != null)
                                        {
                                            Int32 Id = Int32.Parse(values[key, true]);
                                            userDirectory = (ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserDirectory, userDirectory.Id];
                                            foreach (ru.novolabs.SuperCore.LimsDictionary.UserDictionaryValue val in userDirectory.Values)
                                            {
                                                if (val.Id == Id)
                                                {
                                                    userValue.Reference = new ObjectRef(val.Id);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                }
                                userValues.Add(userValue);
                                obj.GetType().GetCustomProperty(UserParamNames.UserValues).SetValue(obj, userValues, null);
                            
                            }


                        }
                        #endregion
                        else
                            throw new MissingFieldException(String.Format("Поле {0} не найдено", key));
                }
            }
            return obj;

        }
    }

    /// <summary>
    /// Одна лексема скрипта
    /// </summary>
    public class Lexem
    {
        /// <summary>
        /// Правая сторона скрипта
        /// </summary>
        public List<String> RightValue = new List<String>();
        /// <summary>
        /// Левая сторона скрипта
        /// </summary>
        public List<String> LeftValue = new List<String>();
        /// <summary>
        /// Средняя сторона скрипта (ORLY) - разделитель
        /// </summary>
        String Delim;
        /// <summary>
        /// Логическое условие (дял составных условий)
        /// </summary>
        public string LogicalCondition;
        /// <summary>
        /// Принадлежит ли лексема пользовательскому полю
        /// </summary>
        public Boolean UserField = false;
        public Int32 UserFieldKind = 0;
        public Lexem(String[] values, String delimeter, Boolean userField = false,  string logicalCondition = "")
        {
            Delim = delimeter;
            if (values.Count() == 2)
            {
                LeftValue = values[0].Split('.').ToList();
                RightValue = values[1].Split('.').ToList();
                //Если какой-то pendejo перепутал вдруг лево и право, меняем их местами
                if (RightValue[0] == "this")
                {
                    List<String> swap = new List<string>();
                    swap.AddRange(LeftValue);
                    LeftValue.Clear();
                    LeftValue.AddRange(RightValue);
                    RightValue = swap;
                }
                UserField = userField;
            }
            LogicalCondition = logicalCondition;
        }

        public Boolean GetValue(Object LeftVal, Object RightVal)
        {
            Type type = LeftVal.GetType();
            if (type == typeof(String))
            {
                type = RightVal.GetType();
                if(type == typeof(string))
                    return CompareString((String)LeftVal, (String)RightVal);
                else
                    return CompareString((String)LeftVal, RightVal.ToString());
            }
            else if (type == typeof(Int32))
                return CompareInt((Int32)LeftVal, RightVal.ToString());
            else if (type == typeof(DateTime))
                return CompareDateTime((DateTime)LeftVal, (DateTime)RightVal);
            else
                if (type.IsSubclassOf(typeof(BaseObject)))
                {
                    return CompareBaseObject(LeftVal, RightVal);
                }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                bool result = false;
                var list = (IList)LeftVal;
                if (list.Count == 0)
                    return false;
                foreach (var item in list)
                    result = result | GetValue(item, RightVal);
                return result;
            }
            return false;
        }

        public Object GetRightValue(Object RightObject)
        {
            Object FinalVal = RightObject;
            if (RightObject == null)
                return null;
            for (int i = 1; i < RightValue.Count(); i++)
            {
                PropertyInfo propInfo = FinalVal.GetType().GetProperty(RightValue[i].Trim());
                if (propInfo != null)
                {
                    FinalVal = propInfo.GetValue(FinalVal, null);
                }
                else
                    return null;
            }
            return FinalVal;
        }

        public Object GetLeftValue(Object LeftObject, int i = 1)
        {
            Object FinalVal = LeftObject;
            bool returnNow = false;
            if (FinalVal == null)
                return null;
            for (; i < LeftValue.Count(); i++)
            {
                Type propType = FinalVal.GetType();
                if (LeftValue[i] == "Item")
                {
                    continue;
                }
                if(propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    List<object> listVals = new List<object>();
                    foreach (var item in (IList)FinalVal)
                    {
                        listVals.Add(GetLeftValue(item, i));
                    }
                    FinalVal = listVals;
                    returnNow = true;
                }
                PropertyInfo propInfo = propType.GetProperty(LeftValue[i].Trim());
                if (propInfo != null)
                {
                    FinalVal = propInfo.GetValue(FinalVal, null);
                    if (returnNow)
                        return FinalVal;
                }
            }
            return FinalVal;
        }

        private Boolean CompareDateTime(DateTime LeftVal, DateTime RightVal)
        {
            switch (Delim)
            {
                case "=":
                    if (LeftVal == RightVal)
                        return true;
                    break;
                case "!=":
                    if (LeftVal != RightVal)
                        return true;
                    break;
                case ">":
                    if (LeftVal > RightVal)
                        return true;
                    break;
                case "<":
                    if (LeftVal < RightVal)
                        return true;
                    break;
                case ">=":
                    if (LeftVal >= RightVal)
                        return true;
                    break;
                case "<=":
                    if (LeftVal <= RightVal)
                        return true;
                    break;
            }
            return false;
        }

        private Boolean CompareInt(Int32 LeftVal, string RightVal)
        {
            switch (Delim)
            {
                case "=":
                    if (LeftVal == Convert.ToInt32(RightVal))
                        return true;
                    break;
                case "!=":
                    if (LeftVal != Convert.ToInt32(RightVal))
                        return true;
                    break;
                case ">":
                    if (LeftVal > Convert.ToInt32(RightVal))
                        return true;
                    break;
                case "<":
                    if (LeftVal < Convert.ToInt32(RightVal))
                        return true;
                    break;
                case ">=":
                    if (LeftVal >= Convert.ToInt32(RightVal))
                        return true;
                    break;
                case "<=":
                    if (LeftVal <= Convert.ToInt32(RightVal))
                        return true;
                    break;
                case "->":
                    if (RightVal.Split(',').Contains(LeftVal.ToString()))
                        return true;
                    break;
            }
            return false;
        }

        private Boolean CompareString(String LeftVal, String RightVal)
        {
            switch (Delim)
            {
                case "=":
                    if (LeftVal == RightVal)
                        return true;
                    break;
                case "!=":
                    if (LeftVal != RightVal)
                        return true;
                    break;
                case ">":
                    if (LeftVal.CompareTo(RightVal) > 0)
                        return true;
                    break;
                case "<":
                    if (LeftVal.CompareTo(RightVal) < 0)
                        return true;
                    break;
                case ">=":
                    if (LeftVal.CompareTo(RightVal) >= 0)
                        return true;
                    break;
                case "<=":
                    if (LeftVal.CompareTo(RightVal) <= 0)
                        return true;
                    break;
            }
            return false;
        }

        private Boolean CompareBaseObject(Object LeftVal, Object RightVal)
        {
            switch (Delim)
            {
                case "=":
                    if (((BaseObject)LeftVal).Id == ((BaseObject)RightVal).Id)
                        return true;
                    break;
                case "!=":
                    if (((BaseObject)LeftVal).Id != ((BaseObject)RightVal).Id)
                        return true;
                    break;
                case ">":
                    if (((BaseObject)LeftVal).Id > ((BaseObject)RightVal).Id)
                        return true;
                    break;
                case "<":
                    if (((BaseObject)LeftVal).Id < ((BaseObject)RightVal).Id)
                        return true;
                    break;
                case ">=":
                    if (((BaseObject)LeftVal).Id >= ((BaseObject)RightVal).Id)
                        return true;
                    break;
                case "<=":
                    if (((BaseObject)LeftVal).Id <= ((BaseObject)RightVal).Id)
                        return true;
                    break;
                case "->":
                    if (RightVal.GetType().IsGenericType && RightVal is IEnumerable)
                    {
                        foreach (BaseObject item in ((IList)RightVal))
                        {
                            if (item.Id == ((BaseObject)LeftVal).Id)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (RightVal.GetType() == typeof(String))
                        {
                            List<String> values = RightVal.ToString().Split(new char[] { ',' }).ToList();
                            foreach (String value in values)
                            {
                                if (((BaseObject)LeftVal).Id.ToString() == value)
                                    return true;
                            }
                        }
                    }
                    break;
            }
            return false;
        }
    }

    public class ValuesDictionary : Dictionary<ValuesIndexer, String>
    {
        public String this[String Key, Boolean isUserField = false]
        {
            get
            {

                ValuesIndexer index = GetIndexer(Key, isUserField);
                if (index != null)
                {
                    return this[index];
                }
                return "";
            }
            set
            {

                ValuesIndexer index = GetIndexer(Key, isUserField);
                if (index != null)
                {
                    this[index] = value;
                }
                else
                {
                    index = new ValuesIndexer() { value = Key };
                    this.Add(index, value);
                }
            }
        }

        public String this[String Key, Int32 UserFieldKind]
        {
            get
            {
                ValuesIndexer index = GetIndexer(Key, UserFieldKind);
                if (index != null)
                {
                    return base[index];
                }
                return "";
            }
            set
            {
                ValuesIndexer index = GetIndexer(Key, UserFieldKind);
                if (index != null)
                {
                    this[index] = value;
                }
                else
                {
                    index = new ValuesIndexer() { value = Key, isUserField = true, UserFieldKind = UserFieldKind };
                    this.Add(index, value);
                }
            }
        }

        public Object this[String Key, WorkingFormsLayout form, Boolean isUserField = false]
        {
            get
            {
                if (this.IsKeysContains(Key))
                {
                    ValuesIndexer index = GetIndexer(Key, isUserField);
                    String val = base[index];
                    String DictionaryName = form.GetControlFieldByPropertyName(Key).DictionaryName;
                    if (DictionaryName != null && DictionaryName != "")
                    {
                        Int32 value = Int32.Parse(val);
                        DictionaryItem di = (DictionaryItem)ProgramContext.Dictionaries[DictionaryName, value];
                        if (di != null)
                            return di;
                    }
                    return val;
                }
                return "";
            }
        }

        public Boolean IsKeysContains(String key)
        {
            foreach (ValuesIndexer indexer in this.Keys)
                if (indexer.value == key)
                    return true;
            return false;
        }

        public ValuesIndexer GetIndexer(String key, Boolean isUserField)
        {
            foreach (ValuesIndexer indexer in this.Keys)
                if (indexer.value == key && indexer.isUserField == isUserField)
                    return indexer;
            return null;
        }

        public ValuesIndexer GetIndexer(String key, Int32 UserFieldKind)
        {
            foreach (ValuesIndexer indexer in this.Keys)
                if (indexer.value == key && indexer.isUserField && indexer.UserFieldKind == UserFieldKind)
                    return indexer;
            return null;
        }
    }

    public enum UserFieldKinds
    {
        UserField = 1,
        BloodParameter = 2,
        Attribute = 3,
        AnamnesisParameter = 4,
        PhysioIndicator = 5,
        Parameter = 6
    }

    public class ValuesIndexer
    {
        public ValuesIndexer()
        {
        }

        public String value
        {
            get;
            set; 
        }

        public override string ToString()
        {
            return value;
        }

        public Boolean isUserField = false;
        public Int32 UserFieldKind;
    }

    public class LexemList : Dictionary<ValuesIndexer, List<Lexem>>
    {
        public List<Lexem> this[String key, Int32 UserFieldKind = 0]
        {
            get
            {
                ValuesIndexer index = GetIndexer(key, UserFieldKind);
                if (index != null)
                {
                    return base[index];
                }
                return null;
            }
            set
            {
                ValuesIndexer index = GetIndexer(key, UserFieldKind);
                if (index != null)
                {
                    base[index] = value;
                }
                else
                {
                    index = new ValuesIndexer();
                    index.value = key;
                    if (UserFieldKind != 0)
                        index.isUserField = true;
                    index.UserFieldKind = UserFieldKind;
                    base.Add(index, value);
                }

            }
        }



        public ValuesIndexer GetIndexer(String key, Boolean isUserField)
        {
            foreach (ValuesIndexer indexer in this.Keys)
                if (indexer.value == key && indexer.isUserField == isUserField)
                    return indexer;
            return null;
        }

        public ValuesIndexer GetIndexer(String key, Int32 UserFieldKind)
        {
            foreach (ValuesIndexer indexer in this.Keys)
                if (indexer.value == key && ((indexer.isUserField && indexer.UserFieldKind == UserFieldKind) || (!indexer.isUserField && UserFieldKind == 0)))
                    return indexer;
            return null;
        }
    }

    
}

