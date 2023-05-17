using ru.novolabs;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using System;

namespace MisExchange.User.Tasks
{
    public class ExportDictionariesTask : Task
    {
        private Object lockObject = new object();

        public ExportDictionariesTask()
        {
            ExportDictionaryMode = GAP.ResultSettings.ExportDictionaryMode;
            DictionaryHelper = GAP.ExportDictionariesHelper;
        }

        private string ExportDictionaryMode { get; set; }
        private ExportDictionariesHelper DictionaryHelper { get; set; }

        public override void Execute()
        {
            lock (lockObject)
            {
                if (String.IsNullOrEmpty(ExportDictionaryMode))
                    return;
                if (DictionaryHelper != null)
                    DictionaryHelper.DoExport();
                else
                    GAP.Logger.WriteError(string.Format("Не найден хелпер по имени [{0}]", ExportDictionaryMode));
            }
        }
    }
}