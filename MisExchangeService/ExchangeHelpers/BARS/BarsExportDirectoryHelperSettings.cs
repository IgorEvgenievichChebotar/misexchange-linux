using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.BARS
{
    public class ExportDirectoryHelperSettings
    {
        public ExportDirectoryHelperSettings()
        {

        }

        public string HospitalCode { get; set; }
        public string SynchronizedPath { get; set; }
        public String DefaultMicroorganismTestCode { get; set; }
        public String DefaultMicroorganismTestName { get; set; }
        public Boolean InitialNomenclatureExport { get; set; }
        public Boolean RepairSynchronizationFiles { get; set; }
    }
}
