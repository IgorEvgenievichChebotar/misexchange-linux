using System;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    public class OutsourceNorm : BaseObject
    {
        [CSN("CriticalLowLimit")]
        public Double? CriticalLowLimit { get; set; }
        [CSN("LowLimit")]
        public Double? LowLimit { get; set; }
        [CSN("CriticalHighLimit")]
        public Double? CriticalHighLimit { get; set; }
        [CSN("HighLimit")]
        public Double? HighLimit { get; set; }
        [CSN("Norms")]
        public String Norms { get; set; }
        [CSN("NormComment")]
        public String NormComment { get; set; }
        [CSN("UnitName")]
        public String UnitName { get; set; }
    }
}
