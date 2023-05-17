using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore
{
    public static class SerializeHelper2
    {
        public static void SaveAsXml(this Object obj, String fileName, Encoding encoding)
        {
            XDocument doc = new XDocument();
            foreach(PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (propInfo.PropertyType.IsSubclassOf(typeof(Object))) 
                    WriteXmlRootObject(doc, propInfo.Name, propInfo.GetValue(obj, null));
                
            }
            doc.Save(fileName);
        }

        private static void WriteXmlRootObject(XDocument doc, String name, Object obj)
        {
            doc.Add(GetXmlObjectElement(name, obj));
        }

        private static void WriteXmlObject(XElement elem, String name, Object obj)
        {
            elem.Add(GetXmlObjectElement(name, obj));
        }


        private static XElement GetXmlObjectElement(String name, Object obj)
        {
            XElement elem = new XElement(name);
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if ((propInfo.PropertyType == typeof(String)) ||
                    (propInfo.PropertyType == typeof(DateTime))) 
                    WriteXmlAttr(elem, propInfo.Name, propInfo.GetCustomAttributes(true), propInfo.GetValue(obj, null));
                else if (propInfo.PropertyType.GetInterface("IList") != null) 
                    WriteXmlList(elem, propInfo.Name, propInfo.GetCustomAttributes(true), propInfo.GetValue(obj, null));
                else if (propInfo.PropertyType.IsSubclassOf(typeof(Object)))
                    WriteXmlObject(elem, propInfo.Name, propInfo.GetValue(obj, null));
            }
            return elem;
        }

        private static void WriteXmlAttr(XElement elem, String name, Object[] attrs, Object value)
        {
            if (null == value) return;

            String strValue = value.ToString();
            if (value.GetType() == typeof(DateTime))
            {
                if ((DateTime)value == DateTime.MinValue) return;
                String format = @"yyyy-MM-ddTHH:mm:ss";  
                foreach (Object attr in attrs)
                    if (attr.GetType() == typeof(DateTimeFormat))
                    {
                        format = ((DateTimeFormat)attr).Format;
                        break;
                    }
                strValue = ((DateTime)value).ToString(format);
            }

            elem.Add(new XAttribute(name, strValue));
        }

        private static void WriteXmlList(XElement elem, String name, Object[] attrs, Object value)
        {
            Boolean noMainNoe = false;
            String listName = name;
            String elementName = name;

            foreach(Object attr in attrs)
                if (attr.GetType() == typeof(NoMainNode))
                {
                    noMainNoe = true;
                    break;
                }

            foreach (Object attr in attrs)
                if (attr.GetType() == typeof(ElementName))
                {
                    elementName = ((ElementName)attr).Name;
                    break;
                }


            XElement mainNode = elem;
            if (!noMainNoe)
            {
                mainNode = new XElement(listName);
                elem.Add(mainNode);
            }

            foreach (Object obj in (IList)value)
            {
                WriteXmlObject(mainNode, elementName, obj);
            }

        }



        public static Object LoadFromXml(Type type, String fileName)
        {
            Object result = Activator.CreateInstance(type);
            XDocument doc = XDocument.Load(fileName);            

            foreach (XElement elem in doc.Elements())
            {
                ReadXmlNode(result, elem);
            }

            return result;
        }

        private static void ReadXmlNode(Object parent, XElement elem)
        {
            

            String nodeName = elem.Name.ToString();

            IList list = FindList(parent, nodeName);
            if (list != null)
            {
                ReadXMLList(list, elem);
            }
            else
            {

                list = FindListByElement(parent, nodeName);
                if (list != null)
                {
                    ReadXMLListElement(list, elem);
                }
                else
                {
                    Object obj = FindObject(parent, nodeName);
                    if (obj != null)
                    {
                        ReadXmlObject(obj, elem);
                    }
                    else {
                        Log.WriteError("Элемент с именем {0} не найден в классе {1}", nodeName, null == parent ? "???" : parent.GetType().Name);
                    }
                }
            }
        }


        private static IList FindList(Object obj, string name)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (propInfo.PropertyType.GetInterface("IList") == null) continue;
                if (propInfo.GetCustomAttributes(typeof(NoMainNode), true).Length > 0) continue;
                if (propInfo.Name.Equals(name))
                {
                    Object list = propInfo.GetValue(obj, null);
                    if (list == null)
                    {
                        list = Activator.CreateInstance(propInfo.PropertyType);
                        propInfo.SetValue(obj, list, null);
                    }
                    return (IList)list;
                }
            }
            return null;
        }

        private static IList FindListByElement(Object obj, string name)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (propInfo.PropertyType.GetInterface("IList") == null) continue;
                if (propInfo.GetCustomAttributes(typeof(NoMainNode), true).Length == 0) continue;
                String elemName = propInfo.Name;
                Object[] attrs = propInfo.GetCustomAttributes(typeof(ElementName), true);
                if (attrs.Length > 0)
                    elemName = ((ElementName)attrs[0]).Name;

                if (elemName.Equals(name))
                {
                    Object list = propInfo.GetValue(obj, null);
                    if (list == null)
                    {
                        list = Activator.CreateInstance(propInfo.PropertyType);
                        propInfo.SetValue(obj, list, null);
                    }
                    return (IList)list;
                }
            }
            return null;
        }

        private static Object FindObject(Object obj, string name)
        {
            PropertyInfo propInfo = obj.GetType().GetProperty(name);
            if (propInfo == null) return null;

            if (!propInfo.PropertyType.IsSubclassOf(typeof(Object))) return null;

            Object value = propInfo.GetValue(obj, null);
            if (value == null)
            {
                value = Activator.CreateInstance(propInfo.PropertyType);
                propInfo.SetValue(obj, value, null);
            }
            return value;
        }

        private static void ReadXMLList(IList list, XElement elem)
        {

            foreach (XElement item in elem.Elements())
            {
                ReadXMLListElement(list, item);
            }
            //Object Item = Activator.CreateInstance(list.GetType().GetGenericArguments()[0]);
            //list.Add(Item);
            //ReadXmlObject(Item, elem);
        }

        private static void ReadXMLListElement(IList list, XElement elem)
        {
            Object Item = Activator.CreateInstance(list.GetType().GetGenericArguments()[0]);
            list.Add(Item);
            ReadXmlObject(Item, elem);
        }

        public static void ReadXmlObject(Object obj, XElement root)
        {
            ReadXmlAttributes(obj, root);
            foreach (XElement elem in root.Elements())
            {
                ReadXmlNode(obj, elem);
            }
        }

        public static void ReadXmlAttributes(Object obj, XElement elem)
        {
            foreach(XAttribute attr in elem.Attributes())
            {
                String name = attr.Name.ToString();
                PropertyInfo propInfo = obj.GetType().GetProperty(name);
                if (propInfo == null)
                {
                    Log.WriteError("Элемент с именем {0} не найден в классе {1}", name, null == obj ? "???" : obj.GetType().Name);
                    continue;
                }
                if (propInfo.PropertyType.Equals(typeof(String))) ReadStringAttr(obj, propInfo, attr);
                else if (propInfo.PropertyType.Equals(typeof(DateTime))) ReadDateTimeAttrAttr(obj, propInfo, attr);
            }
        }

        private static void ReadDateTimeAttrAttr(Object obj, PropertyInfo propInfo, XAttribute attr)
        {
            String strValue = string.Empty;
            try
            {
              strValue = attr.Value.ToString().Substring(0, 19).Replace('T', ' '); ;
              DateTime value = DateTime.ParseExact(strValue, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
              propInfo.SetValue(obj, value, null);
            }
            catch (Exception ex) {
                Log.WriteError(ex.Message + " " + strValue);
            }
        }

        private static void ReadStringAttr(Object obj, PropertyInfo propInfo, XAttribute attr)
        {
            String value = attr.Value.ToString();
            propInfo.SetValue(obj, value, null);
        }
        

    }
}
