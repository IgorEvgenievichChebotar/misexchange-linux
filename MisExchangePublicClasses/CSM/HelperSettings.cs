using System;
using System.IO;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.CSM
{
    [System.Reflection.Obfuscation]
    public class HelperSettings
    {
        private string requestsArchivePath = String.Empty;
        private string resultsArchivePath = String.Empty;
        private string bloodGroupUserFieldName = String.Empty;
        private string photoUrlPrefix = String.Empty;
        private string photoUrlUserFieldName = String.Empty;
        private string purposeCodeUserFieldName = String.Empty;

        public string RequestsArchivePath
        {
            get { return requestsArchivePath; }
            set { requestsArchivePath = value; }
        }        

        public string ResultsArchivePath
        {
            get { return resultsArchivePath; }
            set { resultsArchivePath = value; }
        }

        public string BloodGroupUserFieldName
        {
            get { return bloodGroupUserFieldName; }
            set { bloodGroupUserFieldName = value; }
        }        

        public string PhotoUrlPrefix
        {
            get { return photoUrlPrefix; }
            set { photoUrlPrefix = value; }
        }
        
        public string PhotoUrlUserFieldName
        {
            get { return photoUrlUserFieldName; }
            set { photoUrlUserFieldName = value; }
        }

        public string PurposeCodeUserFieldName
        {
            get { return purposeCodeUserFieldName; }
            set { purposeCodeUserFieldName = value; }
        }
    }
}