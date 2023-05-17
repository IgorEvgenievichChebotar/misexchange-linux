using System;
using System.Reflection;

namespace ru.novolabs.MisExchange.ExchangeHelpers.REGIZ_Spb
{
    [Obfuscation]
    public class HelperSettings
    {
        public string SystemCode { get; set; }
        public string IDMO { get; set; }
        public string MapperConnectionStr { get; set; }
        public string ServiceAddress { get; set; }
        public bool IsLoggingJsonRequest { get; set; }
        public string PublicKey { get; set; }
        public string ReportName { get; set; }
        public string AuthorizationToken { get; set; }
        public Nullable<Int64> MaxCountCachePatients { get; set; }
        public string TestDictionaryOID_ClinicalTest { get; set; }
        public string TestDictionaryVersion_ClinicalTest { get; set; }
        public string TargetDictionaryOID { get; set; }
        public string TargetDictionaryVersion { get; set; }
        public string TargetCategoryDictionaryOID { get; set; }
        public string TargetCategoryDictionaryVersion { get; set; }
        public string InterpretationDictionaryOID { get; set; }
        public string InterpretationDictionaryVersion { get; set; }
        public string AntibioticDictionaryOID { get; set; }
        public string AntibioticDictionaryVersion { get; set; }
        public string SyntheticMicrobiologyTestExternalCode { get; set; }
    }
}
