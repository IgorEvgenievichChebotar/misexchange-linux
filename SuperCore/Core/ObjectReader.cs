using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.HemBusinessObjects;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace ru.novolabs.SuperCore
{
    public class ObjectReader
    {
        // Поставщик форматирования чисел, содержащий параметры форматирования для клиент-серверного взаимодействия
        private System.Globalization.NumberFormatInfo numberFormatInfo { get; set; }
        // Поставщик форматирования даты, содержащий параметры форматирования для клиент-серверного взаимодействия
        public System.Globalization.DateTimeFormatInfo dateTimeFormatInfo { get; set; }

        private BaseDictionaryCache dictionaryCache = null;

        public ObjectReader()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("ru");
            numberFormatInfo = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();
            dateTimeFormatInfo = (System.Globalization.DateTimeFormatInfo)ci.DateTimeFormat.Clone();

            // Заменяем разделитель дробной части, взятый из региональных настроек точкой, т.к. в числах с плавающей точкой,
            // приходящих в XML от сервера ЛИМС, всегда дробная часть отделена точкой. Без этого действия ни один справочник,
            // содержащий числа с плавающей точкой не сможет быть загружен в DictionaryCache
            numberFormatInfo.NumberDecimalSeparator = ".";
        }

        public ObjectReader(BaseDictionaryCache dictionaryCache)
            : this()
        {
            this.dictionaryCache = dictionaryCache;
        }

        public Boolean ReadXMLObjectFromFile(String fileName, Object content, String objectName)
        {
            XmlTextReader Reader;
            MemoryStream MemStream;
            String SessionID = String.Empty;
            Byte[] XMLBytes;
            String XML = File.ReadAllText(fileName, Encoding.GetEncoding(1251));

            XML = NormalizeXML(XML);

            MemStream = new MemoryStream();
            XMLBytes = Encoding.GetEncoding(1251).GetBytes(XML);
            MemStream.Write(XMLBytes, 0, XML.Length);


            MemStream.Position = 0;
            Reader = new XmlTextReader(MemStream);

            while (Reader.Read())
            {
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Content.Equals(Reader.Name.ToLower()))
                {
                    while (Reader.Read())
                    {
                        if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Object.Equals(Reader.Name.ToLower()))
                        {
                            if ((!Reader.IsEmptyElement) && Reader.GetAttribute(XMLConst.XML_Attribute_Name).ToLower().Equals(objectName.ToLower()))
                            {
                                ReadObject(Reader, content);
                            }
                        }
                    }
                }
            }

            return false;
        }

        public Boolean ReadXMLMappingFromFile(String fileName, Hashtable mapping)
        {
            XmlTextReader Reader;
            MemoryStream MemStream;
            String SessionID = String.Empty;
            Byte[] XMLBytes;
            String XML = File.ReadAllText(fileName, Encoding.GetEncoding(1251));

            XML = NormalizeXML(XML);

            MemStream = new MemoryStream();
            XMLBytes = Encoding.GetEncoding(1251).GetBytes(XML);
            MemStream.Write(XMLBytes, 0, XML.Length);


            MemStream.Position = 0;
            Reader = new XmlTextReader(MemStream);

            try
            {
                while (Reader.Read())
                {
                    if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Mapping.Equals(Reader.Name.ToLower()))
                    {
                        while (Reader.Read())
                        {
                            if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Class.Equals(Reader.Name.ToLower())
                                && (!Reader.IsEmptyElement))
                            {
                                SQLMap sqlMap = new SQLMap();
                                String className = Reader.GetAttribute(XMLConst.XML_Attribute_NAME);
                                String table = Reader.GetAttribute(XMLConst.XML_Attribute_Table);
                                String source = Reader.GetAttribute(XMLConst.XML_Attribute_Source);
                                String sql = Reader.GetAttribute(XMLConst.XML_Attribute_sql);
                                String encoding = Reader.GetAttribute(XMLConst.XML_Attribute_Encoding);
                                sqlMap.Table = table;
                                sqlMap.Source = source;
                                sqlMap.Sql = sql;
                                sqlMap.Encoding = encoding;
                                mapping.Add(className, sqlMap);
                                while (Reader.Read() && !((Reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Class.Equals(Reader.Name.ToLower())))
                                {
                                    if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Id.Equals(Reader.Name.ToLower()))
                                    {
                                        OrmPropInfo propInfo = ReadPropInfo(Reader);
                                        if (propInfo.Column != string.Empty)
                                        {
                                            sqlMap.Id = propInfo;
                                        }
                                        else
                                        {
                                            Log.WriteError(String.Format("Mapping file {0} has some errors in line {1}", fileName, Reader.LineNumber));
                                        }
                                    }

                                    if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Property.Equals(Reader.Name.ToLower()))
                                    {
                                        OrmPropInfo propInfo = ReadPropInfo(Reader);
                                        if (propInfo.Property != string.Empty)
                                        {
                                            sqlMap.Properties.Add(propInfo);
                                        }
                                        else
                                        {
                                            Log.WriteError(String.Format("Mapping file {0} has some errors in line {1}", fileName, Reader.LineNumber));
                                        }
                                    }

                                    if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Filter.Equals(Reader.Name.ToLower()))
                                    {
                                        OrmFilterInfo filterInfo = ReadFilterInfo(Reader);
                                        if (filterInfo.Property != string.Empty)
                                        {
                                            sqlMap.Filters.Add(filterInfo.Property, filterInfo);
                                        }
                                        else
                                        {
                                            Log.WriteError(String.Format("Mapping file {0} has some errors in line {1}", fileName, Reader.LineNumber));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
                throw ex;
            }


            return false;
        }

        public OrmFilterInfo ReadFilterInfo(XmlTextReader reader)
        {
            String property = reader.GetAttribute(XMLConst.XML_Attribute_Property);
            String equals = reader.GetAttribute(XMLConst.XML_Attribute_Equals);
            String not_equals = reader.GetAttribute(XMLConst.XML_Attribute_Not_Equals);
            String less_then = reader.GetAttribute(XMLConst.XML_Attribute_Less_Then);
            String grater_then = reader.GetAttribute(XMLConst.XML_Attribute_Grater_Then);
            String in_range = reader.GetAttribute(XMLConst.XML_Attribute_In_Range);


            OrmFilterInfo filterInfo = new OrmFilterInfo(property, equals, not_equals, less_then, grater_then, in_range);

            return filterInfo;
        }

        public OrmPropInfo ReadPropInfo(XmlTextReader Reader)
        {
            String property = Reader.GetAttribute(XMLConst.XML_Attribute_NAME);
            String column = Reader.GetAttribute(XMLConst.XML_Attribute_Column);
            String type = Reader.GetAttribute(XMLConst.XML_Attribute_TYPE);
            Boolean not_null = XMLConst.XML_Bool_Value_True.Equals(Reader.GetAttribute(XMLConst.XML_Attribute_Not_Null));
            String defaultValue = Reader.GetAttribute(XMLConst.XML_Attribute_DefaultValue);
            String source = Reader.GetAttribute(XMLConst.XML_Attribute_Source);
            String encoding = Reader.GetAttribute(XMLConst.XML_Attribute_Encoding);

            Int32 length = 0;
            string attrLength = Reader.GetAttribute(XMLConst.XML_Attribute_Length);
            if (attrLength != null)
            {
                try
                {
                    length = Int32.Parse(attrLength);
                }
                catch { }
            }

            /*if ((column == null) || (column == string.Empty))
            {
                column = property;
            }*/

            OrmPropInfo propInfo = new OrmPropInfo(property, column, type, length, not_null, defaultValue, source, encoding);

            return propInfo;
        }

        public String ReadXMLObject(String xml, Object content, ref Int32 errorCode, ref String errorMessage)
        {
            errorCode = 0;
            errorMessage = String.Empty;

            if (xml.Length == 0)
                return String.Empty;

            XmlTextReader Reader;
            MemoryStream MemStream;
            String SessionID = String.Empty;
            Byte[] XMLBytes;

            xml = NormalizeXML(xml);

            MemStream = new MemoryStream();
            XMLBytes = Encoding.GetEncoding(1251).GetBytes(xml);
            MemStream.Write(XMLBytes, 0, xml.Length);

            MemStream.Position = 0;
            Reader = new XmlTextReader(MemStream);

            while (Reader.Read())
            {
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Response.Equals(Reader.Name.ToLower()))
                {
                    SessionID = Reader.GetAttribute(XMLConst.XML_SessionID);
                }
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Content.Equals(Reader.Name.ToLower()))
                {
                    if (content != null)
                    {
                        ReadResponceContent(Reader, content);
                    }
                }
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Error.Equals(Reader.Name.ToLower()))
                {
                    errorCode = Convert.ToInt32(Reader.GetAttribute(XMLConst.XML_Error_Code));
                    errorMessage = Reader.GetAttribute(XMLConst.XML_Error_Message);

                    return String.Empty;
                }
            }
            return SessionID;
        }

        public String ReadXMLSettings(String xml, Hashtable settingsTable, ref Int32 errorCode, ref String errorMessage)
        {
            errorCode = 0;
            errorMessage = String.Empty;

            if (xml.Length == 0)
                return String.Empty;

            XmlTextReader Reader;
            MemoryStream MemStream;
            String SessionID = String.Empty;
            Byte[] XMLBytes;

            xml = NormalizeXML(xml);

            MemStream = new MemoryStream();
            XMLBytes = Encoding.GetEncoding(1251).GetBytes(xml);
            MemStream.Write(XMLBytes, 0, xml.Length);

            MemStream.Position = 0;
            Reader = new XmlTextReader(MemStream);

            while (Reader.Read())
            {
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Response.Equals(Reader.Name.ToLower()))
                {
                    SessionID = Reader.GetAttribute(XMLConst.XML_SessionID);
                }
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Content.Equals(Reader.Name.ToLower()))
                {
                    if (settingsTable != null)
                    {
                        ReadSettingsContent(Reader, settingsTable);
                    }
                }
                if ((Reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Error.Equals(Reader.Name.ToLower()))
                {
                    errorCode = Convert.ToInt32(Reader.GetAttribute(XMLConst.XML_Error_Code));
                    errorMessage = Reader.GetAttribute(XMLConst.XML_Error_Message);

                    return String.Empty;
                }
            }
            return SessionID;
        }

        public static String CutDTD(String xml)
        {
            var regex = new Regex(XMLConst.DTDSysString);
            xml = regex.Replace(xml, String.Empty, 1);

            regex = new Regex(XMLConst.DTDPubString);
            xml = regex.Replace(xml, String.Empty, 1);

            regex = new Regex(XMLConst.DTDString);
            xml = regex.Replace(xml, String.Empty, 1);

            return xml;
        }

        public static String NormalizeXML(String xml)
        {
            return CutDTD(xml).Replace("&13", "%13").Replace("&10", "%10");
        }

        private Boolean ReadResponceContent(XmlTextReader reader, Object content)
        {
            while (reader.Read() && !((reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Content.Equals(reader.Name.ToLower())))
            {
                if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Object.Equals(reader.Name.ToLower()))
                {
                    if (!reader.IsEmptyElement)
                    {
                        ReadObject(reader, content);
                    }
                    return true;
                }
            }

            return true;
        }

        private Boolean ReadSettingsContent(XmlTextReader reader, Hashtable settingsTable)
        {
            while (reader.Read() && !((reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Content.Equals(reader.Name.ToLower())))
            {
                if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Object.Equals(reader.Name.ToLower()))
                {
                    if (!reader.IsEmptyElement)
                    {
                        ReadSettingsTable(reader, settingsTable);
                    }
                    return true;
                }
            }

            return true;
        }

        public Boolean ReadSettingsTable(XmlTextReader reader, Hashtable settingsTable)
        {
            while (reader.Read() && !((reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Object.Equals(reader.Name.ToLower())))
            {
                if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Field.Equals(reader.Name.ToLower()))
                {
                    ReadSettingsProperty(reader, settingsTable);
                }
                else if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Reference.Equals(reader.Name.ToLower()))
                {
                    ReadSettingsReference(reader, settingsTable);
                }
                else if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Set.Equals(reader.Name.ToLower()))
                {
                    if (!reader.IsEmptyElement)
                    {
                        ReadSettingsSet(reader, settingsTable);
                    }
                }
                else if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Object.Equals(reader.Name.ToLower()))
                {
                    if (!reader.IsEmptyElement)
                    {
                        ReadSettingsObject(reader, settingsTable);
                    }
                }
            }
            return true;
        }

        private void ReadSettingsObject(XmlTextReader reader, Hashtable settingsTable)
        {
            String Name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            String sType = reader.GetAttribute(XMLConst.XML_Attribute_Type);
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_Value);

            Type type = Assembly.GetCallingAssembly().GetType(sType, true, true);
            Object newObject = Activator.CreateInstance(type);
            ReadObject(reader, newObject);

            if (settingsTable != null)
            {
                settingsTable[Name] = newObject;
            }
        }

        public Boolean ReadObject(XmlTextReader reader, Object readObject)
        {
            Object SubObject = null;

            while (reader.Read() && !((reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Object.Equals(reader.Name.ToLower())))
            {
                if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Field.Equals(reader.Name.ToLower()))
                {
                    ReadProperty(reader, readObject);
                }
                else if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Reference.Equals(reader.Name.ToLower()))
                {
                    ReadReference(reader, readObject);
                }
                else if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Set.Equals(reader.Name.ToLower()))
                {
                    if (!reader.IsEmptyElement)
                    {
                        ReadSet(reader, readObject);
                    }
                }
                else if ((reader.NodeType == XmlNodeType.Element) && XMLConst.XML_Node_Object.Equals(reader.Name.ToLower()))
                {
                    if (!reader.IsEmptyElement)
                    {
                        SubObject = GetSubObject(readObject, reader);
                        if (null == SubObject)
                        {
                            PropertyInfo pi = GetPropInfo(readObject, reader);
                            if (pi != null)
                            {
                                Type subObjectType = pi.PropertyType;
                                SubObject = Activator.CreateInstance(subObjectType);
                                pi.SetValue(readObject, SubObject, null);
                            }
                        }
                        this.ReadObject(reader, SubObject);
                    }
                }
            }
            return true;
        }

        public void ReadContent(XmlNode content, Object contentObject)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                String right = ConvertEncode(content.InnerXml, Encoding.Default, Encoding.GetEncoding(1251));
                Byte[] bts = Encoding.UTF8.GetBytes(right);

                memStream.Write(bts, 0, bts.Length);
                memStream.Position = 0;

                using (XmlTextReader reader = new XmlTextReader(memStream))
                    ReadResponceContent(reader, contentObject);
            }
        }

        public static String ConvertEncode(String value, Encoding source, Encoding target)
        {
            Decoder Decoder = source.GetDecoder();
            Byte[] Bytes = target.GetBytes(value);
            Int32 len = Decoder.GetCharCount(Bytes, 0, Bytes.Length);
            Char[] Chars = new Char[len];
            Decoder.GetChars(Bytes, 0, Bytes.Length, Chars, 0);
            return new String(Chars);
        }

        // Возвращает экземпляр одного из наследников T.
        public T ReadContentDynamic<T>(XmlNode content) where T : class, new()
        {
            XmlNode o = content.SelectSingleNode(XMLConst.XML_Node_Object);
            String objType = o.Attributes[XMLConst.XML_Attribute_Type].Value;

            List<Type> types = new List<Type>(Assembly.GetCallingAssembly().GetExportedTypes());

            types.RemoveAll(type => !type.IsSubclassOf(typeof(T)));
            types.RemoveAll(type =>
            {
                Object[] attrs = type.GetCustomAttributes(typeof(LinkToJava), false);

                return attrs.Length == 0 || ((LinkToJava)attrs[0]).Link != Int32.Parse(objType); // attrs[0] - т.к. LinkToJava можно назначить только один раз.
            });

            if (types.Count == 0)
                throw new ApplicationException(String.Format("Нет ни одного наследника {0} с искомым Link ({1}).", typeof(T).Name, objType));

            if (types.Count > 1)
                throw new ApplicationException(String.Format("Несколько наследников {0} используют одинаковый LinkToJava({1}).", typeof(T).Name, objType));

            T instance = (T)Activator.CreateInstance(types[0]);

            ReadContent(content, instance);

            return instance;
        }

        public Object GetSubObject(Object obj, XmlTextReader reader)
        {
            if (obj == null)
                return null;
            String Name;
            String Type;
            String Value;

            Name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            Type = reader.GetAttribute(XMLConst.XML_Attribute_Type);
            Value = reader.GetAttribute(XMLConst.XML_Attribute_Value);

            return GetObjectProperty(obj, Name);
        }

        public PropertyInfo GetPropInfo(Object obj, XmlTextReader reader)
        {
            if (obj == null)
                return null;
            string attPropName = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            //string encryptedPropName = SecurityUtils.EncryptPropertyName(attPropName);
            return obj.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == attPropName.ToLower());
        }

        public Object GetObjectProperty(Object obj, String propName)
        {
            if (obj == null)
                return null;

            //string encryptedPropName = SecurityUtils.EncryptPropertyName(propName);
            PropertyInfo propInfo = obj.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == propName.ToLower());

            if (propInfo == null)
                return null;
            else
                return propInfo.GetValue(obj, null);

            /* if (obj != null)
             {
                 foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
                 {
                     if (propInfo.Name.Equals(propName))
                     {
                         return propInfo.GetValue(obj, null);
                     }
                 }
             }
             return null;*/




            /* propName = FormatPropName(propName);
             if (obj != null)
             {
                 foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
                 {
                     if (propInfo.Name.Equals(propName))
                     {
                         return propInfo.GetValue(obj, null);
                     }
                 }
             }
             return null;*/
        }

        private String FormatPropName(String propName)
        {
            if (String.IsNullOrEmpty(propName))
                return String.Empty;
            else
                return propName.Substring(0, 1).ToUpper() + propName.Substring(1);
        }

        public Boolean ReadProperty(XmlTextReader reader, Object readObject)
        {
            String Name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            String Type = reader.GetAttribute(XMLConst.XML_Attribute_Type);
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_Value);

            if (readObject != null)
            {
                SetObjectProperty(readObject, Name, Type, Value);
            }

            return true;
        }

        public Boolean ReadProperty(XmlTextReader reader, ref Object readObject)
        {

            String Type = reader.GetAttribute(XMLConst.XML_Attribute_Type);
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_Value);

            readObject = Convert.ChangeType(Value, readObject.GetType());

            return true;
        }

        public Boolean ReadSettingsProperty(XmlTextReader reader, Hashtable settingsTable)
        {
            String name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            String type = reader.GetAttribute(XMLConst.XML_Attribute_Type);
            String value = reader.GetAttribute(XMLConst.XML_Attribute_Value);

            if (settingsTable != null)
            {
                if (type.Equals(XMLConst.XML_FieldType_String))
                {
                    settingsTable[name] = value.ToString();
                }
                else if (type.Equals(XMLConst.XML_FieldType_Date))
                {
                    settingsTable[name] = Convert.ToDateTime(value);
                }
                else if (type.Equals(XMLConst.XML_FieldType_Int))
                {
                    settingsTable[name] = Convert.ToInt32(value);
                }
                else if (type.Equals(XMLConst.XML_FieldType_Float))
                {
                    settingsTable[name] = Convert.ToSingle(value);
                }
                else if (type.Equals(XMLConst.XML_FieldType_Bool))
                {
                    settingsTable[name] = Convert.ToBoolean(value);
                }
            }

            return true;
        }

        public Boolean ReadReference(XmlTextReader reader, Object readObject)
        {
            String Name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_ID);

            if (readObject != null)
            {
                SetObjectProperty(readObject, Name, XMLConst.XML_FieldType_Ref, Value);
            }
            return true;
        }

        public Boolean ReadSettingsReference(XmlTextReader reader, Hashtable settingsTable)
        {
            String Name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_ID);

            if (settingsTable != null)
            {
                settingsTable[Name] = new ObjectRef(Int32.Parse(Value));
            }
            return true;
        }

        public Boolean ReadColor(XmlTextReader reader, Object readObject)
        {
            String name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_ID);

            if (readObject != null)
            {
                SetObjectProperty(readObject, name, XMLConst.XML_FieldType_Ref, Value);
            }
            return true;
        }

        public Boolean SetObjectProperty(Object obj, String propName, String propType, String value)
        {
            //string encryptedPropName = SecurityUtils.EncryptPropertyName(propName);
            PropertyInfo propInfo = obj.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == propName.ToLower());
            if (propInfo == null || propInfo.CanWrite == false)
                return false;

            Type pType = propInfo.PropertyType;
            if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                pType = pType.GetGenericArguments()[0];
            }

            if ((pType == typeof(String)) && (XMLConst.XML_FieldType_String.Equals(propType.ToLower())))
            {
                propInfo.SetValue(obj, value, null);
            }
            else if ((pType == typeof(DateTime)) && (XMLConst.XML_FieldType_Date.Equals(propType.ToLower())))
            {
                DateTime Date = Convert.ToDateTime(value, dateTimeFormatInfo);
                propInfo.SetValue(obj, Date, null);
            }
            else if ((pType == typeof(Int32)) && (XMLConst.XML_FieldType_Int.Equals(propType.ToLower())))
            {
                propInfo.SetValue(obj, Convert.ToInt32(value), null);
            }
            else if ((pType == typeof(Int64)) && (XMLConst.XML_FieldType_Long.Equals(propType.ToLower())))
            {
                propInfo.SetValue(obj, Convert.ToInt64(value), null);
            }
            else if ((propInfo.PropertyType == typeof(Color)) && (XMLConst.XML_FieldType_Color.Equals(propType.ToLower())))
            {
                propInfo.SetValue(obj, Color.FromArgb(Convert.ToInt32(value)), null);
            }
            else if ((pType == typeof(Boolean)) && (XMLConst.XML_FieldType_Bool.Equals(propType.ToLower())))
            {
                if (XMLConst.XML_Bool_Value_True.Equals(value.ToLower()))
                {
                    propInfo.SetValue(obj, true, null);
                }
                else
                {
                    propInfo.SetValue(obj, false, null);
                }
            }
            else if ((pType == typeof(ObjectRef)) && (XMLConst.XML_FieldType_Ref.Equals(propType.ToLower())))
            {
                ObjectRef refObject = (ObjectRef)propInfo.GetValue(obj, null);
                if (refObject != null)
                {
                    refObject.SetRef(Convert.ToInt32(value));
                }
                else
                {
                    refObject = new ObjectRef(Convert.ToInt32(value));
                    propInfo.SetValue(obj, refObject, null);
                }
            }
            else if (pType.IsSubclassOf(typeof(DictionaryItem)) && ((XMLConst.XML_FieldType_Ref.Equals(propType.ToLower())) || (XMLConst.XML_FieldType_Int.Equals(propType.ToLower()))))
            {
                DictionaryItem dictionaryItem;

                if (dictionaryCache != null)
                    dictionaryItem = (DictionaryItem)dictionaryCache.GetItemByReference(pType, Int32.Parse(value));
                else
                    dictionaryItem = (DictionaryItem)propInfo.GetValue(obj, null);

                // Кэш справочников ещё не загружен или справочный элемент не был найден по Id, или Id элемента = 0
                if ((dictionaryCache == null) || (dictionaryItem == null) || (dictionaryItem.Id == 0))
                {
                    if (dictionaryItem == null)
                        dictionaryItem = (DictionaryItem)Activator.CreateInstance(pType);
                    ((DictionaryItem)dictionaryItem).Id = Int32.Parse(value);
                }

                propInfo.SetValue(obj, dictionaryItem, null);
            }
            else if ((pType.GetInterface("IBaseObject") != null) && (XMLConst.XML_FieldType_Ref.Equals(propType.ToLower())))
            {
                Object RefObject = propInfo.GetValue(obj, null);
                if (RefObject == null)
                    RefObject = Activator.CreateInstance(propInfo.PropertyType);

                (RefObject as IBaseObject).Id = Int32.Parse(value);
                (RefObject as IBaseObject).AsReference = true;
                propInfo.SetValue(obj, RefObject, null);
            }
            else if ((pType == typeof(Single)) && (XMLConst.XML_FieldType_Float.Equals(propType.ToLower())))
            {
                propInfo.SetValue(obj, Convert.ToSingle(value, numberFormatInfo), null);
            }
            else if (((pType == typeof(Double)) || (pType == typeof(float))) && (XMLConst.XML_FieldType_Float.Equals(propType.ToLower())))
            {
                propInfo.SetValue(obj, Convert.ToDouble(value, numberFormatInfo), null);
            }

            return true;
        }

        public void ReadSingleReference(XmlTextReader reader, Object readObject)
        {
            String Value = reader.GetAttribute(XMLConst.XML_Attribute_ID);

            if (readObject == null)
            { }


            if (readObject != null)
            {
                if (readObject.GetType().IsSubclassOf(typeof(DictionaryItem)))
                {
                    ((DictionaryItem)readObject).Id = Convert.ToInt32(Value);
                }
                if (readObject.GetType() == typeof(ObjectRef))
                {
                    ((ObjectRef)readObject).SetRef(Convert.ToInt32(Value));
                }
            }
        }

        public IList GetCollectionObject(Object obj, String propName)
        {
            if (null == obj)
                return null;

            //string encryptedPropName = SecurityUtils.EncryptPropertyName(propName);
            PropertyInfo propInfo = obj.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == propName.ToLower());
            if (propInfo == null)
                return null;

            return (IList)propInfo.GetValue(obj, null);

            /*  
              //    propName = FormatPropName(propName); 
              Object ListObject;
              IList List;

              if (obj != null)
              {
                  string encryptedPropName = SecurityUtils.EncryptPropertyName(propName);
                  foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
                  {
                      if (propInfo.Name.Equals(encryptedPropName))                                   
                      {
                          ListObject = propInfo.GetValue(obj, null);
                          List = (IList)ListObject;
                          if (List == null)
                          {
                              List = MakeNewList(obj, propInfo);
                              propInfo.SetValue(obj, List, null);
                          }

                          return List;
                      }
                  }
              }
              return null;*/
        }

        private IList MakeNewList(Object obj, PropertyInfo propInfo)
        {
            Type listType = propInfo.PropertyType;
            IList list = (IList)Activator.CreateInstance(listType);
            return list;
        }

        public IList GetUnnamedSet(Object obj)
        {
            Object ListObject;
            IList List;
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if (HasAttribute(propInfo, typeof(Unnamed)))
                    if (propInfo.PropertyType.GetInterface("IList") != null)
                    {
                        ListObject = propInfo.GetValue(obj, null);
                        List = (IList)ListObject;
                        return List;
                    }
            }

            return null;
        }

        public Boolean HasAttribute(PropertyInfo propInfo, Type attType)
        {
            Object[] attributes = propInfo.GetCustomAttributes(attType, true);
            return attributes.GetLength(0) > 0;
        }

        public Boolean ReadSet(XmlTextReader reader, Object readObject)
        {
            Object NewObject = null;
            IList list = null;
            Type ElementType;

            String name = reader.GetAttribute(XMLConst.XML_Attribute_Name);
            if (name != String.Empty)
            {
                list = GetCollectionObject(readObject, name);
            }
            else
            {
                list = GetUnnamedSet(readObject);
            }

            while (reader.Read() && !((reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Set.Equals(reader.Name.ToLower())))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (list != null)
                    {
                        if (list.GetType().GetGenericArguments().Length != 0)
                            ElementType = list.GetType().GetGenericArguments()[0];
                        else
                            ElementType = list.GetType().BaseType.GetGenericArguments()[0];
                        if (ElementType != typeof(String))
                        {
                            NewObject = Activator.CreateInstance(ElementType);
                        }
                        else
                        {
                            NewObject = "";
                        }
                        
                    }

                    if (XMLConst.XML_Node_Object.Equals(reader.Name.ToLower()))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            this.ReadObject(reader, NewObject);
                        }
                    }
                    else if (XMLConst.XML_Node_Reference.Equals(reader.Name.ToLower()))
                    {
                        ReadSingleReference(reader, NewObject);
                    }
                    else if (XMLConst.XML_Node_Field.Equals(reader.Name.ToLower()))
                    {
                        if (NewObject != null)
                            ReadProperty(reader, ref NewObject);
                        //else
                        //Debug.WriteLine(String.Format("Property [{0}] found in XML but not found in class or not instantiated", name));
                    }
                    else if (XMLConst.XML_Node_Set.Equals(reader.Name.ToLower()))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            ReadSet(reader, NewObject);
                        }
                    }

                    if ((list != null) && (NewObject != null))
                    {
                        if (NewObject.GetType() != typeof(BloodParameterValue))
                        {
                            list.Add(NewObject);
                        }
                        else
                        {
                            if (((BloodParameterValue)NewObject).Parameter.Id != 0)
                            {
                                list.Add(NewObject);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public Boolean ReadSettingsSet(XmlTextReader reader, Hashtable settingsTable)
        {
            Object newObject = null;
            String Name = reader.GetAttribute(XMLConst.XML_Attribute_Name);

            String itemClass = reader.GetAttribute(XMLConst.XML_Attribute_Item_Class);
            if (itemClass == null)
            {
                throw new ApplicationException(String.Format("Необходимо обязательно указать значение атрибута \"{0}\" для элемента <s> в строке № {1}",
                    XMLConst.XML_Attribute_Item_Class, reader.LineNumber));
            }

            Type elementType;
            // Пробуем найти тип элемента коллекции в текущей сборке
            try
            {
                elementType = Assembly.GetExecutingAssembly().GetType(itemClass, true, true);
            }
            catch (Exception)
            {
                // Если не удалось найти тип в текущей сборке, пробуем найти тип элемента коллекции в исполняемой сборке(c расширением *.exe, которая подключила текущую сборку)            
                elementType = Assembly.GetEntryAssembly().GetType(itemClass, true, true);
            }

            Type listType = typeof(List<>);
            Type combinedType = listType.MakeGenericType(elementType);
            IList list = (IList)Activator.CreateInstance(combinedType);

            while (reader.Read() && !((reader.NodeType == XmlNodeType.EndElement) && XMLConst.XML_Node_Set.Equals(reader.Name.ToLower())))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (list != null)
                    {
                        if (elementType != typeof(String))
                        {
                            newObject = Activator.CreateInstance(elementType);
                        }
                        else
                        {
                            newObject = null;
                        }
                    }

                    if (XMLConst.XML_Node_Object.Equals(reader.Name.ToLower()))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            this.ReadObject(reader, newObject);
                        }
                    }
                    else if (XMLConst.XML_Node_Reference.Equals(reader.Name.ToLower()))
                    {
                        ReadSingleReference(reader, newObject);
                    }
                    else if (XMLConst.XML_Node_Field.Equals(reader.Name.ToLower()))
                    {
                        ReadProperty(reader, newObject);
                    }
                    else if (XMLConst.XML_Node_Set.Equals(reader.Name.ToLower()))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            ReadSet(reader, newObject);
                        }
                    }

                    if ((list != null) && (newObject != null))
                    {
                        list.Add(newObject);
                    }
                }
            }

            settingsTable[Name] = list;

            return true;
        }
    }
}