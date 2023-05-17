using System;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class StorageDictionaryItem : DictionaryItem
    {
        [CSN("Parent")]
        public StorageDictionaryItem Parent { get; set; }
        [CSN("FullItemName")]
        public String FullItemName { get { return GetFullItemName(); } }
        [CSN("FullItemCode")]
        public String FullItemCode { get { return GetFullItemCode(); } }

        private string GetFullItemCode()
        {
            String result = this.Name;
            if (Parent != null)
                result = Parent.FullItemName + ">" + result;
            return result;
        }

        private string GetFullItemName()
        {
            String result = this.Code;
            if (Parent != null)
                result = Parent.FullItemCode + ">" + result;
            return result;
        }

        private ObjectRef department = new ObjectRef();
        [CSN("Department")]
        public ObjectRef Department
        {
            get { return department; }
            set { department = value; }
        }
    }
}
