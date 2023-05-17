using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;


namespace ru.novolabs.SuperCore.DictionaryCore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OldSaveMethod : Attribute
    { }

    public interface IBaseDictionaryItem
    {
        string Name { get; set; }
        string EngName { get; set; }
        string Code { get; set; }
        string Mnemonics { get; set; }
        string ExternalCode { get; set; }
        bool Removed { get; set; }
        int Id { get; set; }
        string AlternativeCode { get; set; }
    }

    public class DriverDictionaryItem : DictionaryItem
    {
        /// <summary>
        /// CLSID драйвера
        /// </summary>
        /// 
        [CSN("DriverId")]
        public string DriverId { get; set; }
        /// <summary>
        /// Настройки драйвера
        /// </summary>
        /// 
        [CSN("DriverSettings")]
        public string DriverSettings { get; set; }
    }

    public class DictionaryItem : BaseObject, IBaseDictionaryItem, IComparable
    {
        private string name = string.Empty;
        private string engName = string.Empty;
        private string code = string.Empty;
        private string mnemonics = string.Empty;
        private bool removed = false;
        private string description = string.Empty;
        private string externalCode = string.Empty;
        private string alternativeCode = string.Empty;

        [SendToServer(false)]
        [CSN("ExternalCode")]
        public string ExternalCode
        {
            get => externalCode;
            set => externalCode = value;
        }

        [CSN("Removed")]
        public bool Removed
        {
            get => removed;
            set => removed = value;
        }

        [CSN("Name")]
        public virtual string Name
        {
            get => name;
            set => name = value;
        }

        [SendToServer(false)]
        [CSN("EngName")]
        public virtual string EngName
        {
            get => engName;
            set => engName = value;
        }

        [CSN("AlternativeCode")]
        [SendToServer(false)]
        public virtual string AlternativeCode
        {
            get => alternativeCode;
            set => alternativeCode = value;
        }

        [CSN("Code")]
        public string Code
        {
            get => code;
            set => code = value;
        }

        [CSN("Mnemonics")]
        public string Mnemonics
        {
            get => mnemonics;
            set => mnemonics = value;
        }

        [CSN("Description")]
        public string Description
        {
            get => description;
            set => description = value;
        }

        [SendToServer(false)]
        [CSN("Version")]
        public DateTime? Version
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is IBaseDictionaryItem))
            {
                return Id == ((IBaseDictionaryItem)obj).Id;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual void Prepare()
        { }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            if (!obj.GetType().IsInstanceOfType(this))
            {
                return 0;
            }

            if (Name == null)
            {
                return 0;
            }

            return Name.CompareTo(((DictionaryItem)obj).Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Сравнивает два справочных объекта по Id
    /// </summary>
    public class DictionaryItemEqualityComparer : IEqualityComparer<DictionaryItem>
    {

        public bool Equals(DictionaryItem x, DictionaryItem y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(DictionaryItem obj)
        {
            return base.GetHashCode();
        }
    }

    public interface IBaseDictionary
    {
        string Name { get; set; }
        IList DictionaryElements { get; }
        int Version { get; set; }
        void Prepare();
        void Sort();
        object GetByReference(Type type, int objRef);

        void UpdateElement(DictionaryItem element);
        Color GetItemColor(int id);
    }

    public class DictionaryClass<Class> : IBaseDictionary where Class : DictionaryItem
    {
        protected string name = string.Empty;
        private List<Class> elements = new List<Class>();
        private int version = 0;

        public void UpdateElement(DictionaryItem currentItem)
        {
            if ((currentItem == null) || (currentItem.GetType() is Class))
            {
                throw new ArgumentException("Incorrect currentItem object");
            }

            List<Class> elements = (List<Class>)DictionaryElements;
            DictionaryItem outdatedItem = elements.Find(x => x.Id == currentItem.Id);
            lock (DictionaryElements)
            {
                if (outdatedItem != null)
                {
                    elements.Remove((Class)outdatedItem);
                }

                elements.Add((Class)currentItem);
            }
        }


        public Color GetItemColor(int id)
        {
            Color color = Color.White;
            DictionaryItem item = Find(id);
            if (item != null)
            {
                PropertyInfo propInfo = item.GetType().GetCustomProperty("Co" + "lor");
                if (propInfo != null)
                {
                    if (propInfo.PropertyType == typeof(string))
                    {
                        string strColor = (string)propInfo.GetValue(item, null);
                        if (strColor != null)
                        {
                            strColor = strColor.Trim(new char[] { '#' });
                        }

                        if (strColor.Length == 6) // Корректное 6-ти символьное RGB значение цвета
                        {
                            int red = int.Parse(strColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            int green = int.Parse(strColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            int blue = int.Parse(strColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            return Color.FromArgb(red, green, blue);
                        }
                    }
                }
            }
            return color;
        }

        public virtual object GetByReference(Type type, int objRef)
        {
            if (!type.Equals(typeof(Class)))
            {
                return null;
            }

            return elements.FirstOrDefault(x => x.Id == objRef);
        }

        [CSN("Elements")]
        public List<Class> Elements
        {
            get => elements;
            set => elements = value;
        }

        [XmlIgnore()]
        [CSN("DictionaryElements")]
        public IList DictionaryElements => elements;

        [CSN("Name")]
        public string Name
        {
            get => name;
            set => name = value;
        }

        [CSN("Version")]
        public int Version
        {
            get => version;
            set => version = value;
        }

        public DictionaryClass(string DictionaryName)
        {
            name = DictionaryName;
        }

        public DictionaryClass() { }

        public virtual void Prepare()
        {
            IList List = elements;
            foreach (DictionaryItem Item in List)
            {
                Item.Prepare();
            }
        }

        protected virtual int Compare(Class a, Class b)
        {
            object ObjA = a;
            object ObjB = b;
            return ((DictionaryItem)(ObjA)).Name.CompareTo(((DictionaryItem)(ObjB)).Name);
        }

        public virtual void Sort()
        {
            elements.Sort(Compare);
        }

        public DictionaryItem Find(int ID)
        {
            IList List = elements;
            foreach (DictionaryItem Item in List)
            {
                if (Item.Id == ID)
                {
                    return Item;
                }
            }
            return null;
        }

        public DictionaryItem Find(string code)
        {
            IList List = elements;
            foreach (DictionaryItem Item in List)
            {
                if (Item.Code.Equals(code) && !Item.Removed)
                {
                    return Item;
                }
            }
            return null;
        }
    }

    public class DictionarySaveRequest
    {
        [CSN("Directory")]
        public string Directory { get; set; }
        [CSN("Element")]
        public DictionaryItem Element { get; set; }
    }

    public class DictionarySaveResponce
    {
        [CSN("Id")]
        public int Id { get; set; }
        [CSN("Version")]
        public int Version { get; set; }
    }

    public class DictionaryRemoveRequest
    {
        public DictionaryRemoveRequest()
        {
            Ids = new List<ObjectRef>();
        }

        [CSN("Directory")]
        public string Directory { get; set; }
        [CSN("Ids")]
        public List<ObjectRef> Ids { get; set; }
    }

    public class DictionaryRemoveResponse
    {
        [CSN("Name")]
        public string Name { get; set; }
        [CSN("Version")]
        public int Version { get; set; }
    }

    public class DictionaryLoadRequest
    {
        public DictionaryLoadRequest()
        {
            Ids = new List<ObjectRef>();
        }

        [CSN("Directory")]
        public string Directory { get; set; }
        [CSN("Ids")]
        public List<ObjectRef> Ids { get; set; }
    }

    public class DictionaryLoadResponce<T> where T : DictionaryItem
    {
        [CSN("Version")]
        public int Version { get; set; }
    }
}