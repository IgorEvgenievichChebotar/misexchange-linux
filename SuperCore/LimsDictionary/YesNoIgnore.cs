using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public enum YesNoIgnore : int
    {
        Yes = 0,
        No = 1,
        Ignore = 2
    }

    [OldSaveMethod]
    [StaticDictionary(true)]
    public class YesNoIgnoreDictionaryItem : DictionaryItem
    {
    }
}
