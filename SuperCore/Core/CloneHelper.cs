using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ru.novolabs.SuperCore
{
    public static class CloneExtension
    {
        /// <summary>
        /// Создание копии объекта через сериализацию/десериализацию в/из XML-строки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CopyViaSerialization<T>(this T obj)
        {
            DateTime date = DateTime.Now;
            string xml = obj.Serialize(System.Text.Encoding.UTF8);
            T res = xml.Deserialize<T>(System.Text.Encoding.UTF8);
            Console.WriteLine("CopyViaSerialization: {0} ms", DateTime.Now.Subtract(date).TotalMilliseconds.ToString());
            return res;
        }

        public static Object CreateInstance(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Activator.CreateInstance(type.GetGenericArguments()[0]);
            else
                if (type.Equals(typeof(String)))
                    return "";
                else
                    return Activator.CreateInstance(type);
        }

        public static T CreateShallowCopy<T>(this T obj)
        {
            // Поверхностное клонирование.
            MethodInfo memberwiseClone = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

            // Вызов метода MemberwiseClone объекта obj.
            return (T)memberwiseClone.Invoke(obj, null);
        }

        public static void CopyPropertiesFromObject(this Object obj, Object objFrom)
        {

            PropertyInfo[] objToProperties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            PropertyInfo[] objFromProperties = objFrom.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (PropertyInfo piFrom in objFromProperties)
            {
                Type piFromPropType = piFrom.PropertyType;
                if (piFromPropType.IsGenericType && piFromPropType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    piFromPropType = piFromPropType.GetGenericArguments()[0];

                Object piFromValue = (piFrom.CanRead) ? piFrom.GetValue(objFrom, null) : null;

                if (piFromValue != null)
                {
                    String propName;
                    Object[] attrs = piFrom.GetCustomAttributes(typeof(ClonePropName), false);
                    if (attrs.Length > 0)
                        propName = ((ClonePropName)attrs[0]).Name; // attrs[0] - т.к. атрибут ClonePropName можно назначить только один раз.
                    else
                        propName = piFrom.Name;

                    // Выясняем, есть ли в классе-источнике свойство, совпадающее по имени и типу(только для типов-значений)
                    PropertyInfo match = null;
                    foreach (PropertyInfo piTo in objToProperties)
                    {
                        Type piToPropType = piTo.PropertyType;
                        if (piToPropType.IsGenericType && piToPropType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            piToPropType = piToPropType.GetGenericArguments()[0];
                        
                        if (piTo.Name.Equals(propName))
                        {                          
                            if ((piToPropType.IsValueType) || (piFromPropType.IsValueType))
                                if (!(piToPropType.Equals(piFromPropType)))
                                {
                                    Debug.WriteLine(String.Format("debug info: Property [{0}] in source class {1} and target class {2} has not the same Type ({3} and {4})",
                                        propName, objFrom.GetType().Name, obj.GetType().Name, piFrom.PropertyType.Name, piTo.PropertyType.Name));
                                    break;
                                }

                            match = piTo;
                            break;
                        }
                    }

                    // Если свойство, совпадающее по имени и типу, найдено, то копируем его значение в objTo
                    if (match != null)
                    {
                        if (match.CanWrite)
                        {
                           // Debug.WriteLine(objFrom.GetType().Name + "." + match.Name);

                            Object matchValue = null;
                            matchValue = (match.CanRead) ? match.GetValue(obj, null) : null;
                            if (matchValue == null)
                            {
                                if (match.PropertyType.Equals(typeof(String)))
                                    matchValue = String.Empty;
                                else
                                    matchValue = CreateInstance(match.PropertyType);

                                match.SetValue(obj, matchValue, null);
                            }                            

                            // Если свойство является классом
                            if (match.PropertyType.IsClass && !match.PropertyType.Equals(typeof(String)))
                            {
                                // Копирование списков классов с рекурсивным вызовом процедуры копирования для получения глубоких копий элементов списка (deep copy)
                                if (match.PropertyType.GetInterface(typeof(IList<Type>).Name) != null)
                                {
                                    ((IList)matchValue).Clear();
                                    foreach (var item in (IList)piFromValue)
                                    {
                                        Object newItem = CreateInstance(matchValue.GetType().GetGenericArguments()[0]);
                                        newItem.CopyPropertiesFromObject(item);
                                        ((IList)matchValue).Add(newItem);
                                    }
                                }
                                // Копирование единичных классов с рекурсивным вызовом процедуры копирования для получения глубоких копий (deep copy)
                                else
                                    matchValue.CopyPropertiesFromObject(piFromValue);
                            }
                            // Копирование типов-значений и строк
                            else // if ((match.PropertyType.IsValueType) || (match.PropertyType.Equals(typeof(String))))
                                match.SetValue(obj, piFromValue, null);
                        }
                    }
                }
            }
        }
    }
}