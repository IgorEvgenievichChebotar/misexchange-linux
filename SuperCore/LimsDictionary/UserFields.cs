using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public enum UserFieldObjectTypes
    {
        PATIENT = 1,
        REQUEST = 2,
        WORKLIST = 3,
        PATIENT_CARD = 4
    }

    public enum UserFieldTypes
    {
        STRING = 1,
        NUMERIC = 2,
        BOOLEAN = 3,
        DATETIME = 4,
        ENUMERATION = 5,
        SET = 6,
        TEST = 7,
        SEX = 8,
        EXPRESSION = 9,
        PICTURE = 10
    }

    [OldSaveMethod]
    public class UserFieldDictionaryItem : DictionaryItem
    {
        public UserFieldDictionaryItem()
        {
            LoadInSamples = new List<ObjectRef>();
            LoadInWorkJournals = new List<ObjectRef>();
            LoadInRequestForms = new List<ObjectRef>();
        }
        [CSN("FieldType")]
        public Int32 FieldType { get; set; }
        [CSN("ObjectType")]
        public Int32 ObjectType { get; set; }
        [CSN("MaxLength")]
        public Int32 MaxLength { get; set; }
        [CSN("MaxValue")]
        public Single MaxValue { get; set; }
        [CSN("MinValue")]
        public Single MinValue { get; set; }
        [CSN("Precision")]
        public Int32 Precision { get; set; }
        [CSN("NeedTime")]
        public Boolean NeedTime { get; set; }
        [CSN("Strict")]
        public Boolean Strict { get; set; }
        [CSN("UserDirectory")]
        public UserDirectoryDictionaryItem UserDirectory { get; set; }
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
        [CSN("LoadInRequestJournal")]
        public Boolean LoadInRequestJournal { get; set; }
        [CSN("LoadInSamples")]
        public List<ObjectRef> LoadInSamples { get; set; }
        [CSN("LoadInWorkJournals")]
        public List<ObjectRef> LoadInWorkJournals { get; set; }
        [CSN("LoadInRequestForms")]
        public List<ObjectRef> LoadInRequestForms { get; set; }
        [CSN("AllowBatchEdit")]
        public Boolean AllowBatchEdit { get; set; }
    }

    public class UserFieldDictionary : DictionaryClass<UserFieldDictionaryItem>
    {
        public UserFieldDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("UserField")]
        public List<UserFieldDictionaryItem> UserField
        {
            get { return Elements; }
            set { Elements = value; }
        }

        public UserFieldDictionaryItem SearchByName(String UserFieldName)
        {
            if (UserFieldName != "")
                foreach (UserFieldDictionaryItem userField in Elements)
                {
                    if (userField.Name == UserFieldName)
                        return userField;
                }
            return null;
        }
    }
}
