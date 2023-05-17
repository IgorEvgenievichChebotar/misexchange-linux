using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
    public class BaseMapperItem
    {
        public long Id { get; set; }
        public string LisCode { get; set; }
        public string LisName { get; set; }
        public string MisCode { get; set; }
        public string MisName { get; set; }

    }
    public class TargetMapperItem : BaseMapperItem
    {
        public string CategoryCodeInMis { get; set; }
        public bool IsMicrobiology { get; set; }
    }
    public class TestMapperItem : BaseMapperItem { }
    public class BiomaterialMapperItem : BaseMapperItem { }
}
