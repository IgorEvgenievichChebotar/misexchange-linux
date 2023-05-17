using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.Reporting
{
    public class GetReportListResponse
    {
        public GetReportListResponse()
        {
            ReportList = new List<String>();
        }

        public ErrorMessage Message { get; set; }
        public List<String> ReportList { get; set; }
    }
}
