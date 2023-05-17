using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class IncomingMaterialTypeDictionaryItem: DictionaryItem 
    {
        public IncomingMaterialTypeDictionaryItem()
        {
            Biomaterial = new BiomaterialDictionaryItem();
            ContainerType = new ContainerTypeDictionaryItem();
        }
        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial { get; set; }
        [CSN("ContainerType")]
        public ContainerTypeDictionaryItem ContainerType { get; set; }

    }

    public class IncomingMaterialTypeDictionary : DictionaryClass<IncomingMaterialTypeDictionaryItem>
    {
        public IncomingMaterialTypeDictionary(String DictionaryName) : base(DictionaryName) { }
        [CSN("IncomingMaterialType")]
        public List<IncomingMaterialTypeDictionaryItem> IncomingMaterialType
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
