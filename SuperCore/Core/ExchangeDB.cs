using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Data.OleDb;
using System.Text;
using ru.novolabs.SuperCore.HemBusinessObjects;
using ru.novolabs.SuperCore.HemDictionary;

namespace ru.novolabs.SuperCore
{

    public class ExchangeDB
    {
        public String ConnectionString { get; private set; }
        private OleDbConnection connection;
        private Hashtable mapping = new Hashtable();

        public ExchangeDB(String defaultConnectionString)
        {
            ConnectionString = defaultConnectionString;
            ObjectReader reader = new ObjectReader();

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)
    + SettingsConst.Mapping_File_Name;
            Log.WriteText("Reading mapping file...");
            reader.ReadXMLMappingFromFile(path, mapping);
        }

        private Object[] getObjectPropInfo(Object obj, String[] propNames)
        {
            if (null == obj) return null;

            if (propNames.Length == 1)
            {
                return new Object[] { obj, obj.GetType().GetProperty(propNames[0]) };
            }

            PropertyInfo propInfo = obj.GetType().GetProperty(propNames[0]);
            ArrayList subPropsName = new ArrayList(propNames);
            subPropsName.RemoveAt(0);

            Object subObj = propInfo.GetValue(obj, null);
            if (subObj == null)
                propInfo.SetValue(obj, subObj = Activator.CreateInstance(propInfo.PropertyType), null);

            return getObjectPropInfo(subObj, (String[])(subPropsName.ToArray(typeof(String))));
        }

        public Boolean GetNameValues(List<OrmPropInfo> map, Object obj, Hashtable values)
        {
            values.Clear();
            foreach (OrmPropInfo ormPropInfo in map)
            {
                if (String.IsNullOrEmpty(ormPropInfo.Column)) continue;
                if (null != values[ormPropInfo.Column])
                {
                    Debug.WriteLine(String.Format("Duplicate column reference '{0}', column skipped", ormPropInfo.Column));
                    continue;
                }

                Debug.WriteLine(ormPropInfo.Property);
                Object[] propResult = getObjectPropInfo(obj, ormPropInfo.Property.Split('.'));
                Object subObj = propResult[0];
                PropertyInfo propInfo = (PropertyInfo)propResult[1];
                if (propInfo == null)
                {
                    continue;
                }

                String stringValue = GetStringValue(propInfo, subObj, ormPropInfo);

                if ((stringValue != null) || (ormPropInfo.Not_null))
                {
                    Object value = GetPropValue(stringValue, ormPropInfo);


                    values.Add(ormPropInfo.Column, value);
                }
            }

            return true;
        }

        private Object GetPropValue(string stringValue, OrmPropInfo ormPropInfo)
        {
            if ((ormPropInfo.Not_null) && (stringValue == null))
                stringValue = ormPropInfo.DefaultValue;

            if (OrmMappingTypes.VARCHAR.Equals(ormPropInfo.Type))
            {
                if (ormPropInfo.Length > 0)
                {
                    stringValue = stringValue.Substring(0, stringValue.Length < ormPropInfo.Length ? stringValue.Length : ormPropInfo.Length);
                }
                stringValue = stringValue.Replace('"', ' ');
                return "\"" + stringValue + "\"";
            }
            else if (OrmMappingTypes.NUMERIC.Equals(ormPropInfo.Type))
            {
                try
                {
                    return Int32.Parse(stringValue);
                }
                catch
                {
                    return 0;
                }
            }
            else if (OrmMappingTypes.DATE.Equals(ormPropInfo.Type))
            {
                DateTime d;
                if (DateTime.TryParse(stringValue, out d))
                {
                    return "{^" + String.Format("{0:0000}-{1:00}-{2:00}", d.Year, d.Month, d.Day) + "}";
                }

                return "";
            }
            else
            {
                return stringValue;
            }
        }


