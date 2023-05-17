using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using ru.novolabs.MisExchange.ExchangeHelpers.HL7;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ru.novolabs.MisExchange.ExchangeHelpers.ENC
{
    [ExportDictionariesHelperName("ExportDictionaries_ENC_NEW")]
    class ExportDictionariesHelperEncNew : ExportDictionariesHelper_HL7
    {

        private List<string> dictionaryNames = new List<string>()
        {
            LimsDictionaryNames.Target,
            LimsDictionaryNames.Test,
            LimsDictionaryNames.Biomaterial       
        };
        public ExportDictionariesHelperEncNew():base()
        {
        }


        protected override void ProcessDictionary(string dictionary, string exportFileName)
        {
            base.ProcessDictionary(dictionary, exportFileName);
            if (!dictionaryNames.Contains(dictionary))
            {
                DictionariesExportHelper.ExportDictionaryAsNameCodeMnemonicsOld(dictionary, exportFileName);
            }
        }
    }
}
