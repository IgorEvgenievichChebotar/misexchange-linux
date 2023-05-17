using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [StaticDictionary(true)]
    public class QuotaStateDictionaryItem : DictionaryItem
    {
        [CSN("Color")]
        public string Color
        {
            get;
            set;
        }
    }
}
