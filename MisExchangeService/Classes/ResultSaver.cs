using ru.novolabs.MisExchange;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchangeService.Classes
{
    static class ResultSaver
    {
        static ResultSaver()
        {
            ExchangeMode = GAP.ResultSettings.ExchangeMode;
            LisCommunicator = GAP.ResultCommunicator;
            ResultHelper = new ResultHelper(GAP.NotifierLis, GAP.ResultSettings, GAP.DictionaryCache);
            if (ExchangeMode.Contains("ExchangeHelper3"))
            {
                ExchangeHelper3 = GAP.ExchangeHelper3;
                IsAllowParallelResults = GAP.ResultSettings.IsAllowParallelResults && ExchangeHelper3 != null && ExchangeHelper3.IsAllowedParallelResults;
            }
            else if (ExchangeMode.Contains("ExchangeHelper"))
                ExchangeHelper = GAP.ExchangeHelper;           
        }
        static readonly object _sync = new object();

        private static string ExchangeMode { get; set; }
        private static ResultHelper ResultHelper { get; set; }
        private static IProcessResultCommunicator LisCommunicator { get; set; }
        private static ExchangeHelper ExchangeHelper { get; set; }
        private static ExchangeHelper3 ExchangeHelper3 { get; set; }
        private static Boolean IsAllowParallelResults { get; set; }

        internal static void StoreRequestsResults(List<ObjectRef> requestIds)
        {
            // В зависимости от режима обмена одним из способов сохраняем результаты
            if (!ExchangeMode.Contains("ExchangeHelper"))
            {
                GAP.Logger.WriteError("Необходимо выбрать режим обмена данными с внешней системой, параметр \"exchangeMode\"");
                return;
            }
            FilterRequestResults(requestIds);

            if (requestIds.Count <= 0)
                return;

            List<ExternalResult> results = LisCommunicator.GetRequestsResults(requestIds);

            StoreRequestResults(results);
        }
        private static void FilterRequestResults(List<ObjectRef> requestIds)
        {    
            if (ExchangeMode.Contains("ExchangeHelper3"))
            {
                ExchangeHelper3.FilterRequestResults(requestIds);
            }
            else
            {
                ExchangeHelper.FilterRequestResults(requestIds);
            }       
        }
        private static void StoreRequestResults(List<ExternalResult> results)
        {                
            if (ExchangeMode.Contains("ExchangeHelper3"))
            {
                ResultHelper.StoreRequestsResults(results, IsAllowParallelResults);
            }
            else
            {
                lock (_sync)
                {
                    ExchangeHelper.StoreRequestsResults(results);
                }
            }
        }

        public static void ProcessDeliveredBiomaterials(List<DeliveredBiomaterial> deliveredBiomaterials)
        {
            if (ExchangeMode.Contains("ExchangeHelper3"))
            {
                ExchangeHelper3.ProcessDeliveredBiomaterials(deliveredBiomaterials);
            }
            else
            {
                /* nop */
            }
        }
    }
}