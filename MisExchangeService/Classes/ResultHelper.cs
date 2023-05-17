using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.MisExchangeService.Adapters;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.MisExchange.Classes
{
    class ResultHelper
    {
        public ResultHelper(INotifierLis notifier, IProcessResultSettings settings, IDictionaryCache dictionaryCache)
        {
            NotifierLis = notifier;
            Settings = settings;
            ExchangeHelper = GAP.ExchangeHelper3;
            ExternalResultAdapter = new ExternalResultAdapter(dictionaryCache);
        }
        private INotifierLis NotifierLis { get; set; }
        private IProcessResultSettings Settings { get; set; }
        private ExchangeHelper3 ExchangeHelper { get; set; }
        private ExternalResultAdapter ExternalResultAdapter { get; set; }

        static readonly object _sync = new object();

        public void StoreRequestsResults(List<ExternalResult> results, bool isAllowParallelResults)
        {
            Dictionary<ExchangeDTOs.Result, ExternalResult> resultExtResultMapping = new Dictionary<ExchangeDTOs.Result, ExternalResult>();
            List<ExchangeDTOs.Result> resultDTOs = BuildResultDTOList(results, resultExtResultMapping);

            if (ExchangeHelper != null && ExchangeHelper.StoreNewResultsWhenReceived)
            {
                foreach (var resultDTO in resultDTOs)
                {
                    long resultId = resultDTO.Id;
                    ExchangeHelper.StoreResult(resultDTO, Common.StatusObjectCache.New);
                    resultDTO.Id = resultId;
                }
            }

            if (isAllowParallelResults)
            {
                StoreRequestsResults(results, resultDTOs, resultExtResultMapping);
                return;
            }
            lock (_sync)
            {
                StoreRequestsResults(results, resultDTOs, resultExtResultMapping);
            }
        }

        private void StoreRequestsResults(List<ExternalResult> results, List<ExchangeDTOs.Result> resultDTOs, Dictionary<ExchangeDTOs.Result, ExternalResult> resultExtResultMapping)
        {
            if (Settings.SortWorkResultsByTargetProfile)
                sortWorkResultsByTargetProfile(results);
            
            foreach (ExchangeDTOs.Result result in resultDTOs)
            {
                DoBackwardCompatibilityActionsForResult(result);
            }
            List<ExternalResult> successfullExtResults = results;
            try
            {
                ExchangeHelper.SpecificStoreResults(resultDTOs);
            }
            catch (HandledResultException ex)
            {
                //There was exception and resuts was not send, not NotifyLis
                // Exception was handled and not needed to handelled in Processor
                var errorResultList = ex.ErrorDict.Select(map => map.Key).ToList();
                successfullExtResults = ex.SuccssfullExternalResultsProvider.GetSuccesfullExternalResults(resultExtResultMapping, errorResultList);
            }
            foreach (ExternalResult result in successfullExtResults)
            {
                try
                {
                    if (result.State == (int)RequestState.Closed)
                        NotifierLis.NotifyLisServer(result.Id.ToString(), "50");
                }
                catch { }
            }
        }

        private ExternalTargetResult getProfileTargetResult(String profileCode, String profileName, List<ExternalTargetResult> sortedTargetResults)
        {
            ExternalTargetResult result = new ExternalTargetResult() { Code = profileCode, Name = profileName };
            Boolean isNewResult = true;

            foreach (ExternalTargetResult targetResult in sortedTargetResults)
                if ((targetResult.Code == profileCode) && (targetResult.Name == profileName))
                {
                    result = targetResult;
                    isNewResult = false;
                    break;
                }

            if (isNewResult)
                sortedTargetResults.Add(result);

            return result;
        }

        private void sortWorkResultsByTargetProfile(List<ExternalResult> results)
        {
            foreach (ExternalResult result in results)
                foreach (ExternalSampleResult sampleResult in result.SampleResults)
                {
                    List<ExternalTargetResult> sortedTargetResults = new List<ExternalTargetResult>();

                    for (Int32 i = 0; i < sampleResult.TargetResults.Count; i++)
                    {
                        ExternalTargetResult targetResult = sampleResult.TargetResults[i];
                        if (targetResult.Parents.Count > 0)
                        {
                            foreach (SuperCore.ObjectRef targetProfileRef in targetResult.Parents)
                            {
                                TargetDictionaryItem targetDictItem = ((TargetDictionaryItem)SuperCore.ProgramContext.Dictionaries[SuperCore.LimsDictionaryNames.Target, targetProfileRef.Id]);
                                if (targetDictItem != null)
                                {
                                    ExternalTargetResult profileTargetResult = getProfileTargetResult(targetDictItem.Code, targetDictItem.Name, sortedTargetResults);
                                    profileTargetResult.Works.AddRange(targetResult.Works);
                                }
                            }
                        }
                        else
                        {
                            sortedTargetResults.Add(targetResult);
                        }
                    }

                    sampleResult.TargetResults.Clear();
                    sampleResult.TargetResults.AddRange(sortedTargetResults);
                }
        }

        public List<ExchangeDTOs.Result> BuildResultDTOList(List<ExternalResult> results)
        {
            Dictionary<ExchangeDTOs.Result, ExternalResult> resultExtResultMapping = new Dictionary<ExchangeDTOs.Result, ExternalResult>();
            return BuildResultDTOList(results, resultExtResultMapping);
        }

        private List<ExchangeDTOs.Result> BuildResultDTOList(List<ExternalResult> results, Dictionary<ExchangeDTOs.Result, ExternalResult> resultExtResultMapping)
        {
            List<ExchangeDTOs.Result> resultDTOs = new List<ExchangeDTOs.Result>();
            results.ForEach(result =>
            {
                ResultPrepareHelper.PrepareResult(result);
                ExchangeDTOs.Result resultDTO = ExternalResultAdapter.WriteDTO(result);
                resultDTOs.Add(resultDTO);
                resultExtResultMapping.Add(resultDTO, result);
                if (result.SampleResults.Count == 0)
                    GAP.Logger.WriteText("Warning: в заявке № {0} не было ни одной работы, либо все работы были отфильтрованы в соответствии с настройками", result.RequestCode);
            });
            return resultDTOs;
        }
        // If it will have to be customized for any helper this method will be able to be moved to new dependence of ExchangeHelper or to new  Facade of exchangeHelper dependencies 
        public virtual void DoBackwardCompatibilityActionsForResult(ExchangeDTOs.Result result)
        {
            if (!Settings.IssueResultPriority)
                result.Priority = null;

            if (!Settings.IssueResultRegistrationDate)
                result.RegistrationDate = null;

            if (!Settings.IssueResultOrganizationCode)
                result.OrganizationCode = null;

            if (!Settings.IssueResultOrganizationName)
                result.OrganizationName = null;

            if (!Settings.IssueResultDepartmentCode)
                result.DepartmentCode = null;

            if (!Settings.IssueResultDepartmentName)
                result.DepartmentName = null;

            if (!Settings.IssueResultDoctorCode)
                result.DoctorCode = null;

            if (!Settings.IssueResultDoctorName)
                result.DoctorName = null;

            if (!Settings.IssueResultSamplingDate)
                result.SamplingDate = null;

            if (!Settings.IssueResultWorkPatientGroupCode)
                result.SampleResults.ForEach(sr => sr.TargetResults.ForEach(tr => tr.Works.ForEach(w => w.PatientGroupCode = null)));
            if (!Settings.IssueResultWorkPatientGroupName)
                result.SampleResults.ForEach(sr => sr.TargetResults.ForEach(tr => tr.Works.ForEach(w => w.PatientGroupName = null)));

            if (!Settings.IssueResultImages)
            {
                result.SampleResults.ForEach(sr => sr.Images = null);
                result.SampleResults.ForEach(sr => sr.TargetResults.ForEach(tr => tr.Works.ForEach(w => w.Images = null)));
                result.SampleResults.ForEach(sr => sr.MicroResults.ForEach(tr => tr.Antibiotics.ForEach(a => a.Images = null)));
            }

            if (!Settings.IssueResultDepartmentNr)
                result.SampleResults.ForEach(sr => sr.DepartmentNr = null);

            if (!Settings.IssueResultEndDate)
                result.SampleResults.ForEach(sr => sr.EndDate = null);

            if (!Settings.IssueResultEquipmentCode)
                result.SampleResults.ForEach(sr => sr.TargetResults.ForEach(tr => tr.Works.ForEach(w => w.EquipmentCode = null)));

            if (!Settings.IssueResultEquipmentName)
                result.SampleResults.ForEach(sr => sr.TargetResults.ForEach(tr => tr.Works.ForEach(w => w.EquipmentName = null)));

            if (!Settings.IssueResultWorkDiameter)
            {
                result.SampleResults.ForEach(sr => sr.TargetResults.ForEach(tr => tr.Works.ForEach(w => w.Diameter = null)));
                result.SampleResults.ForEach(sr => sr.MicroResults.ForEach(tr => tr.Antibiotics.ForEach(w => w.Diameter = null)));
            }

            if (!Settings.IssueResultPayCategoryCode)
                result.PayCategoryCode = null;

            if (!Settings.IssueResultPayCategoryName)
                result.PayCategoryName = null;
        }
    }
}
