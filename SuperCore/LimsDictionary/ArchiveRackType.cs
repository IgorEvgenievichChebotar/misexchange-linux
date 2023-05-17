using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class ArchiveRackTypeDictionaryItem : DictionaryItem
    {
        [CSN("DefaultFillingDirection")]
        public int DefaultFillingDirection { get; set; }

        [CSN("DefaultHorizontalAxisNumerationType")]
        public int DefaultHorizontalAxisNumerationType { get; set; }

        [CSN("DefaultVerticalAxisNumerationType")]
        public int DefaultVerticalAxisNumerationType { get; set; }

        [CSN("Width")]
        public int Width { get; set; }

        [CSN("Height")]
        public int Height { get; set; }

        [CSN("ContentType")]
        public int ContentType { get; set; }
    }

    public class ArchiveRackTypeDictionary : DictionaryClass<ArchiveRackTypeDictionaryItem>
    {
        public ArchiveRackTypeDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("ArchiveRackType")]
        public List<ArchiveRackTypeDictionaryItem> ArchiveRackType
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}