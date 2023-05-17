using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore
{
    public class ErrorMessage
    {
        [CSN("Message")]
        public string Message { get; set; }

        [CSN("Severity")]
        public int Severity { get; set; }
    }

    [Serializable]
    public class BaseFilterSettings
    {
        public DateTime dateFrom = DateTime.Now;
        public DateTime dateTill = DateTime.Now;
        public string Name = string.Empty;
        public string Nr = string.Empty;
    }

    public interface IBaseObject
    {
        int Id { get; set; }
        bool AsReference { get; set; }
        bool Saved { get; }
    }

    public interface ISendableObject
    {
        void PrepareToSend();
    }

    public interface IClearableObject
    {
        void Clear();
    }

    public interface IJournalFilter : IClearableObject, ISendableObject
    {

    }

    [Serializable]
    public class BaseObject : IBaseObject
    {
        private int id = 0;
        private bool asReference = false;

        [CSN("Saved")]
        [XmlIgnore, SendToServer(false)]
        public bool Saved => id > 0;

        // Не совсем понятно назначение данного атрибута
        /*[XmlIgnore]*/
        [CSN("Id")]
        public int Id
        {
            get => id;
            set => id = value;
        }

        [SendToServer(false)]
        [XmlIgnore]
        [CSN("AsReference")]
        public bool AsReference
        {
            get => asReference;
            set => asReference = value;
        }
    }

    [Serializable]
    public class RemovableObject : BaseObject
    {
        public bool removed = false;
    }

    [Serializable]
    public class NamedObject : RemovableObject
    {
        public NamedObject()
        {
            Name = string.Empty;
            Code = string.Empty;
            Mnemonics = string.Empty;
        }
        [CSN("Name")]
        public string Name { get; set; }
        [CSN("Code")]
        public string Code { get; set; }
        [CSN("Mnemonics")]
        public string Mnemonics { get; set; }
    }

    [Serializable]
    public class RankedNamedObject : NamedObject
    {
        [CSN("Rank")]
        public int Rank { get; set; }
    }



    [Serializable]
    public class ObjectRef : IConvertible, IBaseObject
    {
        public ObjectRef() { }

        public ObjectRef(int NewID)
        {
            id = NewID;
        }

        public ObjectRef(IBaseObject iObject)
        {
            id = iObject.Id;
        }

        public ObjectRef(ObjectRef NewRef)
        {
            id = NewRef.GetRef(); ;
        }

        private int id;

        [CSN("Id")]
        public int Id
        {
            get => id;
            set => id = value;
        }

        [CSN("Saved")]
        [XmlIgnore, SendToServer(false)]
        public bool Saved => id > 0;

        public void SetRef(int Ref)
        {
            id = Ref;
        }

        public void SetRef(ObjectRef Ref)
        {
            id = Ref.GetRef();
        }

        public int GetRef()
        {
            return id;
        }

        public override bool Equals(object Obj)
        {
            return id == ((ObjectRef)Obj).id;
        }

        public override string ToString()
        {
            return id.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IBaseObject Members
        public bool isValidObject()
        {
            return Id > 0;
        }

        public bool AsReference { get => true; set { } }
        #endregion

        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Id > 0;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return (byte)Id;
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return Id;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return Id;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return (short)Id;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return id;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return Id;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return (sbyte)Id;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return Id;
        }

        public string ToString(IFormatProvider provider)
        {
            return id.ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Id;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return (ushort)Id;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return (uint)Id;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return (ulong)Id;
        }

        #endregion
    }

    public class RefSet : List<ObjectRef>
    {
        public RefSet() : base() { }

        public RefSet(int capacity) : base(capacity) { }

        public bool Exists(ObjectRef reference)
        {
            foreach (ObjectRef item in this)
            {
                if (item.Equals(reference))
                {
                    return true;
                }
            }
            return false;
        }

        public new void Add(ObjectRef item)
        {
            if (!Exists(item))
            {
                base.Add(item);
            }
        }
    }

    public class OrmFilterInfo
    {
        public OrmFilterInfo(string property)
        {
            this.property = property;
        }

        public OrmFilterInfo(string property, string equals, string not_equals, string less_then, string grater_then, string in_range)
        {
            if (property != null)
            {
                this.property = property;
            }

            if (equals != null)
            {
                this.equals = equals;
            }

            if (not_equals != null)
            {
                this.not_equals = not_equals;
            }

            if (less_then != null)
            {
                this.less_then = less_then;
            }

            if (grater_then != null)
            {
                this.grater_then = grater_then;
            }

            if (in_range != null)
            {
                string[] range = in_range.Split(',');
                foreach (string value in range)
                {
                    if (!value.Trim().Equals(string.Empty))
                    {
                        this.in_range.Add(value.Trim());
                    }
                }
            }
        }

        private string property = string.Empty;
        private HashSet<string> in_range = new HashSet<string>();
        private string equals = string.Empty;
        private string not_equals = string.Empty;
        private string less_then = string.Empty;
        private string grater_then = string.Empty;

        public string Property
        {
            get => property;
            set => property = value;
        }

        public HashSet<string> In_range
        {
            get => in_range;
            set => in_range = value;
        }

        public new string Equals
        {
            get => equals;
            set => equals = value;
        }

        public string Not_equals
        {
            get => not_equals;
            set => not_equals = value;
        }

        public string Less_then
        {
            get => less_then;
            set => less_then = value;
        }

        public string Grater_then
        {
            get => grater_then;
            set => grater_then = value;
        }
    }

    public class OrmPropInfo
    {
        public OrmPropInfo(string property, string column)
        {
            Property = property;
            Column = column;
            Type = OrmMappingTypes.VARCHAR;
        }

        public OrmPropInfo(string property, string column, string type)
        {
            Property = property;
            Column = column;
            if ((OrmMappingTypes.VARCHAR.Equals(type)) || (OrmMappingTypes.NUMERIC.Equals(type)))
            {
                Type = type;
            }
            else
            {
                Type = OrmMappingTypes.VARCHAR;
            }
        }

        public OrmPropInfo(string property, string column, string type, int length)
        {
            Property = property;
            Column = column;
            Length = length;
            if ((OrmMappingTypes.VARCHAR.Equals(type)) || (OrmMappingTypes.NUMERIC.Equals(type)))
            {
                Type = type;
            }
            else
            {
                Type = OrmMappingTypes.VARCHAR;
            }
        }

        public OrmPropInfo(string property, string column, string type, int length, bool not_null)
        {
            Property = property;
            Column = column;
            Length = length;
            Not_null = not_null;

            if ((OrmMappingTypes.VARCHAR.Equals(type)) || (OrmMappingTypes.NUMERIC.Equals(type)))
            {
                Type = type;
            }
            else
            {
                Type = OrmMappingTypes.VARCHAR;
            }
        }

        public OrmPropInfo(string property, string column, string type, int length, bool not_null, string defaultValue)
        {
            Property = property;
            Column = column;
            Length = length;
            Not_null = not_null;
            DefaultValue = defaultValue;

            if ((OrmMappingTypes.VARCHAR.Equals(type)) || (OrmMappingTypes.NUMERIC.Equals(type)))
            {
                Type = type;
            }
            else
            {
                Type = OrmMappingTypes.VARCHAR;
            }
        }

        public OrmPropInfo(string property, string column, string type, int length, bool not_null, string defaultValue, string source)
        {
            Property = property;
            Column = column;
            Length = length;
            Not_null = not_null;
            DefaultValue = defaultValue;
            Source = source;

            if ((OrmMappingTypes.VARCHAR.Equals(type)) || (OrmMappingTypes.NUMERIC.Equals(type))
                || (OrmMappingTypes.DATE.Equals(type)))
            {
                Type = type;
            }
            else
            {
                Type = OrmMappingTypes.UNDEFINED;
            }
        }

        public OrmPropInfo(string property, string column, string type, int length, bool not_null, string defaultValue, string source, string encoding)
        {
            Property = property;
            Column = column;
            Length = length;
            Not_null = not_null;
            DefaultValue = defaultValue;
            Source = source;
            Encoding = encoding;

            if ((OrmMappingTypes.VARCHAR.Equals(type)) || (OrmMappingTypes.NUMERIC.Equals(type))
                || (OrmMappingTypes.DATE.Equals(type)))
            {
                Type = type;
            }
            else
            {
                Type = OrmMappingTypes.UNDEFINED;
            }
        }

        private string property = string.Empty;
        private string column = string.Empty;
        private string type = string.Empty;
        private int length = 0;
        private bool not_null = false;
        private string defaultValue;
        private string source = string.Empty;
        private string encoding = string.Empty;

        public string Encoding
        {
            get => encoding;
            set => encoding = value;
        }

        public string Source
        {
            get => source;
            set => source = value;
        }

        public bool Not_null
        {
            get => not_null;
            set => not_null = value;
        }

        public int Length
        {
            get => length;
            set => length = value;
        }

        public string Type
        {
            get => type;
            set => type = value;
        }

        public string Property
        {
            get => property;
            set => property = value;
        }

        public string Column
        {
            get => column;
            set => column = value;
        }

        public string DefaultValue
        {
            get => defaultValue;
            set => defaultValue = value;
        }
    }

    public class SQLMap
    {
        private string sql = string.Empty;
        private string className = string.Empty;
        private string table = string.Empty;
        private string source = string.Empty;
        private OrmPropInfo id = null;
        private List<OrmPropInfo> properties = new List<OrmPropInfo>();
        private Hashtable filters = new Hashtable();
        private string encoding = string.Empty;

        public string Encoding
        {
            get => encoding;
            set => encoding = value;
        }



        public string Sql
        {
            get => sql;
            set => sql = value;
        }

        public string Source
        {
            get => source;
            set => source = value;
        }

        public Hashtable Filters
        {
            get => filters;
            set => filters = value;
        }

        public OrmPropInfo Id
        {
            get => id;
            set => id = value;
        }


        public string ClassName
        {
            get => className;
            set => className = value;
        }

        public string Table
        {
            get => table;
            set => table = value;
        }

        public List<OrmPropInfo> Properties
        {
            get => properties;
            set => properties = value;
        }
    }

    public class AndOrIdList : BaseObject
    {
        private bool isAnd = false;
        private List<ObjectRef> idList = new List<ObjectRef>();

        [CSN("Operator")]
        public string Operator
        {
            get => IsAnd ? AndOrListContst.AndOperator : AndOrListContst.OrOperator;
            set => IsAnd = value != AndOrListContst.OrOperator;
        }

        [CSN("IdList")]
        public List<ObjectRef> IdList
        {
            get => idList;
            set => idList = value;
        }

        [SendToServer(false)]
        [CSN("IsAnd")]
        public bool IsAnd
        {
            get => isAnd;
            set => isAnd = value;
        }

        public void Clear()
        {
            IdList.Clear();
            IsAnd = false;
        }
    }

    public class AndOrFieldList
    {
        private bool isAnd = false;
        private List<int> idList = new List<int>();

        [CSN("Operator")]
        public string Operator
        {
            get => IsAnd ? AndOrListContst.AndOperator : AndOrListContst.OrOperator;
            set => IsAnd = value != AndOrListContst.OrOperator;
        }

        [CSN("IdList")]
        public List<int> IdList
        {
            get => idList;
            set => idList = value;
        }

        [SendToServer(false)]
        [CSN("IsAnd")]
        public bool IsAnd
        {
            get => isAnd;
            set => isAnd = value;
        }

        public void Clear()
        {
            IdList.Clear();
            IsAnd = false;
        }
    }

    public class BaseJournalFilter : IJournalFilter
    {
        public bool DateAutpRound = true;

        public void InitDefaults(JournalFilterSettings journalFilterSettings)
        {
            foreach (ControlRow row in journalFilterSettings.Rows)
            {
                PropertyInfo propInfo = GetType().GetProperty(row.PropertyName);
                if (propInfo == null)
                {
                    continue;
                }

                Type pType = propInfo.PropertyType;

                if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    pType = pType.GetGenericArguments()[0];
                }

                string value = row.DefaultValue;

                if (pType == typeof(string))
                {
                    propInfo.SetValue(this, value, null);
                }
                else if (pType == typeof(DateTime))
                {
                    if (DateTime.TryParse(value, out DateTime dateTime))
                    {
                        propInfo.SetValue(this, dateTime, null);
                    }
                    else if (int.TryParse(value, out int int32))
                    {
                        propInfo.SetValue(this, DateTime.Now.AddDays(int32), null);
                    }
                }
                else if (pType == typeof(int))
                {
                    if (int.TryParse(value, out int int32))
                    {
                        propInfo.SetValue(this, int32, null);
                    }
                }
                else if (pType == typeof(float) || pType == typeof(double))
                {
                    if (float.TryParse(value, out float single))
                    {
                        propInfo.SetValue(this, single, null);
                    }
                }
                else if (pType == typeof(bool))
                {
                    if (bool.TryParse(value, out bool boolean))
                    {
                        propInfo.SetValue(this, boolean, null);
                    }
                }
                else if (pType == typeof(ObjectRef))
                {
                    if (int.TryParse(value, out int objRef))
                    {
                        propInfo.SetValue(this, new ObjectRef(objRef), null);
                    }
                }
                else if (typeof(DictionaryItem).IsAssignableFrom(pType))
                {
                    if (int.TryParse(value, out int objRef))
                    {
                        object obj = ProgramContext.Dictionaries.GetItemByReference(pType, objRef);
                        propInfo.SetValue(this, new ObjectRef(objRef), null);
                    }
                }
                else if (pType.GetInterface("IList", false) != null)
                {
                    List<int> objRefs = new List<int>();
                    string values = value.Trim().Replace(" ", "");
                    char separator = ';';
                    values.Split(separator).ToList().ForEach(v =>
                        {
                            if (int.TryParse(v, out int id))
                            {
                                objRefs.Add(id);
                            }
                        });

                    if (objRefs.Count > 0)
                    {
                        object list = propInfo.GetValue(this, null);
                        ((IList)list).Clear();
                        Type itemType = list.GetType().GetGenericArguments()[0];

                        foreach (int objRef in objRefs)
                        {
                            object item = null;
                            if (itemType.IsAssignableFrom(typeof(DictionaryItem)))
                            {
                                item = ProgramContext.Dictionaries.GetItemByReference(itemType, objRef);
                            }
                            else if (itemType.IsAssignableFrom(typeof(ObjectRef)))
                            {
                                IBaseDictionaryItem dictionaryItem = ProgramContext.Dictionaries.GetIDictionaryItem(row.DictionaryName, objRef);
                                if (dictionaryItem != null)
                                {
                                    item = new ObjectRef(dictionaryItem.Id);
                                }
                            }
                            else if ((itemType == typeof(Nullable<int>)) || (itemType == typeof(int)))
                            {
                                item = objRef;
                            }

                            if (item != null)
                            {
                                ((IList)list).Add(item);
                            }
                        }
                    }
                }
            }
        }

        public virtual void Clear() { /* nop */ }

        public virtual void PrepareToSend()
        {
            if (DateAutpRound)
            {
                foreach (PropertyInfo propInfo in GetType().GetProperties())
                {
                    if ((propInfo.PropertyType.Equals(typeof(DateTime?))) || (propInfo.PropertyType.Equals(typeof(DateTime))))
                    {
                        if (propInfo.GetValue(this, null) != null)
                        {
                            if (propInfo.Name.EndsWith("From"))
                            {
                                if (null != propInfo.GetValue(this, null))
                                {
                                    propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).StartOfTheDay(), null);
                                }
                            }

                            if (propInfo.Name.EndsWith("Till"))
                            {
                                if (null != propInfo.GetValue(this, null))
                                {
                                    propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).EndOfTheDay(), null);
                                }
                            }
                        }
                    }
                    else if (propInfo.PropertyType.Equals(typeof(string)) && string.IsNullOrEmpty((string)propInfo.GetValue(this, null)))
                    {
                        propInfo.SetValue(this, null, null);
                    }
                }
            }
        }
    }
}