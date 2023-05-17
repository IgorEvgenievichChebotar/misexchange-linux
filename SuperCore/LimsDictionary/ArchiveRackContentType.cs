using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{

    public enum ArchiveRackContentType : int
    {
        Tube = 1,
        Glass = 2
    }

    [StaticDictionary(true)]
    public class ArchiveRackContentTypeDictionaryItem : DictionaryItem
    {
    }
}
