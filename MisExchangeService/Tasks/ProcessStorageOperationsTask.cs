using ru.novolabs;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using System;

namespace MisExchange.User.Tasks
{
    public class ProcessStorageOperationsTask : Task
    {
        private Object lockObject = new object();

        public ProcessStorageOperationsTask()
        {
            processStorageOperationsMode = GAP.ResultSettings.ProcessStorageOperationsMode;
            processStorageOperationsHelper = GAP.ProcessStorageOperationsHelper;
        }

        private string processStorageOperationsMode { get; set; }
        private ProcessStorageOperationsHelper processStorageOperationsHelper { get; set; }

        public override void Execute()
        {
            lock (lockObject)
            {
                if (String.IsNullOrEmpty(processStorageOperationsMode))
                    return;
                if (processStorageOperationsHelper != null)
                    processStorageOperationsHelper.ProcessData();
                else
                    GAP.Logger.WriteError(string.Format("Не найден хелпер обработки складских данных по имени [{0}]", processStorageOperationsMode));
            }
        }
    }
}