using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes.HandleResultExceptions
{
    class RequestCodeEqualityExternalResultProvider : IProvideSuccessfullExternalResults
    {
        public List<SuperCore.LimsBusinessObjects.Exchange.ExternalResult> GetSuccesfullExternalResults(Dictionary<ExchangeDTOs.Result, SuperCore.LimsBusinessObjects.Exchange.ExternalResult> resultExtResultMapping, List<ExchangeDTOs.Result> errorResults)
        {
            List<ExternalResult> results = new List<ExternalResult>();
            return resultExtResultMapping.Where(map => IsNotContainByCode(errorResults, map.Key)).Select(map => map.Value).ToList();
        }
        private bool IsNotContainByCode(List<ExchangeDTOs.Result> errorResults, ExchangeDTOs.Result result)
        {
            return errorResults.Find(r => r.RequestCode == result.RequestCode) != null;
        }
    }
}
