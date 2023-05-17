using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore
{
    public static class SerializeHelper
    {
        /// <summary>
        /// Сериализация экземпляра справочника. Если какое-либо поле является ссылкой на другой справочник и помеченно аттрибутом XmlIgnore, то будет сохранено Id ссылки
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns>Xml файл сериализованного экземпляра</returns>
        public static string SerializeDictWithIgnoredReferences(this object obj, Encoding encoding)
        {
            string xml = obj.Serialize(Encoding.GetEncoding(1251));
            var itemsType = obj.GetType().GetGenericArguments()[0]; // Тип элементов справочника
            var elements = ((IBaseDictionary)obj).DictionaryElements;   // Элементы справочника
            XElement xel = XElement.Parse(xml);
            // Ищем ссылки на другие экземпляры с аттрибутом XmlIgnore, не списки
            foreach (PropertyInfo propInfo in itemsType.GetProperties().Where(x => x.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length > 0))
                foreach (var element in elements)
                {
                    if ((propInfo.PropertyType.IsClass) && (typeof(IEnumerable).IsAssignableFrom(propInfo.PropertyType) == false) && (!propInfo.PropertyType.Equals(typeof(String))))
                    {
                        Object refInstance = propInfo.GetValue(element, null);
                        if (refInstance != null)
                        {
                            var refType = refInstance.GetType();
                            PropertyInfo refIdProp = refType.GetProperties().FirstOrDefault(x => x.Name == "Id");
                            if (refIdProp == null)
                                continue;
                            int refId = (int)refIdProp.GetValue(refInstance, null);
                            XElement curEl = xel.Element("Elements").Elements(itemsType.Name).FirstOrDefault(x => x.Element("Id").Value == ((DictionaryItem)element).Id.ToString());
                            if(curEl != null)
                                curEl.Add(new XElement(refType.Name, new XElement("Id", refId)));
                        }
                    }
                }
            var st = new XStreamingElement("root");
            StringBuilder bld = new StringBuilder(); 
            XmlWriter wrt = XmlWriter.Create(bld);
            xel.WriteTo(wrt);
            wrt.Close();
            return bld.ToString();
        }
        public static String Serialize(this Object obj, Encoding encoding)
        {
            if (obj == null)
                throw new ArgumentException("Parameter obj must be not null.");

            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter encodingWriter = new StreamWriter(ms, encoding);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(encodingWriter, obj);
                ms.Position = 0;
                StreamReader encodingReader = new StreamReader(ms, encoding);
                return encodingReader.ReadToEnd();
            }
        }
        /// <summary>
        /// Десериализация экземпляра справочника вместе с заигноренными ссылками на справочники.
        /// </summary>
        /// <param name="type">Тип справочника, унаследованного от DictionaryClass</param>
        /// <param name="xml">Строка, содержащая данные справочника, сериализованные в xml</param>
        /// <param name="encoding"></param>
        /// <returns>Экземпляр справочника типа type</returns>
        public static object DeserializeDictWithIgnoredReferences(Type type, string xml, Encoding encoding)
        {
            object deserlzdObj = Deserialize(type, xml, encoding);
            var itemsType = deserlzdObj.GetType().GetGenericArguments()[0]; // Тип элементов справочника            
            // Ищем ссылки на другие экземпляры с аттрибутом XmlIgnore, не списки
            var properties = itemsType.GetProperties().Where(x => x.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length > 0 && x.PropertyType.IsClass);
            if (properties.Count() == 0)
                return deserlzdObj;
            var elements = ((IBaseDictionary)deserlzdObj).DictionaryElements;   // Элементы справочника  
            XElement xel = XElement.Parse(xml);            
            foreach (PropertyInfo propInfo in properties)
                foreach (var element in elements)          
                {
                    if ((typeof(IEnumerable).IsAssignableFrom(propInfo.PropertyType) == false) && (!propInfo.PropertyType.Equals(typeof(String))))
                    {
                        Type refType = propInfo.PropertyType;
                        XElement curEl = xel.Element("Elements").Elements(itemsType.Name).FirstOrDefault(x => x.Element("Id").Value == ((DictionaryItem)element).Id.ToString());
                        XElement refEl = curEl.Element(refType.Name);
                        if (curEl != null && refEl != null)
                        {
                            object refInstance = Deserialize(refType, refEl.ToString(), encoding);
                            propInfo.SetValue(element, refInstance, null);
                        }
                        
                    }
                }
            return deserlzdObj;
        }
        public static T Deserialize<T>(this String xml, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter encodingWriter = new StreamWriter(ms, encoding);
                encodingWriter.Write(xml);
                encodingWriter.Flush();
                ms.Position = 0;
                StreamReader encodingReader = new StreamReader(ms, encoding);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(encodingReader);
            }
        }

        public static Object Deserialize(Type type, String xml, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter encodingWriter = new StreamWriter(ms, encoding);
                encodingWriter.Write(xml);
                encodingWriter.Flush();
                ms.Position = 0;
                StreamReader encodingReader = new StreamReader(ms, encoding);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(encodingReader);
            }
        }

        private static void RemoveNilElements(XElement element)
        {
            foreach (XElement child in element.Elements())
                if (child.HasElements)
                    RemoveNilElements(child);

            var xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            var nills = from n in element.Elements()
                        where n.Attribute(xsi + "nil") != null
                        select n;

            nills.ToList().ForEach(x => x.Remove());
        }

        public static String RemoveNilElements(this String xmlText,LoadOptions options = LoadOptions.None )
        {
            var doc = XDocument.Parse(xmlText, options);
            RemoveNilElements(doc.Root);
            return doc.Declaration.ToString() + "\r\n" + doc.ToString();
        }

        public static String RemoveEmptyAttributes(this String xmlText, LoadOptions options = LoadOptions.None)
        {
            var doc = XDocument.Parse(xmlText, options);
            RemoveEmptyAttributes(doc.Root);
            return doc.Declaration.ToString() + "\r\n" + doc.ToString();
        }

        private static void RemoveEmptyAttributes(XElement element)
        {
            foreach (XElement child in element.Elements())
                RemoveEmptyAttributes(child);

            var emptyAttributes = (from a in element.Attributes()
                                   where String.IsNullOrEmpty(a.Value)
                                   select a).ToList();

            emptyAttributes.ForEach(x => x.Remove());
        }

        /// <summary>
        /// serializes the given object into memory stream
        /// </summary>
        /// <param name="obj">the object to be serialized</param>
        /// <returns>The serialized object as memory stream</returns>
        public static MemoryStream SerializeToStream(object obj)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011 // Тип или член устарел
            formatter.Serialize(stream, obj);