        private String GetStringValue(PropertyInfo propInfo, object obj, OrmPropInfo ormPropInfo)
        {
            if (propInfo == null)
            {
                return null;
            }

            try
            {
                LinkedDictionary displayValue = GetDisplayValueAttribute(propInfo);
                WriteNull writeNull = GetWriteNullAttribute(propInfo);

                Object value = propInfo.GetValue(obj, null);



                if ((value == null) && (writeNull != null) && (writeNull.IsWriteNull))
                {
                    value = writeNull.DefaultValue;
                }

                if (value == null)
                {
                    return null;
                }
                else
                {
                    if (value.GetType().Equals(typeof(DateTime)))
                    {
                        return ((DateTime)value).ToShortDateString();
                    }
                    else if (value.GetType().Equals(typeof(float)))
                    {
                        return ((float)value).ToString((String)ProgramContext.Settings["numberFormatInfo"]);
                    }
                    else if ((value.GetType().Equals(typeof(int)) || (value.GetType().Equals(typeof(ObjectRef))))
                             && (displayValue != null))
                    {
                        return ProgramContext.HemCommunicator.DictionaryCache.GetDictionaryValue(value, displayValue);
                    }
                    else if (typeof(DictionaryItem).IsAssignableFrom(propInfo.PropertyType))
                    {
                        String propName = ((displayValue != null) && (!String.Empty.Equals(displayValue.PropertyName))) ? displayValue.PropertyName : "ExternalCode";
                        PropertyInfo pi = value.GetType().GetProperty(propName);
                        if (pi != null)
                        {
                            return pi.GetValue(value, null).ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (value.GetType().Equals(typeof(String)))
                    {
                        return (String)value;
                    }
                    else if (value.GetType().Equals(typeof(Int32)))
                    {
                        return value.ToString();
                    }

                    else return value.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Required property name is " + propInfo.Name);
                throw ex;
            }
        }


        private WriteNull GetWriteNullAttribute(PropertyInfo propInfo)
        {
            object[] attributes = propInfo.GetCustomAttributes(typeof(WriteNull), true);
            foreach (object obj in attributes)
            {
                return (WriteNull)obj;
            }
            return null;
        }

        private LinkedDictionary GetDisplayValueAttribute(PropertyInfo propInfo)
        {
            object[] attributes = propInfo.GetCustomAttributes(typeof(LinkedDictionary), true);
            foreach (object obj in attributes)
            {
                return (LinkedDictionary)obj;
            }
            return null;
        }

        public bool StroreObject(Object obj)
        {
            String className = obj.GetType().Name;
            SQLMap map = (SQLMap)mapping[className];


            if (map != null)
            {
                if (!CheckFilter(obj, map)) return false;

                if (ObjectIsStored(obj, map))
                    return UpdateObject(obj, map);
                else
                    return InsertObject(obj, map);
            }
            Log.WriteError(String.Format("Описание класса \"{0}\" не найдено в файле мэппинга", className));
            return false;
        }

        public bool CheckFilter(object obj)
        {
            String className = obj.GetType().Name;
            SQLMap map = (SQLMap)mapping[className];

            return CheckFilter(obj, map);
        }

        private bool CheckFilter(object obj, SQLMap map)
        {
            foreach (String property in map.Filters.Keys)
            {
                OrmFilterInfo filter = (OrmFilterInfo)map.Filters[property];
                PropertyInfo propInfo = obj.GetType().GetProperty(property);
                if (propInfo != null)
                {
                    Object value = propInfo.GetValue(obj, null);
                    if (value != null)
                    {
                        if (!String.Empty.Equals(filter.Equals))
                        {
                            return value.Equals(filter.Equals);
                        }

                        if (!String.Empty.Equals(filter.Not_equals))
                        {
                            return !value.Equals(filter.Not_equals);
                        }

                        if (!String.Empty.Equals(filter.Less_then))
                        {
                            return filter.Less_then.CompareTo(value) > 0;
                        }

                        if (!String.Empty.Equals(filter.Grater_then))
                        {
                            return filter.Grater_then.CompareTo(value) < 0;
                        }

                        if (filter.In_range.Count > 0)
                        {
                            Boolean result = false;
                            foreach (String range in filter.In_range)
                            {
                                if (range.Equals(value))
                                {
                                    result = true;
                                }
                            }
                            return result;
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        public string InsertQuery(object obj)
        {

            String className = obj.GetType().Name;
            SQLMap map = (SQLMap)mapping[className];
            String query = null;


            if (map != null)
            {


                Hashtable nameValues = new Hashtable();
                GetNameValues(map.Properties, obj, nameValues);


                String names = String.Empty;
                String valuesReal = String.Empty;

                List<Object> parameters = new List<Object>();
                foreach (String key in nameValues.Keys)
                    if (!String.IsNullOrEmpty(nameValues[key].ToString()))
                    {
                        if (!String.IsNullOrEmpty(names)) valuesReal += ",";
                        if (!String.IsNullOrEmpty(names)) names += ",";

                        names += key;
                        valuesReal += nameValues[key].ToString();
                        parameters.Add(nameValues[key]);

                        Log.WriteText("Parameter name = " + key + ",  value = " + nameValues[key].ToString());
                    }


                query = string.Format(SQL_Queries.KLADR_FIC_Queries.Insert_Object, map.Table, names, valuesReal);
                Log.WriteText(query + "\n\r");

                // Real values
                Log.WriteText(string.Format(SQL_Queries.KLADR_FIC_Queries.Insert_Object, map.Table, names, valuesReal) + "\n\r");

            }
            return query;
        }


        private bool InsertObject(object obj, SQLMap map)
        {
            Hashtable nameValues = new Hashtable();
            GetNameValues(map.Properties, obj, nameValues);

            String idColumn = map.Id.Column;
            String valueColumn = obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString();
            if (idColumn == null) return false;

            String names = String.Empty;
            String values = String.Empty;
            String valuesReal = String.Empty;

            List<OleDbParameter> parameters = new List<OleDbParameter>();
            foreach (String key in nameValues.Keys)
            {

                names += (", " + key);
                values += (", @" + key);
                valuesReal += (", " + nameValues[key]);
                parameters.Add(new OleDbParameter(key, nameValues[key]));
                Log.WriteText("Parameter name = " + key + ",  value = " + nameValues[key].ToString());
            }

            names = names.Substring(2) + ", " + idColumn;
            values = values.Substring(2) + ", @" + idColumn;
            Log.WriteText("Parameter name = " + idColumn + ",  value = " + obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString());
            parameters.Add(new OleDbParameter(idColumn, obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString()));

            String query = string.Format(SQL_Queries.KLADR_FIC_Queries.Insert_Object, map.Table, names, values);
            Log.WriteText(query + "\n\r");

            // Real values
            Log.WriteText(string.Format(SQL_Queries.KLADR_FIC_Queries.Insert_Object, map.Table, names, valuesReal) + "\n\r");


            return OleDbExecute(query, parameters);
        }

        private bool UpdateObject(object obj, SQLMap map)
        {
            Hashtable nameValues = new Hashtable();
            GetNameValues(map.Properties, obj, nameValues);

            String valuesReal = String.Empty;
            String names = String.Empty;

            String idColumn = map.Id.Column;
            if (idColumn == null) return false;

            String arguments = string.Empty;
            List<OleDbParameter> parameters = new List<OleDbParameter>();
            foreach (String key in nameValues.Keys)
            {
                //names += (", " + key);
                arguments += (", " + key + " = @" + key);
                //valuesReal += (", " + nameValues[key]);
                parameters.Add(new OleDbParameter(key, nameValues[key]));
                Log.WriteText("Parameter name = " + key + ",  value = " + nameValues[key].ToString());
            }

            arguments = arguments.Substring(2);
            parameters.Add(new OleDbParameter(idColumn, obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString()));
            Log.WriteText("Parameter name = " + idColumn + ",  value = " + obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString());

            String query = string.Format(SQL_Queries.KLADR_FIC_Queries.Update_Object, map.Table, arguments, idColumn);
            Log.WriteText(query + "\n\r");

            // Real values
            //Log.WriteText(string.Format(SQL_Queries.KLADR_FIC_Queries.Update_Object, map.Table, names, valuesReal) + "\n\r");

            return OleDbExecute(query, parameters);
        }

        private bool ObjectIsStored(object obj, SQLMap map)
        {
            String idColumn = map.Id.Column;
            if (idColumn == null) return false;
            String query = String.Format(SQL_Queries.KLADR_FIC_Queries.Find_Object, map.Table, idColumn);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter(idColumn, obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null)));
            DataTable table = GetSqlDataSet(query, parameters);
            return table.Rows.Count > 0;
        }

        public OleDbConnection GetConnection()
        {
            try
            {
                if (null == connection)
                    connection = new OleDbConnection(ConnectionString);

                return new OleDbConnection(ConnectionString);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Connection data base '{0}' fail. Reason: {1}", ConnectionString, ex.Message));
                return null;
            }

        }

        public DataTable GetSqlDataSet(string query, List<SqlParameter> parameters)
        {
            using (OleDbConnection conn = GetConnection())
            {
                using (DataSet ds = new DataSet())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        adapter.SelectCommand = new OleDbCommand(query, conn);
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                adapter.SelectCommand.Parameters.Add(param);
                            }
                        }
                        adapter.Fill(ds);
                        conn.Close();
                        adapter.Dispose();
                        conn.Dispose();
                        return ds.Tables[0];
                    }
                }
            }
        }

        public Boolean OleDbExecute(string query, List<OleDbParameter> parameters)
        {
            using (OleDbConnection conn = GetConnection())
            {
                using (OleDbCommand Command = new OleDbCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (OleDbParameter param in parameters)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                    conn.Open();

                    Command.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
                return true;
            }
        }

        public bool MarkAllAsRemoved(Object obj)
        {
            String className = obj.GetType().Name;
            SQLMap map = (SQLMap)mapping[className];
            if (map != null)
            {
                String query = string.Format(SQL_Queries.KLADR_FIC_Queries.Mark_As_Removed_Objects, map.Table);
                Log.WriteText(query + "\n\r");

                return OleDbExecute(query, null);
            }
            Log.WriteError(String.Format("Описание класса \"{0}\" не найдено в файле мэппинга", className));
            return false;
        }

        public SQLMap getMap(Type objType)
        {
            return (SQLMap)mapping[objType.Name];
        }

        public void FillObject(BaseObject obj, DataRow row)
        {
            SQLMap map = getMap(obj.GetType());
            foreach (OrmPropInfo ormProp in map.Properties)
            {
                String value = String.Empty;
                if (!String.IsNullOrEmpty(ormProp.Source))
                    value = row[ormProp.Source].ToString();

                value = processValue(value, map, ormProp);

                setObjectProp(obj, ormProp.Property.Split('.'), value, ormProp.Source);

            }
        }

        private string processValue(string strIn, SQLMap map, OrmPropInfo ormProp)
        {

            String encoding = map.Encoding;
            if (!String.IsNullOrEmpty(ormProp.Encoding))
                encoding = ormProp.Encoding;

            String value = strIn;

            if (XMLConst.XML_Attribute_Encoding_866.Equals(encoding))
            {
                char[] chars = strIn.ToCharArray();
                byte[] bytes = Encoding.GetEncoding(1251).GetBytes(strIn);
                byte[] b = Encoding.Convert(Encoding.GetEncoding(866), Encoding.GetEncoding(1251), bytes);
                value = Encoding.GetEncoding(1251).GetString(b);
            }


            if ((ormProp.Not_null) && String.IsNullOrEmpty(value))
                value = ormProp.DefaultValue;
            if (OrmMappingTypes.VARCHAR.Equals(ormProp.Type))
            {
                if (ormProp.Length > 0)
                    value = value.Substring(0, value.Length < ormProp.Length ? value.Length : ormProp.Length);
            }

            if (!XMLConst.XML_Attribute_Encoding_none.Equals(encoding)) return clearString(value.Trim());
            return value;
        }

        private void setObjectProp(Object obj, String[] propNames, string value, string source)
        {
            if (null == obj) return;
            if (String.IsNullOrEmpty(value.Trim())) return;

            if (propNames.Length == 1)
            {
                setObjectProp(obj, propNames[0], value, source);
                return;
            }

            Int32 index = getItemIndex(ref propNames[0]);
            PropertyInfo propInfo = obj.GetType().GetProperty(propNames[0]);
            ArrayList subPropsName = new ArrayList(propNames);
            subPropsName.RemoveAt(0);

            Object subObj = propInfo.GetValue(obj, null);
            if (subObj == null)
                propInfo.SetValue(obj, subObj = Activator.CreateInstance(propInfo.PropertyType), null);

            if (null != subObj.GetType().GetInterface("IList"))
                subObj = getListItemValue((IList)subObj, index);


            setObjectProp(subObj, (String[])(subPropsName.ToArray(typeof(String))), value, source);
        }

        private Int32 getItemIndex(ref String propName)
        {
            Int32 index = 0;
            Int32 a = propName.IndexOf("[");
            Int32 b = propName.IndexOf("]");
            if (b > a)
            {
                Int32.TryParse(propName.Substring(a + 1, b - a - 1), out index);
                propName = propName.Substring(0, a);
            }
            return index;
        }

        private void setObjectProp(Object obj, String propName, string value, string source)
        {
            try
            {
                PropertyInfo propInfo = obj.GetType().GetProperty(propName);
                Type propType = propInfo.PropertyType;
                bool isNullable = false;
                if (isNullable = (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propType = propType.GetGenericArguments()[0];

                Object objValue = null;
                if (null != propType.GetInterface("IList"))
                {
                    IList list = (IList)propInfo.GetValue(obj, null);
                    if (null == list)
                        propInfo.SetValue(obj, list = (IList)Activator.CreateInstance(propType), null);
                    Type itemType = propType.GetGenericArguments()[0];
                    if (null != itemType.GetInterface("IBaseDictionaryItem"))
                    {
                        if (null != (objValue = toDictionary(propType, value))) list.Add(objValue);
                    }
                    else if (typeof(BloodParameterValue).Equals(itemType))
                    {
                        addBloodParam((IList<BloodParameterValue>)list, value, source);
                    }
                }
                else if (typeof(int).Equals(propType) || typeof(Int32).Equals(propType))
                {
                    if ((null != (objValue = toInt(value))) || isNullable) propInfo.SetValue(obj, objValue, null);
                }
                else if (typeof(bool).Equals(propType) || typeof(Boolean).Equals(propType))
                {
                    if ((null != (objValue = toBoolean(value))) || isNullable) propInfo.SetValue(obj, objValue, null);
                }
                else if (typeof(DateTime).Equals(propType))
                {
                    if ((null != (objValue = toDateTime(value))) || isNullable) propInfo.SetValue(obj, objValue, null);
                }
                else if (typeof(float).Equals(propType) || typeof(Single).Equals(propType) || typeof(Double).Equals(propType))
                {
                    if ((null != (objValue = toFloat(value))) || isNullable) propInfo.SetValue(obj, objValue, null);
                }
                else if (null != propType.GetInterface("IBaseDictionaryItem"))
                {
                    if (null != (objValue = toDictionary(propType, value))) propInfo.SetValue(obj, objValue, null);
                }
                else if (typeof(String).Equals(propType))
                {
                    propInfo.SetValue(obj, value, null);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }

        }

        private String clearString(string value)
        {
            String result = String.Empty;
            foreach (char c in value.ToCharArray())
            {
                if (c >= ' ') result += c;
                else result += ' ';
            }
            return result;
        }

        private Object getListItemValue(IList list, int index)
        {
            while (list.Count < (index + 1))
            {
                list.Add(Activator.CreateInstance(list.GetType().GetGenericArguments()[0]));
            }

            return list[index];
        }

        private void addBloodParam(IList<BloodParameterValue> bloodParams, string value, string source)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrEmpty(source)) return;
            IBaseDictionary bloodDict = (IBaseDictionary)ProgramContext.HemCommunicator.DictionaryCache.GetDictionary(HemDictionaryNames.BloodParameterGroup);
            foreach (BloodParameterGroup grp in bloodDict.DictionaryElements)
                foreach (BloodParameterItem bp in grp.Parameters)
                    if (bp.Name.ToUpper().Equals(source.ToUpper()))
                    {
                        if (bp.UserDirectory == null) return;
                        foreach (UserDictionaryValue udv in bp.UserDirectory.Values)
                        {
                            if (udv.Name.Equals(value)
                                || value.Contains("\"" + udv.Name + "\""))
                            {
                                BloodParameterValue bpv = new BloodParameterValue();
                                bpv.Parameter = bp;
                                bpv.Reference = udv;
                                bloodParams.Add(bpv);
                                return;
                            }
                        }
                    }
        }

        private DateTime? toDateTime(String str)
        {
            try
            {
                return DateTime.Parse(str.Trim());
            }
            catch
            {
                return null;
            }
        }

        private Int32? toInt(String str)
        {
            try
            {
                return Int32.Parse(str.Trim());
            }
            catch
            {
                return null;
            }
        }

        private float? toFloat(String str)
        {
            try
            {
                return float.Parse(str.Trim());
            }
            catch
            {
                return null;
            }
        }

        private Boolean toBoolean(String str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            return (str.ToUpper() == "TRUE") || (str.ToUpper() == "Да") || (str == "1");
        }

        private IBaseDictionaryItem toDictionary(Type itemClass, String value)
        {
            if (String.IsNullOrEmpty(value)) return null;
            IBaseDictionary dict = (IBaseDictionary)ProgramContext.HemCommunicator.DictionaryCache.GetDictionary(itemClass);
            if (null == dict) return null;
            foreach (IBaseDictionaryItem item in dict.DictionaryElements)
            {
                if ((item.ExternalCode != null) && (value.ToUpper().Equals(item.ExternalCode.ToUpper()))) return item;
                if (item.Id.ToString().Equals(value)) return item;
            }

            return null;
        }
    }
    /*public class ExchangeDB
    {
        public String ConnectionString { get; private set; }
        private Hashtable mapping = new Hashtable();

        public ExchangeDB(String defaultConnectionString)
        {
            ConnectionString = defaultConnectionString;
            ObjectReader reader = new ObjectReader(ProgramContext.Dictionaries);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)
    + SettingsConst.Mapping_File_Name;
            Log.WriteText("Reading mapping file...");
            reader.ReadXMLMappingFromFile(path, mapping);
        }

        public Boolean GetNameValues(Hashtable map, Object obj, Hashtable values)
        {
            values.Clear();
            foreach (String column in map.Keys)
            {
                OrmPropInfo ormPropInfo = (OrmPropInfo)map[column];
                Debug.WriteLine(ormPropInfo.Property);
                PropertyInfo propInfo = obj.GetType().GetProperty(ormPropInfo.Property);
                if (propInfo == null)
                {
                  //  continue;
                }

                String stringValue = GetStringValue(propInfo, obj, ormPropInfo);
                if ((stringValue != null) || (ormPropInfo.Not_null))
                {
                    Object value = GetPropValue(stringValue, ormPropInfo);

                    values.Add(ormPropInfo.Column, value);
                }
            }

            return true;
        }

        private Object GetPropValue(string stringValue, OrmPropInfo ormPropInfo)
        {
            if ((ormPropInfo.Not_null) && (stringValue == null))
                stringValue = ormPropInfo.DefaultValue;    
            
            if (OrmMappingTypes.VARCHAR.Equals(ormPropInfo.Type))
            {
                if (ormPropInfo.Length > 0)
                {
                    return stringValue.Substring(0, stringValue.Length < ormPropInfo.Length ? stringValue.Length : ormPropInfo.Length);
                }
                return stringValue; 
            }
            else if (OrmMappingTypes.NUMERIC.Equals(ormPropInfo.Type))
            {
                try
                {
                    return Int32.Parse(stringValue);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return stringValue;
            }
        }


        private String GetStringValue(PropertyInfo propInfo, object obj, OrmPropInfo ormPropInfo)
        {
            if (propInfo == null)
            {
                return null;
            }

            try
            {
                LinkedDictionary displayValue = GetDisplayValueAttribute(propInfo);
                WriteNull writeNull = GetWriteNullAttribute(propInfo);

                Object value = propInfo.GetValue(obj, null);



                if ((value == null) && (writeNull != null) && (writeNull.IsWriteNull))
                {
                    value = writeNull.DefaultValue;
                }

                if (value == null)
                {
                    return null;
                }
                else
                {
                    if (value.GetType().Equals(typeof(DateTime)))
                    {
                        return ((DateTime)value).ToShortDateString();
                    }
                    else if (value.GetType().Equals(typeof(float)))
                    {
                        return ((float)value).ToString(ProgramContext.Settings.NumberFormatInfo);
                    }
                    else if ((value.GetType().Equals(typeof(int)) || (value.GetType().Equals(typeof(ObjectRef))))
                             && (displayValue != null))
                    {
                        return ProgramContext.Dictionaries.GetDictionaryValue(value, displayValue);
                    }
                    else if (typeof(DictionaryItem).IsAssignableFrom(propInfo.PropertyType))
                    {
                        String propName = ((displayValue != null) && (!String.Empty.Equals(displayValue.PropertyName))) ? displayValue.PropertyName : "Name";
                        PropertyInfo pi = value.GetType().GetProperty(propName);
                        if (pi != null)
                        {
                            return pi.GetValue(value, null).ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (value.GetType().Equals(typeof(String)))
                    {
                        return (String)value;
                    }
                    else if (value.GetType().Equals(typeof(Int32)))
                    {
                        return value.ToString();
                    }

                    else return value.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("Required property name is " + propInfo.Name);
                throw ex;
            }            
        }


        private WriteNull GetWriteNullAttribute(PropertyInfo propInfo)
        {
            object[] attributes = propInfo.GetCustomAttributes(typeof(WriteNull), true);
            foreach (object obj in attributes)
            {
                return (WriteNull)obj;
            }
            return null;
        }

        private LinkedDictionary GetDisplayValueAttribute(PropertyInfo propInfo)
        {
            object[] attributes = propInfo.GetCustomAttributes(typeof(LinkedDictionary), true);
            foreach (object obj in attributes)
            {
                return (LinkedDictionary)obj;
            }
            return null;
        }

        public bool StroreObject(Object obj)
        {
            String className = obj.GetType().Name;
            SQLMap map = (SQLMap)mapping[className];
            if (map != null)
            {
                if (ObjectIsStored(obj, map))
                    return UpdateObject(obj, map);
                else
                    return InsertObject(obj, map);
            }
            Log.WriteError(String.Format("Описание класса \"{0}\" не найдено в файле мэппинга", className));
            return false;
        }

        private bool InsertObject(object obj, SQLMap map)
        {
            Hashtable nameValues = new Hashtable();
            GetNameValues(map.Properties, obj, nameValues);

            String idColumn = map.Id.Column;
            if (idColumn == null) return false;

            String names = String.Empty;
            String values = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (String key in nameValues.Keys)
            {
                names += (", " + key);
                values += (", @" + key);
                parameters.Add(new SqlParameter(key, nameValues[key]));
                Log.WriteText("Parameter name = " + key + ",  value = " + nameValues[key].ToString());
            }

            names = names.Substring(2) + ", " + idColumn;
            values = values.Substring(2) + ", @" + idColumn;
            Log.WriteText("Parameter name = " + idColumn + ",  value = " + obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString());
            parameters.Add(new SqlParameter(idColumn, obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString()));

            String query = string.Format(SQL_Queries.KLADR_FIC_Queries.Insert_Object, map.Table, names, values);
            Log.WriteText(query + "\n\r");

            return OleDbExecute(query, parameters);
        }

        private bool UpdateObject(object obj, SQLMap map)
        {
            Hashtable nameValues = new Hashtable();
            GetNameValues(map.Properties, obj, nameValues);

            String idColumn = map.Id.Column;
            if (idColumn == null) return false;

            String arguments = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (String key in nameValues.Keys)
            {
                arguments += (", " + key + " = @" + key);
                parameters.Add(new SqlParameter(key, nameValues[key]));
                Log.WriteText("Parameter name = " + key + ",  value = " + nameValues[key].ToString());
            }

            arguments = arguments.Substring(2);
            parameters.Add(new SqlParameter(idColumn, obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString()));
            Log.WriteText("Parameter name = " + idColumn + ",  value = " + obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null).ToString());

            String query = string.Format(SQL_Queries.KLADR_FIC_Queries.Update_Object, map.Table, arguments, idColumn);
            Log.WriteText(query + "\n\r");

            return OleDbExecute(query, parameters);
        }

        private bool ObjectIsStored(object obj, SQLMap map)
        {
            String idColumn = map.Id.Column;
            if (idColumn == null) return false;
            String query = String.Format(SQL_Queries.KLADR_FIC_Queries.Find_Object, map.Table, idColumn);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter(idColumn, obj.GetType().GetProperty(map.Id.Property).GetValue(obj, null)));
            DataTable table = GetSqlDataSet(query, parameters);
            return table.Rows.Count > 0;
        }

        public SqlConnection GetConnection()
        {
            try
            {
                return new SqlConnection(ConnectionString);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Connection data base '{0}' fail. Reason: {1}", ConnectionString, ex.Message));
                return null;
            }

        }

        public DataTable GetSqlDataSet(string query, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (DataSet ds = new DataSet())
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.SelectCommand = new SqlCommand(query, conn);
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                adapter.SelectCommand.Parameters.Add(param);
                            }
                        }
                        adapter.Fill(ds);
                        conn.Close();
                        adapter.Dispose();
                        conn.Dispose();
                        return ds.Tables[0];
                    }
                }
            }
        }

        public Boolean OleDbExecute(string query, List<SqlParameter> parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand Command = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                    conn.Open();
                    Command.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
                return true;
            }
        }

        public bool MarkAllAsRemoved(Object obj)
        {
            String className = obj.GetType().Name;
            SQLMap map = (SQLMap)mapping[className];
            if (map != null)
            {
                String query = string.Format(SQL_Queries.KLADR_FIC_Queries.Mark_As_Removed_Objects, map.Table);
                Log.WriteText(query + "\n\r" );

                return OleDbExecute(query, null);
            }
            Log.WriteError(String.Format("Описание класса \"{0}\" не найдено в файле мэппинга", className));
            return false;
        }
    } */
}
