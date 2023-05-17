using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.INOVUS
{
    [System.Reflection.Obfuscation]
    public class HelperSettings
    {
        public HelperSettings()
        {
            HttpAuthorizationToken = "YWRtaW46VmxAZDMz";
            HttpUserAgent = "Mozilla/3.0";
            IsLoggingAllSoapMessages = false;
            IsValidationBySchema = false;
            IsEnabledSchemaCache = true;
            ParallelismDegree = 1;
        }
        public string ReportName { get; set; }
        public string ServerAddress { get; set; }
        public string HttpAuthorizationToken { get; set; }
        public string HttpUserAgent { get; set; }
        public string HttpPatientSearchAuToken { get; set; }
        public bool IsLoggingAllSoapMessages { get; set; }
        public bool IsValidationBySchema { get; set; }
        public bool IsEnabledSchemaCache { get; set; }
        public string GetNewRequestSchemaUri { get; set; }
        public string RequestProcessingStatusSchemaUri { get; set; }
        public string SendResultObtainedSchemaUri { get; set; }
        public string PatientInterchangeSchemaUri { get; set; }
        public Int32 ParallelismDegree { get; set; }
    }
}
