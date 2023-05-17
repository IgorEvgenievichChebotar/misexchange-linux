using System.Collections.Generic;

namespace ru.novolabs.SuperCore.Reporting
{
    public enum DocumentFormat
    {
        None,
        PDF,
        Excel2007,
        JPEG,
        PNG,
        HTML,
        RTF,
        Word2007,
        BMP,
        FPX
    }

    public class ExecuteReportParams
    {
        public ExecuteReportParams()
        {
            Params = new List<ReportParamValue>();
        }

        public string DestinationFolder { get; set; }
        public string ReportName { get; set; }
        public DocumentFormat Format { get; set; }
        public List<ReportParamValue> Params { get; set; }
    }
}
