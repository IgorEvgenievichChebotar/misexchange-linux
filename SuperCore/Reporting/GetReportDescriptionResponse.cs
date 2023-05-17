using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Reporting
{
    public class GetReportDescriptionResponse
    {
        public ErrorMessage Message { get; set; }
        public ReportDescription ReportDescription { get; set; }
    }
}
