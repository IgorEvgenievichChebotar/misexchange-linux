using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Linq;

namespace ru.novolabs.SuperCore.LimsDictionary
{

    public class UserDictionaryValue : DictionaryItem
    {
        private int userDirectory = 0;
        [CSN("UserDirectory")]
        public UserDirectoryDictionaryItem UserDirectory { get; set; }
    }

    [OldSaveMethod]
    public class UserDirectoryDictionaryItem : DictionaryItem
    {
        private string category = string.Empty;
        private List<UserDictionaryValue> values = new List<UserDictionaryValue>();
        [CSN("Category")]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        [CSN("Values")]
        public List<UserDictionaryValue> Values
        {
            get { return values; }
            set { values = value; }
        }


        public UserDictionaryValue Find(int Id)
        {
            foreach (UserDictionaryValue Value in values)
            {
                if (Value.Id == Id)
                { return Value; }
            }
            return null;
        }
        public UserDictionaryValue FindValueByCode(String code, bool skipRemoved=true)
        {
            Predicate<UserDictionaryValue> predicateRemoved = (UserDictionaryValue v)=>(!v.Removed && skipRemoved) || !skipRemoved;
            return values.Where(v => predicateRemoved(v)).ToList().Find(v => v.Code == code);
        }
        public UserDictionaryValue FindValueByName(String name, bool skipRemoved=true)
        {
            Predicate<UserDictionaryValue> predicateRemoved = (UserDictionaryValue v)=>(!v.Removed && skipRemoved) || !skipRemoved;
            return values.Where(v => predicateRemoved(v)).ToList().Find(v => v.Name == name);
        }
    }

    public class UserDirectoryDictionary : DictionaryClass<UserDirectoryDictionaryItem>
    {
        public UserDirectoryDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("UserDirectory")]
        public List<UserDirectoryDictionaryItem> UserDirectory
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
