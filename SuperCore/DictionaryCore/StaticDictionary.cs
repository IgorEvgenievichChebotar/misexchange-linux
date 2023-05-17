using System;

namespace ru.novolabs.SuperCore.DictionaryCore
{
    [AttributeUsage(AttributeTargets.All)]
    public class StaticDictionary : Attribute
    {
        private bool isStatic = false;

        public StaticDictionary(bool isStatic)
        {
            this.isStatic = isStatic;
        }

        public virtual bool IsStatic
        {
            get { return isStatic; }
        }
    }
}
