//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Reflection;
//using ru.novolabs.SuperCore.DictionaryCore;
//using ru.novolabs.SuperCore.LimsDictionary;

//namespace ru.novolabs.SuperCore
//{
//    public class FilterItem
//    {
//        public FilterItem(string AParameterName, string AParameterValue)
//        {
//            ParamenetName = AParameterName;
//            ParamenetValue = AParameterValue;
//        }
//        public string ParamenetName = string.Empty;
//        public string ParamenetValue = string.Empty;
//    }

//    public class DataSetFilter
//    {
//        public void AddFilter(string ParameterName, string ParameterValue)
//        {
//            Filter.Add(new FilterItem(ParameterName, ParameterValue));
//        }

//        public List<FilterItem> Filter = new List<FilterItem>();
//    }

//    public class DatasetBuilder
//    {
//        private BaseDictionaryCache dictionaryCash = null;
//        private String hyperlinkColumnName = String.Empty;
//        private String refPage = String.Empty;
//        private String refProp = String.Empty;


//        public String HyperlinkColumnName
//        {
//            get { return hyperlinkColumnName; }
//            set { hyperlinkColumnName = value; }
//        }

//        public String RefPage
//        {
//            get { return refPage; }
//            set { refPage = value; }
//        }


//        public String RefProp
//        {
//            get { return refProp; }
//            set { refProp = value; }
//        }

//        public Boolean IsReferenced
//        {
//            get { return !String.Empty.Equals(hyperlinkColumnName) && !String.Empty.Equals(refPage) && !String.Empty.Equals(refProp); }
//        }

//        public DatasetBuilder() { }

//        public DatasetBuilder(BaseDictionaryCache dictionaryCache)
//        {
//            this.dictionaryCash = dictionaryCache;
//        }

//        private bool Filter(DataSetFilter FilterObject, Object Obj)
//        {

//            bool Removed = false;
//            if (Obj.GetType().GetProperty("Removed") != null)
//            {
//                Removed = Convert.ToBoolean(Obj.GetType().GetProperty("Removed").GetValue(Obj, null));
//            }

//            if (Removed)
//            {
//                return false;
//            }

//            if (FilterObject == null) return true;
//            foreach (FilterItem FilterItem in FilterObject.Filter)
//            {
//                foreach (PropertyInfo propInfo in Obj.GetType().GetProperties())
//                {
//                    if (FilterItem.ParamenetName.ToLower().Equals(propInfo.Name.ToLower()))
//                    {
//                        if (propInfo.PropertyType == typeof(ObjectRef))
//                        {
//                            if (!(((ObjectRef)propInfo.GetValue(Obj, null)).GetRef().ToString() == FilterItem.ParamenetValue))
//                            {
//                                return false;
//                            }
//                        }
//                        else if (propInfo.GetType() == typeof(Object))
//                        {
//                            if (!(propInfo.GetValue(Obj, null).ToString().ToLower().Equals(FilterItem.ParamenetValue.ToLower())))
//                            {
//                                return false;
//                            }
//                        }
//                    }
//                }
//            }
//            return true;
//        }

//        public DataTable BuildDataTable(Object[] ObjectSets, DataSetFilter FilterObject, List<string> columns = null)
//        {
//            DataTable Table = new DataTable();
//            foreach (Object set in ObjectSets)
//            {
//                BuildDataTable(Table, set, FilterObject, columns);
//            }

//            return Table;
//        }

//        public DataTable BuildDataTable(DataTable Table, Object ObjectSet, DataSetFilter FilterObject, List<string> columns = null)
//        {

//            DataRow Row = null; ;
//            IList list = null;
//            Type ElementType;

//            list = (IList)ObjectSet;
//            ElementType = list.GetType().GetGenericArguments()[0];


//            foreach (PropertyInfo propInfo in ElementType.GetProperties())
//            {
//                if ((Table.Columns[propInfo.Name] == null)
//                     && columns.Exists(s => propInfo.Name.Equals(s)))
//                    Table.Columns.Add(propInfo.Name, GetCellType(propInfo));
//            }

//            if (IsReferenced)
//            {
//                Table.Columns.Add(hyperlinkColumnName, typeof(string));
//            }

//            foreach (Object Item in list)
//            {
//                if (Filter(FilterObject, Item))
//                {
//                    Row = Table.NewRow();
//                    foreach (PropertyInfo propInfo in Item.GetType().GetProperties())
//                    {
//                        if (columns.Exists(s => propInfo.Name.Equals(s)))
//                            Row[propInfo.Name] = GetRowValue(propInfo, Item);
//                    }

//                    Row[hyperlinkColumnName] = refPage + "?" + refProp + "=" + Item.GetType().GetProperty(refProp).GetValue(Item, null).ToString();
//                    Table.Rows.Add(Row);
//                }

//            }
//            return Table;

//        }

