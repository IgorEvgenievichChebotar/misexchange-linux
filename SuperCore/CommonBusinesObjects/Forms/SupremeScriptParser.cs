using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.HemBusinessObjects;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ru.novolabs.SuperCore.CommonBusinesObjects.Forms
{
    public class SupremeScriptParser
    {
        //Разделители
        public static String[] delimeters = new String[] { ">=", "<=", "!=", "=", "->", "<", ">"  };
        //Сама форма
        public WorkingFormsLayout form;
        //Промежуточный объект
        private Object currentObject;
        public Object CurrentObject
        {
            get
            {
                return currentObject;
            }
            set
            {
                currentObject = value;
            }
        }
        //Первичный объект для вычисления дельты (если понадобится)
        public Object SourceObject { get; set; }
        
        /// <summary>
        /// Список скриптов, определяющих наполнение справочника поля
        /// </summary>
        public Dictionary<String, List<Lexem>> filter_lexems = new Dictionary<String, List<Lexem>>();
        /// <summary>
        /// Список скриптов, определяющих видимость поля
        /// </summary>
        public LexemList visibility_lexems = new LexemList();

        /// <summary>
        /// Список скриптов, определяющих деактивацию поля
        /// </summary>
        public LexemList readonly_lexems = new LexemList();

        /// <summary>
        /// Список скриптов, определяющих проверку на не пустоту
        /// </summary>
        public LexemList notnullval_lexems = new LexemList();

        /// <summary>
        /// Список выражений, определяющих значение поля по умолчанию
        /// </summary>
        public Dictionary<string, List<PowerExpression>> defaultvals_expressions = new Dictionary<string, List<PowerExpression>>();

        public SupremeScriptParser()
        {
            CurrentObject = new object();
        }

        public SupremeScriptParser(WorkingFormsLayout Form)
        {
            form = Form;
            Type t = null;
                //switch (form.BuisnessObjectType)
                //{
                //    case ObjectType.Patient:
                //        t = typeof(Patient);
                //        break;
                //    case ObjectType.Request:
                //        t = typeof(CreateRequest3Request);
                //        break;
                //    case ObjectType.Sample:
                //        t = typeof(BaseSample);
                //        break;
                //    case ObjectType.Hem_Donor:
                //        t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Donor);
                //        break;
                //    case ObjectType.Hem_Product:
                //        t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Product);
                //        break;
                //    case ObjectType.Hem_Transfusion:
                //        t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.Transfusion);
                //        break;
                //    case ObjectType.Hem_Transfusion_request:
                //        t = typeof(ru.novolabs.SuperCore.HemBusinessObjects.TransfusionRequest);
                //        break;
                //}
            t = WorkingFormsLayout.GetBuisnessObjectType(form.BuisnessObjectType);
            if (t != null)
                CurrentObject = Activator.CreateInstance(t);
        }
        /// <summary>
        /// Метод, загружающий все скрипты (видимость, фильтрация и т.д.) контролов
        /// </summary>
        public void ParseForm()
        {
            foreach (WorkingForm page in form.Pages)
            {
                foreach (ControlField control in page.Controls)
                {
                    Int32 UserFieldKind = GetUserFieldKind(control.FieldType);
                    if (UserFieldKind != 0 && control.UserParameterCode != null && control.UserParameterCode != "")
                    {
                        List<Lexem> visibilities = ParseScript(control.VisibleCondition, true, UserFieldKind);
                        List<Lexem> readonlies = ParseScript(control.ReadOnlyCondition, true, UserFieldKind);
                        List<Lexem> notnulls = ParseScript(control.NotNullValidationCondition, true, UserFieldKind);
                        string lexKey = control.UserParameterCode;
                        if (visibilities != null)
                            visibility_lexems[lexKey, UserFieldKind] = visibilities;
                        if (readonlies != null)
                            readonly_lexems[lexKey, UserFieldKind] = readonlies;
                        if (notnulls != null)
                            notnullval_lexems[lexKey, UserFieldKind] = notnulls;
                        defaultvals_expressions[lexKey] = PowerExpression.GetListFromDefaultValueCondition(control.DefaultValueCondition, this, true, UserFieldKind);
                    }
                    else
                    {
                        List<Lexem> lexems = ParseScript(control.FilterCondition);
                        if (lexems != null && lexems.Count > 0)
                            filter_lexems.Add(control.PropertyName, lexems);
                        lexems = ParseScript(control.VisibleCondition);
                        if (lexems != null && lexems.Count > 0)
                            visibility_lexems[control.PropertyName] = lexems;
                        lexems = ParseScript(control.ReadOnlyCondition);
                        if (lexems != null && lexems.Count > 0)
                            readonly_lexems[control.PropertyName] = lexems;
                        lexems = ParseScript(control.NotNullValidationCondition);
                        if (lexems != null && lexems.Count > 0)
                            notnullval_lexems[control.PropertyName] = lexems;
                        defaultvals_expressions[control.PropertyName] = PowerExpression.GetListFromDefaultValueCondition(control.DefaultValueCondition, this);
                    }
                }
            }
        }
        /// <summary>
        /// Получить числовое представление FieldKind'а из enum'а
        /// </summary>
        /// <param name="userField">Имя FieldType</param>
        /// <returns></returns>
        public int GetUserFieldKind(string userField)
        {
            int played;
            if (int.TryParse(userField, out played))
                return played;
            switch (userField)
            {
                case NlsControlKinds.UserValue:
                    return (int)UserFieldKinds.UserField;
                case NlsControlKinds.BloodParameter:
                    return (int)UserFieldKinds.BloodParameter;
                case NlsControlKinds.AttrValue:
                    return (int)UserFieldKinds.Attribute;
                case NlsControlKinds.AnamnesisParameter:
                    return (int)UserFieldKinds.AnamnesisParameter;
                case NlsControlKinds.PhysioIndicator:
                    return (int)UserFieldKinds.PhysioIndicator;
                case NlsControlKinds.Parameter:
                    return (int)UserFieldKinds.Parameter;
            }
            return 0;
        }
        /// <summary>
        /// Преобразование строки условного выражения в список лексем
        /// </summary>
        /// <param name="expr">Строка с условиями. Каждое условие может разделяться символами && и ||</param>
        /// <param name="UserField"></param>
        /// <param name="UserFieldKind"></param>
        /// <returns></returns>
        public List<Lexem> ParseScript(String expr, Boolean UserField = false, Int32 UserFieldKind = 0)
        {
            List<Lexem> result = new List<Lexem>();
            string[] conditions = expr.Split(new string[]{"&&", "||"}, StringSplitOptions.RemoveEmptyEntries);
            char[] logicalCond = expr.Where(x => x == '&' || x == '|').ToArray();
            for (int i = 0; i < conditions.Length; ++i)
            {
                String sign = delimeters.FirstOrDefault(x => conditions[i].Contains(x));
                if (sign == null || sign == "")
                    return null;
                String[] values = conditions[i].Trim().Split(new String[] { sign }, StringSplitOptions.RemoveEmptyEntries);
                Lexem lex = new Lexem(values, sign, UserField);
                lex.UserFieldKind = UserFieldKind;
                if (i != 0)
                    lex.LogicalCondition = (logicalCond[(i - 1) * 2]).ToString() + (logicalCond[(i -1) * 2]).ToString();
                result.Add(lex);
            }
            return result;
        }

        public void SetValue(String FieldName, String Value, String UserFieldKind = "0")
        {
            Object value = null;
            if (FieldName.Contains('.') && UserFieldKind == "0")
            {
                String[] FieldParts = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                PropertyInfo propInfo = null;
                Object obj = CurrentObject;
                Object prevObj = CurrentObject;
                for (int i = 0; i < FieldParts.Length; i++)
                {
                    if (obj == null)
                    {
                        obj = Activator.CreateInstance(propInfo.PropertyType);
                        propInfo.SetValue(prevObj, obj, null);
                    }
                    propInfo = obj.GetType().GetCustomProperty(FieldParts[i]);
                    if (propInfo != null && i != FieldParts.Length - 1)
                    {
                        prevObj = obj;
                        obj = propInfo.GetValue(obj, null);
                    }
                }
                if (propInfo != null)
                {
                    Type propType = propInfo.PropertyType;
                    if ((propType == typeof(Int32)) || propType == typeof(Int32?))
                    {
                        Int32 val;
                        if (Int32.TryParse(Value, out val))
                            value = val;
                    }
                    else
                        if ((propType == typeof(DateTime)) || (propType == typeof(DateTime?)))
                        {
                            if (Value != "")
                                value = DateTime.Parse(Value);
                            else
                                value = null;
                        }
                        else
                            if ((propType == typeof(float)) || (propType == typeof(float?)))
                            {
                                float val;
                                if (float.TryParse(Value, out val))
                                    value = val;
                            }
                            else
                                if (propType.IsSubclassOf(typeof(DictionaryItem)))
                                {
                                    Int32 Id;
                                    if (Int32.TryParse(Value, out Id))
                                    {
                                        value = ProgramContext.Dictionaries.GetItemByReference(propType, Id);
                                    }
                                }
                                else
                                    if (propType == typeof(ObjectRef))
                                    {
                                        Int32 Id;
                                        if (Int32.TryParse(Value, out Id))
                                        {
                                            value = new ObjectRef(Id);
                                        }
                                    }
                                    else
                                        if (propType == typeof(Boolean))
                                        {
                                            Boolean b;
                                            if (Boolean.TryParse(Value, out b))
                                                value = b;
                                        }
                                        else
                                            if (propType == typeof(String))
                                            {
                                                value = Value;
                                            }
                                            else
                                                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                                                {
                                                    List<String> values = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                                    if (propType.GetGenericArguments()[0].IsSubclassOf(typeof(DictionaryItem)))
                                                    {

                                                        IList val = (IList)Activator.CreateInstance(propType);
                                                        foreach (String v in values)
                                                        {
                                                            var newItem = (DictionaryItem)Activator.CreateInstance(propType.GetGenericArguments()[0]);
                                                            newItem.Id = Int32.Parse(v);
                                                            val.Add(newItem);
                                                        }
                                                        value = val;

                                                    }
                                                }
                    
                    //value = obj;
                    if (propInfo.CanWrite && propInfo.GetSetMethod(true).IsPublic)
                        propInfo.SetValue(obj, value, null);
                }
            }
            else
            {
                PropertyInfo propInfo = CurrentObject.GetType().GetCustomProperty(FieldName);

                if (propInfo != null && UserFieldKind == "0")
                {
                    Type propType = propInfo.PropertyType;
                    if ((propType == typeof(Int32)) || propType == typeof(Int32?))
                    {
                        Int32 val;
                        if (Int32.TryParse(Value, out val))
                            value = val;
                    }
                    else
                        if ((propType == typeof(DateTime)) || (propType == typeof(DateTime?)))
                        {
                            if (Value != "")
                            {
                                DateTime val = new DateTime();
                                if (DateTime.TryParse(Value, out val))
                                    value = val;
                            }
                            else
                                value = null;
                        }
                        else
                            if ((propType == typeof(float)) || (propType == typeof(float?)))
                            {
                                float val;
                                if (float.TryParse(Value, out val))
                                    value = val;
                            }
                            else
                                if (propType.IsSubclassOf(typeof(DictionaryItem)))
                                {
                                    Int32 Id;
                                    if (Int32.TryParse(Value, out Id))
                                    {
                                        value = ProgramContext.Dictionaries.GetItemByReference(propType, Id);
                                    }
                                }
                                else
                                    if (propType == typeof(ObjectRef))
                                    {
                                        Int32 Id;
                                        if (Int32.TryParse(Value, out Id))
                                        {
                                            value = new ObjectRef(Id);
                                        }
                                    }
                                    else
                                        if (propType == typeof(Boolean))
                                        {
                                            Boolean b;
                                            if (Boolean.TryParse(Value, out b))
                                                value = b;
                                        }
                                        else
                                            if (propType == typeof(String))
                                            {
                                                value = Value;
                                            }
                                            else
                                                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                                                {
                                                    List<String> values = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                                    if (propType.GetGenericArguments()[0].IsSubclassOf(typeof(DictionaryItem)))
                                                    {

                                                        IList val = (IList)Activator.CreateInstance(propType);
                                                        foreach (String v in values)
                                                        {
                                                            var newItem = (DictionaryItem)Activator.CreateInstance(propType.GetGenericArguments()[0]);
                                                            newItem.Id = Int32.Parse(v);
                                                            val.Add(newItem);
                                                        }
                                                        value = val;

                                                    }
                                                }
                    if (propInfo.CanWrite && propInfo.GetSetMethod(true).IsPublic)
                        propInfo.SetValue(CurrentObject, value, null);
                }
                else
                {
                    Int32 ufk = Int32.Parse(UserFieldKind);
                    switch (ufk)
                    {
                        
                        case (int)UserFieldKinds.BloodParameter:
                            #region BloodParameter

                            List<BloodParameterValue> bloodParams;
                            String code = ""; String list;
                                if (FieldName.Contains('.'))
                                {
                                    //WRONG!!11 Need to use GetValue
                                    List<String> Splits = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    bloodParams = (List<BloodParameterValue>)GetValue(Splits.GetRange(0, Splits.Count - 1).ToArray(), CurrentObject);
                                    //list = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];
                                    //CurrentObject.GetType().GetCustomProperty(list).GetValue(CurrentObject, null);
                                    code = Splits[Splits.Count - 1];
                                    list = String.Join(".", Splits.GetRange(0, Splits.Count - 1));
                                }
                                else
                                {
                                    bloodParams = (List<BloodParameterValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.BloodParameters).GetValue(CurrentObject, null);
                                    list = UserParamNames.BloodParameters;
                                    code = FieldName;
                                }
                            BloodParameterItem param = ((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterByCode(code);
                            if (param != null)
                            {
                                
                                BloodParameterValue paramValue = new BloodParameterValue();
                                paramValue.Parameter = new Parameter() { Id = param.Id };
                                ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem userDirectory = param.UserDirectory;
                                switch (param.FieldType)
                                {
                                    case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                                        if (userDirectory != null)
                                        {
                                            userDirectory = (ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                                            foreach (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue val in userDirectory.Values)
                                            {
                                                if (val.Id.ToString().Equals(Value))
                                                {
                                                    paramValue.Reference = (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue)((ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionary)
                                                        ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory]).GetByReference(typeof(ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue), val.Id);
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
                                        paramValue.Value = Value;
                                        break;
                                    case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                                        if (userDirectory != null)
                                        {
                                            List<String> ids = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

                                if (bloodParams.Exists(x => x.Parameter.Id == paramValue.Parameter.Id))
                                {
                                    BloodParameterValue oldParam = bloodParams.Find(x => x.Parameter.Id == paramValue.Parameter.Id);
                                    bloodParams.Remove(oldParam);
                                }
                                bloodParams.Add(paramValue);
                                //CurrentObject.GetType().GetCustomProperty(list).SetValue(CurrentObject, bloodParams, null);
                            }
                            
                    #endregion
                            break;
                        case (int)UserFieldKinds.Parameter:
                            #region Parameter

                            List<ParameterValue> Params;
                            code = ""; list = "";
                                if (FieldName.Contains('.'))
                                {
                                    list = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];
                                    Params = (List<ParameterValue>)CurrentObject.GetType().GetCustomProperty(list).GetValue(CurrentObject, null);
                                    code = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[1];
                                }
                                else
                                {
                                    Params = (List<ParameterValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.Parameters).GetValue(CurrentObject, null);
                                    list = UserParamNames.Parameters;
                                    code = FieldName;
                                }
                                Parameter parameter = ((ParameterGroupDictionary<ParameterGroup>)ProgramContext.Dictionaries[HemDictionaryNames.ParameterGroup]).GetParameterByCode(code);
                                if (parameter != null && Params != null)
                                {

                                    ParameterValue paramValue = new ParameterValue();
                                    paramValue.Parameter = parameter;
                                    ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem userDirectory = parameter.UserDirectory;
                                    switch (parameter.FieldType)
                                    {
                                        case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                                            if (userDirectory != null)
                                            {
                                                userDirectory = (ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                                                foreach (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue val in userDirectory.Values)
                                                {
                                                    if (val.Id.ToString().Equals(Value))
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
                                            paramValue.Value = Value;
                                            break;
                                        case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                                            if (userDirectory != null)
                                            {
                                                List<String> ids = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

                                    if (Params.Exists(x => x.Parameter.Id == paramValue.Parameter.Id))
                                    {
                                        ParameterValue oldParam = Params.Find(x => x.Parameter.Id == paramValue.Parameter.Id);
                                        Params.Remove(oldParam);
                                    }
                                    Params.Add(paramValue);
                                    //CurrentObject.GetType().GetCustomProperty(UserParamNames.Parameters).SetValue(CurrentObject, Params, null);
                                }
                            
                    #endregion
                            break;
                        case (int)UserFieldKinds.UserField:
                            #region UserField
                            List<UserValue> userValues; 
                            code = ""; list = "";
                            if (FieldName.Contains('.'))
                            {
                                List<String> Splits = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                userValues = (List<UserValue>)GetValue(Splits.GetRange(0, Splits.Count - 1).ToArray(), CurrentObject);
                                code = Splits[Splits.Count - 1];
                                list = String.Join(".", Splits.GetRange(0, Splits.Count - 1));
                            }
                            else
                            {
                                userValues = (List<UserValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.UserValues).GetValue(CurrentObject, null);
                                list = UserParamNames.UserValues;
                                code = FieldName;
                            }

                            UserFieldDictionaryItem userField = (UserFieldDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserField, code];
                            if (userField != null)
                            {
                                //List<UserValue> userValues = (List<UserValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.UserValues).GetValue(CurrentObject, null);
                                UserValue userValue = new UserValue();
                                userValue.UserField = new ObjectRef(userField.Id);
                                userValue.Name = code;
                                ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem userDirectory = userField.UserDirectory;
                                switch (userField.FieldType)
                                {
                                    case (Int32)UserFieldTypes.DATETIME:
                                        if (userField.Strict == false)
                                            userValue.Value = Value.Replace(".", "");
                                        else
                                        {
                                            string time = "00:00";
                                            string[] splitted = Value.Split(' ');
                                            if(splitted.Length == 2 && userField.NeedTime)
                                                time = splitted[1];
                                            userValue.Value = splitted[0] + " " + time;
                                        }
                                        break;
                                    case (Int32) UserFieldTypes.BOOLEAN:
                                    case (Int32)UserFieldTypes.NUMERIC:
                                    case (Int32)UserFieldTypes.STRING:
                                    case (Int32)UserFieldTypes.TEST:
                                        userValue.Value = Value;
                                        break;
                                    case (Int32)UserFieldTypes.ENUMERATION:
                                        if (userDirectory != null)
                                        {
                                            Int32 Id = Int32.Parse(Value);
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
                                            List<String> ids = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                            userDirectory = (ru.novolabs.SuperCore.LimsDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserDirectory, userDirectory.Id];
                                            foreach (ru.novolabs.SuperCore.LimsDictionary.UserDictionaryValue val in userDirectory.Values)
                                            {
                                                foreach (String id in ids)
                                                {
                                                    //if (val.Id.ToString().Equals(id))
                                                    //{
                                                    //    userValue.Reference = new ObjectRef(val.Id);
                                                    //}
                                                    int objRef = 0;
                                                    if (int.TryParse(id, out objRef) && userValue.Values.FirstOrDefault(x => x.Id == objRef) == null)
                                                        userValue.Values.Add(new ObjectRef(objRef));
                                                }
                                            }

                                        }
                                        break;
                                 }
                                var existingUserVal = userValues.FirstOrDefault(x => x.UserField.Id == userValue.UserField.Id || (!string.IsNullOrEmpty(userValue.Name) && x.Name == userValue.Name));
                                if (existingUserVal != null)
                                {
                                    userValues.Remove(existingUserVal);
                                }
                                userValues.Add(userValue);
                                CurrentObject.GetType().GetCustomProperty(UserParamNames.UserValues).SetValue(CurrentObject, userValues, null);
                            }
                            #endregion
                            break;
                        case (int)UserFieldKinds.Attribute:
                            #region Attribute
                            #endregion
                            break;
                        case (int)UserFieldKinds.AnamnesisParameter:
                            #region Anamnesis
                            List<AnamnesisParameterValue> aParams = null;
                            list = ""; code = "";
                                if (FieldName.Contains('.'))
                                {
                                    list = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];
                                    aParams = (List<AnamnesisParameterValue>)CurrentObject.GetType().GetCustomProperty(FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[0]).GetValue(CurrentObject, null);
                                    code = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[1];
                                }
                            AnamnesisParameter aparam = ((AnamnesisParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.AnamnesisParameterGroup]).GetParameterByCode(code);
                            if (aparam != null && aParams != null)
                            {

                                AnamnesisParameterValue paramValue = new AnamnesisParameterValue();
                                paramValue.Parameter = new Parameter() { Id = aparam.Id };
                                ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem userDirectory = aparam.UserDirectory;
                                switch (aparam.FieldType)
                                {
                                    case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                                        if (userDirectory != null)
                                        {
                                            userDirectory = (ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                                            foreach (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue val in userDirectory.Values)
                                            {
                                                if (val.Id.ToString().Equals(Value))
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
                                        paramValue.Value = Value;
                                        break;
                                    case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                                        if (userDirectory != null)
                                        {
                                            List<String> ids = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

                                if (aParams.Exists(x => x.Parameter.Id == paramValue.Parameter.Id))
                                {
                                    AnamnesisParameterValue oldParam = aParams.Find(x => x.Parameter.Id == paramValue.Parameter.Id);
                                    aParams.Remove(oldParam);
                                }
                                aParams.Add(paramValue);
                                CurrentObject.GetType().GetCustomProperty(list).SetValue(CurrentObject, aParams, null);
                            }
                            #endregion
                            break;
                        case (int)UserFieldKinds.PhysioIndicator:
                            #region PhysioIndicator
                            List<PhysioIndicatorValue> pParams = null;
                            list = ""; code = "";
                                if (FieldName.Contains('.'))
                                {
                                    list = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];
                                    pParams = (List<PhysioIndicatorValue>)CurrentObject.GetType().GetCustomProperty(FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[0]).GetValue(CurrentObject, null);
                                    code = FieldName.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList()[1];
                                }
                                PhysioIndicator pparam = ((PhysioIndicatorGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.PhysioIndicatorGroup]).GetParameterByCode(code);
                            if (pparam != null && pParams != null)
                            {

                                PhysioIndicatorValue paramValue = new PhysioIndicatorValue();
                                paramValue.Indicator = new ObjectRef(pparam.Id);
                                ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem userDirectory = pparam.UserDirectory;
                                switch (pparam.FieldType)
                                {
                                    case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                                        if (userDirectory != null)
                                        {
                                            userDirectory = (ru.novolabs.SuperCore.HemDictionary.UserDirectoryDictionaryItem)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory, userDirectory.Id];
                                            foreach (ru.novolabs.SuperCore.HemDictionary.UserDictionaryValue val in userDirectory.Values)
                                            {
                                                if (val.Id.ToString().Equals(Value))
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
                                        paramValue.Value = Value;
                                        break;
                                    case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                                        if (userDirectory != null)
                                        {
                                            List<String> ids = Value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

                                if (pParams.Exists(x => x.Parameter.Id == paramValue.Parameter.Id))
                                {
                                    PhysioIndicatorValue oldParam = pParams.Find(x => x.Parameter.Id == paramValue.Parameter.Id);
                                    pParams.Remove(oldParam);
                                }
                                pParams.Add(paramValue);
                                CurrentObject.GetType().GetCustomProperty(list).SetValue(CurrentObject, pParams, null);
                            }
                            #endregion
                            break;
                    }

                }
            }

            int end = 0;
            end++;
        }

        public Object GetValue(String[] FieldNames, Object Obj)
        {
            
            PropertyInfo propInfo = Obj.GetType().GetCustomProperty(FieldNames[0]);
            if (propInfo != null)
            {
                if (FieldNames.Length == 1)
                    return propInfo.GetValue(Obj, null);

                String[] Copy = new String[FieldNames.Length - 1];
                
                //FieldNames.CopyTo(Copy, 1);
                Copy = FieldNames.ToList().GetRange(1, FieldNames.Length - 1).ToArray();
                return GetValue(Copy, propInfo.GetValue(Obj, null));
            }
            return null;
        }

        public Object GetValue(String FieldName, Object Obj)
        {
            String[] FieldNames = FieldName.Split('.');
            return GetValue(FieldNames, Obj);
        }

        public Object GetUserValueValue(string fieldName, object obj)
        {
            List<UserValue> userVals = (List<UserValue>)obj.GetType().GetCustomProperty(UserParamNames.UserValues).GetValue(obj, null);
            foreach(UserValue userVal in userVals)
            {
                if (userVal.Name == fieldName)
                    return userVal.Reference.Id == 0 ? userVal.Value : userVal.Reference.Id.ToString();
            }
            return null;
        }

        public Object GetObject(Object obj)
        {
            obj = CurrentObject;
            return obj;
        }

        
       /// <summary>
       /// Получение элементов справочника с учётом Filter Condition поля
       /// </summary>
       /// <param name="FieldName">Имя поля с filter condition</param>
       /// <param name="DictionaryName">Имя справочника</param>
       /// <returns></returns>
        public List<Object> GetDictionaryField(String FieldName, String DictionaryName)
        {
            if (null == ProgramContext.Dictionaries)
                throw new ApplicationException("Кэш справочников не создан");
            if (String.IsNullOrEmpty(DictionaryName))
                throw new ApplicationException(String.Format("Не указан справочник для элемента управления [{0}]", FieldName));
            Object dict = ProgramContext.Dictionaries[DictionaryName];
            if (null == dict)
                throw new ApplicationException(String.Format("Справочник [{0}] не найден", DictionaryName));

            List<Object> result = new List<object>();
            List<Lexem> lexems = null;
            if(filter_lexems.ContainsKey(FieldName))
                lexems = filter_lexems[FieldName];
            
            if (lexems != null)
            {
                IList Elements = ProgramContext.Dictionaries.GetDictionaryElements(DictionaryName);
                foreach (Object element in Elements)
                {
                    bool flag = false;
                    foreach (Lexem lex in lexems)
                    {
                        if (lex.LeftValue.Count != 0 && lex.RightValue.Count != 0)
                        {
                            Object RightRes = null;
                            Type t = CurrentObject.GetType();
                            RightRes = GetLexemValue(t, lex.RightValue[0], lex);
                            if (RightRes != null)
                            {
                                if (FieldName == "CustHospital")
                                {
                                    //Если нет ограничений по ЛПУ, то грузим все
                                    IEnumerable<object> lst = RightRes as IEnumerable<object>;
                                    if (lst != null && lst.Count() == 0)
                                    {
                                        flag = true;
                                        continue;
                                    }
                                }
                                Object LeftRes = lex.GetLeftValue(element);
                                if (LeftRes != null)
                                {
                                    if (lex.LogicalCondition == "&&")
                                        flag = flag && lex.GetValue(LeftRes, RightRes);
                                    else
                                        flag = flag || lex.GetValue(LeftRes, RightRes); // Логическое "или", либо условия нет
                                }
                            }
                        }
                    }
                    if (flag)
                        result.Add(element);
                }
            }
            if (result.Count == 0 && (lexems == null || lexems.Count == 0))
            {
                List<DictionaryItem> Elements = (List<DictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElementsForFilters(DictionaryName);
                foreach (DictionaryItem element in Elements)
                    result.Add(element);
            }
            return result;
        }

        public List<Lexem> GetFieldVisibilityLexem(String FieldName, String UserFieldKind = "")
        {
            List<Lexem> lexems;
            if (UserFieldKind != "")
            {
                lexems = visibility_lexems[FieldName, GetUserFieldKind(UserFieldKind)];
            }
            else
            {
                lexems = visibility_lexems[FieldName];
            }
            return lexems;
        }
        /// <summary>
        /// Определить видимость поля
        /// </summary>
        /// <param name="FieldName">Имя поля</param>
        /// <param name="UserFieldKind"></param>
        /// <returns>True - видимо. Иначе - false</returns>
        public Boolean GetFieldVisibility(String FieldName, String UserFieldKind = "")
        {
            List<Lexem> lexems = GetFieldVisibilityLexem(FieldName, UserFieldKind);
            if (lexems.Count == 0)
                return true;
            return GetLexemsValue(lexems);
        }
        /// <summary>
        /// Доступно ли поле только для чтение
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="userFieldKind"></param>
        /// <returns>True - да. Иначе - false (ваш К.О.)</returns>
        public bool GetFieldReadonly(string fieldName, string userFieldKind = "")
        {
            List<Lexem> lexems;
            if (userFieldKind != "")
                lexems = readonly_lexems[fieldName, GetUserFieldKind(userFieldKind)];
            else
                lexems = readonly_lexems[fieldName];
            if (lexems == null || lexems.Count == 0)
                return false;
            return GetLexemsValue(lexems);
        }
        /// <summary>
        /// Обязательно ли поле для ввода
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="userFieldKind"></param>
        /// <returns>True - значение </returns>
        public bool GetFieldNotNullValidation(string fieldName, string userFieldKind = "")
        {
            List<Lexem> lexems;
            if (userFieldKind != "")
                lexems = notnullval_lexems[fieldName, GetUserFieldKind(userFieldKind)];
            else
                lexems = notnullval_lexems[fieldName];
            if (lexems == null || lexems.Count == 0)
                return false;
            return GetLexemsValue(lexems);
        }
        /// <summary>
        /// Получить значение поля по умолчанию из условия DefaulValueCondition
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="userFieldKind"></param>
        /// <returns></returns>
        public string GetFieldDefaultValue(string fieldName, string userFieldKind = "")
        {
            List<PowerExpression> expressions;
            if (fieldName == null || defaultvals_expressions.ContainsKey(fieldName) == false)
                return "";
            expressions = defaultvals_expressions[fieldName];
            foreach (PowerExpression exprs in expressions)
                if (GetLexemsValue(exprs.Lexems))
                    return exprs.Value;
            return "";
        }

        /// <summary>
        /// Получение булевского значения составного условия
        /// </summary>
        /// <param name="lexems">Список лексем условия</param>
        /// <returns>True, если лексемы удовлетворяют условию</returns>
        private bool GetLexemsValue(List<Lexem> lexems)
        {
            try
            {
                bool result = false;
                if (lexems == null)
                    return result;
                foreach (Lexem lex in lexems)
                {
                    Object RightRes = null, LeftRes = null;
                    Type t = CurrentObject.GetType();
                    LeftRes = GetLexemValue(t, lex.LeftValue[0], lex, false);
                    RightRes = GetLexemValue(t, lex.RightValue[0], lex);
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
                        if (lex.LogicalCondition == "&&")
                            result = result && val;
                        else
                            result = result || val;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.WriteError(e.ToString());
                return false;
            }
	}
                
        /// <summary>
        /// Получение списка полей, относящихся к данному в Filter Condition
        /// </summary>
        /// <param name="FieldName">Имя поля, для которого ищутся относящиеся поля</param>
        /// <returns></returns>
        public List<String> GetRelatedFilterFields(String FieldName)
        {
            List<String> result = new List<String>();
            foreach (KeyValuePair<String, List<Lexem>> kp in filter_lexems)
            {
                List<Lexem> lexem = kp.Value;
                foreach(Lexem lex in lexem)
                    if (lex.RightValue.Count > 0 && lex.RightValue[0] == FieldName && !result.Contains(kp.Key))
                        result.Add(kp.Key);
            }
            return result;
        }
        /// <summary>
        /// Получение списка полей, относящихся к данному в Default Value Condition (которое есть в строке условия)
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <returns></returns>
        public List<string> GetRelatedDefValFields(string fieldName)
        {
            List<String> result = new List<String>();
            foreach (KeyValuePair<String, List<PowerExpression>> keyval in defaultvals_expressions)
            {
                foreach (PowerExpression pe in keyval.Value)
                    foreach (Lexem lex in pe.Lexems)
                    {
                        if (lex.RightValue.Count > 0 && lex.LeftValue[0] == fieldName && !result.Contains(keyval.Key))
                            result.Add(keyval.Key);
                    }
            }
            return result;
        }
        /// <summary>
        /// Получение списка полей, относящихся к данному в Visibility Condition
        /// </summary>
        /// <param name="FieldName">Имя поля, для которого ищутся относящиеся поля</param>
        /// <returns></returns>
        public List<String> GetRelatedVisibleFields(String FieldName)
        {
            return GetRelatedFields(FieldName, false);
        }

        /// <summary>
        /// Получение списка полей, относящихся к данному в Read Only Condition
        /// </summary>
        /// <param name="fieldName">Имя поля, для которого ищутся относящиеся поляn</param>
        /// <returns></returns>
        public List<string> GetRelatedReadOnlyFields(string fieldName)
        {
            return GetRelatedFields(fieldName, true);
        }

        private List<string> GetRelatedFields(string fieldName, bool getReadOnly)
        {
            List<String> result = new List<String>();
            var lexems = getReadOnly ? readonly_lexems : visibility_lexems;
            foreach (KeyValuePair<ValuesIndexer, List<Lexem>> kp in lexems)
            {
                List<Lexem> lexem = kp.Value;
                foreach (Lexem lex in lexem)
                    if (lex.LeftValue.Count > 0 && lex.LeftValue[0] == fieldName && !result.Contains(kp.Key.value))
                        result.Add(kp.Key.value);
            }
            return result;
        }

        private Object GetLexemValue(Type ParentType, String ObjectName, Lexem lex, Boolean right = true)
        {

            if ((ObjectName[0] == '"' && ObjectName.Last() == '"') || (ObjectName[0] == '\'' && ObjectName.Last() == '\''))
                return ObjectName.Substring(1, ObjectName.Length - 2);

            Object item = null;

            //if (lex.UserField)
            //{
            //    switch (lex.UserFieldKind)
            //    {

            //        case (int)UserFieldKinds.BloodParameter:
            //            List<BloodParameterValue> bloodParams = (List<BloodParameterValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.BloodParameters).GetValue(CurrentObject, null);
            //            BloodParameterItem param = ((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterByCode(ObjectName);
            //            if (param != null)
            //            {
            //                BloodParameterValue paramValue = bloodParams.Find(x => x.Parameter.Id == param.Id);
            //                if (paramValue != null)
            //                {
            //                    item = paramValue;
            //                }
            //            }
            //            break;
            //        case (int)UserFieldKinds.UserField:
            //            List<UserValue> userValues = (List<UserValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.UserValues).GetValue(CurrentObject, null);
            //            UserFieldDictionaryItem userField = ((UserFieldDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.UserField, ObjectName]);
            //            if (userField != null)
            //            {
            //                UserValue userValue = userValues.Find(x => x.UserField.Id == userField.Id);
            //                if (userValue != null)
            //                {
            //                    item = userValue;
            //                }
            //            }
            //            break;
            //        case (int)UserFieldKinds.Attribute:
            //            break;
            //        case (int)UserFieldKinds.Parameter:
            //            List<ParameterValue> paramValues = (List<ParameterValue>)CurrentObject.GetType().GetCustomProperty(UserParamNames.Parameters).GetValue(CurrentObject, null);
            //            Parameter parameter = ((ParameterGroupDictionary<ParameterGroup>)ProgramContext.Dictionaries[HemDictionaryNames.ParameterGroup]).GetParameterByCode(ObjectName);
            //            if (parameter != null)
            //            {
            //                ParameterValue paramValue = paramValues.Find(x => x.Parameter.Id == parameter.Id);
            //                if (paramValue != null)
            //                {
            //                    item = paramValue;
            //                }
            //            }
            //            break;
            //    }
            //}
            //else
            {

                Type t = GetObjectType(ParentType, ObjectName);


                Int32 res;
                if (ObjectName.Contains(','))
                {
                    return ObjectName.Replace("\"", "");
                }
                if (Int32.TryParse(ObjectName, out res))
                    return res;


                Object val = right ? CurrentObject.GetType().GetCustomProperty(lex.RightValue[0]).GetValue(CurrentObject, null)
                    : CurrentObject.GetType().GetCustomProperty(lex.LeftValue[0]).GetValue(CurrentObject, null);
                
                if (val != null && (val.GetType().IsSubclassOf(typeof(DictionaryItem)) || val.GetType().IsAssignableFrom(typeof(DictionaryItem))))
                {
                    Int32 Id = (Int32)val.GetType().GetCustomProperty("Id").GetValue(val, null);
                    item = ProgramContext.Dictionaries.GetDictionaryItem(t, Id);
                }
                else
                {
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

        public List<String> CheckFilled()
        {
            List<String> Result = new List<string>();
            foreach (WorkingForm page in form.Pages)
            {
                foreach (ControlField control in page.Controls)
                {
                    if (control.NotNull != false)
                    {
                        Object value = GetValue(control.PropertyName, currentObject);
                        
                        if (value == null)
                        {
                            Result.Add(String.Format("Значение {0} не может быть пустым", control.PropertyName));
                            continue;
                        }

                        Type valueType = value.GetType();
                        if (valueType == typeof(Int32) || valueType == typeof(Int32?))
                        {
                            Int32 iMin, iMax;
                            if (!String.IsNullOrEmpty(control.MinValue))
                            {
                                if (Int32.TryParse(control.MinValue, out iMin))
                                {
                                    if (((Int32)value) < iMin)
                                    {
                                        Result.Add(String.Format("Значение {0} не может быть меньше {1}", control.PropertyName, control.MinValue));
                                        continue;
                                    }
                                }
                            }

                            if (!String.IsNullOrEmpty(control.MaxValue))
                            {
                                if (Int32.TryParse(control.MaxValue, out iMax))
                                {
                                    if (((Int32)value) > iMax)
                                    {
                                        Result.Add(String.Format("Значение {0} не может быть больше {1}", control.PropertyName, control.MaxValue));
                                        continue;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (valueType == typeof(String))
                            {
                                Int32 iMin, iMax;
                                if (!String.IsNullOrEmpty(control.MinValue))
                                {
                                    if (Int32.TryParse(control.MinValue, out iMin))
                                    {
                                        if (value.ToString().Length < iMin)
                                        {
                                            Result.Add(String.Format("Значение {0} должно содержать не менее {1} символов", control.PropertyName, control.MinValue));
                                            continue;
                                        }
                                    }

                                }
                                if (!String.IsNullOrEmpty(control.MaxValue))
                                {
                                    if (Int32.TryParse(control.MaxValue, out iMax))
                                    {
                                        if (value.ToString().Length > iMax)
                                        {
                                            Result.Add(String.Format("Значение {0} должно содержать не более {1} символов", control.PropertyName, control.MaxValue));
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (valueType == typeof(DateTime) || valueType == typeof(DateTime?))
                                {
                                    DateTime dMin, dMax;
                                    if (!String.IsNullOrEmpty(control.MinValue))
                                    {
                                        if (DateTime.TryParse(control.MinValue, out dMin))
                                        {
                                            if (((DateTime)value) < dMin)
                                            {
                                                Result.Add(String.Format("Дата {0} не может быть меньше {1}", control.PropertyName, control.MinValue));
                                                continue;
                                            }
                                        }
                                    }

                                    if (!String.IsNullOrEmpty(control.MaxValue))
                                    {
                                        if (DateTime.TryParse(control.MaxValue, out dMax))
                                        {
                                            if (((DateTime)value) > dMax)
                                            {
                                                Result.Add(String.Format("Дата {0} не может быть больше {1}", control.PropertyName, control.MaxValue));
                                                continue;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (valueType.IsSubclassOf(typeof(BaseObject)))
                                    {
                                        if (((BaseObject)value).Id == 0)
                                        {
                                            Result.Add(String.Format("Не выбран справочный элемент {0}", control.PropertyName));
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Result;
        }
    }

    public class PowerExpression
    {
        /// <summary>
        /// Список лексем, определяющий логическое условие
        /// </summary>
        public List<Lexem> Lexems;
        /// <summary>
        /// Значение, возвращаемое в случае, если выражение является истинным
        /// </summary>
        public string Value;
        private SupremeScriptParser _parser;

        public PowerExpression(string expression, SupremeScriptParser parser, bool userField = false, int userFieldKind = 0)
        {
            int begin = expression.IndexOf("if(");
            if (begin < 0)
                begin = expression.IndexOf("if (");
            else
                begin -= 1;
            int end = expression.IndexOf(')');
            string condition = expression.Substring(begin + 4, end - 4).Trim();
            Lexems = parser.ParseScript(condition, userField, userFieldKind);
            int valIdx = expression.IndexOf("then ") + 5;
            Value = expression.Substring(valIdx, expression.Length - valIdx);
            this._parser = parser;
        }
        /// <summary>
        /// Получить из строки DefaultValueCondition контрола список объектов PowerExpression 
        /// </summary>
        /// <param name="defaultValueCondition"></param>
        /// <param name="parser"></param>
        /// <param name="userField"></param>
        /// <param name="userFieldKind"></param>
        /// <returns></returns>
        public static List<PowerExpression> GetListFromDefaultValueCondition(string defaultValueCondition, SupremeScriptParser parser, bool userField = false, int userFieldKind = 0)
        {
            List<PowerExpression> result = new List<PowerExpression>();
            foreach(string expression in defaultValueCondition.Split(';').Where(x => x != ""))
                result.Add(new PowerExpression(expression, parser, userField, userFieldKind));
            return result;
        }
    }

    public class WorkingValuesDictionary : Dictionary<ValuesIndexer, Object>
    {
        public Object this[String Key, Boolean isUserField = false]
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

        public Object this[String Key, Int32 UserFieldKind]
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
                    Object val = base[index];
                    String DictionaryName = form.GetControlFieldByPropertyName(Key).DictionaryName;
                    if (DictionaryName != null && DictionaryName != "")
                    {
                        Int32 value = (Int32)val.GetType().GetCustomProperty("Id").GetValue(val, null);
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
}
