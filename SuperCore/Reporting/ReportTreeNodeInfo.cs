using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Reporting
{
    public enum ReportTreeNodeType : int { None = 0, Group = 1, Report = 2 }

    public class ReportTreeNodeInfo
    {
        public ReportTreeNodeType NodeType { get; set; }
        public ReportDescription Report { get; set; }
    }
}