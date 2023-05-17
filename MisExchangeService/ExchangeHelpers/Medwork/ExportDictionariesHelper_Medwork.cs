using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.DirectoryExport;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.DictionaryExport;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork
{
    [ExportDictionariesHelperName("ExportDictionaries_Medwork")]
    class ExportDictionariesHelperMedwork : ExportDictionariesHelper
    {
        private DictionaryExporter medworkDictionaryExporter = null;

        public ExportDictionariesHelperMedwork()
        {
            medworkDictionaryExporter = new DictionaryExporter(GAP.DictionaryCache);
        }

        public override void DoExport()
        {
            medworkDictionaryExporter.CheckDictionaries();
        }
    }
}