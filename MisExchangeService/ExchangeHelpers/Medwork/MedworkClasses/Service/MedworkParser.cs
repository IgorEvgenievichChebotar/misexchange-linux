using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore;
using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult;
using ru.novolabs.MisExchangeService;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.Service
{
    static class MedworkParser
    {
        public static HelperSettings HelperSettings { get; set; }
        static IDictionaryCache DictionaryCache { get; set; }
        static string PatientPrefix { get; set; }

        static MedworkParser()
        {
            DictionaryCache = GAP.DictionaryCache;
            object objectPatientPrefix = ProgramContext.Settings["patientCodePrefix", false];
            PatientPrefix = objectPatientPrefix != null ? (String)objectPatientPrefix : String.Empty;
        }

        public static Result PrepareResult(ExchangeDTOs.Result result)
        {
            Result medworkResult = new Result();
            medworkResult.CopyPropertiesFromObject(result);

            if (!String.IsNullOrEmpty(PatientPrefix))
                medworkResult.Patient.Code = medworkResult.Patient.Code.Replace(PatientPrefix, String.Empty);

            foreach (SampleResult sampleResult in medworkResult.SampleResults)
            {
                sampleResult.BiomaterialName = getBiomaterialName(sampleResult.BiomaterialCode);

                foreach (TargetResult targetResult in sampleResult.TargetResults)
                {
                    prepareTarget(targetResult);
                    foreach (Work work in targetResult.Works)
                        prepareWork(work);
                }
            }

            return medworkResult;
        }

        private static String getBiomaterialName(String biomaterialCode)
        {
            String Result = String.Empty;

            BiomaterialDictionaryItem biomaterial = DictionaryCache.GetDictionaryItem<BiomaterialDictionaryItem>(biomaterialCode);
            if (biomaterial != null)
                Result = biomaterial.Name;

            return Result;
        }

        private static void prepareTarget(TargetResult targetResult)
        {
            TargetDictionaryItem target = DictionaryCache.GetDictionaryItem<TargetDictionaryItem>(targetResult.Code);
            if (target == null)
                return;

            targetResult.GroupDescription = target.Department.Name.Trim();
            targetResult.LabName = target.Department.Name.Trim();
        }

        private static void prepareWork(Work work)
        {
            if (String.IsNullOrEmpty(work.UnitName))
                work.UnitName = "-";

            work.Images = null;
            work.Diameter = null;
        }
    }
}
