using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class BiomaterialContainerDictionaryItem : DictionaryItem
    {
        [CSN("ContainerType")]
        public ContainerTypeDictionaryItem ContainerType { get; set; }

        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial { get; set; }

        [CSN("MinBiomaterialVolume")]
        public int MinBiomaterialVolume { get; set; }

        public BiomaterialContainerDictionaryItem() { }
    }

    public class BiomaterialContainerDictionary : DictionaryClass<BiomaterialContainerDictionaryItem>
    {
        public BiomaterialContainerDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("BiomaterialContainer")]
        public List<BiomaterialContainerDictionaryItem> BiomaterialContainer
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
