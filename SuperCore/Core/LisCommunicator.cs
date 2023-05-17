using System;
using System.Collections.Generic;
using System.Text;
using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LisBusinessObjects;
using ru.novolabs.SuperCore.LisBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LisBusinessObjects.Outsource;
using ru.novolabs.SuperCore.LisDictionary;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Содержит все методы, используемые для коммуникации с сервером ЛИС
    /// </summary>
    public class LisCommunicator : BaseCommunicator, IDisposable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса LisCommunicator
        /// </summary>
        public LisCommunicator() : base() { }
        /// <summary>
        /// Инициализирует новый экземпляр класса LisCommunicator с указанием режима (однопользовательский/многопользовательский)
        /// </summary>
        public LisCommunicator(bool multiUserMode) : base(multiUserMode) { }

        /// <summary>
        /// Возвращает или задает кэш справочников коммуникатора с ЛИС
        /// </summary>
        public LisDictionaryCache DictionaryCache
        {
            get { return (LisDictionaryCache)dictionaryCache; }
            set { dictionaryCache = value; }
        }

        /// <summary>
        /// Освобождает неуправляемые ресурсы
        /// </summary>
        public void Dispose()
        {
            if (LoggedIn)
                Logout();
        }

        protected void SystemLogin(string externalSystemCode, string loggingLevel) 
        {
            SystemLogin systemLogin = new SystemLogin(loggingLevel);
            systemLogin.Code = externalSystemCode;
            SessionId = LoadObject(systemLogin, null, XMLConst.XML_METHOD_SYSTEM_LOGIN, false, true);
        }

        protected void SystemLogin(string externalSystemCode, string loggingLevel, LimsUserSession UserSession)
        {
            SystemLogin systemLogin = new SystemLogin(loggingLevel);
            systemLogin.Code = externalSystemCode;
            SessionId = LoadObject(systemLogin, null, XMLConst.XML_METHOD_SYSTEM_LOGIN, false, true);
        }

        /// <summary>
        /// Создаёт или модифицирует заявку
        /// </summary>
        public Boolean CreateRequest(BaseRequest request)
        {
            List<RequestId> ids;
            return CreateRequest(request, out ids);
        }

        public Boolean CreateRequest(BaseRequest request, LimsUserSession UserSession)
        {
            List<RequestId> ids;
            return CreateRequest(request, out ids, UserSession);
        }


        /// <summary>
        /// Создаёт или модифицирует заявку
        /// </summary>
        public Boolean CreateRequest(BaseRequest request, out List<RequestId> ids)
        {
            RequestSaveParams saveParams = new RequestSaveParams();
            saveParams.Request = request;
            RequestSaveResponce responce = new RequestSaveResponce();
            LoadObject(saveParams, responce, XMLConst.XML_METHOD_CREATE_REQUESTS, false, true);
            ids = responce.Ids;
            return true;
        }

        public Boolean CreateRequest(BaseRequest request, out List<RequestId> ids, LimsUserSession UserSession)
        {
            RequestSaveParams saveParams = new RequestSaveParams();
            saveParams.Request = request;
            RequestSaveResponce responce = new RequestSaveResponce();
            LoadObject(saveParams, responce, XMLConst.XML_METHOD_CREATE_REQUESTS, false, true);
            ids = responce.Ids;
            return true;
        }

        // Создает заявку.
        /* public Boolean CreateRequest(BaseRequest request, out List<RequestId> ids, ExternalRequest externalRequest = null)
         {
             int errorCode = 0;
             string errorMessage = String.Empty;

             RequestSaveParams saveParams = new RequestSaveParams();
             saveParams.Request = request;

             RequestSaveResponce responce = new RequestSaveResponce();
             List<ErrorMessage> infoMessages;

             LoadObject(saveParams, responce, XMLConst.XML_METHOD_CREATE_REQUESTS, false, true, ref errorCode, ref errorMessage);

             ids = null;

             if (externalRequest != null)
             {
                 externalRequest.Errors = responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.LIS_RESPONSE_MESSAGE_ERROR);
                 externalRequest.Warnings.AddRange(responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.LIS_RESPONSE_MESSAGE_WARNING));
                 infoMessages = responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.LIS_RESPONSE_MESSAGE_INFO);
                 infoMessages.ForEach(info => Log.WriteText(info.Message));
             }

             if (errorCode == 0)
             {
                 ids = responce.Ids;
                 return true;
             }
             else
                 throw new ApplicationException(errorMessage);
         }*/

        // Удаляет заявки.
        /// <summary>
        /// Удаляет заявки с указанными идентификаторами
        /// </summary>
        public Boolean RemoveRequests(List<ObjectRef> ids)
        {
            RemoveRequestsRequest request = new RemoveRequestsRequest();
            request.Requests.AddRange(ids);
            LoadObject(request, null, XMLConst.XML_METHOD_REQUEST_DELETE, false, false);
            return true;
        }

        public Boolean RemoveRequests(List<ObjectRef> ids, LimsUserSession UserSession)
        {
            RemoveRequestsRequest request = new RemoveRequestsRequest();
            request.Requests.AddRange(ids);
            LoadObject(request, null, XMLConst.XML_METHOD_REQUEST_DELETE, false, false);
            return true;
        }

        /// <summary>
        /// Возвращает коллекцию заявок, удовлетворяющих фильтру
        /// </summary>
        public List<BaseRequest> RegistrationJournal(RegistrationJournalFilter filter)
        {
            RequestSet response = new RequestSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_REGISTRATION_JOURNAL, false, true);
            return response.Request;
        }

        public List<BaseRequest> RegistrationJournal(RegistrationJournalFilter filter, LimsUserSession UserSession)
        {
            RequestSet response = new RequestSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_REGISTRATION_JOURNAL, false, true);
            return response.Request;
        }

        /// <summary>
        /// Возвращает коллекцию проб, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<BaseSample> WorkJournal(WorkJournalFilter filter)
        {
            SampleSet response = new SampleSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_WORK_JOURNAL, false, true);
            return response.Samples;
        }

        public List<BaseSample> WorkJournal(WorkJournalFilter filter, LimsUserSession UserSession)
        {
            SampleSet response = new SampleSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_WORK_JOURNAL, false, true);
            return response.Samples;
        }
        
        //public List<BaseSample> WorkJournal(Registration

        /// <summary>
        /// Возвращает информацию по заявке
        /// </summary>
        public BaseRequest RequestInfo(int id)
        {
            var request = new { Request = new ObjectRef(id) };
            BaseRequest response = new BaseRequest();
            LoadObject(request, response, XMLConst.XML_METHOD_REQUEST_INFO, false, false);
            return response;
        }

        public BaseRequest RequestInfo(int id, LimsUserSession UserSession)
        {
            var request = new { Request = new ObjectRef(id) };
            BaseRequest response = new BaseRequest();
            LoadObject(request, response, XMLConst.XML_METHOD_REQUEST_INFO, false, false);
            return response;
        }

        public Boolean IsRequestNrUnique(String internalNr)
        {
            CheckRequestNrsParamsRequest request = new CheckRequestNrsParamsRequest();
            request.Nrs.Add(new RequestBatchNr() { InternalNr = internalNr });
            CheckRequestNrsParamsResponce response = new CheckRequestNrsParamsResponce();            
            LoadObject(request, response, XMLConst.XML_METHOD_CHECK_REQUEST_NRS, false, true);
            return response.Nrs.Count == 0;
        }

        public Boolean IsRequestNrUnique(String internalNr, LimsUserSession UserSession)
        {
            CheckRequestNrsParamsRequest request = new CheckRequestNrsParamsRequest();
            request.Nrs.Add(new RequestBatchNr() { InternalNr = internalNr });
            CheckRequestNrsParamsResponce response = new CheckRequestNrsParamsResponce();
            LoadObject(request, response, XMLConst.XML_METHOD_CHECK_REQUEST_NRS, false, true);
            return response.Nrs.Count == 0;
        }

        /// <summary>
        /// Сохраняет пациента. В выходном аргументе patientCardIds возвращает список объектов PatientCardId
        /// </summary>
        public Int32 SavePatient(Patient patient, out List<PatientCardId> patientCardIds)
        {
            PatientResponce response = new PatientResponce();
            LoadObject(patient, response, XMLConst.XML_METHOD_PATIENT_SAVE, false, true);
            patientCardIds = response.PatientCards;
            return response.Id;
        }

        public Int32 SavePatient(Patient patient, out List<PatientCardId> patientCardIds, LimsUserSession UserSession)
        {
            PatientResponce response = new PatientResponce();
            LoadObject(patient, response, XMLConst.XML_METHOD_PATIENT_SAVE, false, true);
            patientCardIds = response.PatientCards;
            return response.Id;
        }

        private Boolean SaveDictionary(DictionarySaveRequest request)
        {
            DictionarySaveResponce result = new DictionarySaveResponce();
            LoadObject(request, result, XMLConst.XML_METHOD_SAVE_DICTIONARY, false, false);
            request.Element.Id = result.Id;
            return true;
        }

        private Boolean SaveDictionary(DictionarySaveRequest request, LimsUserSession UserSession)
        {
            DictionarySaveResponce result = new DictionarySaveResponce();
            LoadObject(request, result, XMLConst.XML_METHOD_SAVE_DICTIONARY, false, false);
            request.Element.Id = result.Id;
            return true;
        }

        public Boolean SaveMaterial(MaterialDictionaryItem material)
        {
            DictionarySaveRequest request = new DictionarySaveRequest();

            request.Directory = "material";
            request.Element = material;

            return SaveDictionary(request);
        }

        public Boolean SaveMaterial(MaterialDictionaryItem material, LimsUserSession UserSession)
        {
            DictionarySaveRequest request = new DictionarySaveRequest();

            request.Directory = "material";
            request.Element = material;

            return SaveDictionary(request, UserSession);
        }

        public Boolean RemoveDictionary(DictionaryRemoveRequst request)
        {
            LoadObject(request, null, XMLConst.XML_METHOD_REMOVE_DICTIONARY, false, false);
            return true;
        }

        public Boolean RemoveDictionary(DictionaryRemoveRequst request, LimsUserSession UserSession)
        {
            LoadObject(request, null, XMLConst.XML_METHOD_REMOVE_DICTIONARY, false, false);
            return true;
        }


        public Boolean RemoveMaterials(List<ObjectRef> ids)
        {
            DictionaryRemoveRequst request = new DictionaryRemoveRequst();
            request.Directory = "material";
            request.Ids = ids;

            return RemoveDictionary(request);
        }

        public Boolean RemoveMaterials(List<ObjectRef> ids, LimsUserSession UserSession)
        {
            DictionaryRemoveRequst request = new DictionaryRemoveRequst();
            request.Directory = "material";
            request.Ids = ids;

            return RemoveDictionary(request);
        }

        public Boolean RemoveMaterial(ObjectRef id)
        {
            return RemoveMaterials(new List<ObjectRef>() { id });
        }

        public Boolean RemoveMaterial(ObjectRef id, LimsUserSession UserSession)
        {
            return RemoveMaterials(new List<ObjectRef>() { id }, UserSession);
        }

        public Boolean RemoveMaterial(Int32 id)
        {
            return RemoveMaterial(new ObjectRef(id));
        }

        public Boolean RemoveMaterial(Int32 id, LimsUserSession UserSession)
        {
            return RemoveMaterial(new ObjectRef(id), UserSession);
        }

        public List<ErrorMessage> OperationArrivalTemplateSave(OperationArrivalTemplate template)
        {
            OperationArrivalTemplateSaveResponce response = new OperationArrivalTemplateSaveResponce();
            LoadObject(template, response, XMLConst.XML_METHOD_OPERATION_TEMPLATE_SAVE, false, true);
            template.Id = response.Id;
            return response.Errors;
        }

        public List<ErrorMessage> OperationArrivalTemplateSave(OperationArrivalTemplate template, LimsUserSession UserSession)
        {
            OperationArrivalTemplateSaveResponce response = new OperationArrivalTemplateSaveResponce();
            LoadObject(template, response, XMLConst.XML_METHOD_OPERATION_TEMPLATE_SAVE, false, true);
            template.Id = response.Id;
            return response.Errors;
        }

        public SystemOperation SystemOperationInfo(Int32 id)
        {
            SystemOperationInfoRequest request = new SystemOperationInfoRequest();
            request.Operation.Id = id;
            SystemOperation response = new SystemOperation();

            LoadObject(request, response, XMLConst.XML_METHOD_SYSTEM_OPERATION_INFO, false, true);

            return response;
        }

        public SystemOperation SystemOperationInfo(Int32 id, LimsUserSession UserSession)
        {
            SystemOperationInfoRequest request = new SystemOperationInfoRequest();
            request.Operation.Id = id;
            SystemOperation response = new SystemOperation();

            LoadObject(request, response, XMLConst.XML_METHOD_SYSTEM_OPERATION_INFO, false, true);

            return response;
        }

        public Int32 SystemNextMaterialCatalogNr()
        {
            SystemNextMaterialCatalogNrResponce response = new SystemNextMaterialCatalogNrResponce();

            LoadObject(new Object(), response, XMLConst.XML_METHOD_SYSTEM_NEXT_MATERIAL_CATALOG_NR, false, true);

            return response.Nr;
        }

        public Int32 SystemNextMaterialCatalogNr(LimsUserSession UserSession)
        {
            SystemNextMaterialCatalogNrResponce response = new SystemNextMaterialCatalogNrResponce();

            LoadObject(new Object(), response, XMLConst.XML_METHOD_SYSTEM_NEXT_MATERIAL_CATALOG_NR, false, true);

            return response.Nr;
        }

        private Boolean LoadDictionaryElement<T>(Int32 id, ref T item, String directoryName) where T : DictionaryItem
        {
            DictionaryLoadRequest request = new DictionaryLoadRequest();
            request.Directory = directoryName;
            request.Ids.Add(new ObjectRef(id));
            DictionaryLoadResponce<T> response = new DictionaryLoadResponce<T>();

            LoadObject(request, response, XMLConst.XML_METHOD_DIRECTORY, false, false);

            item = response.Material[0];
            return true;
        }

        private Boolean LoadDictionaryElement<T>(Int32 id, ref T item, String directoryName, LimsUserSession UserSession) where T : DictionaryItem
        {
            DictionaryLoadRequest request = new DictionaryLoadRequest();
            request.Directory = directoryName;
            request.Ids.Add(new ObjectRef(id));
            DictionaryLoadResponce<T> response = new DictionaryLoadResponce<T>();

            LoadObject(request, response, XMLConst.XML_METHOD_DIRECTORY, false, false);

            item = response.Material[0];
            return true;
        }

        public Boolean LoadMaterialElement(Int32 id, ref MaterialDictionaryItem item)
        {
            return LoadDictionaryElement(id, ref item, "material");
        }

        public Boolean LoadMaterialElement(Int32 id, ref MaterialDictionaryItem item, LimsUserSession UserSession)
        {
            return LoadDictionaryElement(id, ref item, "material", UserSession);
        }

        // Создаёт/изменяет заявку
        /* public Int32 CreateRequest2(ExternalRequest Request)
         {
             int errorCode = 0;
             string errorMessage = String.Empty;

             ExternalRequestResponce responce = new ExternalRequestResponce();
             List<ErrorMessage> infoMessages;

             LoadObject(Request, responce, XMLConst.XML_METHOD_CREATE_REQUEST_2, true, true, ref errorCode, ref errorMessage);

             Request.Errors = responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.LIS_RESPONSE_MESSAGE_ERROR);
             Request.Warnings.AddRange(responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.LIS_RESPONSE_MESSAGE_WARNING));
             infoMessages = responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.LIS_RESPONSE_MESSAGE_INFO);
             infoMessages.ForEach(info => Log.WriteText(info.Message));
             if (errorCode != 0)
             {
                 ErrorMessage err = new ErrorMessage();
                 err.Message = errorMessage;
                 Request.Errors.Add(err);
             }
             return responce.Id;
         }
         * */

        public List<ExternalResult> GetRequestsResults(List<ObjectRef> requestIds)
        {
            GetRequestResultsParams queryParams = new GetRequestResultsParams();
            queryParams.Requests = requestIds;
            GetRequestResultsResponce response = new GetRequestResultsResponce();
            LoadObject(queryParams, response, XMLConst.XML_METHOD_GET_REQUESTS_RESULTS_2, false, true);
            return response.Results;
        }

        public List<ExternalResult> GetRequestsResults(List<ObjectRef> requestIds, LimsUserSession UserSession)
        {
            GetRequestResultsParams queryParams = new GetRequestResultsParams();
            queryParams.Requests = requestIds;
            GetRequestResultsResponce response = new GetRequestResultsResponce();
            LoadObject(queryParams, response, XMLConst.XML_METHOD_GET_REQUESTS_RESULTS_2, false, true);
            return response.Results;
        }

        public BaseSample GetSampleResults(BaseSample sample)
        {
            var Id = new { sample = new ObjectRef(sample.Id) };
            LoadObject(Id, sample, XMLConst.XML_METHOD_REQUEST_WORKS, false, false);
            return sample;
        }

        public BaseSample GetSampleResults(BaseSample sample, LimsUserSession UserSession)
        {
            var Id = new { sample = new ObjectRef(sample.Id) };
            LoadObject(Id, sample, XMLConst.XML_METHOD_REQUEST_WORKS, false, false);
            return sample;
        }

        public BaseSample GetSample(Int32 sampleId)
        {
            BaseSample sample = new BaseSample();
            var Id = new { sample = new ObjectRef(sampleId) };
            LoadObject(Id, sample, XMLConst.XML_METHOD_SAMPLE_INFO, false, false);
            return sample;
        }

        public BaseSample GetSample(Int32 sampleId, LimsUserSession UserSession)
        {
            BaseSample sample = new BaseSample();
            var Id = new { sample = new ObjectRef(sampleId) };
            LoadObject(Id, sample, XMLConst.XML_METHOD_SAMPLE_INFO, false, false);
            return sample;
        }

        public Boolean SaveSample(BaseSample sample)
        {
            // PatientResponce responce = new PatientResponce();
            //LoadObject(patient, responce, XMLConst.XML_METHOD_PATIENT_SAVE, false, true);
            Object Responce = new Object();
            LoadObject(sample, Responce, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true);
            return false;
        }

        public Boolean SaveSample(BaseSample sample, LimsUserSession UserSession)
        {
            // PatientResponce responce = new PatientResponce();
            //LoadObject(patient, responce, XMLConst.XML_METHOD_PATIENT_SAVE, false, true);
            Object Responce = new Object();
            LoadObject(sample, Responce, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true);
            return false;
        }

        public Boolean SaveSample(Object sample)
        {
            // PatientResponce responce = new PatientResponce();
            //LoadObject(patient, responce, XMLConst.XML_METHOD_PATIENT_SAVE, false, true);
            Object Responce = new Object();
            LoadObject(sample, Responce, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true);
            return false;
        }

        public Boolean SaveSample(Object sample, LimsUserSession UserSession)
        {
            // PatientResponce responce = new PatientResponce();
            //LoadObject(patient, responce, XMLConst.XML_METHOD_PATIENT_SAVE, false, true);
            Object Responce = new Object();
            LoadObject(sample, Responce, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true);
            return false;
        }

        /// <summary>
        /// Возвращает пробы по указанному идентификатору заявки
        /// </summary>
        public List<BaseSample> RequestSamples(Int32 requestId)
        {
            RequestSamplesParams queryParams = new RequestSamplesParams();
            queryParams.Request.Id = requestId;
            RequestSamplesResponce response = new RequestSamplesResponce();
            LoadObject(queryParams, response, XMLConst.XML_METHOD_REQUESTS_SAMPLES, false, false);
            return response.Samples;
        }

        public List<BaseSample> RequestSamples(Int32 requestId, LimsUserSession UserSession)
        {
            RequestSamplesParams queryParams = new RequestSamplesParams();
            queryParams.Request.Id = requestId;
            RequestSamplesResponce response = new RequestSamplesResponce();
            LoadObject(queryParams, response, XMLConst.XML_METHOD_REQUESTS_SAMPLES, false, false);
            return response.Samples;
        }

        internal class EquipmentDataResponse
        {
            public EquipmentDataResponse()
            {
                Messages = new List<EquipmentResponseMessage>();
            }

            [Unnamed]
            public List<EquipmentResponseMessage> Messages { get; set; }
        }

        /// <summary>
        /// Посылает данные с анализатора серверу
        /// </summary>
        public List<EquipmentResponseMessage> EquipmentData(EquipmentData data)
        {
            EquipmentDataResponse response = new EquipmentDataResponse();
            try
            {
                LoadObject(data, response, XMLConst.XML_METHOD_EQUIPMENT_DATA, false, true);
                return response.Messages;
            }
            catch (NlsServerException ex)
            {
                var messages = new List<EquipmentResponseMessage>();
                messages.Add(new EquipmentResponseMessage { Severity = LisErrorMessageTypes.ERROR, Message = ex.ErrorMessage });
                return messages;
            }
        }

        public List<EquipmentResponseMessage> EquipmentData(EquipmentData data, LimsUserSession UserSession)
        {
            EquipmentDataResponse response = new EquipmentDataResponse();
            try
            {
                LoadObject(data, response, XMLConst.XML_METHOD_EQUIPMENT_DATA, false, true);
                return response.Messages;
            }
            catch (NlsServerException ex)
            {
                var messages = new List<EquipmentResponseMessage>();
                messages.Add(new EquipmentResponseMessage { Severity = LisErrorMessageTypes.ERROR, Message = ex.ErrorMessage });
                return messages;
            }
        }

        /// <summary>
        /// Запрашивает у сервера задание указанному анализатору для указанных проб(образцов)
        /// </summary>
        public WorkListQueryResponse TestsForSamples(WorkListQuery query)
        {
            WorkListQueryResponse response = new WorkListQueryResponse();
            LoadObject(query, response, XMLConst.XML_METHOD_TESTS_FOR_SAMPLES, false, true);
            return response;
        }

        public WorkListQueryResponse TestsForSamples(WorkListQuery query, LimsUserSession UserSession)
        {
            WorkListQueryResponse response = new WorkListQueryResponse();
            LoadObject(query, response, XMLConst.XML_METHOD_TESTS_FOR_SAMPLES, false, true);
            return response;
        }


        private class WorkListJournalResponce
        {
            public WorkListJournalResponce()
            {
                Worklists = new List<WorkListShortInfo>();
            }

            public List<WorkListShortInfo> Worklists { get; set; }
        }

        /// <summary>
        /// Возвращает коллекцию кратких сведений о рабочих списках, удовлетворяющих фильтру
        /// </summary>
        public List<WorkListShortInfo> WorklistJournal(WorklistJournalFilter filter)
        {
            WorkListJournalResponce response = new WorkListJournalResponce();
            LoadObject(filter, response, XMLConst.XML_METHOD_WORKLIST_JOURNAL, false, true);
            return response.Worklists;
        }

        public List<WorkListShortInfo> WorklistJournal(WorklistJournalFilter filter, LimsUserSession UserSession)
        {
            WorkListJournalResponce response = new WorkListJournalResponce();
            LoadObject(filter, response, XMLConst.XML_METHOD_WORKLIST_JOURNAL, false, true);
            return response.Worklists;
        }


        internal class WorklistInfosResponce
        {            
            public WorklistInfosResponce()
            {
                Worklists = new List<WorkList>();
            }

            [Unnamed]
            public List<WorkList> Worklists { get; set; }
        }

        /// <summary>
        /// Возвращает коллекцию рабочих списков по Id
        /// </summary>        
        public List<WorkList> WorklistInfos(RefSet worklistIds)
        {
            var request = new { Worklists = worklistIds };
            var response = new WorklistInfosResponce();

            LoadObject(request, response, XMLConst.XML_METHOD_WORKLIST_INFOS, false, false);
            return response.Worklists;
        }

        public List<WorkList> WorklistInfos(RefSet worklistIds, LimsUserSession UserSession)
        {
            var request = new { Worklists = worklistIds };
            var response = new WorklistInfosResponce();

            LoadObject(request, response, XMLConst.XML_METHOD_WORKLIST_INFOS, false, false);
            return response.Worklists;
        }


        /// <summary>
        /// Изменяет "статус отправки в удалённый анализатор" рабочего списка
        /// </summary>        
        public void WorklistChangeSendRemoteState(WorkList worklist, Int32 newState)
        {
            WorklistDelta request = new WorklistDelta()
            {
                Id = worklist.Id,
                WorklistDef = new ObjectRef(worklist.WorklistDef.Id),
                Rack = new ObjectRef(worklist.Rack.Id),
                Code = worklist.Code,
                ExpireDate = worklist.ExpireDate,
                SendRemote = newState
            };

            LoadObject(request, null, XMLConst.XML_METHOD_WORKLIST_SAVE, true, true);
        }

        public void WorklistChangeSendRemoteState(WorkList worklist, Int32 newState, LimsUserSession UserSession)
        {
            WorklistDelta request = new WorklistDelta()
            {
                Id = worklist.Id,
                WorklistDef = new ObjectRef(worklist.WorklistDef.Id),
                Rack = new ObjectRef(worklist.Rack.Id),
                Code = worklist.Code,
                ExpireDate = worklist.ExpireDate,
                SendRemote = newState
            };

            LoadObject(request, null, XMLConst.XML_METHOD_WORKLIST_SAVE, true, true);
        }

        internal class OutsourceRequestJournalResponce
        {
            public OutsourceRequestJournalResponce()
            {
                Requests = new List<OutsourceRequest>();
            }

            [Unnamed]
            public List<OutsourceRequest> Requests { get; set; }
        }
        
        /// <summary>
        /// Возвращает коллекцию "заявок во внешнюю лабораторию", удовлетворяющих фильтру
        /// </summary>
        public List<OutsourceRequest> OutsourceRequestJournal(OutsourceRequestJournalFilter filter)
        {
            OutsourceRequestJournalResponce response = new OutsourceRequestJournalResponce();
            LoadObject(filter, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_JOURNAL, false, true);
            return response.Requests;
        }

        public List<OutsourceRequest> OutsourceRequestJournal(OutsourceRequestJournalFilter filter, LimsUserSession UserSession)
        {
            OutsourceRequestJournalResponce response = new OutsourceRequestJournalResponce();
            LoadObject(filter, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_JOURNAL, false, true);
            return response.Requests;
        }
               
        /// <summary>
        /// Возвращает "заявку во внешнюю лабораторию"
        /// </summary>
        public OutsourceRequest OutsourceRequestInfo(ObjectRef outsourceRequestRef)
        {
            OutsourceRequest response = new OutsourceRequest();
            LoadObject(new { OutsourceRequest = outsourceRequestRef }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_INFO, false, true);
            return response;
        }

        public OutsourceRequest OutsourceRequestInfo(ObjectRef outsourceRequestRef, LimsUserSession UserSession)
        {
            OutsourceRequest response = new OutsourceRequest();
            LoadObject(new { OutsourceRequest = outsourceRequestRef }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_INFO, false, true);
            return response;
        }

        internal class OutsourceRequestSaveResultsResponce
        {
            public OutsourceRequestSaveResultsResponce()
            {
                Errors = new List<ErrorMessage>();
            }

            [Unnamed]
            public List<ErrorMessage> Errors { get; set; }
        }

        /// <summary>
        /// Сохраняет результаты по "заявке во внешнюю лабораторию"
        /// </summary>
        public List<ErrorMessage> OutsourceRequestSaveResults(OutsourceRequest outsourceRequest)
        {
            var response = new OutsourceRequestSaveResultsResponce();
            LoadObject(new { Request = outsourceRequest }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_SAVE_RESULTS, false, true);
            return response.Errors;
        }

        public List<ErrorMessage> OutsourceRequestSaveResults(OutsourceRequest outsourceRequest, LimsUserSession UserSession)
        {
            var response = new OutsourceRequestSaveResultsResponce();
            LoadObject(new { Request = outsourceRequest }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_SAVE_RESULTS, false, true);
            return response.Errors;
        }

        /// <summary>
        /// Изменяет состояние "заявки во внешнюю лабораторию"
        /// </summary>
        public void OutsourceRequestChangeState(OutsourceRequest outsourceRequest, int newState)
        {
            var request = new { OutsourceRequests = new RefSet() { new ObjectRef(outsourceRequest.Id)}, NewState = newState };
            LoadObject(request, null, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_CHANGE_STATE, false, true);
        }

        public void OutsourceRequestChangeState(OutsourceRequest outsourceRequest, int newState, LimsUserSession UserSession)
        {
            var request = new { OutsourceRequests = new RefSet() { new ObjectRef(outsourceRequest.Id) }, NewState = newState };
            LoadObject(request, null, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_CHANGE_STATE, false, true);
        }    

        /// <summary>
        /// Загружает в профайлы пользователей фильтр журнала
        /// </summary>
        public void GetUserProfileFilter(FilterInfo filterInfo)
        {
            filterInfo.JournalFilter = GetObjectFromFileServer<JournalFilterSettings>(filterInfo.Filter, Encoding.GetEncoding(1251));
        }

        public void GetUserProfileFilter(FilterInfo filterInfo, LimsUserSession UserSession)
        {
            filterInfo.JournalFilter = GetObjectFromFileServer<JournalFilterSettings>(filterInfo.Filter, Encoding.GetEncoding(1251));
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetServicePrices(Int32 timestamp)
        {
            ServicePriceRequest request = new ServicePriceRequest(timestamp);
            ServicePrice prices = new ServicePrice();
            LoadObject(request, prices, XMLConst.XML_METHOD_LOAD_SERVICE_PRICE, false, true);

            HospitalDictionary hospitals = (HospitalDictionary)ProgramContext.Dictionaries.GetDictionary(LisDictionaryNames.Hospital);


            if (hospitals != null)
            {
                hospitals.UpdatePrices(prices);
            }
        }

        public void GetServicePrices(Int32 timestamp, LimsUserSession UserSession)
        {
            ServicePriceRequest request = new ServicePriceRequest(timestamp);
            ServicePrice prices = new ServicePrice();
            LoadObject(request, prices, XMLConst.XML_METHOD_LOAD_SERVICE_PRICE, false, true);

            HospitalDictionary hospitals = (HospitalDictionary)ProgramContext.Dictionaries.GetDictionary(LisDictionaryNames.Hospital);


            if (hospitals != null)
            {
                hospitals.UpdatePrices(prices);
            }
        }

        public void GetOrderForm(RequestFormDictionaryItem requestForm)
        {
            if (requestForm._OrderForm != null)
                return;
            requestForm._OrderForm = GetOrderForm(requestForm.OrderForm);

        }

        public void GetOrderForm(RequestFormDictionaryItem requestForm, LimsUserSession UserSession)
        {
            if (requestForm._OrderForm != null)
                return;
            requestForm._OrderForm = GetOrderForm(requestForm.OrderForm, UserSession);

        }


        public OrderForm GetOrderForm(Int32 id)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<OrderForm>(id, Encoding.UTF8);
            }
            return null;
        }

        public OrderForm GetOrderForm(Int32 id, LimsUserSession UserSession)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<OrderForm>(id, Encoding.UTF8);
            }
            return null;
        }

        public void GetControlsFormLayout(RequestFormDictionaryItem requestForm)
        {
            if (requestForm._ControlsFormLayout != null)
                return;
            requestForm._ControlsFormLayout = GetControlsFormLayout(requestForm.ControlsFormLayout);
        }

        public void GetControlsFormLayout(RequestFormDictionaryItem requestForm, LimsUserSession UserSession)
        {
            if (requestForm._ControlsFormLayout != null)
                return;
            requestForm._ControlsFormLayout = GetControlsFormLayout(requestForm.ControlsFormLayout, UserSession);
        }


        public ControlsFormLayout GetControlsFormLayout(Int32 id)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<ControlsFormLayout>(id, Encoding.UTF8);
            }
            return null;
        }

        public ControlsFormLayout GetControlsFormLayout(Int32 id, LimsUserSession UserSession)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<ControlsFormLayout>(id, Encoding.UTF8);
            }
            return null;
        }

        internal class TeamSet
        {
            public TeamSet()
            {
                Teams = new List<Team>();
            }

            [Unnamed]
            public List<Team> Teams { get; set;}
        }

        /// <summary>
        /// Возвращает коллекцию бригад (смен) сотрудников, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Team> TeamJournal(TeamJournalFilter filter)
        {
            var response = new TeamSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_TEAM_JOURNAL, false, true);
            return response.Teams;
        }

        public List<Team> TeamJournal(TeamJournalFilter filter, LimsUserSession UserSession)
        {
            var response = new TeamSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_TEAM_JOURNAL, false, true);
            return response.Teams;
        }

        /// <summary>
        /// Сохраняет бригаду (смену) сотрудников
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public int SaveTeam(Team team)
        {
            var response = new ObjectRef();
            LoadObject(team, response, XMLConst.XML_METHOD_TEAM_SAVE, false, true);
            return response.Id;
        }

        public int SaveTeam(Team team, LimsUserSession UserSession)
        {
            var response = new ObjectRef();
            LoadObject(team, response, XMLConst.XML_METHOD_TEAM_SAVE, false, true);
            return response.Id;
        }

        /// <summary>
        /// Получает информацию о бригаде (смене) сотрудников
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public Team GetTeam(int teamId)
        {
            var response = new Team();
            LoadObject(new { Id = new ObjectRef(teamId) }, response, XMLConst.XML_METHOD_TEAM_GET, false, true);
            return response;
        }

        public Team GetTeam(int teamId, LimsUserSession UserSession)
        {
            var response = new Team();
            LoadObject(new { Id = new ObjectRef(teamId) }, response, XMLConst.XML_METHOD_TEAM_GET, false, true);
            return response;
        }
    }
}
