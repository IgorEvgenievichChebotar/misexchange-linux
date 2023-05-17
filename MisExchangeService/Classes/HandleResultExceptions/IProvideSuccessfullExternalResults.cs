using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes.HandleResultExceptions
{
    interface IProvideSuccessfullExternalResults
    {
        List<ExternalResult> GetSuccesfullExternalResults(Dictionary<ExchangeDTOs.Result, ExternalResult> resultExtResultMapping, List<ExchangeDTOs.Result> errorResults);
    }
}
