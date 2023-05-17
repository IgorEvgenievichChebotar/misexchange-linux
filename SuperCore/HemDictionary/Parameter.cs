using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class Parameter : DictionaryItem
    {
        private string externalCode = string.Empty;
        [CSN("ExternalCode")]
        public new string ExternalCode
        {
            get { return externalCode; }
            set { externalCode = value; }
        }
        private UserDirectoryDictionaryItem userDirectory;
        [CSN("UserDirectory")]
        public UserDirectoryDictionaryItem UserDirectory
        {
            get { return userDirectory; }
            set { userDirectory = value; }
        }

        public override string ToString()
        {
            return Name;
        }
        [CSN("FieldType")]
        public int FieldType { get; set; }
    }

    public class BaseParameterClass : DictionaryItem
    {
        public int fieldType = 0;
        public ObjectRef userDirectory = new ObjectRef();
    }

    public class ParameterClass : BaseParameterClass
    {
        public bool enableDelay = false;
    }
}