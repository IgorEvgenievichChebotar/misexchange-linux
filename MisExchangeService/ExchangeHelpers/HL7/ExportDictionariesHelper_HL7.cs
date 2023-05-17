using ru.novolabs.ExchangeDTOs;
using ru.novolabs.HL7.Files;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ru.novolabs.MisExchange.ExchangeHelpers.HL7
{
    [ExportDictionariesHelperName("ExportDictionaries_HL7")]
    class ExportDictionariesHelper_HL7 : ExportDictionariesHelperAbstractFile.ExportDictionariesHelperAbstractFile
    {
        private ExportDirectoryHelperSettings helperSettings = null;
        public ExportDictionariesHelper_HL7()
            : base()
        {
        }
    }
}
