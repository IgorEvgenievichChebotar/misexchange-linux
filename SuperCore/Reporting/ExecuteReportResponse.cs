using System.Collections.Generic;

namespace ru.novolabs.SuperCore.Reporting
{
    public class ReportingDocument
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
    
    }
    
    public class ExecuteReportResponse
    {
        public ExecuteReportResponse()
        {
            Documents = new List<ReportingDocument>();        
        }

        public ErrorMessage Message { get; set; }
        public List<ReportingDocument> Documents { get; set; }
    }
}
