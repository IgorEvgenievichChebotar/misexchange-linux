using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    
    public enum FillingDirection : int
    {
        LeftToRightUpToDown = 1,
        UpToDownLeftToRight = 2
    }

    [StaticDictionary(true)]
    public class FillingDirectionDictionaryItem : DictionaryItem
    {
    }
}
