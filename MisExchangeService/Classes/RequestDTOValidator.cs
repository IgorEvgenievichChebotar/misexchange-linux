using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ru.novolabs.MisExchange.Classes
{
   
    static class RequestDTOValidator
    {
        private static String MaxValuePrefix = "MaxPropertyValue_";

        internal static void CheckData(ExchangeDTOs.Request requestDTO)
        {
            var errors = new List<String>();

            CheckObjectFields(requestDTO, errors);

            if (errors.Count > 0)
                throw new CustomDataCheckException(errors);
        }

        private static void CheckObjectFields(Object obj, List<string> errors)
        {

            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {

                Object[] attrs = pi.GetCustomAttributes(typeof(FieldProps), false);

                if (attrs.Length != 1)
                    continue;

                FieldProps fieldProps = (FieldProps)attrs[0];

                Object value = pi.GetValue(obj, null);
                Type propType = pi.PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    propType = propType.GetGenericArguments()[0];

                if (fieldProps.Mandatory && ((null == value)
                    || ((propType == typeof(String)) && String.IsNullOrEmpty((string)value))))
                {
                    UserField uf = obj as UserField;
                    if (uf != null && pi.Name == "Value")
                    {
                        errors.Add(String.Format("Свойство [Value] в объекте [UserField] с именем [Name] = {0} и кодом [Code] = {1} не может быть пустым",uf.Name,uf.Code));
                    }
                    else
                    {
                        errors.Add(String.Format("Свойство [{0}] в объекте [{1}] не может быть пустым", pi.Name, obj.GetType().Name));
                    } 
                    continue;
                }


                if ((propType == typeof(String)) && (null != value) && (null != fieldProps.MaxLength || null != ProgramContext.Settings[MaxValuePrefix + obj.GetType().Name + "_" + pi.Name, false]))
                {
                    string strValue = (string)value;
                    Boolean isError = false;
                    Int32 maxVal;
                    if (ProgramContext.Settings[MaxValuePrefix + obj.GetType().Name + "_" + pi.Name, false] != null && Int32.TryParse(ProgramContext.Settings[MaxValuePrefix + obj.GetType().Name + "_" + pi.Name, false].ToString(), out maxVal))
                    {
                        if (strValue.Length > maxVal)
                            isError = true;
                    }
                    else
                    {
                        if (strValue.Length > fieldProps.MaxLength.Value)
                            isError = true;
                    }
                    if (isError)
                        errors.Add(String.Format("Длина свойства [{0}] в объекте [{1}] превышает масимально допустимое значение = {2}", pi.Name, obj.GetType().Name, fieldProps.MaxLength.Value));
                    continue;
                }

                if ((null != value) &&
                    ((pi.PropertyType == typeof(Int16)) || (pi.PropertyType == typeof(Int32))
                    || (pi.PropertyType == typeof(Int64)) || (pi.PropertyType == typeof(int))
                    || (pi.PropertyType == typeof(long)) || (pi.PropertyType == typeof(byte))
                    || (pi.PropertyType == typeof(float)) || (pi.PropertyType == typeof(double))
                    || (pi.PropertyType == typeof(Single))))
                {
                    if (null != fieldProps.MinValue)
                        if (fieldProps.MinValue.Value.CompareTo(Convert.ToInt32(value)) == 1)
                            errors.Add(String.Format("Значение свойства [{0}] в объекте [{1}] меньше минимально допустимого значения = {2}", pi.Name, obj.GetType().Name, fieldProps.MinValue.Value));

                    if (null != fieldProps.MaxValue)
                        if (fieldProps.MaxValue.Value.CompareTo(Convert.ToInt32(value)) == -1)
                            errors.Add(String.Format("Значение свойства [{0}] в объекте [{1}] больше максимально допустимого значения = {2}", pi.Name, obj.GetType().Name, fieldProps.MaxValue.Value));

                    continue;
                }


                if ((null != value) && (pi.PropertyType.GetInterface("IList") != null))
                {
                    IList list = (IList)pi.GetValue(obj, null);
                    if ((list.Count == 0) && fieldProps.Mandatory)
                       errors.Add(String.Format("Свойство [{0}] в объекте [{1}] не может быть пустым", pi.Name, obj.GetType().Name));

                    foreach (Object subObj in list)                    
                        CheckObjectFields(subObj, errors);

                    continue;
                }

                if ((null != value) && (pi.PropertyType.IsClass))
                    CheckObjectFields(value, errors);
            }

        }
                


    }
    class PropertyStrigPair
    {
        public string ClassName {get;set;}
        public string PropertyName {get;set;}
    
    }
}
