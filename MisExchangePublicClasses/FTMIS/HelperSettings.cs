namespace ru.novolabs.MisExchange.ExchangeHelpers.FTMIS
{
    public class HelperSettings
    {
        public HelperSettings()
        {
            TimeDelay = 0;
        }
        public string HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public int TimeDelay { get; set; }
        public string ReportName { get; set; }
        public string OrderCaseId { get; set; }
        public bool NotFormPdf { get; set; }
        public bool NotFormPracSign { get; set; }
        public bool NotFormOrgSign { get; set; }
    }
}
