using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.DictionaryExport
{
    public class ExportDirectoryHelperSettings
    {
        public ExportDirectoryHelperSettings()
        {
            SourcePath = String.Empty;
            Output = String.Empty;
            Archive = String.Empty;
            Errors = String.Empty;
            HospitalCode = String.Empty;
            SynchronizedPath = String.Empty;
            DefaultMicroorganismTestCode = String.Empty;
            DefaultMicroorganismTestName = String.Empty;
            InitialNomenclatureExport = false;
            RepairSynchronizationFiles = false;
        }

        public String SourcePath { get; set; }
        public String Output { get; set; }
        public String Archive { get; set; }
        public String Errors { get; set; }
        public String HospitalCode { get; set; }
        public String SynchronizedPath { get; set; }
        public String DefaultMicroorganismTestCode { get; set; }
        public String DefaultMicroorganismTestName { get; set; }
        public Boolean InitialNomenclatureExport { get; set; }
        public Boolean RepairSynchronizationFiles { get; set; }
    }
}