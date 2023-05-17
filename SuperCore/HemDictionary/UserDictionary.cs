using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class UserDictionaryValue : DictionaryItem
    {
        [CSN("UserDirectory")]
        public int UserDirectory { get ; set; }
    }

    public class UserDirectoryDictionaryItem : DictionaryItem
    {
        public UserDirectoryDictionaryItem()
        {
            Category = String.Empty;
            Values = new List<UserDictionaryValue>();
        }

        [CSN("Category")]
        public string Category { get ; set; }
        [CSN("Values")]
        public List<UserDictionaryValue> Values { get; set; }


        public UserDictionaryValue Find(int Id)
        {
            foreach (UserDictionaryValue Value in Values)
            {
                if (Value.Id == Id)
                { return Value; }
            }
            return null;
        }
    }

    public class UserDirectoryDictionary : DictionaryClass<UserDirectoryDictionaryItem>
    {
        public UserDirectoryDictionary(string dictionaryName) : base(dictionaryName) { }

        public override object GetByReference(Type type, int objRef)
        {
            if (type.Equals(typeof(UserDirectoryDictionaryItem)))
            {
                return base.GetByReference(type, objRef);
            }
            else if (type.Equals(typeof(UserDictionaryValue)))
            {
                foreach (UserDirectoryDictionaryItem userDirectory in Elements)
                {
                    foreach (UserDictionaryValue value in userDirectory.Values)
                    {
                        if (value.Id == objRef)
                        {
                            return value;
                        }
                    }
                }
            }
            return null;
        }
    }
}
