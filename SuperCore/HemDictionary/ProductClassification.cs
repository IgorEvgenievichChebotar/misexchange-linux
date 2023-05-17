using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{


    public class ProductClassificationDictionaryItem : DictionaryItem
    {

        private string externalCode = string.Empty;
        [CSN("ExternalCode")]
        public new string ExternalCode
        {
            get { return externalCode; }
            set { externalCode = value; }
        }
    }
}
