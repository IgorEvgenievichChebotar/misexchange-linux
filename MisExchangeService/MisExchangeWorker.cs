using Microsoft.Extensions.Hosting;
using ru.novolabs.MisExchangeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ru.novolabs.MisExchange
{
    public class MisExchangeWorker : BackgroundService
    {
        private CompositionRoot compositionRoot;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            compositionRoot = new CompositionRoot();
            compositionRoot.Main();
        }

        /*public override async Task StopAsync(CancellationToken cancellationToken)
        {
            compositionRoot.ApplicationExitRegistrator();
        }*/
    }
}
