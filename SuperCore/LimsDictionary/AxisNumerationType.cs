using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    
    public enum AxisNumerationType : int
    {
        Numeric = 1,
        Letters = 2
    }

    [StaticDictionary(true)]
    public class AxisNumerationTypeDictionaryItem : DictionaryItem
    {
    }
}
