using ru.novolabs;
using ru.novolabs.MisExchangeService;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;

namespace MisExchange.User.Tasks
{
    public class ProcessNewMISDataTask : Task
    {
        private string ExchangeMode { get; set; }
        private ExchangeHelper ExchangeHelper { get; set; }
        private ExchangeHelper3 ExchangeHelper3 { get; set; }

        public ProcessNewMISDataTask()
        {
            ExchangeMode = GAP.ResultSettings.ExchangeMode;
            if (ExchangeMode.Contains("ExchangeHelper3"))
                ExchangeHelper3 = GAP.ExchangeHelper3;
            else if (ExchangeMode.Contains("ExchangeHelper"))
                ExchangeHelper = GAP.ExchangeHelper;
        }

        public override void Execute()
        {
            // В зависимости от режима обмена одним из способов обрабатываем новые данные
            if (ExchangeMode.Contains("ExchangeHelper3"))
                ExchangeHelper3.ProcessNewData();
            else if (ExchangeMode.Contains("ExchangeHelper"))
                ExchangeHelper.ProcessNewData();
            else
                GAP.Logger.WriteError("Необходимо выбрать режим обмена данными с МИС, параметр \"exchangeMode\"");
        }
    }
}