using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes.HandleResultExceptions
{
    class RefEqualityExtrernalResultProvider : IProvideSuccessfullExternalResults
    {
        public List<ExternalResult> GetSuccesfullExternalResults(Dictionary<ExchangeDTOs.Result, ExternalResult> resultExtResultMapping, List<ExchangeDTOs.Result> errorResults)
        {
            List<ExternalResult> results = new List<ExternalResult>();
            return resultExtResultMapping.Where(map => !errorResults.Contains(map.Key)).Select(map => map.Value).ToList();
        }
    }
}
