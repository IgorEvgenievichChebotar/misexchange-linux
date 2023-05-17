using System;
using System.Collections.Generic;
using System.Globalization;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    public static class ResultPrepareHelperOld
    {
        private static void PrepareWorks(List<ExternalWork> works)
        {
            NumberFormatInfo formatInfo = new NumberFormatInfo();
            formatInfo.NumberDecimalSeparator = ".";

            for (int index = works.Count - 1; index >= 0; index--)
            {
                ExternalWork work = works[index];
                // Статусы работ, описанные в протоколе интеграции на 1 меньше принятых в ЛИС
                work.State = work.State - 1;

                // Ищем тест по коду, взятому из work
                TestDictionaryItem test = (TestDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Test, work.Code] as TestDictionaryItem;
                if (test != null)
                {
                    // Если необходимо, фильтруем выгружемые работы и подставляем внешние коды
                    // в соответствии с настройками справочника "Настройка обмена данными"
                    String externalSystemCode = (String)ProgramContext.Settings["externalSystemCode"];
                    var externalSystem = ProgramContext.Dictionaries[LimsDictionaryNames.ExternalSystem, externalSystemCode] as ExternalSystemDictionaryItem;
                    if (!externalSystem.ExportAllWorks)
                    {
                        ExternalSystemTestMapping testMapping = externalSystem.Tests.Find(tm => tm.Test.Id == test.Id);
                        if (testMapping != null)
                        {
                            if (!testMapping.Code.Equals(String.Empty))
                                work.Code = testMapping.Code;
                        }
                        else
                        {
                            works.Remove(work);
                            continue;
                        }
                    }

                    // Значения количественных показателей приводим к привычному виду через преобразование строки к double и обратно
                    if ((test.ResultType == TestTypes.VALUE) /*&& (test.Format != null)*/)
                    {
                        work.Precision = test.Format;
                        try
                        {
                            if (work.Value != null)
                            {
                                work.Value = work.Value.Replace(',', '.');
                                Double doubleValue = Convert.ToDouble(work.Value, formatInfo);
                                work.Value = doubleValue.ToString(formatInfo);
                                // work.Value = doubleValue.ToString("F"+test.Format, formatInfo); // Округление до "test.Format" знаков после запятой                                
                            }
                        }
                        catch (Exception)
                        {
                            Log.WriteError("Could not covert to double value [{0}]. Value skipped", work.Value);
                            //Log.WriteError("\r\n" + ex.ToString());
                        }
                    }
                    work.TestRank = test.Rank;
                    String defaultSampleBlankCode = (String)ProgramContext.Settings["defaultSampleBlankCode"];
                    if (!String.IsNullOrEmpty(defaultSampleBlankCode))
                    {
                        var sampleBlank = ProgramContext.Dictionaries[LimsDictionaryNames.SampleBlank, defaultSampleBlankCode] as SampleBlankDictionaryItem;
                        if (sampleBlank != null)
                            foreach (SampleBlankGroup group in sampleBlank.Groups)
                            {
                                for (int i = 0; i <= group.Elements.Count - 1; i++)
                                {
                                    if (group.Elements[i].Test.Id == test.Id)
                                    {
                                        work.GroupRank = group.Rank;
                                        work.RankInGroup = group.Elements[i].Rank;
                                        break;
                                    }
                                }
                            }
                    }
                }
                else
                    Log.WriteError(String.Format("Тест не найден в справочнике по коду [{0}]", work.Code));
            }
        }

        private static void PrepareMicroorganismsInfo(List<ExternalMicroResult> microorganisms)
        {
            lock (ProgramContext.Dictionaries)
            {
                MicroOrganismDictionary microorganismDict = (MicroOrganismDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.MicroOrganism);

                foreach (ExternalMicroResult microorganism in microorganisms)
                {
                    var dictionaryItem = (MicroOrganismDictionaryItem)microorganismDict.Find(microorganism.MicroOrganism.Id);
                    if (dictionaryItem != null)
                    {
                        microorganism.Code = dictionaryItem.Code;
                        microorganism.Name = dictionaryItem.Name;
                    }
                    else
                        Log.WriteError(String.Format("Microorganism not found by id = {0}", microorganism.MicroOrganism.Id));
                }
            }
        }

        public static void PrepareResult(ExternalResult result)
        {
            if (result.Patient != null)
                UserFieldHelperOld.PrepareOutputUserFieldsValuesByIds(result.Patient.UserValues);
            if (result.Patient.PatientCard != null)
                UserFieldHelperOld.PrepareOutputUserFieldsValuesByIds(result.Patient.PatientCard.UserValues);
            if (result.UserValues != null)
                UserFieldHelperOld.PrepareOutputUserFieldsValuesByIds(result.UserValues);
            foreach (ExternalSampleResult sample in result.SampleResults)
            {
                PrepareMicroorganismsInfo(sample.MicroResults);
                foreach (ExternalTargetResult targetResult in sample.TargetResults)
                    PrepareWorks(targetResult.Works);
                foreach (ExternalMicroResult microResult in sample.MicroResults)
                    PrepareWorks(microResult.Antibiotics);
                // Удаляем все результаты по исследованиям, не содержащие полезной информации
                sample.TargetResults.RemoveAll(tr => tr.Works.Count == 0);
            }
            // Удаляем все результаты по пробам, не содержащие полезной информации
            result.SampleResults.RemoveAll(sr =>
                {
                    return sr.TargetResults.Count == 0 &&
                        sr.MicroResults.Count == 0 &&
                        sr.Defects.Count == 0 &&
                        String.IsNullOrEmpty(sr.Comments);
                });
        }
    }
}
