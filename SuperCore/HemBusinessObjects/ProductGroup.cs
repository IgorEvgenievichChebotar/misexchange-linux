using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    class ProductGroup
    {
        [CSN("DepartmentCode")]
        public String DepartmentCode { get; set; }
        [CSN("ClassificationCode")]
        public String ClassificationCode { get; set; }
        [CSN("BloodGroupCode")]
        public String BloodGroupCode { get; set; }
        [CSN("BloodRhCode")]
        public String BloodRhCode { get; set; }
        [CSN("QuarantineDone")]
        public Boolean QuarantineDone { get; set; }
        [CSN("HospitalCode")]
        public String HospitalCode { get; set; }
        [CSN("AbsoluteDefect")]
        public Boolean AbsoluteDefect { get; set; }
        [CSN("LastProcessDate")]
        public DateTime LastProcessDate { get; set; }
        [CSN("Volume")]
        public Int32 Volume { get; set; }
        [CSN("Quantity")]
        public Int32 Quantity { get; set; }
    }
}