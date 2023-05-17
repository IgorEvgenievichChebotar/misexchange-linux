using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore
{
    public enum ServerSendType
    {
        DEFAULT = 0,
        REF = 1,
        INT = 2
    }

    public class ObjectWriter
    {
        // Поставщик форматирования чисел, содержащий параметры форматирования для клиент-серверного взаимодействия
        private System.Globalization.NumberFormatInfo numberFormatInfo { get; set; }
        // Поставщик форматирования даты, содержащий параметры форматирования для клиент-серверного взаимодействия
        public System.Globalization.DateTimeFormatInfo dateTimeFormatInfo { get; set; }

        public ObjectWriter()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("ru");
            numberFormatInfo = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();
            dateTimeFormatInfo = (System.Globalization.DateTimeFormatInfo)ci.DateTimeFormat.Clone();

            // Заменяем разделитель дробной части, взятый из региональных настроек точкой, т.к. в числах с плавающей точкой,
            // приходящих в XML от сервера ЛИМС, всегда дробная часть отделена точкой. Без этого действия ни один справочник,
            // содержащий числа с плавающей точкой не сможет быть загружен в DictionaryCache
            numberFormatInfo.NumberDecimalSeparator = ".";
        }

        public String MakeXMLRequest(Object obj, String methodName, String sessionID, Boolean sendId)
        {
            return MakeXMLRequest(obj, methodName, sessionID, sendId, true);
        }

        public String MakeXMLRequest(Object obj, String methodName, String sessionID, Boolean sendId, Boolean writeObjectTag)
        {
            MemoryStream XMLStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.GetEncoding(1251);
            settings.NewLineOnAttributes = false;
            settings.Indent = true;
            
            XmlWriter writer = XmlWriter.Create(XMLStream, settings);

            writer.WriteStartDocument();
            // <!DOCTYPE phox-request SYSTEM "phox.dtd">
            writer.WriteDocType(XMLConst.XML_DOCTYPE_NAME, null, XMLConst.XML_DocType_SysId, null);
            // <phox-request type="login" version="3.8">
            writer.WriteStartElement(XMLConst.XML_Request);
            writer.WriteAttributeString(XMLConst.XML_Request_Type, methodName);
            writer.WriteAttributeString(XMLConst.XML_Request_Version, XMLConst.XML_Request_VersionId);           
            if (!XMLConst.XML_Request_BuildNumberId.Equals("@buildnumber@"))
                writer.WriteAttributeString(XMLConst.XML_Request_BuildNumber, XMLConst.XML_Request_BuildNumberId);

            if (!sessionID.Equals(String.Empty))
                writer.WriteAttributeString(XMLConst.XML_SessionID, sessionID);
            // <o>
            writer.WriteStartElement(XMLConst.XML_Node_Content);
            // <content>

            WriteObject(writer, String.Empty, obj, sendId, writeObjectTag, ServerSendType.DEFAULT);

            // </content>
            writer.WriteEndElement();
            // </phox-request>
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            String ResultString = System.Text.Encoding.GetEncoding(1251).GetString(XMLStream.ToArray());

            return ResultString;
        }



        public String MakeXMLResponce(Object obj, String sessionID, Boolean sendId, Boolean writeObjectTag)
        {
            using (var xmlStream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.GetEncoding(1251);
                settings.NewLineOnAttributes = false;
                settings.Indent = true;

                using (var writer = XmlWriter.Create(xmlStream, settings))
                {
                    writer.WriteStartDocument();
                    // <!DOCTYPE phox-response SYSTEM "phox.dtd">
                    writer.WriteDocType(XMLConst.XML_Node_Response, null, XMLConst.XML_DocType_SysId, null);
                    // <phox-response buildnumber="@version@">
                    writer.WriteStartElement(XMLConst.XML_Node_Response);
                    if (!XMLConst.XML_Request_BuildNumberId.Equals("@buildnumber@"))
                        writer.WriteAttributeString(XMLConst.XML_Request_BuildNumber, XMLConst.XML_Request_BuildNumberId);

                    if (!sessionID.Equals(String.Empty))
                        writer.WriteAttributeString(XMLConst.XML_SessionID, sessionID);
                    // <o>
                    writer.WriteStartElement(XMLConst.XML_Node_Content);
                    // <content>

                    WriteObject(writer, String.Empty, obj, sendId, writeObjectTag, ServerSendType.DEFAULT);

                    // </content>
                    writer.WriteEndElement();
                    // </phox-request>
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();

                    String ResultString = System.Text.Encoding.GetEncoding(1251).GetString(xmlStream.ToArray());

                    if ((ProgramContext.Settings != null) && (ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_DEBUG))
                        Log.WriteText(String.Format("\nReport Server Responce:\n\n {0}\n", ResultString));

                    return ResultString;
                }
            }
        }

        private Boolean IsSendable(PropertyInfo propInfo)
        {
            //PropertyInfo propInfo = obj.GetType().GetProperty(propName);
            Object[] attributes = propInfo.GetCustomAttributes(typeof(SendToServer), true);
            foreach (Object attribute in attributes)
            {
                if (!((SendToServer)attribute).Sendable)
                {
                    return false;
                }
            }

            return true;
        }

        private void WriteObject(XmlWriter writer, String name, Object obj, Boolean writeId, Boolean writeObjectTag, ServerSendType sendType)
        {
            
            PropertyInfo pi = obj.GetType().GetCustomIdProperty();
            Int32 id = 0;
            if (pi != null)
            {

                if (pi.PropertyType == typeof(ObjectRef))
                {
                    id = ((ObjectRef)pi.GetValue(obj, null)).GetRef();
                }
                else
                {
                    id = Convert.ToInt32(pi.GetValue(obj, null));
                }
            }

            if ((sendType == ServerSendType.REF) && (null != pi))
            {
                WriteReference(writer, name, new ObjectRef(id));
                return;
            }

            if ((sendType == ServerSendType.INT) && (null != pi))
            {
                WriteIntField(writer, name, id);
                return;
            }


            if (writeObjectTag) writer.WriteStartElement(XMLConst.XML_Node_Object);

            if (name != String.Empty)
            {
                writer.WriteAttributeString(XMLConst.XML_Attribute_Name, name);
            }

            if ((writeId) && (id > 0))
            {
                writer.WriteAttributeString(XMLConst.XML_Attribute_Obejct_ID, id.ToString());
            }
             
            

            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                string propName;
                bool hasCustomName = false;
                if (obj.GetType().IsAnonymousType())
                    propName = propInfo.Name;
                else
                {
                    propName = propInfo.GetDecryptedPropName();
                    hasCustomName = !String.IsNullOrEmpty(propName);                
                }

                if (obj.GetType().IsAnonymousType() || hasCustomName)
                {
                    if (IsSendable(propInfo) && (propInfo.GetValue(obj, null) != null))
                    {
                        // Генерируем имя в кэмел кейсинге.
                        String camelField = propName;
                        camelField = camelField[0].ToString().ToLower() + camelField.Substring(1);

                        Type pType = propInfo.PropertyType;

                        // Если свойство имеет тип дженерика
                        if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            pType = pType.GetGenericArguments()[0];

                            if (pType == typeof(DateTime))
                            {
                                WriteDateTimeField(writer, camelField, Convert.ToDateTime(propInfo.GetValue(obj, null).ToString()));
                            }
                            else if (pType == typeof(Int32))
                            {
                                WriteIntField(writer, camelField, Convert.ToInt32(propInfo.GetValue(obj, null).ToString()));
                            }
                            else if (pType == typeof(Int64))
                            {
                                WriteLongField(writer, camelField, Convert.ToInt64(propInfo.GetValue(obj, null).ToString()));
                            }
                            else if (pType == typeof(Single) || pType == typeof(Double))
                            {
                                WriteFloatField(writer, camelField, Convert.ToDouble(propInfo.GetValue(obj, null).ToString()));
                            }
                            else if (pType == typeof(Boolean))
                            {
                                WriteBooleanField(writer, camelField, Convert.ToBoolean(propInfo.GetValue(obj, null).ToString()));
                            }
                        }
                        // Если свойство нативного типа
                        else
                        {
                            if (pType == typeof(String))
                            {
                                WriteStringField(writer, camelField, propInfo.GetValue(obj, null).ToString());
                            }
                            else if (pType == typeof(DateTime))
                            {
                                WriteDateTimeField(writer, camelField, Convert.ToDateTime(propInfo.GetValue(obj, null)));
                            }
                            else if (pType == typeof(Int32))
                            {
                                WriteIntField(writer, camelField, Convert.ToInt32(propInfo.GetValue(obj, null)));
                            }
                            else if (pType == typeof(Single) || pType == typeof(Double))
                            {
                                WriteFloatField(writer, camelField, Convert.ToDouble(propInfo.GetValue(obj, null)));
                            }
                            else if (pType == typeof(Boolean))
                            {
                                WriteBooleanField(writer, camelField, Convert.ToBoolean(propInfo.GetValue(obj, null)));
                            }
                            else if ((pType == typeof(ObjectRef)))
                            {
                                WriteReference(writer, camelField, (ObjectRef)propInfo.GetValue(obj, null));
                            }
                            else if (pType.IsSubclassOf(typeof(DictionaryItem)))
                            {
                                WriteReference(writer, camelField, new ObjectRef((IBaseObject)propInfo.GetValue(obj, null)));
                            }
                            else if (propInfo.PropertyType == typeof(Color))
                            {
                                WriteColor(writer, propInfo.Name, (Color)propInfo.GetValue(obj, null));
                            }
                            else if (pType.GetInterface("IList", false) != null)
                            {
                                WriteSet(writer, camelField, propInfo.GetValue(obj, null), getSengType(propInfo));
                            }
                            else if (propInfo.GetValue(obj, null).GetType().GetProperties().GetLength(0) != 0)
                            {
                                WriteObject(writer, camelField, propInfo.GetValue(obj, null), true, true, getSengType(propInfo));
                            }
                        }
                    }

                }
            }
            // </o>
            if (writeObjectTag)
                writer.WriteEndElement();
        }

        private ServerSendType getSengType(PropertyInfo propInfo)
        {
            Object[] attributes = propInfo.GetCustomAttributes(typeof(SendAsRef), true);
            foreach (Object attribute in attributes)
              if (((SendAsRef)attribute).AsRef)  return ServerSendType.REF;

            attributes = propInfo.GetCustomAttributes(typeof(SendAsInt), true);
            foreach (Object attribute in attributes)
                if (((SendAsInt)attribute).AsRef) return ServerSendType.REF;


            return ServerSendType.DEFAULT;
        }

        private void WriteColor(XmlWriter Writer, string FieldName, Color color)
        {
            WriteField(Writer, XMLConst.XML_FieldType_Color, FieldName, color.ToArgb().ToString());
        }

        private Boolean WriteSet(XmlWriter writer, String name, Object writeObj, ServerSendType sendType, string itemClassName = "")
        {
            IList Set = (IList)writeObj;
            writer.WriteStartElement(XMLConst.XML_Node_Set);

            if (name != String.Empty)
            {
                writer.WriteAttributeString(XMLConst.XML_Attribute_Name, name);
            }

            if (itemClassName != "")
            {
                writer.WriteAttributeString(XMLConst.XML_Attribute_Item_Class, itemClassName);
            }

            foreach (Object Obj in Set)
            {
                if (Obj != null)
                {
                    if (Obj.GetType() == typeof(String))
                    {
                        WriteStringField(writer, String.Empty, Obj.ToString());
                    }
                    else if (Obj.GetType() == typeof(DateTime))
                    {
                        WriteDateTimeField(writer, String.Empty, Convert.ToDateTime(Obj));
                    }
                    else if (Obj.GetType() == typeof(Int32))
                    {
                        WriteIntField(writer, String.Empty, Convert.ToInt32(Obj));
                    }
                    else if (Obj.GetType() == typeof(Single) || Obj.GetType() == typeof(Double))
                    {
                        WriteFloatField(writer, String.Empty, Convert.ToDouble(Obj));
                    }
                    else if (Obj.GetType() == typeof(Boolean))
                    {
                        WriteBooleanField(writer, String.Empty, Convert.ToBoolean(Obj));
                    }
                    else if (Obj.GetType() == typeof(ObjectRef))
                    {
                        WriteReference(writer, String.Empty, (ObjectRef)Obj);
                    }
                    else if (Obj.GetType().IsSubclassOf(typeof(DictionaryItem)))
                    {
                        WriteReference(writer, String.Empty, new ObjectRef(((IBaseObject)Obj).Id));
                    }
                    else if (Obj.GetType().GetInterface("IList", false) != null)
                    {
                        WriteSet(writer, String.Empty, Obj, sendType);
                    }
                    else if (Obj.GetType().GetProperties().GetLength(0) != 0)
                    {
                        WriteObject(writer, String.Empty, Obj, true, true, sendType);
                    }
                }
            }
            writer.WriteEndElement();
            return true;
        }

        private Boolean WriteField(XmlWriter writer, String fieldType, String fieldName, String fieldValue)
        {
            fieldName = FormatFieldName(fieldName);

            if (fieldName == "id")
                if (Int32.Parse(fieldValue) == 0)
                    return false;

            writer.WriteStartElement(XMLConst.XML_Node_Field);        
            writer.WriteAttributeString(XMLConst.XML_Attribute_Type, fieldType);
            if (fieldName != String.Empty)
            {
                writer.WriteAttributeString(XMLConst.XML_Attribute_Name, fieldName);
            }
            writer.WriteAttributeString(XMLConst.XML_Attribute_Value, fieldValue);
            writer.WriteEndElement();
            return true;
        }

        private String FormatFieldName(String fieldName)
        {
            return fieldName.Length > 0 ? fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1) : fieldName;
        }

        private void WriteStringField(XmlWriter writer, String fieldName, String value)
        {
            WriteField(writer, XMLConst.XML_FieldType_String, fieldName, value);
        }

        private void WriteIntField(XmlWriter writer, String fieldName, Int32 value)
        {
            WriteField(writer, XMLConst.XML_FieldType_Int, fieldName, value.ToString());
        }

        private void WriteLongField(XmlWriter writer, String fieldName, Int64 value)
        {
            WriteField(writer, XMLConst.XML_FieldType_Long, fieldName, value.ToString());
        }

        private void WriteFloatField(XmlWriter writer, String fieldName, Double value)
        {
            WriteField(writer, XMLConst.XML_FieldType_Float, fieldName, value.ToString(numberFormatInfo));
        }

        private void WriteDateTimeField(XmlWriter writer, String fieldName, DateTime value)
        {
            if (value != DateTime.MinValue)
            {
                WriteField(writer, XMLConst.XML_FieldType_Date, fieldName, value.ToString(dateTimeFormatInfo));
            }
        }

        private void WriteBooleanField(XmlWriter writer, String fieldName, Boolean value)
        {
            if (value)
            {
                WriteField(writer, XMLConst.XML_FieldType_Bool, fieldName, XMLConst.XML_Bool_Value_True);
            }
            else
            {
                WriteField(writer, XMLConst.XML_FieldType_Bool, fieldName, XMLConst.XML_Bool_Value_False);
            }
        }

        private void WriteReference(XmlWriter writer, String fieldName, ObjectRef value)
        {
            if (value.GetRef() > 0)
            {
                writer.WriteStartElement(XMLConst.XML_Node_Reference);
                if (fieldName != String.Empty)
                {
                    writer.WriteAttributeString(XMLConst.XML_Attribute_Name, fieldName);
                }
                writer.WriteAttributeString(XMLConst.XML_Attribute_ID, value.GetRef().ToString());
                writer.WriteEndElement();
            }
        }

        private void WriteReference(XmlWriter writer, String fieldName, IBaseObject value)
        {
            if (value.Id != 0)
            {
                writer.WriteStartElement(XMLConst.XML_Node_Reference);
                if (fieldName != String.Empty)
                {
                    writer.WriteAttributeString(XMLConst.XML_Attribute_Name, fieldName);
                }
                writer.WriteAttributeString(XMLConst.XML_Attribute_ID, value.Id.ToString());
                writer.WriteEndElement();
            }
        }

        public String MakeXMLSettings(Hashtable settingsTable)
        {
            MemoryStream XMLStream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.GetEncoding(1251);
            settings.NewLineOnAttributes = false;
            settings.Indent = true;

            XmlWriter writer = XmlWriter.Create(XMLStream, settings);

            writer.WriteStartDocument();
            // <!DOCTYPE phox-request SYSTEM "phox.dtd">
            writer.WriteDocType(XMLConst.XML_DOCTYPE_NAME, null, XMLConst.XML_DocType_SysId, null);
            // <phox-request type="login" version="3.8">
            writer.WriteStartElement(XMLConst.XML_Request);
            writer.WriteAttributeString(XMLConst.XML_Request_Type, "");
            writer.WriteAttributeString(XMLConst.XML_Request_Version, XMLConst.XML_Request_VersionId);
            // <content>            
            writer.WriteStartElement(XMLConst.XML_Node_Content);
            // <o>            
            writer.WriteStartElement(XMLConst.XML_Node_Object);

            WriteSettings(writer, settingsTable);

            // </o>
            writer.WriteEndElement();
            // </content>
            writer.WriteEndElement();
            // </phox-request>
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            String resultString = System.Text.Encoding.Default.GetString(XMLStream.ToArray());

            return resultString;
        }

        public void WriteSettings(XmlWriter writer, Hashtable setttingsTable)
        {
            foreach(var key in setttingsTable.Keys)
            {
                Object value = setttingsTable[key];

                if (value.GetType() == typeof(String))
                {
                    WriteStringField(writer, key.ToString(), value.ToString());
                }
                else if (value.GetType() == typeof(DateTime))
                {
                    WriteDateTimeField(writer, key.ToString(), Convert.ToDateTime(value));
                }
                else if (value.GetType() == typeof(Int32))
                {
                    WriteIntField(writer, key.ToString(), Convert.ToInt32(value));
                }
                else if (value.GetType() == typeof(Single) || value.GetType() == typeof(Double))
                {
                    WriteFloatField(writer, key.ToString(), Convert.ToDouble(value));
                }
                else if (value.GetType() == typeof(Boolean))
                {
                    WriteBooleanField(writer, key.ToString(), Convert.ToBoolean(value));
                }
                else if (value.GetType() == typeof(ObjectRef)) 
                {
                    WriteReference(writer, key.ToString(), (ObjectRef)value);
                }
                else if (value.GetType().IsSubclassOf(typeof(DictionaryItem)))
                {
                    WriteReference(writer, key.ToString(), new ObjectRef(((IBaseObject)value).Id));
                }
                else if (value.GetType().GetInterface("IList", false) != null)
                {
                    WriteSet(writer, key.ToString(), value, ServerSendType.DEFAULT, value.GetType().GetGenericArguments()[0].FullName);
                }
                else if (value.GetType().IsClass)
                {
                    WriteObject(writer, key.ToString(), value, false, true, ServerSendType.DEFAULT);
                }
            }
        }
    }
}