//        private Object GetRowValue(PropertyInfo propInfo, Object item)
//        {
//            Object value = propInfo.GetValue(item, null);
//            if (value == null) return null;

//            Object[] attributes = propInfo.GetCustomAttributes(typeof(LinkedDictionary), true);
//            foreach (LinkedDictionary attribute in attributes)
//            {
//                return dictionaryCash.GetDictionaryValue(value, attribute);
//            }

//            if ((propInfo.PropertyType == typeof(string)) || propInfo.PropertyType == typeof(int))
//                return value.ToString();
//            else if (propInfo.PropertyType == typeof(DateTime))
//                return GetDateTimeString((DateTime)value, propInfo);
//            else if (propInfo.PropertyType == typeof(Boolean))
//                return GetBooleanString((Boolean)value);
//            else return value;
//        }

//        private object GetBooleanString(Boolean value)
//        {
//            if (value) return "Да";
//            else return "Нет";
//        }

//        private object GetDateTimeString(DateTime value, PropertyInfo propInfo)
//        {
//            foreach (DateTimeFormat attribute in propInfo.GetCustomAttributes(typeof(DateTimeFormat), true))
//            {
//                return value.ToString(attribute.Format);
//            }
//            return value.ToString("dd/MM/yyyy HH:mm");
//        }

//        private Type GetCellType(PropertyInfo propInfo)
//        {
//            Object[] attributes = propInfo.GetCustomAttributes(typeof(LinkedDictionary), true);
//            foreach (Object attribute in attributes)
//            {
//                return typeof(string);
//            }

//            Type pType = propInfo.PropertyType;

//            if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
//                return pType.GetGenericArguments()[0];
//            else if (pType.Equals(typeof(Boolean)))
//                return typeof(string);


//            return pType;
//        }

//        public void SetDestination(String hyperlinkColumnName, String refPage, String refProp)
//        {
//            this.hyperlinkColumnName = hyperlinkColumnName;
//            this.refPage = refPage;
//            this.refProp = refProp;
//        }

//        public bool BuildDataSet(Object ObjectSet, string TableName, DataSetFilter FilterObject, DataSet DS, List<String> columns = null)
//        {
//            DataTable Table = BuildDataTable(new Object[] { ObjectSet }, FilterObject, columns);
//            Table.TableName = TableName;
//            DS.Tables.Add(Table);

//            return Table != null;
//        }

//        public DataSet MergeDataSetColumns(DataSet DS1, string TableName1, DataSet DS2, string TableName2, string ResultTableName)
//        {
//            DataSet DS = new DataSet();
//            DataTable ResultTable = DS.Tables.Add(ResultTableName);

//            DataTable Table1 = DS1.Tables[TableName1];
//            DataTable Table2 = DS2.Tables[TableName2];

//            foreach (DataColumn Column in Table1.Columns)
//            {
//                ResultTable.Columns.Add(Column.ColumnName);
//            }

//            foreach (DataColumn Column in Table2.Columns)
//            {
//                if (null == ResultTable.Columns[Column.ColumnName])
//                {
//                    ResultTable.Columns.Add(Column.ColumnName);
//                }
//            }

//            return DS;
//        }

//        public void ResetRow(DataTable Table, DataRow Row, string Value)
//        {
//            foreach (DataColumn Column in Table.Columns)
//            {
//                Row[Column.ColumnName] = Value;
//            }
//        }

//        public void MergeDataSetData(DataSet ResultDS, string ResultTableName, DataSet DS1, string TableName1,
//                           DataSet DS2, string TableName2, BaseMergeRules Rules)
//        {
//            DataTable Table1 = DS1.Tables[TableName1];
//            DataTable Table2 = DS2.Tables[TableName2];
//            DataTable ResultTable = ResultDS.Tables[ResultTableName];

//            foreach (DataRow Row in Table1.Rows)
//            {
//                DataRow NewRow = ResultTable.Rows.Add();
//                ResetRow(ResultTable, NewRow, "-");
//                foreach (DataColumn Column in Table1.Columns)
//                {
//                    string NewName = Rules.NameRule1(Column.ColumnName);
//                    if (null != ResultTable.Columns[NewName])
//                    {
//                        NewRow[NewName] = Row[Column.ColumnName];
//                    }
//                }
//            }

//            foreach (DataRow Row in Table2.Rows)
//            {
//                if (!Rules.DuplicateRule(ResultTable, Row))
//                {
//                    DataRow NewRow = ResultTable.Rows.Add();
//                    ResetRow(ResultTable, NewRow, "-");
//                    foreach (DataColumn Column in Table2.Columns)
//                    {
//                        string NewName = Rules.NameRule2(Column.ColumnName);
//                        if (null != ResultTable.Columns[NewName])
//                        {
//                            NewRow[NewName] = Row[Column.ColumnName];
//                        }
//                    }
//                }
//            }
//        }
//    }

//}
