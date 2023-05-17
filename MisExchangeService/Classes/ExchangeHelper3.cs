using Ninject;
using ru.novolabs.Common;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchangeService.Classes
{
    public abstract class ExchangeHelper3
    {
        // В каждом ExchangeHelper-е необходимо переопределить 3 метода, отвечающих за специфическую обработку данных:
        //
        // - SpecificProcessNewData() - специфическим образом обрабатывает новые входящие данные. В реализации этого метода нужно вызвать унаследованный метод ProcessRequest()
        // - SpecificFilterRequestResults() - cпецифическим образом фильтрует результаты по заявкам
        // - SpecificStoreResults() - специфическим образом сохраняет список результатов
        // - SpecificProcessDeliveredBiomaterials() - специфическим образом обрабатывает доставленные биоматериалы

        /// <summary>
        /// Специфическим образом обрабатывает новые входные данные. В реализации этого метода нужно вызвать унаследованный метод ProcessRequest()
        /// </summary>
        protected abstract void SpecificProcessNewData();
        /// <summary>
        /// Специфическим образом фильтрует результаты по заявкам
        /// </summary>
        /// <param name="requestIds"></param>
        protected virtual void SpecificFilterRequestResults(List<ObjectRef> requestIds)
        {
            if (!IsFilterRequestResults)
            {
                return;
            }

            RequestIdsFilter filter = new RequestIdsFilter();
            filter.Filter(requestIds);
        }
        /// <summary>
        /// Специфическим образом сохраняет список результатов
        /// </summary>
        /// <param name="results"></param>
        public abstract void SpecificStoreResults(List<Result> results);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        public virtual void PatientSynchronize(MisExchange.Processors.Patient4Synchronize patient)
        { }
        /// <summary>
        /// Determine filter results for request, which wasn't acepted from External System
        /// </summary>
        protected virtual bool IsFilterRequestResults => true;
        /// <summary>
        /// Determin whether results may be sent parallel
        /// </summary>
        public virtual bool IsAllowedParallelResults => false;

        /// <summary>
        /// Кешировать ли новые результаты при получении
        /// </summary>
        public virtual bool StoreNewResultsWhenReceived => false;


        #region Dependencies
        [Inject]
        public IDbExchangeProvider DbExchangeProvider { get; set; }
        [Inject]
        public IRequestValidator RequestValidator { get; set; }
        [Inject]
        public ICreateRequestAdapter CreateRequestAdapter { get; set; }
        [Inject]
        public IProcessRequestCommunicator LisCommunicator { get; set; }
        [Inject]
        public IExchangeCacheHelper CacheHelper { get; set; }

        #endregion

        public ExchangeHelper3()
        {

        }

        internal void ProcessNewData()
        {
            try
            {
                SpecificProcessNewData();
            }
            catch (Exception ex)
            {
                GAP.Logger.WriteError("Ошибка при обработке новых данных от МИС: {0}", ex.ToString());
            }
        }


        public void FilterRequestResults(List<ObjectRef> requestIds)
        {
            SpecificFilterRequestResults(requestIds);
        }

        protected virtual int GetRequestLisId(string requestCode)
        {
            return DbExchangeProvider.GetRequestLisId(requestCode);
        }

        protected bool isResetPatientCode()
        {
            bool? setting = (bool?)ProgramContext.Settings["resetPatientCode", false];
            return setting != null ? setting.Value : false;
        }

        protected void ResetRequestDTOPatientInfo(Request requestDTO)
        {
            requestDTO.Patient.Code = string.Empty;
            requestDTO.Patient.PatientCard = new ExchangeDTOs.PatientCard() { CardNr = string.Empty };
        }

        protected virtual void ProcessRequest(Request requestDTO, bool savePatient = true)
        {
            PrepareSampleDeliveryDate(requestDTO);

            CheckDefaultBiomaterial(requestDTO);

            CorrectRequestByEmptyPatientCard(requestDTO);

            RequestValidator.CheckData(requestDTO);

            int requestLisId = GetRequestLisId(requestDTO.RequestCode);

            if (isResetPatientCode())
            {
                ResetRequestDTOPatientInfo(requestDTO);
            }

            CreateRequest3Request createRequest3Request = CreateRequestAdapter.MakeRequest(requestDTO, requestLisId, savePatient);

            LisCommunicator.CreateRequestXXX(createRequest3Request, out requestLisId);

            SaveToExchangeDb(requestDTO, requestLisId);
        }

        protected virtual void SaveToExchangeDb(Request requestDTO, int requestLisId)
        {
            DbExchangeProvider.SaveToExchangeDb(requestDTO, requestLisId);
        }

        protected void CorrectRequestByEmptyPatientCard(Request request)
        {
            if (isResetPatientCode() || (request.Patient == null))
            {
                return;
            }

            if (request.Patient.PatientCard == null)
            {
                request.Patient.PatientCard = new ExchangeDTOs.PatientCard() { CardNr = request.Patient.Code ?? string.Empty };
                return;
            }
            if (request.Patient.PatientCard != null && !string.IsNullOrEmpty(request.Patient.PatientCard.CardNr))
            {
                return;
            }

            request.Patient.PatientCard.CardNr = request.Patient.Code ?? string.Empty;
        }

        protected void PrepareSampleDeliveryDate(Request requestDTO)
        {
            bool? setting = (bool?)ProgramContext.Settings["resetSampleDeliveryDate", false];
            bool resetSampleDeliveryDate = setting != null ? setting.Value : false;

            if (resetSampleDeliveryDate)
            {
                requestDTO.SampleDeliveryDate = null;
            }
            else
            {
                // Если МИС не указала дату доставки биоматериала в лабораторию
                DateTime? defaultSampleDeliveryDate = (DateTime?)ProgramContext.Settings["defaultSampleDeliveryDate", false];

                if (requestDTO.SampleDeliveryDate == null)
                {
                    if (defaultSampleDeliveryDate == null)
                    {
                        requestDTO.SampleDeliveryDate = DateTime.Now; // Дату доставки биоматериала делаем равной дате обработки файла заявки 
                    }
                    else
                    {
                        requestDTO.SampleDeliveryDate = defaultSampleDeliveryDate; // Дату доставки биоматериала по умолчанию берём из файла настроек
                    }
                }
            }
        }

        protected void CheckDefaultBiomaterial(Request requestDTO)
        {
            foreach (Sample sample in requestDTO.Samples)
            {
                if (!string.IsNullOrEmpty(sample.BiomaterialCode))
                {
                    continue;
                }

                GAP.Logger.WriteError("Для пробы [{0}] заявки [{1}] не указан код биоматериала", sample.Barcode, requestDTO.RequestCode);

                string biomaterialCode = string.Empty;
                string errorSuffix = string.Empty;

                foreach (ExchangeDTOs.Target sampleTarget in sample.Targets)
                {
                    TargetDictionaryItem target = GAP.DictionaryCache.GetDictionaryItem<TargetDictionaryItem>(sampleTarget.Code);
                    if (target != null)
                    {
                        if (target.DefaultBiomaterial == null)
                        {
                            biomaterialCode = string.Empty;
                            errorSuffix = string.Format("Для исследования с кодом [{0}] не настроен биоматериал по умолчанию", sampleTarget.Code);
                            break;
                        }

                        if (string.IsNullOrEmpty(biomaterialCode))
                        {
                            biomaterialCode = target.DefaultBiomaterial.Code;
                        }
                        else
                        {
                            if (biomaterialCode != target.DefaultBiomaterial.Code)
                            {
                                biomaterialCode = string.Empty;
                                errorSuffix = "У заказываемых исследований не совпадают коды биоматериалов по умолчанию";
                                break;
                            }
                        }
                    }
                    else
                    {
                        biomaterialCode = string.Empty;
                        errorSuffix = string.Format("Исследование с кодом [{0}] не найдено в справочнике", sampleTarget.Code);
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(biomaterialCode))
                {
                    sample.BiomaterialCode = biomaterialCode;
                    GAP.Logger.WriteText("Для пробы [{0}] заявки [{1}] установлен код биоматериала по умолчанию: {2}", sample.Barcode, requestDTO.RequestCode, sample.BiomaterialCode);
                }
                else
                {
                    GAP.Logger.WriteError("Для пробы [{0}] заявки [{1}] не удалось установить код биоматериала. Причина:\r\n\r\n{2}", sample.Barcode, requestDTO.RequestCode, errorSuffix);
                }
            }
        }


        /// <summary>
        /// Store request to cache database
        /// </summary>
        /// <param name="request"></param>
        public virtual void StoreRequest(Request request, StatusObjectCache status, string errorStr = null, bool isThrowCacheException = true)
        {
            RequestObjectStatus reqObjS = new RequestObjectStatus() { Request = request, StatusId = status, Comment = errorStr };
            try
            {
                CacheHelper.SaveRequest(reqObjS);
                GAP.Logger.WriteText("Request {0} was successfully cached with status [{1}]", request.RequestCode, status.ToString());
            }
            catch (CacheException)
            {
                if (isThrowCacheException)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Stare result to cache database
        /// </summary>
        public virtual void StoreResult(Result result, StatusObjectCache status, string errorStr = null, bool isThrowCacheException = true)
        {
            ResultObjectStatus resObjS = new ResultObjectStatus() { Result = result, StatusId = status, Comment = errorStr };
            try
            {
                CacheHelper.SaveResult(resObjS);
                GAP.Logger.WriteText("Result for RequestCode {0} was successfully cached with status [{1}]", result.RequestCode, status);
            }
            catch (CacheException)
            {
                if (isThrowCacheException)
                {
                    throw;
                }
            }
        }

        internal void ProcessDeliveredBiomaterials(List<DeliveredBiomaterial> deliveredBiomaterials)
        {
            try
            {
                SpecificProcessDeliveredBiomaterials(deliveredBiomaterials);
            }
            catch (Exception ex)
            {
                GAP.Logger.WriteError("Ошибка при обработке доставленных биоматериалов: {0}", ex.ToString());
            }
        }

        public virtual void SpecificProcessDeliveredBiomaterials(List<DeliveredBiomaterial> deliveredBiomaterials)
        { }
    }
}