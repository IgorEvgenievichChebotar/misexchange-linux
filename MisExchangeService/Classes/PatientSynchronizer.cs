using ru.novolabs.MisExchange.Processors;
using ru.novolabs.MisExchangeService;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    class PatientSynchronizer
    {
        static PatientSynchronizer()
        {
            ExchangeMode = GAP.ResultSettings.ExchangeMode;
            if (ExchangeMode.Contains("ExchangeHelper3"))
                ExchangeHelper = GAP.ExchangeHelper3;
        }

        static string ExchangeMode { get; set; }
        static ExchangeHelper3 ExchangeHelper { get; set; }

        internal static void Synchronize(Patient4Synchronize patient)
        {
            // В зависимости от режима обмена одним из способов сохраняем результаты
            if (ExchangeMode.Contains("ExchangeHelper"))
            {
                if (ExchangeMode.Contains("ExchangeHelper3"))
                {
                    ExchangeHelper.PatientSynchronize(patient);
                }
            }
            else
                GAP.Logger.WriteError("Необходимо выбрать режим обмена данными с внешней системой, параметр \"exchangeMode\"");        
        }
    }
}
