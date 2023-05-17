//using System;
//using System.Collections.Generic;

//namespace ru.novolabs.SuperCore.DictionaryCore
//{

//    public class UserDictionaryValue : DictionaryItem
//    {
//        private int userDirectory = 0;

//        public int UserDirectory
//        {
//            get { return userDirectory; }
//            set { userDirectory = value; }
//        }
//    }

//    public class UserDirectoryDictionaryItem : DictionaryItem
//    {
//        private string category = string.Empty;
//        private List<UserDictionaryValue> values = new List<UserDictionaryValue>();

//        public string Category
//        {
//            get { return category; }
//            set { category = value; }
//        }        

//        public List<UserDictionaryValue> Values
//        {
//            get { return values; }
//            set { values = value; }
//        }


//        public UserDictionaryValue Find(int Id)
//        {
//            foreach (UserDictionaryValue Value in values)
//            {
//                if (Value.Id == Id)
//                { return Value; }
//            }
//            return null;
//        }

//    }


//    public class UserDirectoryDictionaryClass<Class> : DictionaryClass<Class> where Class : UserDirectoryDictionaryItem
//    {
//        // Methods
//        public UserDirectoryDictionaryClass(string DictionaryName)
//            : base(DictionaryName)
//        {
//            base.name = DictionaryName;
//        }

//        public override object GetByReference(Type type, int objRef)
//        {
//            if (type.Equals(typeof(UserDirectoryDictionaryItem)))
//            {
//                return base.GetByReference(type, objRef);
//            }
//            else if (type.Equals(typeof(UserDictionaryValue)))
//            {
//                foreach (UserDirectoryDictionaryItem userDirectory in Elements)
//                {
//                    foreach (UserDictionaryValue value in userDirectory.Values)
//                    {
//                        if (value.Id == objRef)
//                        {
//                            return value;
//                        }
//                    }
//                }
//            }
//            return null;
//        }


//    }

//}
