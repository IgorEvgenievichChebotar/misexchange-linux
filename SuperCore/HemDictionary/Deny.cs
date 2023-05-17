using System;
using System.Collections;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class DenyDictionaryItem : DictionaryItem
    {
        // Fields
        public int nodeType = 0;


        public int denyType = 0;
        public int denyDurationUnit = 0;
        public int denyDuration = 0;
        [CSN("DenyType")]
        public int DenyType
        {
            get { return denyType; }
            set { denyType = value; }
        }

        [CSN("DenyDuration")]
        public int DenyDuration
        {
            get { return denyDuration; }
            set { denyDuration = value; }
        }

        [CSN("DenyDurationUnit")]
        public int DenyDurationUnit
        {
            get { return denyDurationUnit; }
            set { denyDurationUnit = value; }
        }
        // Properties
        [SendToServer(false)]
        [CSN("CodeName")]
        public string CodeName
        {
            get
            {
                return string.Format("{0}: {1}", base.Code, base.Name);
                //return string.Format("{0}", base.Name);
            }
        }
        [CSN("NodeType")]
        public int NodeType
        {
            get
            {
                return nodeType;
            }
            set
            {
                nodeType = value;
            }
        }
    }

    public class DenyDictionaryClass<Class> : DictionaryClass<Class>
        where Class : DictionaryItem
    {
        // Methods
        public DenyDictionaryClass(string DictionaryName)
            : base(DictionaryName)
        {
            base.name = DictionaryName;
        }

        protected override int Compare(Class a, Class b)
        {
            object obj2 = a;
            object obj3 = b;
            return ((DenyDictionaryItem)obj2).CodeName.CompareTo(((DenyDictionaryItem)obj3).CodeName);
        }


        public override void Prepare()
        {
            base.Prepare();

            IList DictionaryElements = (IList)Elements;
            
            Elements.RemoveAll(new Predicate<Class>(CanRemove));
        }


        private static bool CanRemove(Class elem)
        {
            return ((DenyDictionaryItem)((Object)elem)).NodeType == 1;
        }
    }
}
