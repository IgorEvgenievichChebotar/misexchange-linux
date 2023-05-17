using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class ProductTemplate
    {
        [CSN("Volume")]
        public int Volume { get; set; }
        [CSN("ContainerType")]
        public ContainerTypeDictionaryItem ContainerType  { get; set; }
        [CSN("Removed")]
        public bool Removed { get; set; }
    }
}
