using System;

namespace ru.novolabs.SuperCore.DictionaryCore
{
    [AttributeUsage(AttributeTargets.All)]
    public class LinkedDictionary : Attribute
    {
        private string dictionaryName = string.Empty;
        private string propertyName = "Name";

        public LinkedDictionary(string dictionaryName, string propertyName)
        {
            this.dictionaryName = dictionaryName;
            if (!propertyName.Equals(string.Empty))
            {
                this.propertyName = propertyName;
            }
        }

        public virtual string DictionaryName
        {
            get { return dictionaryName; }
        }

        public virtual string PropertyName
        {
            get { return propertyName; }
        }
    }
}
