using ru.novolabs.SuperCore;
using System;
using System.IO;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom
{
    [System.Reflection.Obfuscation]
    public class DailySequenceCounter
    {
        public DailySequenceCounter()
        {
            Date = DateTime.Now.Date;
        }

        public UInt64 SequenceNr { get; set; }
        public DateTime Date { get; set; }
    }

    [System.Reflection.Obfuscation]
    public class HelperSettings
    {
        private string requestsPath = String.Empty;
        private string resultsPath = String.Empty;
        private string reportByServicesPath = String.Empty;
        private string acknowledgeMessagesPath = String.Empty;

        private bool needArchive = true;
        private string requestsArchivePath = Path.Combine(PathHelper.AssemblyDirectory, "RequestsArchive");
        private string resultsArchivePath = Path.Combine(PathHelper.AssemblyDirectory, "ResultsArchive");
        private DailySequenceCounter resultDailyCounter = new DailySequenceCounter();
        private DailySequenceCounter acknowledgeMessageDailyCounter = new DailySequenceCounter();
        private DailySequenceCounter reportByServicesDailyCounter = new DailySequenceCounter();       

        public string ResultsPath
        {
            get { return resultsPath; }
            set { resultsPath = value; }
        }

        public string ReportByServicesPath
        {
            get { return reportByServicesPath; }
            set { reportByServicesPath = value; }
        }

        public string AcknowledgeMessagesPath
        {
            get { return acknowledgeMessagesPath; }
            set { acknowledgeMessagesPath = value; }
        }

        public string RequestsPath
        {
            get { return requestsPath; }
            set { requestsPath = value; }
        }

        public string ResultsArchivePath
        {
            get { return resultsArchivePath; }
            set { resultsArchivePath = value; }
        }

        public string RequestsArchivePath
        {
            get { return requestsArchivePath; }
            set { requestsArchivePath = value; }
        }

        public bool NeedArchive
        {
            get { return needArchive; }
            set { needArchive = value; }
        }

        public DailySequenceCounter ResultDailyCounter
        {
            get { return resultDailyCounter; }
            set { resultDailyCounter = value; }
        }


        public DailySequenceCounter AcknowledgeMessageDailyCounter
        {
            get { return acknowledgeMessageDailyCounter; }
            set { acknowledgeMessageDailyCounter = value; }
        }

        public DailySequenceCounter ReportByServicesDailyCounter
        {
            get { return reportByServicesDailyCounter; }
            set { reportByServicesDailyCounter = value; }
        }
    }
}