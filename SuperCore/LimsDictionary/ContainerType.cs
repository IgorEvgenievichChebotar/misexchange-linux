using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class ContainerTypeDictionaryItem : DictionaryItem
    {
        [CSN("Volume")]
        public int Volume { get; set; }

        [CSN("Unit")]
        public UnitDictionaryItem Unit { get; set; }

        [CSN("CapColor")]
        public int CapColor { get; set; }
    }

    public class ContainerTypeDictionary : DictionaryClass<ContainerTypeDictionaryItem>
    {
        public ContainerTypeDictionary(String DictionaryName) : base(DictionaryName) { }
        [CSN("ContainerType")]
        public List<ContainerTypeDictionaryItem> ContainerType
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