#pragma warning restore SYSLIB0011 // Тип или член устарел
            return stream;
        }

        /// <summary>
        /// deserializes as an object
        /// </summary>
        /// <param name="stream">the stream to deserialize</param>
        /// <returns>the deserialized object</returns>
        public static object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011 // Тип или член устарел
            object obj = formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Тип или член устарел
            return obj;
        }

        // Для того, чтобы объект мог быть побитно клонирован через память, его тип и все вложенные типы должны быть помечены атрибутом [Serializable] 
      /*  public static T BitwiseClone<T>(this T obj) where T : class
        {
            try
            {
                var ms = SerializeToStream(obj);
                return (T)DeserializeFromStream(ms);
            }
            catch (Exception ex)
            {
                throw new Exception("{AB5A2059-9735-4CBA-83C0-E94DEE894ED7}" + String.Format("\r\nUnable to bitwise clone object. Reason: {0}", ex.Message), ex);
            }
        }*/

        public static T1 CopyFromSubClassObject<T1, T2>(this T1 ancestorClassObject, T2 subClassObj)
            where T1 : class
            where T2 : T1
        {
            foreach (var pi in ancestorClassObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty))
            {
                try
                {
                    var sourcePropInfo = subClassObj.GetType().GetCustomProperty(pi.GetDecryptedPropName());
                    if (sourcePropInfo != null)
                    {
                        if (pi.CanWrite)
                            pi.SetValue(ancestorClassObject, sourcePropInfo.GetValue(subClassObj, null), null);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("<Guid(0C15AF72-015D-4774-A61C-F6631940FB78)>\r\n." + ex.Message, ex);
                }
            }
            return ancestorClassObject;
        }

        public static T1 CopyFromAncestorClassObject<T1, T2>(this T1 subClassObj, T2 ancestorClassObject)
            where T2 : class
            where T1 : T2
        {
            var props = subClassObj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy);

            foreach (var pi in subClassObj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy))
            {
                try
                {
                    var sourcePropInfo = ancestorClassObject.GetType().GetCustomProperty(pi.GetDecryptedPropName());
                    if (sourcePropInfo != null)
                    {
                        if (pi.CanWrite)
                            pi.SetValue(subClassObj, sourcePropInfo.GetValue(ancestorClassObject, null), null);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("<Guid(0C15AF72-015D-4774-A61C-F6631940FB78)>\r\n." + ex.Message, ex);
                }
            }
            return subClassObj;
        }
    }
}
