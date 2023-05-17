using System;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class ContainerTypeDictionaryItem : DictionaryItem
    {
        public ContainerTypeDictionaryItem()
        {
            NrMask = String.Empty;
        }
        [CSN("NrMask")]
        public string NrMask { get; set; }
        [CSN("AdditiveType")]
        public AdditiveTypeDictionaryItem AdditiveType { get; set; }
        [CSN("AdditiveVolume")]
        public int AdditiveVolume { get; set; }
        [CSN("Volume")]
        public int Volume { get; set; }
    }
}
