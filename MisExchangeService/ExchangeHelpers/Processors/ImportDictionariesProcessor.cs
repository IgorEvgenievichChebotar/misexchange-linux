/*
using System.Xml;
using ru.novolabs.SuperCore;

namespace MisExchange.User.Processors
{

    [ProcessorName(ProcessorsNames.ImportDictionaries)]
    public class ImportDictionariesProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            lock (ServiceContext.DictionaryCash)
            {
                DictionariesExportHelper.ExportDictionaries();
            }
            RequestDone = true;
        }
    }
}
 * */
