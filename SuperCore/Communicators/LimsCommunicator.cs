using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.Crypto;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Audit;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsBusinessObjects.Outsource;
using ru.novolabs.SuperCore.LimsBusinessObjects.Payment;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.HardwareWork.WMI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.Core.HardwareWork;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Содержит все методы, используемые для коммуникации с сервером ЛИС
    /// </summary>
    public class LimsCommunicator : BaseCommunicator, IDisposable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса LisCommunicator
        /// </summary>
        public LimsCommunicator() : base() { }

        /// <summary>
        /// Хранит настройки сервера
        /// </summary>
        public ServerOptionList ServerOptions { get; set; }

        public LimsUserSession LimsUserSession { get { return (LimsUserSession)UserSession; } }

        public override void LoadServerOptions()
        {
            if (ServerOptions == null)
                ServerOptions = GetOptions(null);
        }

        /// <summary>
        /// Возвращает или задает кэш справочников коммуникатора с ЛИС
        /// </summary>
        public LimsDictionaryCache DictionaryCache
        {
            get { return (LimsDictionaryCache)dictionaryCache; }
            set { dictionaryCache = value; }
        }

        /// <summary>
        /// Освобождает неуправляемые ресурсы
        /// </summary>
        public void Dispose()
        {
            NeedShowAwaitForm = false;
            if (LoggedIn)
                Logout();
        }

        public override void Login(string login, string password, string serverAddress, out BaseUserSession userSession, AbstractCodec codec)
        {
            Log.WriteText(String.Format("Logging in to server {0} as '{1}'", serverAddress, login));
            this.ServerAddress = serverAddress;
            this.UserSession = userSession = new LimsUserSession();
            Log.WriteText("ClientIdFull: {0}", codec.GetClientIdView());
            LoginRequest Login = new LoginRequest();
            LoginResponce loginResult = new LoginResponce();

            Login.Password = password;
            Login.Login = login;
            Login.Machine = codec.GetMachineName();
            Login.ClientId = codec.GetClientId();

            Login.InstanceCount = "0";
            Login.SessionCode = "19154";
            Login.Lab = "Лаборатория";
            Login.Company = "Компания";

            bool writeObjectTag = false;
            try
            {

                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                if (versionAfter129)
                    writeObjectTag = true;
            }
            catch { }

            userSession.SessionId = LoadObject(Login, loginResult, XMLConst.XML_METHOD_LOGIN, false, writeObjectTag);

            ((LimsUserSession)userSession).User = new EmployeeDictionaryItem() { Id = loginResult.Employee.GetRef(), Rights = ConvertAccessRights(loginResult.Rights) };
            if (dictionaryCache != null)
                ((LimsUserSession)userSession).User = (EmployeeDictionaryItem)dictionaryCache[LimsDictionaryNames.Employee, loginResult.Employee.GetRef()];
            ((LimsUserSession)userSession).WorkPlace = new WorkPlaceDictionaryItem() { Id = loginResult.WorkPlace.GetRef() };
            ((LimsUserSession)userSession).DoctorId = loginResult.DoctorId;
            ((LimsUserSession)userSession).ServerVersion = loginResult.ServerVersion;
            LoggedIn = !String.IsNullOrEmpty(userSession.SessionId);
        }

        public void Logout()
        {
            base.Logout();
        }

        protected override void InitLocals()
        {
            LoadServerOptions();
            if (LimsUserSession.User != null)
            {
                var rights = LimsUserSession.User.Rights;
                LimsUserSession.User = (EmployeeDictionaryItem)DictionaryCache[LimsDictionaryNames.Employee, LimsUserSession.User.Id];
                if (LimsUserSession.User == null)
                    throw new Exception("User not found");
                LimsUserSession.User.Rights = rights;

                UpdateAccessRights(LimsUserSession.User.Rights);
                ReloadHospitals();
                var workPlaceDictionary = (WorkPlaceDictionary)DictionaryCache[LimsDictionaryNames.WorkPlace];
                LimsUserSession.WorkPlace = (workPlaceDictionary != null) ? (WorkPlaceDictionaryItem)DictionaryCache[LimsDictionaryNames.WorkPlace, LimsUserSession.WorkPlace.Id] : null;
                Log.WriteText("-------------------------------------------  InitLocals completed -------------------------------------------");
            }
        }

        /// <summary>
        /// Метод "догрузки" полей справочника Hospital (Заказчики). Сервер не присылает custDepartments в составе этого справочника, поэтому их придется добавить самому.
        /// </summary>
        private void ReloadHospitals()
        {
            HospitalDictionary hospitals = (HospitalDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Hospital];
            CustDepartmentDictionary custDepartments = (CustDepartmentDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.CustDepartment];
            if (hospitals != null && custDepartments != null)
            {
                foreach (CustDepartmentDictionaryItem custDepartment in custDepartments.Elements)
                {
                    HospitalDictionaryItem hospital = (HospitalDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Hospital, custDepartment.Hospital.Id];
                    if (hospital != null && !hospital.CustDepartments.Contains(custDepartment))
                        hospital.CustDepartments.Add(custDepartment);

                }
            }
        }

        protected void SystemLogin(string externalSystemCode, string loggingLevel, out BaseUserSession userSession)
        {
            SystemLogin systemLogin = new SystemLogin(loggingLevel);
            systemLogin.Code = externalSystemCode;
            this.UserSession = new LimsUserSession();
            this.UserSession.SessionId = LoadObject(systemLogin, null, XMLConst.XML_METHOD_SYSTEM_LOGIN, false, writeObjectTag: true);
            userSession = (LimsUserSession)this.UserSession;
        }

        private List<AccessRightDictionaryItem> ConvertAccessRights(List<ObjectRef> rights)
        {
            List<AccessRightDictionaryItem> accessRights = new List<AccessRightDictionaryItem>();
            foreach (ObjectRef right in rights)
            {
                AccessRightDictionaryItem accessRight = new AccessRightDictionaryItem();
                accessRight.Id = right.Id;
                accessRights.Add(accessRight);
            }
            return accessRights;
        }

        private void UpdateAccessRights(List<AccessRightDictionaryItem> accessRights)
        {
            for (int i = 0; i < accessRights.Count; i++)
            {
                accessRights[i] = (AccessRightDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.AccessRight, accessRights[i].Id];
            }
        }

        /// <summary>
        /// Создаёт или модифицирует заявку
        /// </summary>
        //public Boolean CreateRequest(BaseRequest request)
        //{
        //    List<RequestId> ids;
        //    return CreateRequest(request, out ids);
        //}

        public Boolean CreateRequest(BaseRequest request, LimsUserSession userSession)
        {
            List<RequestId> ids;
            return CreateRequest(request, out ids, userSession);
        }


        /// <summary>
        /// Создаёт или модифицирует заявку
        /// </summary>
        //public Boolean CreateRequest(BaseRequest request, out List<RequestId> ids)
        //{
        //    RequestSaveParams saveParams = new RequestSaveParams();
        //    saveParams.Request = request;
        //    RequestSaveResponce responce = new RequestSaveResponce();
        //    LoadObject(saveParams, responce, XMLConst.XML_METHOD_CREATE_REQUESTS, false, true);
        //    ids = responce.Ids;
        //    return true;
        //}

        public Boolean CreateRequestXXX(CreateRequest3Request request, out Int32 id, LimsUserSession userSession)
        {
            CreateRequest3Response responce = CreateRequestXXXWithWarnings(request, userSession);
            id = responce.Id;
            return true;
        }

        public CreateRequest3Response CreateRequestXXXWithWarnings(CreateRequest3Request request, LimsUserSession userSession)
        {
            CreateRequest3Response response = new CreateRequest3Response();
            request.FirstName = !String.IsNullOrEmpty(request.FirstName) ? request.FirstName.ToUpper() : String.Empty;
            request.MiddleName = !String.IsNullOrEmpty(request.MiddleName) ? request.MiddleName.ToUpper() : String.Empty;
            request.LastName = !String.IsNullOrEmpty(request.LastName) ? request.LastName.ToUpper() : String.Empty;
            LoadObject(request, response, XMLConst.XML_METHOD_CREATE_REQUEST_XXX, false, true, userSession);
            return response;
        }

        public Boolean CreateRequestXXXBatch(CreateRequest3Request request, out List<Int32> ids, LimsUserSession userSession)
        {

            //RequestSaveResponce responce = new RequestSaveResponce();
            RequestSaveResponce responce = new RequestSaveResponce();
            request.FirstName = !String.IsNullOrEmpty(request.FirstName) ? request.FirstName.ToUpper() : String.Empty;
            request.MiddleName = !String.IsNullOrEmpty(request.MiddleName) ? request.MiddleName.ToUpper() : String.Empty;
            request.LastName = !String.IsNullOrEmpty(request.LastName) ? request.LastName.ToUpper() : String.Empty;
            LoadObject(request, responce, XMLConst.XML_METHOD_CREATE_REQUEST_XXX, false, true, userSession);
            ids = new List<int>();
            foreach (var id in responce.Ids)
                ids.Add(id.Id);
            return true;
        }
        public Boolean CreateRequest(BaseRequest request, out List<RequestId> ids, LimsUserSession userSession)
        {
            RequestSaveParams saveParams = new RequestSaveParams();
            saveParams.Request = request;
            RequestSaveResponce responce = new RequestSaveResponce();
            LoadObject(saveParams, responce, XMLConst.XML_METHOD_CREATE_REQUESTS, false, true, userSession);
            ids = responce.Ids;
            return true;
        }

        /// <summary>
        /// Создаёт или модифицирует заявку
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int CreateRequest2(ExternalRequest request)
        {
            ExternalRequestResponce responce = new ExternalRequestResponce();
            try
            {
                LoadObject(request, responce, XMLConst.XML_METHOD_CREATE_REQUEST_2, true, true);
            }
            catch (NlsServerException ex)
            {
                request.Errors.Add(new ErrorMessage() { Severity = ErrorMessageTypes.ERROR, Message = ex.ErrorMessage });
            }

            request.Errors = responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.ERROR);
            request.Warnings.AddRange(responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.WARNING));
            var infoMessages = responce.Errors.FindAll(e => e.Severity == ErrorMessageTypes.INFO);
            infoMessages.ForEach(info => Log.WriteText(info.Message));
            return responce.Id;
        }

        /// <summary>
        /// Возвращает коллекцию версий справочников
        /// </summary>
        /// <returns></returns>
        public List<DirectoryVersionInfo> DirectoryVersions()
        {
            DirectoryVesionInfoSet responce = new DirectoryVesionInfoSet();
            LoadObject(new object(), responce, XMLConst.XML_METHOD_DIRECTORY_VERSIONS, false, true);
            return responce.Versions;
        }



        // Удаляет заявки.
        /// <summary>
        /// Удаляет заявки с указанными идентификаторами
        /// </summary>
        //public Boolean RemoveRequests(List<ObjectRef> ids)
        //{
        //    RemoveRequestsRequest request = new RemoveRequestsRequest();
        //    request.Requests.AddRange(ids);
        //    LoadObject(request, null, XMLConst.XML_METHOD_REQUEST_DELETE, false, true);
        //    return true;
        //}

        public Boolean RemoveRequests(List<ObjectRef> ids, LimsUserSession userSession)
        {
            RemoveRequestsRequest request = new RemoveRequestsRequest();
            request.Requests.AddRange(ids);
            LoadObject(request, null, XMLConst.XML_METHOD_REQUEST_DELETE, false, true, userSession);
            return true;
        }

        /// <summary>
        /// Возвращает коллекцию заявок, удовлетворяющих фильтру
        /// </summary>
        //public List<BaseRequest> RegistrationJournal(RegistrationJournalFilter filter)
        //{
        //    RequestSet response = new RequestSet();
        //    LoadObject(filter, response, XMLConst.XML_METHOD_REGISTRATION_JOURNAL, false, true);
        //    return response.Request;
        //}

        public List<BaseRequest> RegistrationJournal(RegistrationJournalFilter filter, LimsUserSession userSession)
        {
            RequestSet response = new RequestSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_REGISTRATION_JOURNAL, false, true, userSession);
            return response.Request;
        }

        public List<Patient> PatientJournal(PatientJournalFilter filter, LimsUserSession userSession)
        {
            var result = new { Patients = new List<Patient>() };
            if (filter.SkipRequestDate)
            {
                filter.RequestDateFrom = null;
                filter.RequestDateTill = null;
            }
            LoadObject(filter, result, XMLConst.XML_METHOD_PATIENT_JOURNAL, false, true, userSession);
            return result.Patients;
        }

        public List<Patient> PatientQuickSearch(PatientQuickSearchRequest filter, LimsUserSession userSession)
        {
            var result = new { Patients = new List<Patient>() };
            LoadObject(filter, result, XMLConst.XML_METHOD_PATIENT_QUICK_SEARCH, false, true, userSession);
            return result.Patients;
        }

        /// <summary>
        /// Возвращает коллекцию проб, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        //public List<BaseSample> WorkJournal(WorkJournalFilter filter)
        //{
        //    SampleSet response = new SampleSet();
        //    LoadObject(filter, response, XMLConst.XML_METHOD_WORK_JOURNAL, false, true);
        //    return response.Samples;
        //}

        public List<BaseSample> WorkJournal(WorkJournalFilter filter, LimsUserSession userSession)
        {
            SampleSet response = new SampleSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_WORK_JOURNAL, false, true, userSession);
            return response.Samples;
        }

        //public List<BaseSample> WorkJournal(Registration

        /// <summary>
        /// Возвращает информацию о пациенте
        /// </summary>
        /// <param name="id">Id пациента</param>
        /// <returns></returns>
        public Patient PatientInfo(int id, LimsUserSession userSession)
        {
            var request = new { Patient = new ObjectRef(id) };
            Patient response = new Patient();
            LoadObject(request, response, XMLConst.XML_METHOD_PATIENT_INFO, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Возвращает информацию по заявке
        /// </summary>
        public RequestsInfoResponse RequestInfo(int id, LimsUserSession userSession)
        {
            var request = new { Request = new ObjectRef(id) };
            RequestsInfoResponse response = new RequestsInfoResponse();
            bool writeObjectTag = false;
            object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
            bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
            if (versionAfter129)
                writeObjectTag = true;

            LoadObject(request, response, XMLConst.XML_METHOD_REQUEST_INFO, false, writeObjectTag, userSession);
            return response;
        }

        public CreateRequest3Request RequestXXXInfo(int id, LimsUserSession userSession)
        {
            var request = new { Request = new ObjectRef(id) };
            CreateRequest3Request response = new CreateRequest3Request();
            bool writeObjectTag = false;
            object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
            bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
            if (versionAfter129)
                writeObjectTag = true;

            LoadObject(request, response, XMLConst.XML_METHOD_REQUEST_INFO, false, writeObjectTag, userSession);
            return response;
        }
        public List<ExternalResult> ResendCloseEvents(List<ObjectRef> requestIds, LimsUserSession userSession)
        {
            GetRequestResultsParams queryParams = new GetRequestResultsParams();
            queryParams.Requests = requestIds;
            GetRequestResultsResponce response = new GetRequestResultsResponce();
            ProgramContext.LisCommunicator.LoadObject(queryParams, response, XMLConst.XML_METHOD_RESEND_CLOSE_EVENTS, false, true, userSession);
            return response.Results;
        }


        class RequestsShortInfoRequestParams
        {
            public RequestsShortInfoRequestParams()
            {
                Requests = new List<ObjectRef>();
            }

            [CSN("AllowOpenRequests")]
            public bool AllowOpenRequests { get; set; }
            [CSN("AllowOpenSamples")]
            public bool AllowOpenSamples { get; set; }
            [CSN("Requests")]
            [Unnamed]
            public List<ObjectRef> Requests { get; set; }
        }

        class RequestsShortInfoResponse
        {
            public RequestsShortInfoResponse()
            {
                Requests = new List<BaseRequest>();
            }

            [Unnamed]
            public List<BaseRequest> Requests { get; set; }
        }

        /// <summary>
        /// Возвращает краткую информацию о заявке по её номеру
        /// </summary>
        /// <param name="nr"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public BaseRequest RequestShortInfo(string nr, LimsUserSession userSession)
        {
            var request = new { Nr = nr };
            var response = new RequestsShortInfoResponse();
            LoadObject(request, response, XMLConst.XML_METHOD_REQUEST_SHORT_INFO, false, true, userSession);
            return response.Requests.Count > 0 ? response.Requests.First() : null;
        }

        /// <summary>
        /// Возвращает краткие данные заявок по их id
        /// </summary>
        /// <param name="requestRefs"></param>
        /// <returns></returns>
        public List<BaseRequest> RequestsShortInfo(List<ObjectRef> requestRefs, LimsUserSession userSession)
        {
            var request = new RequestsShortInfoRequestParams()
            {
                AllowOpenRequests = true,
                AllowOpenSamples = true,
                Requests = requestRefs
            };

            var response = new RequestsShortInfoResponse();
            LoadObject(request, response, XMLConst.XML_METHOD_REQUEST_SHORT_INFO, false, true, userSession);
            return response.Requests;
        }

        public Boolean IsRequestNrUnique(String internalNr, LimsUserSession userSession)
        {
            CheckRequestNrsParamsRequest request = new CheckRequestNrsParamsRequest();
            request.Nrs.Add(new RequestBatchNr() { InternalNr = internalNr });
            CheckRequestNrsParamsResponce response = new CheckRequestNrsParamsResponce();
            LoadObject(request, response, XMLConst.XML_METHOD_CHECK_REQUEST_NRS, false, true, userSession);
            return response.Nrs.Count == 0;
        }


        /// <summary>
        /// Получить свободный внутренний номер для заявки
        /// </summary>
        /// <returns></returns>
        public String GetFreeNr(LimsUserSession userSession)
        {
            CheckRequestXXXSNrsParamsResponce responce = new CheckRequestXXXSNrsParamsResponce();
            LoadObject(new Object(), responce, XMLConst.XML_METHOD_GET_NEXT_REQUEST_NR, false, true, userSession);
            return responce.Nr;
        }

        /// <summary>
        /// Получить диапазон номеров для заявки
        /// </summary>
        /// <param name="Count"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public NrsList GetRequestNrRange(Int32 Count, LimsUserSession userSession)
        {
            NrsList result = new NrsList();
            LoadObject(new { Count = Count }, result, XMLConst.XML_METHOD_GET_REQUEST_NR_RANGE, false, true, userSession);
            return result;
        }

        /// <summary>
        /// Разлочить номера
        /// </summary>
        /// <param name="nrs"></param>
        public void UnlockRequestNrs(NrsList nrs, LimsUserSession userSession)
        {
            LoadObject(nrs, new Object(), XMLConst.XML_METHOD_REQUEST_NR_UNLOCK, false, true, userSession);
            return;
        }

        public void CheckAndReserve(String nr, LimsUserSession userSession)
        {
            LoadObject(new { Nr = nr }, new Object(), XMLConst.XML_METHOD_REQUEST_NR_CHECK_AND_RESERVE, false, true, userSession);
            return;
        }

        /// <summary>
        /// Получить список настроек сервера
        /// </summary>
        /// <returns></returns>
        public ServerOptionList GetOptions(LimsUserSession userSession)
        {
            ServerOptionList userOptions = new ServerOptionList();
            LoadObject(new Object(), userOptions, XMLConst.XML_METHOD_USER_OPTIONS, false, true, userSession);
            Log.WriteText("Server options received");
            return userOptions;
        }

        /// <summary>
        /// Сохраняет пациента. В выходном аргументе patientCardIds возвращает список объектов PatientCardId
        /// </summary>
        public Int32 SavePatient(Patient patient, out List<PatientCardId> patientCardIds, LimsUserSession userSession)
        {
            PatientResponce response = new PatientResponce();
            LoadObject(patient, response, XMLConst.XML_METHOD_PATIENT_SAVE, false, true, userSession);
            patientCardIds = response.PatientCards;
            return response.Id;
        }

        public List<ErrorMessage> OperationArrivalTemplateSave(OperationArrivalTemplate template, LimsUserSession userSession)
        {
            OperationArrivalTemplateSaveResponce response = new OperationArrivalTemplateSaveResponce();
            LoadObject(template, response, XMLConst.XML_METHOD_OPERATION_TEMPLATE_SAVE, false, true, userSession);
            template.Id = response.Id;
            return response.Errors;
        }

        public List<ErrorMessage> OperationArrival(List<ObjectRef> ids, LimsUserSession userSession)
        {
            OperationArrivalResponce response = new OperationArrivalResponce();
            ProgramContext.LisCommunicator.LoadObject(new { operationTemplates = ids }, response, XMLConst.XML_METHOD_OPERATION_ARRIVAL, false, true, userSession);
            return response.Errors;
        }

        public List<OperationJournalRow> OperationJournal(object f, LimsUserSession userSession)
        {
            OperationJournalFilter filter = new OperationJournalFilter();
            // to-do: убрать это, получать фильтр извне
            filter.DateFrom = DateTime.Now;
            filter.DateTill = DateTime.Now;
            filter.OperationTypes.Add(5);

            OperationJournalResponce response = new OperationJournalResponce();
            ProgramContext.LisCommunicator.LoadObject(filter, response, XMLConst.XML_METHOD_OPERATION_JOURNAL, false, true, userSession);
            return response.Operations;
        }

        public SystemOperation SystemOperationInfo(Int32 id, LimsUserSession userSession)
        {
            SystemOperationInfoRequest request = new SystemOperationInfoRequest();
            request.Operation.Id = id;
            SystemOperation response = new SystemOperation();

            LoadObject(request, response, XMLConst.XML_METHOD_SYSTEM_OPERATION_INFO, false, true, userSession);

            return response;
        }

        public Int32 SystemNextMaterialCatalogNr(LimsUserSession userSession)
        {
            SystemNextMaterialCatalogNrResponce response = new SystemNextMaterialCatalogNrResponce();

            LoadObject(new Object(), response, XMLConst.XML_METHOD_SYSTEM_NEXT_MATERIAL_CATALOG_NR, false, true, userSession);

            return response.Nr;
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

        public List<ExternalResult> GetRequestsResults(List<ObjectRef> requestIds, LimsUserSession userSession)
        {
            GetRequestResultsParams queryParams = new GetRequestResultsParams();
            queryParams.Requests = requestIds;
            GetRequestResultsResponce response = new GetRequestResultsResponce();
            LoadObject(queryParams, response, XMLConst.XML_METHOD_GET_REQUESTS_RESULTS_2, false, true, userSession);
            return response.Results;
        }

        public BaseSample GetSampleResults(BaseSample sample, LimsUserSession userSession)
        {
            var Id = new { sample = new ObjectRef(sample.Id) };
            LoadObject(Id, sample, XMLConst.XML_METHOD_REQUEST_WORKS, false, true, userSession);
            return sample;
        }

        public BaseSample GetSample(Int32 sampleId, LimsUserSession userSession)
        {
            BaseSample sample = new BaseSample();
            var Id = new { sample = new ObjectRef(sampleId) };
            LoadObject(Id, sample, XMLConst.XML_METHOD_SAMPLE_INFO, false, true, userSession);
            return sample;
        }

        //public BaseSample GetSample(Int32 sampleId, LimsUserSession userSession)
        //{
        //    BaseSample sample = new BaseSample();
        //    var Id = new { sample = new ObjectRef(sampleId) };
        //    LoadObject(Id, sample, XMLConst.XML_METHOD_SAMPLE_INFO, false, true, userSession);
        //    return sample;
        //}

        public Boolean SaveSample(BaseSample sample, LimsUserSession userSession)
        {
            LoadObject(sample, null, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true, userSession);
            return true;
        }

        //public Boolean SaveSample(BaseSample sample, LimsUserSession userSession)
        //{
        //    Object Responce = new Object();
        //    LoadObject(sample, null, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true, userSession);
        //    return true;
        //}

        //public Boolean SaveSample(Object sample, LimsUserSession userSession)
        //{
        //    Object Responce = new Object();
        //    LoadObject(sample, null, XMLConst.XML_METHOD_SAVE_SAMPLE, true, true, userSession);
        //    return true;
        //}

        public Boolean SaveSample(Object sample, LimsUserSession userSession)
        {
            LoadObject(sample, null, XMLConst.XML_METHOD_SAVE_SAMPLE, false, true, userSession);
            return true;
        }

        /// <summary>
        /// Возвращает пробы по указанному идентификатору заявки
        /// </summary>
        //public List<BaseSample> RequestSamples(Int32 requestId)
        //{
        //    RequestSamplesParams queryParams = new RequestSamplesParams();
        //    queryParams.Request.Id = requestId;
        //    RequestSamplesResponce response = new RequestSamplesResponce();
        //    LoadObject(queryParams, response, XMLConst.XML_METHOD_REQUESTS_SAMPLES, false, false);
        //    return response.Samples;
        //}

        public List<BaseSample> RequestSamples(Int32 requestId, LimsUserSession userSession)
        {
            RequestSamplesParams queryParams = new RequestSamplesParams();
            queryParams.Request.Id = requestId;
            RequestSamplesResponce response = new RequestSamplesResponce();
            bool writeObjectTag = false;
            object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
            bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
            if (versionAfter129)
                writeObjectTag = true;
            LoadObject(queryParams, response, XMLConst.XML_METHOD_REQUESTS_SAMPLES, false, writeObjectTag, userSession);
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
        //public List<EquipmentResponseMessage> EquipmentData(EquipmentData data, LimsUserSession userSession)
        //{
        //    EquipmentDataResponse response = new EquipmentDataResponse();
        //    try
        //    {
        //        LoadObject(data, response, XMLConst.XML_METHOD_EQUIPMENT_DATA, false, true, userSession);
        //        return response.Messages;
        //    }
        //    catch (NlsServerException ex)
        //    {
        //        var messages = new List<EquipmentResponseMessage>();
        //        messages.Add(new EquipmentResponseMessage { Severity = LisErrorMessageTypes.ERROR, Message = ex.ErrorMessage });
        //        return messages;
        //    }
        //}

        public List<EquipmentResponseMessage> EquipmentData(EquipmentData data, LimsUserSession userSession)
        {
            bool writeObjectTag = false;
            try
            {
                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                writeObjectTag = versionAfter129;
            }
            catch { }

            bool useOldEquipmentDataStructure = false;
            try
            {
                object useOldEquipmentDataStructureObj = ProgramContext.Settings["useOldEquipmentDataStructure", false];
                useOldEquipmentDataStructure = useOldEquipmentDataStructureObj != null ? Boolean.Parse(useOldEquipmentDataStructureObj.ToString()) : false;
            }
            catch { }

            if (useOldEquipmentDataStructure)
                clearFileInfos(data);

            EquipmentDataResponse response = new EquipmentDataResponse();
            try
            {
                LoadObject(data, response, XMLConst.XML_METHOD_EQUIPMENT_DATA, false, writeObjectTag, userSession);
                return response.Messages;
            }
            catch (NlsServerException ex)
            {
                var messages = new List<EquipmentResponseMessage>();
                messages.Add(new EquipmentResponseMessage { Severity = (int)LisErrorMessageTypes.ERROR, Message = ex.ErrorMessage });
                return messages;
            }
        }

        private void clearFileInfos(EquipmentData data)
        {
            foreach (EquipmentDataPatient patient in data.Patients)
                foreach (EquipmentOrder order in patient.Orders)
                {
                    order.FileInfos = null;
                    order.Comments = null;
                    foreach (EquipmentResult result in order.Results)
                        result.FileInfos = null;
                }
        }

        /// <summary>
        /// Запрашивает у сервера задание указанному анализатору для указанных проб(образцов)
        /// </summary>
        //public WorkListQueryResponse TestsForSamples(WorkListQuery query)
        //{
        //    WorkListQueryResponse response = new WorkListQueryResponse();
        //    LoadObject(query, response, XMLConst.XML_METHOD_TESTS_FOR_SAMPLES, false, true);
        //    return response;
        //}

        public WorkListQueryResponse TestsForSamples2(TestsForSamples2Query query)
        {
            WorkListQueryResponse response = new WorkListQueryResponse();
            LoadObject(query, response, XMLConst.XML_METHOD_TESTS_FOR_SAMPLES_2, false, true);
            return response;
        }

        public WorkListQueryResponse TestsForSamples(WorkListQuery query, LimsUserSession userSession)
        {
            WorkListQueryResponse response = new WorkListQueryResponse();
            LoadObject(query, response, XMLConst.XML_METHOD_TESTS_FOR_SAMPLES, false, true, userSession);
            return response;
        }


        private class WorkListJournalResponce
        {
            public WorkListJournalResponce()
            {
                Worklists = new List<WorkListShortInfo>();
            }

            [CSN("Worklists")]
            public List<WorkListShortInfo> Worklists { get; set; }
        }

        /// <summary>
        /// Возвращает коллекцию кратких сведений о рабочих списках, удовлетворяющих фильтру
        /// </summary>
        //public List<WorkListShortInfo> WorklistJournal(WorklistJournalFilter filter)
        //{
        //    WorkListJournalResponce response = new WorkListJournalResponce();
        //    LoadObject(filter, response, XMLConst.XML_METHOD_WORKLIST_JOURNAL, false, true);
        //    return response.Worklists;
        //}

        public List<WorkListShortInfo> WorklistJournal(WorklistJournalFilter filter, LimsUserSession userSession)
        {
            bool writeObjectTag = true;
            /*try
            {
                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                writeObjectTag = versionAfter129;
            }
            catch { }*/

            WorkListJournalResponce response = new WorkListJournalResponce();
            LoadObject(filter, response, XMLConst.XML_METHOD_WORKLIST_JOURNAL, false, writeObjectTag, userSession);
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
        //public List<WorkList> WorklistInfos(RefSet worklistIds)
        //{
        //    var request = new { Worklists = worklistIds };
        //    var response = new WorklistInfosResponce();

        //    LoadObject(request, response, XMLConst.XML_METHOD_WORKLIST_INFOS, false, true);
        //    return response.Worklists;
        //}

        public List<WorkList> WorklistInfos(RefSet worklistIds, LimsUserSession userSession)
        {
            bool writeObjectTag = false;
            try
            {
                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                writeObjectTag = versionAfter129;
            }
            catch { }

            var request = new { Worklists = worklistIds };
            var response = new WorklistInfosResponce();

            LoadObject(request, response, XMLConst.XML_METHOD_WORKLIST_INFOS, false, writeObjectTag, userSession);
            return response.Worklists;
        }


        /// <summary>
        /// Изменяет "статус отправки в удалённый анализатор" рабочего списка
        /// </summary>        
        //public void WorklistChangeSendRemoteState(WorkList worklist, Int32 newState)
        //{
        //    WorklistDelta request = new WorklistDelta()
        //    {
        //        Id = worklist.Id,
        //        WorklistDef = new ObjectRef(worklist.WorklistDef.Id),
        //        Rack = new ObjectRef(worklist.Rack.Id),
        //        Code = worklist.Code,
        //        ExpireDate = worklist.ExpireDate,
        //        SendRemote = newState
        //    };

        //    LoadObject(request, null, XMLConst.XML_METHOD_WORKLIST_SAVE, true, true);
        //}

        public void WorklistChangeSendRemoteState(WorkList worklist, Int32 newState, LimsUserSession userSession)
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

            LoadObject(request, null, XMLConst.XML_METHOD_WORKLIST_SAVE, true, true, userSession);
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
        //public List<OutsourceRequest> OutsourceRequestJournal(OutsourceRequestJournalFilter filter)
        //{
        //    OutsourceRequestJournalResponce response = new OutsourceRequestJournalResponce();
        //    LoadObject(filter, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_JOURNAL, false, true);
        //    return response.Requests;
        //}

        public List<OutsourceRequest> OutsourceRequestJournal(OutsourceRequestJournalFilter filter, LimsUserSession userSession)
        {
            OutsourceRequestJournalResponce response = new OutsourceRequestJournalResponce();
            LoadObject(filter, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_JOURNAL, false, true, userSession);
            return response.Requests;
        }

        /// <summary>
        /// Возвращает "заявку во внешнюю лабораторию"
        /// </summary>
        //public OutsourceRequest OutsourceRequestInfo(ObjectRef outsourceRequestRef)
        //{
        //    OutsourceRequest response = new OutsourceRequest();
        //    LoadObject(new { OutsourceRequest = outsourceRequestRef }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_INFO, false, true);
        //    return response;
        //}

        public OutsourceRequest OutsourceRequestInfo(ObjectRef outsourceRequestRef, LimsUserSession userSession)
        {
            OutsourceRequest response = new OutsourceRequest();
            LoadObject(new { OutsourceRequest = outsourceRequestRef }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_INFO, false, true, userSession);
            return response;
        }

        public OutsourceRequest OursourceRequestInfo(String RequestCode, LimsUserSession userSession)
        {
            OutsourceRequest response = new OutsourceRequest();
            LoadObject(new { RequestCode = RequestCode }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_INFO, false, true, userSession);
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
        //public List<ErrorMessage> OutsourceRequestSaveResults(OutsourceRequest outsourceRequest)
        //{
        //    var response = new OutsourceRequestSaveResultsResponce();
        //    LoadObject(new { Request = outsourceRequest }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_SAVE_RESULTS, false, true);
        //    return response.Errors;
        //}

        public List<ErrorMessage> OutsourceRequestSaveResults(OutsourceRequest outsourceRequest, LimsUserSession userSession)
        {
            var response = new OutsourceRequestSaveResultsResponce();
            LoadObject(new { Request = outsourceRequest }, response, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_SAVE_RESULTS, false, true, userSession);
            return response.Errors;
        }

        /// <summary>
        /// Изменяет состояние "заявки во внешнюю лабораторию"
        /// </summary>
        //public void OutsourceRequestChangeState(OutsourceRequest outsourceRequest, int newState)
        //{
        //    var request = new { OutsourceRequests = new RefSet() { new ObjectRef(outsourceRequest.Id)}, NewState = newState };
        //    LoadObject(request, null, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_CHANGE_STATE, false, true);
        //}

        public void OutsourceRequestChangeState(OutsourceRequest outsourceRequest, int newState, LimsUserSession userSession)
        {
            var request = new { OutsourceRequests = new RefSet() { new ObjectRef(outsourceRequest.Id) }, NewState = newState };
            LoadObject(request, null, XMLConst.XML_METHOD_OUTSOURCE_REQUEST_CHANGE_STATE, false, true, userSession);
        }

        /// <summary>
        /// 
        /// </summary>
        //public void GetServicePrices(Int32 timestamp)
        //{
        //    ServicePriceRequest request = new ServicePriceRequest(timestamp);
        //    ServicePrice prices = new ServicePrice();
        //    LoadObject(request, prices, XMLConst.XML_METHOD_LOAD_SERVICE_PRICE, false, true /*false*/);

        //    HospitalDictionary hospitals = (HospitalDictionary)ProgramContext.Dictionaries.GetDictionary(LisDictionaryNames.Hospital);


        //    if (hospitals != null)
        //    {
        //        hospitals.UpdatePrices(prices);
        //    }
        //}

        public void GetServicePrices(Int32 timestamp, LimsUserSession userSession)
        {
            ServicePriceRequest request = new ServicePriceRequest(timestamp);
            ServicePrice prices = new ServicePrice();
            LoadObject(request, prices, XMLConst.XML_METHOD_LOAD_SERVICE_PRICE, false, true, userSession);

            HospitalDictionary hospitals = (HospitalDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.Hospital);


            if (hospitals != null)
            {
                hospitals.UpdatePrices(prices);
            }
            PricelistDictionary pricelists = (PricelistDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.PricelistShort);
            foreach (var price in prices.Elements)
            {
                PricelistDictionaryItem pricelist = pricelists.Elements.FirstOrDefault(x => x.Id == price.Pricelist.Id);
                if (pricelist != null)
                    pricelist.Prices.Add(new PriceItem() { Price = (float)price.Price, Service = new ObjectRef(price.Service.Id) });
            }
        }

        public void GetOrderForm(RequestFormDictionaryItem requestForm, LimsUserSession userSession)
        {
            if (requestForm._OrderFormLayout != null)
                return;
            requestForm._OrderFormLayout = GetOrderForm(requestForm.OrderForm, userSession);

        }

        public OrderFormLayout GetOrderForm(Int32 id, LimsUserSession userSession)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<OrderFormLayout>(id, Encoding.UTF8, userSession);
            }
            return null;
        }


        //public OrderForm GetOrderForm(Int32 id)
        //{
        //    if (id > 0)
        //    {
        //        return GetObjectFromFileServer<OrderForm>(id, Encoding.UTF8);
        //    }
        //    return null;
        //}


        public void GetControlsFormLayout(RequestFormDictionaryItem requestForm, LimsUserSession userSession)
        {
            if (requestForm._ControlsFormLayout != null)
                return;
            requestForm._ControlsFormLayout = GetControlsFormLayout(requestForm.ControlsFormLayout, userSession);
        }


        public ControlsFormLayout GetControlsFormLayout(Int32 id, LimsUserSession userSession)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<ControlsFormLayout>(id, Encoding.UTF8, userSession);
            }
            return null;
        }

        public void GetWorkingFormsLayout(RequestFormDictionaryItem requestForm, LimsUserSession userSession)
        {
            if (requestForm._WorkingFormsLayout != null)
                return;
            requestForm._WorkingFormsLayout = GetWorkingFormsLayout(requestForm.ControlsFormLayout, userSession);
        }

        public WorkingFormsLayout GetWorkingFormsLayout(Int32 id, LimsUserSession userSession)
        {
            if (id > 0)
            {
                return GetObjectFromFileServer<WorkingFormsLayout>(id, Encoding.UTF8, userSession);
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
            public List<Team> Teams { get; set; }
        }

        /// <summary>
        /// Возвращает коллекцию бригад (смен) сотрудников, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        //public List<Team> TeamJournal(TeamJournalFilter filter)
        //{
        //    var response = new TeamSet();
        //    LoadObject(filter, response, XMLConst.XML_METHOD_TEAM_JOURNAL, false, true);
        //    return response.Teams;
        //}

        public List<Team> TeamJournal(TeamJournalFilter filter, LimsUserSession userSession)
        {
            var response = new TeamSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_TEAM_JOURNAL, false, true, userSession);
            return response.Teams;
        }

        /// <summary>
        /// Сохраняет бригаду (смену) сотрудников
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        //public int SaveTeam(Team team)
        //{
        //    var response = new ObjectRef();
        //    LoadObject(team, response, XMLConst.XML_METHOD_TEAM_SAVE, false, true);
        //    return response.Id;
        //}

        public int SaveTeam(Team team, LimsUserSession userSession)
        {
            var response = new ObjectRef();
            LoadObject(team, response, XMLConst.XML_METHOD_TEAM_SAVE, false, true, userSession);
            return response.Id;
        }

        /// <summary>
        /// Получает информацию о бригаде (смене) сотрудников
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public Team GetTeam(int teamId, LimsUserSession userSession)
        {
            var response = new Team();
            LoadObject(new { Id = new ObjectRef(teamId) }, response, XMLConst.XML_METHOD_TEAM_GET, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Возвращает коллекцию объектов, содержащих краткую информацию о штативах архива
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ArchiveRackHeader> ArchiveJournal(ArchiveJournalFilter filter, LimsUserSession userSession)
        {
            var response = new { Results = new List<ArchiveRackHeader>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_ARCHIVE_RACK_JOURNAL, false, true, userSession);
            return response.Results;
        }

        /// <summary>
        /// Создаёт штатив в архиве биоматериалов
        /// </summary>
        public int ArchiveRackCreate(ArchiveRackHeader rackHeader, LimsUserSession userSession)
        {
            var request = new ArchiveRackCreateRequestParams();
            request.CopyPropertiesFromObject(rackHeader);
            //var rackHeader = new ArchiveRackHeader().CopyFromSubClassObject(rack);
            var response = new BaseObject();
            LoadObject(request, response, XMLConst.XML_METHOD_ARCHIVE_RACK_CREATE, false, true, userSession);
            return response.Id;
        }

        /// <summary>
        /// Сохраняет штатив в архиве биоматериалов
        /// </summary>
        /// <param name="RackHeader"></param>
        /// <returns></returns>
        public int ArchiveRackSave(ArchiveRackHeader rackHeader, LimsUserSession userSession)
        {
            var request = new ArchiveRackSaveRequestParams();//().CopyFromSubClassObject(rackHeader);
            request.CopyPropertiesFromObject(rackHeader);
            var response = new BaseObject();
            LoadObject(request, response, XMLConst.XML_METHOD_ARCHIVE_RACK_SAVE, false, true, userSession);
            return response.Id;
        }

        /// <summary>
        /// Возвращает штатив архива биоматериалов
        /// </summary>
        /// <param name="racktId"></param>
        /// <returns></returns>
        public ArchiveRack ArchiveRackGet(int racktId, LimsUserSession userSession)
        {
            var response = new ArchiveRack();
            LoadObject(new { Id = racktId }, response, XMLConst.XML_METHOD_ARCHIVE_RACK_GET, false, true, userSession);
            response.Slots.ForEach(slot => slot.Rack = response);
            return response;
        }

        /// <summary>
        /// Помещает пробирки в штатив архива
        /// </summary>
        /// <param name="rack"></param>
        /// <param name="slotsToAdd"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<ArchiveRackSlot> ArchiveRackPutTubes(ArchiveRack rack, List<ArchiveRackSlot> slotsToAdd, LimsUserSession userSession)
        {
            var request = new
            {
                ArchiveRack = new ObjectRef(rack.Id),
                Tubes =
                (from slot in slotsToAdd
                 select new { X = slot.X, Y = slot.Y, Request = slot.Request, TubeNr = slot.TubeNr, Sample = slot.Sample }).ToList()
            };
            var response = new { Slots = new List<ArchiveRackSlot>() };

            LoadObject(request, response, XMLConst.XML_METHOD_ARCHIVE_RACK_PUT_TUBES, false, true, userSession);
            return response.Slots;
        }

        /// <summary>
        /// Помещает одну пробирку в штатив архива
        /// </summary>
        /// <param name="rack"></param>
        /// <param name="slot"></param>
        /// <param name="userSession"></param>
        public ArchiveRackSlot ArchiveRackPutSingleTube(ArchiveRack rack, ArchiveRackSlot slot, LimsUserSession userSession)
        {
            List<ArchiveRackSlot> savedSlots = ArchiveRackPutTubes(rack, new List<ArchiveRackSlot>() { slot }, userSession);
            return savedSlots.First();
        }

        /// <summary>
        /// Извлекает пробирки из штатива
        /// </summary>
        /// <param name="slots"></param>
        public void ArchiveRackExtractTubes(ArchiveRack rack, List<ArchiveRackSlot> slots, LimsUserSession userSession)
        {
            var query =
                from slot in slots
                select new ObjectRef(slot.Id);

            var slotRefs = query.ToList();
            var request = new
            {
                ArchiveRack = new ObjectRef(rack.Id),
                Slots = slotRefs
            };

            LoadObject(request, null, XMLConst.XML_METHOD_ARCHIVE_RACK_EXTRACT_TUBES, false, true, userSession);
        }

        /// <summary>
        /// Очищает штатив архива биоматериалов
        /// </summary>
        /// <param name="rackId"></param>
        public void ArchiveRackClear(List<ObjectRef> racks, LimsUserSession userSession)
        {
            var request = new { Racks = racks };
            LoadObject(request, null, XMLConst.XML_METHOD_ARCHIVE_RACK_CLEAR, false, true, userSession);
        }

        /// <summary>
        /// Возвращает коллекцию объектов типа LogItem
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<LogItem> LoggingJournal(LogJournalFilter filter, LimsUserSession userSession)
        {
            var response = new { Logs = new List<LogItem>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_LOGGING_JOURNAL, false, true, userSession);
            return response.Logs;
        }

        /// <summary>
        /// Сохраняет информацию о квоте
        /// </summary>
        public int QuotaCreate(Quota quota, LimsUserSession userSession)
        {
            var response = new ObjectRef();
            LoadObject(quota, response, XMLConst.XML_METHOD_QUOTA_CREATE, false, true, userSession);
            return response.Id;
        }

        /// <summary>
        /// Получает информацию о квоте
        /// </summary>
        public Quota QuotaGet(int quotaId, LimsUserSession userSession)
        {
            var request = new { Id = quotaId };
            var response = new Quota();
            LoadObject(request, response, XMLConst.XML_METHOD_QUOTA_GET, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Возвращает коллекцию объектов типа Quota
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Quota> QuotaJournal(QuotaJournalFilter filter, LimsUserSession userSession)
        {
            var response = new { Quotas = new List<Quota>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_QUOTA_JOURNAL, false, true, userSession);
            return response.Quotas;
        }

        /// <summary>
        /// Активирует/деактивирует квоты
        /// </summary>
        public List<ErrorMessage> QuotaChangeActiveState(List<ObjectRef> quotas, bool state, LimsUserSession userSession)
        {
            var request = new { Quotas = quotas, Active = state };
            var response = new { Errors = new List<ErrorMessage>() };
            LoadObject(request, response, XMLConst.XML_METHOD_QUOTA_CHANGE_ACTIVE_STATE, false, true, userSession);
            return response.Errors;
        }

        /// <summary>
        /// Удаляет квоты
        /// </summary>
        public List<ErrorMessage> QuotaRemove(List<ObjectRef> quotas, LimsUserSession userSession)
        {
            var request = new { Quotas = quotas };
            var response = new { Errors = new List<ErrorMessage>() };
            LoadObject(request, response, XMLConst.XML_METHOD_QUOTA_REMOVE, false, true, userSession);
            return response.Errors;
        }

        /// <summary>
        /// Ищет пробы по номеру
        /// </summary>
        public RequestSamplesInfo FindSamplesByNr(String nr, Boolean needRequest, Boolean needSamples, LimsUserSession userSession)
        {
            var request = new { Nr = nr, NeedRequest = needRequest, NeedSamples = needSamples };
            var response = new RequestSamplesInfo();
            LoadObject(request, response, XMLConst.XML_METHOD_FIND_SAMPLES_BY_NR, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Запрашивает информацию о пробах
        /// </summary>
        public SampleShortInfoSet GetSamplesShortInfo(List<ObjectRef> ids, LimsUserSession userSession)
        {
            var request = new { Ids = ids };
            var response = new SampleShortInfoSet();
            LoadObject(request, response, XMLConst.XML_METHOD_GET_SAMPLES_SHORT_INFO, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Удаляет штативы архива биоматериалов
        /// </summary>
        /// <param name="rackId"></param>
        public List<ErrorMessage> ArchiveRackDelete(List<ObjectRef> racks, LimsUserSession userSession)
        {
            var request = new { Racks = racks };
            var response = new { Errors = new List<ErrorMessage>() };
            LoadObject(request, response, XMLConst.XML_METHOD_ARCHIVE_RACK_DELETE, false, true, userSession);
            return response.Errors;
        }

        /// <summary>
        /// Изменяет место хранения для указанных штативов
        /// </summary>
        /// <param name="racks"></param>
        /// <param name="archiveStorageId"></param>
        public void ArchiveRackChangeStorage(List<ObjectRef> racks, int archiveStorageId, LimsUserSession userSession)
        {
            var request = new { Racks = racks, ArchiveStorage = new ObjectRef(archiveStorageId) };
            LoadObject(request, null, XMLConst.XML_METHOD_ARCHIVE_RACK_CHANGE_STORAGE, false, true, userSession);
        }

        /// <summary>
        /// Ищет штативы архива по штрих-коду хранилища/штатива/пробы
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public List<ArchiveRackHeader> ArchiveRackFind(string barcode, LimsUserSession userSession)
        {
            var request = new { Barcode = barcode };
            var response = new { Results = new List<ArchiveRackHeader>() };
            LoadObject(request, response, XMLConst.XML_METHOD_ARCHIVE_RACK_FIND, false, true, userSession);
            return response.Results;
        }
        /// <summary>
        /// Ищет завяки по сканнированному штрихкоду
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public object ProcessBarcode(string barcode, LimsUserSession userSession)
        {
            var request = new { barcode = barcode };
            ProcessBarcodeResult response = new ProcessBarcodeResult();
            LoadObject(request, response, XMLConst.XML_METHOD_PROCESS_BARCODE, false, true, userSession);
            return response;
        }

        public PatientHistoryResponse GetPatientHistory(int patient, LimsUserSession userSession)
        {
            PatientHistoryRequest request = new PatientHistoryRequest() { Patient = new ObjectRef(patient) };
            PatientHistoryResponse response = new PatientHistoryResponse();
            LoadObject(request, response, XMLConst.XML_METHOD_PATIENT_HISTORY, false, true, userSession);
            return response;
        }

        private class ProcessBarcodeResult
        {
            public ProcessBarcodeResult()
            {
                Errors = new List<ErrorMessage>();
            }
            [Unnamed]
            public List<ErrorMessage> Errors { get; set; }
        }

        private class WaybillJournalResult
        {
            public WaybillJournalResult()
            {
                WaybillList = new List<Waybill>();
            }

            [Unnamed]
            public List<Waybill> WaybillList { get; set; }
        }

        /// <summary>
        /// Возвращает коллекцию инвентарных ведомостей, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<Waybill> WaybillJournal(WaybillJournalFilter filter, LimsUserSession userSession)
        {
            var response = new WaybillJournalResult();
            LoadObject(filter, response, XMLConst.XML_METHOD_WAYBILL_JOURNAL, false, true, userSession);
            return response.WaybillList;
        }

        private class WaybillGetNextNrResponse
        {
            public WaybillGetNextNrResponse()
            {
                Nr = String.Empty;
            }

            [CSN("Nr")]
            public String Nr { get; set; }
        }

        /// <summary>
        /// Запрашивает новый номер инвентарной ведомости, генерируемый по шаблону
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public String WaybillGetNextNr(LimsUserSession userSession)
        {
            var response = new WaybillGetNextNrResponse();
            LoadObject(new object(), response, XMLConst.XML_METHOD_WAYBILL_GET_NEXT_NR, false, true, userSession);
            return response.Nr;
        }

        /// <summary>
        /// Сохраняет инвентарную ведомость
        /// </summary>
        /// <param name="waybill"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public int WaybillSave(Waybill waybill, LimsUserSession userSession)
        {
            var request = waybill;
            var response = new BaseObject();
            LoadObject(request, response, XMLConst.XML_METHOD_WAYBILL_SAVE, false, true, userSession);
            return response.Id;
        }

        /// <summary>
        /// Получает инвентарную ведомость
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Waybill WaybillGet(int id, LimsUserSession userSession)
        {
            var request = new { Id = id };
            var response = new Waybill();
            LoadObject(request, response, XMLConst.XML_METHOD_WAYBILL_GET, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Помечает указанные инвентарные ведомости как удалённые
        /// </summary>
        /// <param name="refs"></param>
        /// <param name="userSession"></param>
        public void WaybillRemove(List<ObjectRef> refs, LimsUserSession userSession)
        {
            var request = new { Ids = refs };
            LoadObject(request, null, XMLConst.XML_METHOD_WAYBILL_REMOVE, false, true, userSession);
        }

        /// <summary>
        /// Ищет слоты в штативе, для которых справедливы условия поиска
        /// </summary>
        /// <param name="archiveRack"></param>
        /// <param name="tests"></param>
        /// <param name="workStates"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public HashSet<int> ArchiveRackSlotsFind(ArchiveRack archiveRack, List<ObjectRef> tests, List<int> workStates, LimsUserSession userSession)
        {
            // Слот связан с заявкой, заявка содержит пробы, пробы содержат работы.
            // Слот удовлетворяет условиям, если в соответствущем слоту списке работ(works) присутствует хотя бы одна работа с TestId, содержащимся в списке tests
            // и при этом работа имеет статус State, содержащийся в списке workStates
            // Например, интересуют слоты в которых содержатся пробирки с невыполненным(Статус = НОВАЯ) тестом на "ВИЧ" (соотв. TestId)
            var request = new
            {
                ArchiveRack = new ObjectRef(archiveRack.Id),
                Tests = tests,
                WorkStates = workStates
            };
            var response = new { Slots = new List<int>() };
            LoadObject(request, response, XMLConst.XML_METHOD_ARCHIVE_RACK_SLOTS_FIND, false, true, userSession);
            return new HashSet<int>(response.Slots);
        }


        public void RequestPayment(Int32 RequestId, LimsUserSession userSession)
        {
            var request = new { Request = new ObjectRef(RequestId) };
            LoadObject(request, null, XMLConst.XML_METHOD_REQUEST_PAYMENT, false, true, userSession);
        }

        public void RequestPaymentCancel(Int32 RequestId, LimsUserSession userSession)
        {
            var request = new { Request = new ObjectRef(RequestId) };
            LoadObject(request, null, XMLConst.XML_METHOD_REQUEST_PAYMENT_CANCEL, false, true, userSession);
        }

        private class PatientsByCodeResult
        {
            public PatientsByCodeResult()
            {
                Patients = new List<Patient>();
            }

            [Unnamed]
            public List<Patient> Patients { get; set; }
        }

        public List<Patient> PatientsByCode(string code, LimsUserSession userSession)
        {
            var request = new { PatientCode = code };
            var response = new PatientsByCodeResult();
            LoadObject(request, response, XMLConst.XML_METHOD_PATIENTS_BY_CODE, false, true, userSession);
            return response.Patients;
        }

        public List<Patient> PatientsByCodePatientNr(string patientNr, LimsUserSession userSession)
        {
            var request = new { PatientNr = patientNr };
            var response = new PatientsByCodeResult();
            LoadObject(request, response, XMLConst.XML_METHOD_PATIENTS_BY_CODE, false, true, userSession);
            return response.Patients;
        }

        public int PatientSave(Patient patient, out List<PatientCardId> patientCardIds, LimsUserSession userSession)
        {
            PatientResponce response = new PatientResponce();
            patient.FirstName = !String.IsNullOrEmpty(patient.FirstName) ? patient.FirstName.ToUpper() : String.Empty;
            patient.MiddleName = !String.IsNullOrEmpty(patient.MiddleName) ? patient.MiddleName.ToUpper() : String.Empty;
            patient.LastName = !String.IsNullOrEmpty(patient.LastName) ? patient.LastName.ToUpper() : String.Empty;
            LoadObject(patient, response, XMLConst.XML_METHOD_PATIENT_SAVE, false, true, userSession);
            //LoadObject(patient, response, XMLConst.XML_METHOD_PATIENT_SAVE, false, true, ref errorCode, ref errorMessage);
            patientCardIds = response.PatientCards;
            return response.Id;
        }

        public TargetMap TargetMapGet(TargetMap targetMap, LimsUserSession userSession)
        {
            TargetMap response = new TargetMap();
            LoadObject(new { targetMap = targetMap }, response, XMLConst.XML_METHOD_TARGET_MAP_GET, false, true, userSession);
            return response;
        }

        public Int32 TargetMapVersionGet(LimsUserSession userSession)
        {
            TargetMapVersionResponse response = new TargetMapVersionResponse();
            LoadObject(new object(), response, XMLConst.XML_METHOD_TARGET_MAP_VERSION, false, true, userSession);
            return response.Id;
        }


        public SexDictionaryItem getSexById(int id)
        {
            return (SexDictionaryItem)dictionaryCache.GetDictionaryItem(LimsDictionaryNames.Sex, id);
        }

        public void LoginOffice(OfficeDictionaryItem office, LimsUserSession userSession)
        {
            var request = new { office = office };
            LoadObject(request, null, XMLConst.XML_METHOD_LOGIN_OFFICE, true, true, userSession);
        }

        /// <summary>
        /// LIS-6255 Журнал аудита
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Список объектов, соотв. ревизиям</returns>
        public List<AuditJournalItem> AuditJournal(AuditJournalFilter filter, LimsUserSession userSession)
        {
            AuditJournalSet response = new AuditJournalSet();
            LoadObject(filter, response, XMLConst.XML_METHOD_AUDIT_JOURNAL, false, true, userSession);
            return response.Revisions;
        }

        /// <summary>
        /// LIS-6256
        /// </summary>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public AuditResults getAuditObject(String name, int id, int rev)
        {
            AuditResults response = new AuditResults();
            var request = new { ObjectType = name, Revision = rev, ObjectId = id };
            LoadObject(request, response, "audit-object", false, true, null);
            return response;
        }
        /// <summary>
        /// <summary>
        /// LIS-7145
        /// Возвращает множество автоматических рабочих списков
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<AutoWorklist> FindAutoWorklists(FindAutoWorklistsFilter filter, LimsUserSession userSession)
        {
            var response = new { Autoworklists = new List<AutoWorklist>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_FIND_AUTO_WORKLISTS, false, true, userSession);
            return response.Autoworklists;
        }
        /// <summary>
        /// <summary>
        /// DRV-426
        /// Возвращает множество микробиологических автоматических рабочих списков
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<MicrobyologyAutoWorklist> FindMicrobyologyAutoWorklists(FindAutoWorklistsFilter filter, LimsUserSession userSession)
        {
            var response = new { microbyologyAutoworklists = new List<MicrobyologyAutoWorklist>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_FIND_MICROBYOLOGY_AUTO_WORKLISTS, false, true, userSession);
            return response.microbyologyAutoworklists;
        }
        /// <summary>
        /// LIS-7152
        /// Изменяет статус указанным автоматическим рабочим спискам
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="newState"></param>
        /// <param name="userSession"></param>
        public void AutoWorklistsChangeState(List<ObjectRef> ids, AutoWorklistStates newState, LimsUserSession userSession)
        {
            var request = new { Ids = ids, NewState = (int)newState };
            LoadObject(request, null, XMLConst.XML_METHOD_AUTO_WORKLISTS_CHANGE_STATE, false, true, userSession);
        }
        /// <summary>
        /// DRV-428
        /// Изменяет статус указанным микробиологическим автоматическим рабочим спискам
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="newState"></param>
        /// <param name="userSession"></param>
        public void MicrobyologyAutoWorklistsChangeState(List<ObjectRef> ids, AutoWorklistStates newState, LimsUserSession userSession)
        {
            var request = new { Ids = ids, NewState = (int)newState };
            LoadObject(request, null, XMLConst.XML_METHOD_MICROBYOLOGY_AUTO_WORKLISTS_CHANGE_STATE, false, true, userSession);
        }
        /// <summary>
        /// LIS-7236
        /// Загружает стоимость исследований
        /// </summary>
        /// <param name="payCategory"></param>
        /// <param name="hospital"></param>
        /// <param name="cito"></param>
        /// <param name="targets"></param>
        /// <param name="calcDate"></param>
        /// <returns></returns>
        public PriceSet GetPrices(ObjectRef payCategory, ObjectRef hospital, bool cito, List<ObjectRef> targets, DateTime calcDate)
        {
            PriceSet result = new PriceSet();
            var request = new { payCategory, hospital, cito, targets, calcDate };
            LoadObject(request, result, XMLConst.XML_METHOD_GET_PRICES, false, true);
            return result;
        }

        public Statistics GetStatistics(DateTime dateFrom, DateTime dateTill, LimsUserSession userSession)
        {
            Statistics result = new Statistics();
            var request = new { dateFrom, dateTill };
            LoadObject(request, result, XMLConst.XML_METHOD_GET_STATISTICS, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-7259
        /// Загрузка неисчерпанных и непросроченных квот с сервера
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public List<QuotaInfo> FindQuotasForHospital(ObjectRef hospital, LimsUserSession userSession)
        {
            QuotaInfoList result = new QuotaInfoList();
            var request = new { hospital = hospital };
            LoadObject(request, result, XMLConst.XML_METHOD_FIND_QUOTAS_FOR_HOSPITAL, false, true, userSession);
            return result.Quotas;
        }
        /// <summary>
        /// LIS-7500
        /// Получение результатов по заявке с временным паролем. Для идентификации пациента используется или Email, или набор Фамилия+Дата рождения
        /// </summary>
        /// <param name="email">E-mail пациента</param>
        /// <param name="tempPassword">Временный пароль выданный при регистрации заявки</param>
        /// <param name="nr">Номер заявки</param>
        /// <param name="lastName">Фамилия пациента</param>
        /// <param name="birthdate">Дата рождения пациента</param>
        public GetPatientLkRequestResponse GetPatientLkRequest(string email, string tempPassword, string nr, string lastName, string birthdate, int patientId)
        {
            var request = new { email, tempPassword, nr, lastName, birthdate, patientId };
            GetPatientLkRequestResponse result = new GetPatientLkRequestResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_GET_PATIENTLK_REQUEST, false, true);
            return result;
        }

        /// <summary>
        /// LIS-7500
        /// Получение временного пароля для просмотра результатов по заявке
        /// </summary>
        /// <param name="id">Id заявки</param>
        /// <returns>Строка с временным паролем</returns>
        public GetRequestTempPasswordResponse GetRequestTempPassword(int id, LimsUserSession userSession)
        {
            var request = new { id };
            GetRequestTempPasswordResponse result = new GetRequestTempPasswordResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_GET_REQUEST_TEMP_PASSWORD, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-7500
        /// Генерация временного пароля для просмотра результатов по заявке
        /// </summary>
        /// <param name="id">Id заявки</param>
        /// <returns>Строка с временным паролем</returns>
        public string GenerateRequestTempPassword(int id, LimsUserSession userSession)
        {
            var request = new { id };
            GenerateRequestTempPasswordResponse result = new GenerateRequestTempPasswordResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_GENERATE_REQUEST_TEMP_PASSWORD, false, true, userSession);
            return result.TempPassword;
        }

        /// <summary>
        /// LIS-7500
        /// Получение списка всех заявок пациента в личном кабинете
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public GetPatientLkAllRequestsResponse GetPatientLkAllRequests(int patientId)
        {
            var request = new { patientId };
            GetPatientLkAllRequestsResponse response = new GetPatientLkAllRequestsResponse();
            LoadObject(request, response, XMLConst.XML_METHOD_GET_PATIENTLK_ALL_REQUESTS, false, true);
            return response;
        }

        /// <summary>
        /// LIS-7568
        /// Метод отправки сообщений сотрудникам (заказчика или лаборатории)
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="employees">Список сотрудников-получателей</param>
        public LimsServerResponse CreateEmployeesMessage(CreateEmployeesMessageRequest request, LimsUserSession userSession)
        {
            LimsServerResponse response = new LimsServerResponse();
            LoadObject(request, response, XMLConst.XML_METHOD_CREATE_EMPLOYEES_MESSAGE, false, true, userSession);
            return response;
        }

        /// <summary>
        /// LIS-7570
        /// Метод получения сообщений для сотрудника
        /// </summary>
        /// <param name="dateFrom">Период начала загрузки сообщений</param>
        /// <param name="dateTill">Период окончания загрузки сообщений</param>
        /// <param name="onlyNew">True - грузить только непрочитанные сообщения. Иначе false</param>
        /// <returns></returns>
        public GetEmployeeMessagesResponse GetEmployeeMessages(DateTime dateFrom, DateTime dateTill, bool onlyNew, LimsUserSession userSession)
        {
            GetEmployeeMessagesResponse result = new GetEmployeeMessagesResponse();
            LoadObject(new { dateFrom, dateTill, onlyNew }, result, XMLConst.XML_METHOD_GET_EMPLOYEE_MESSAGES, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-7572
        /// Метод получения текста сообщения для сотрудника
        /// </summary>
        /// <param name="id">id сообщения</param>
        /// <returns></returns>
        public EmployeeMessageText GetEmployeeMessageText(int id, bool sent, LimsUserSession userSession)
        {
            EmployeeMessageText result = new EmployeeMessageText();
            GetEmployeeMessageTextRequest req = new GetEmployeeMessageTextRequest()
            {
                Id = id,
                Sent = sent
            };
            LoadObject(req, result, XMLConst.XML_METHOD_GET_EMPLOYEE_MESSAGE_TEXT, false, true, userSession);
            result.Text = System.Web.HttpUtility.UrlDecode(result.Text);
            return result;
        }

        /// <summary>
        /// LIS-7592
        /// Метод загрузки метаинформации о новых сообщениях сотрудников
        /// </summary>
        /// <param name="employees">Список сотрудников, для которых грузится метаинформация</param>
        /// <returns></returns>
        public GetEmployeesNewMessagesResponse GetEmployeesNewMessages(List<ObjectRef> employees)
        {
            GetEmployeesNewMessagesResponse result = new GetEmployeesNewMessagesResponse();
            GetEmployeesNewMessagesRequest request = new GetEmployeesNewMessagesRequest();
            request.Employees = employees;
            LoadObject(request, result, XMLConst.XML_METHOD_GET_EMPLOYEES_NEW_MESSAGE, false, true);
            return result;
        }

        /// <summary>
        /// LIS-8049
        /// Метод загрузки отправленных сообщений сотрудником
        /// </summary>
        /// <param name="dateFrom">Начало периода для загрузки</param>
        /// <param name="dateTill">Окончание периода для загрузки</param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public GetEmployeeSentMessagesResponse GetEmployeeSentMessages(DateTime dateFrom, DateTime dateTill, LimsUserSession userSession)
        {
            GetEmployeeSentMessagesResponse result = new GetEmployeeSentMessagesResponse();
            LoadObject(new { dateFrom, dateTill }, result, XMLConst.XML_METHOD_GET_EMPLOYEE_SENT_MESSAGES, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-7615
        /// Метод загрузки исследований с ценами для данной заявки
        /// </summary>
        /// <param name="id">id заявки</param>
        /// <returns></returns>
        public TargetsSet RequestGetTargets(int id, LimsUserSession userSession)
        {
            TargetsSet result = new TargetsSet();
            LoadObject(new { id = id }, result, XMLConst.XML_METHOD_REQUEST_TARGETS_WITH_PRICES, false, true);
            return result;
        }

        /// <summary>
        /// LIS-8139
        /// Метод получения информации по исследованиям у биоматериалов
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public GetSamplesTargetinfoResponse GetSamplesTargetinfo(List<ObjectRef> samples, LimsUserSession userSession)
        {
            GetSamplesTargetinfoRequest request = new GetSamplesTargetinfoRequest();
            request.Samples = samples;
            GetSamplesTargetinfoResponse result = new GetSamplesTargetinfoResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_GET_SAMPLES_TARGETINFO, false, true);
            return result;
        }

        /// <summary>
        /// LIS-8147
        /// Загружает деталей исследований 
        /// </summary>
        /// <param name="payCategory"></param>
        /// <param name="hospital"></param>
        /// <param name="cito"></param>
        /// <param name="targets"></param>
        /// <param name="calcDate"></param>
        /// <returns></returns>
        public TargetsSet GetTargetsDetails(ObjectRef payCategory, ObjectRef hospital, bool cito, List<TargetDetails> targetsDetails, DateTime calcDate)
        {
            TargetsSet result = new TargetsSet();
            var request = new { payCategory, hospital, cito, targetsDetails, calcDate };
            LoadObject(request, result, XMLConst.XML_METHOD_GET_TARGETS_DETAILS, false, true);
            return result;
        }

        /// <summary>
        /// LIS-8177
        /// Создание платежа
        /// </summary>
        /// <param name="createPaymentRequest"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public ExternalRequestResponce CreatePayment(CreatePaymentRequest createPaymentRequest, LimsUserSession userSession)
        {
            ExternalRequestResponce result = new ExternalRequestResponce();
            LoadObject(createPaymentRequest, result, XMLConst.XML_METHOD_CREATE_PAYMENT, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-8188
        /// Получение платежа
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Payment PaymentInfo(int id, LimsUserSession userSession)
        {
            Payment result = new Payment();
            LoadObject(new { id = id }, result, XMLConst.XML_METHOD_PAYMENT_NFO, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-8954
        /// Получение платежей и возвратов для заявки
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<Payment> GetRequestPayments(ObjectRef requestId, LimsUserSession userSession)
        {
            GetRequestPaymentsResponse result = new GetRequestPaymentsResponse();
            LoadObject(new { request = requestId }, result, XMLConst.XML_METHOD_GET_REQUEST_PAYMENTS, false, true, userSession);
            return result.Payments;
        }

        /// <summary>
        /// LIS-8179
        /// Завершить платёж
        /// </summary>
        /// <param name="requst"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public CompletePaymentResponse CompletePayment(CompletePaymentRequest requst, LimsUserSession userSession)
        {
            CompletePaymentResponse result = new CompletePaymentResponse();
            LoadObject(requst, result, XMLConst.XML_METHOD_COMPLETE_PAYMENT, false, true, userSession);
            return result;
        }

        /// <summary>
        /// LIS-8952
        /// Возвратить позиции платежа
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userSession"></param>
        public void RejectPaymentServices(RejectPaymentServicesRequest request, LimsUserSession userSession)
        {
            LoadObject(request, new object(), XMLConst.XML_METHOD_REJECT_PAYMENT_SERVICES, false, true, userSession);
        }

        /// <summary>
        /// LIS-8968
        /// Завершить возврат
        /// </summary>
        /// <param name="id">Id возврата</param>
        /// <param name="userSession"></param>
        public void CompleteRejectedPayment(int id, LimsUserSession userSession)
        {
            CompleteRejectedPaymentRequest request = new CompleteRejectedPaymentRequest(id);
            LoadObject(request, new object(), XMLConst.XML_METHOD_COMPLETE_REJECTED_PAYMENT, false, true, userSession);
        }


        /// <summary>
        /// LIS-8996
        /// Удалить позиции возврата
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userSession"></param>
        public void CancelRejectedPaymentServices(CancelRejectedPaymentServicesRequest request, LimsUserSession userSession)
        {
            LoadObject(request, new object(), XMLConst.XML_METHOD_CANCEL_REJECTED_PAYMENT_SERVICES, false, true, userSession);
        }

        /// <summary>
        /// LIS-
        /// Обновление информации о платеже (номер кассового чека, дата, фактические виды оплаты)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userSession"></param>
        public void UpdatePayment(UpdatePaymentRequest request, LimsUserSession userSession)
        {
            LoadObject(request, new object(), XMLConst.XML_METHOD_UPDATE_PAYMENT, false, true, userSession);
        }
    }
}