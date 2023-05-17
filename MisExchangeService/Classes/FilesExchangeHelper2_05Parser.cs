using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore;
using ru.novolabs.MisExchangeService;

namespace ru.novolabs.MisExchange.Classes
{
    static class FilesExchange2_05Parser
    {
        public static HelperSettings HelperSettings { get; set; }
        static IDictionaryCache DictionaryCache { get; set; }
        static string PatientPrefix { get; set; }

        static FilesExchange2_05Parser()
        {
            DictionaryCache = GAP.DictionaryCache;
            object objectPatientPrefix = ProgramContext.Settings["patientCodePrefix", false];
            PatientPrefix = objectPatientPrefix != null ? (String)objectPatientPrefix : String.Empty;
        }

        public static void PrepareResult(ExchangeDTOs.Result result)
        {
            if (!String.IsNullOrEmpty(PatientPrefix))
                result.Patient.Code = result.Patient.Code.Replace(PatientPrefix, String.Empty);

            foreach (ExchangeDTOs.SampleResult sampleResult in result.SampleResults)
            {
                foreach (ExchangeDTOs.TargetResult targetResult in sampleResult.TargetResults)
                {
                    foreach (ExchangeDTOs.Work work in targetResult.Works)
                        prepareWork(work);
                }

                foreach (ExchangeDTOs.MicroResult microResult in sampleResult.MicroResults)
                {
                    foreach (ExchangeDTOs.Work work in microResult.Antibiotics)
                        prepareWork(work);
                }
            }
        }

        private static void prepareWork(ExchangeDTOs.Work work)
        {
            work.GroupRank = null;
            work.RankInGroup = null;
            work.TestRank = null;
            work.Precision = null;
            work.Images = null;
        }
    }
}

