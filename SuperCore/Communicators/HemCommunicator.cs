using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.Crypto;
using ru.novolabs.SuperCore.HemBusinessObjects;
using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ru.novolabs.SuperCore.Core.HardwareWork;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Содержит все методы, используемые для коммуникации с сервером Службы Крови
    /// </summary>
    public class HemCommunicator : BaseCommunicator, IDisposable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BloodCommunicator
        /// </summary>
        public HemCommunicator() : base() { }
        /// <summary>
        /// Получает или устанавливает кэш справочников коммуникатора со Службой Крови
        /// </summary>
        [CSN("DictionaryCache")]
        public HemDictionaryCache DictionaryCache
        {
            get { return (HemDictionaryCache)dictionaryCache; }
            set { dictionaryCache = value; }
        }

        protected override Type DictionaryCacheType()
        {
            return typeof(HemDictionary.HemDictionaryCache);
        }

        [CSN("HemUserSession")]
        public HemUserSession HemUserSession { get { return (HemUserSession)UserSession; } }

        public void Dispose()
        {
            //    // Logout();
        }

        /// <summary>
        /// Выполняет аутентификацию и авторизацию пользователя и создаёт
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="serverAddress"></param>
        /// <param name="userSession"></param>
        public override void Login(string login, string password, string serverAddress, out BaseUserSession userSession, AbstractCodec codec)
        {
            Log.WriteText(String.Format("Logging in to server {0} as '{1}'", serverAddress, login));
            this.ServerAddress = serverAddress;
            this.UserSession = userSession = new HemUserSession();

            LoginRequest Login = new LoginRequest();
            LoginResponce loginResult = new LoginResponce();

            Login.Password = password;
            Login.Login = login;
            Login.Machine = Environment.MachineName;

            bool writeObjectTag = true;
            userSession.SessionId = LoadObject(Login, loginResult, XMLConst.XML_METHOD_LOGIN, false, writeObjectTag);

            ((HemUserSession)userSession).User = new EmployeeDictionaryItem() { Id = loginResult.Employee.GetRef(), Rights = ConvertAccessRights(loginResult.Rights) };
            if (dictionaryCache != null)
                ((HemUserSession)userSession).User = (EmployeeDictionaryItem)dictionaryCache[LimsDictionaryNames.Employee, loginResult.Employee.GetRef()];

            LoggedIn = !String.IsNullOrEmpty(userSession.SessionId);
        }

        public void Logout()
        {
            base.Logout();
        }

        public String GetFileFromServerAsString(Int32 id, HemUserSession userSession)
        {

            return GetFileFromServerAsString(id, Encoding.GetEncoding(1251), userSession);
        }

        protected override void InitLocals()
        {
            //LoadServerOptions();
            if (HemUserSession.User != null)
            {
                var rights = HemUserSession.User.Rights;
                HemUserSession.User = (EmployeeDictionaryItem)DictionaryCache[HemDictionaryNames.Employee, HemUserSession.User.Id];
                HemUserSession.User.Rights = rights;
                UpdateAccessRights(HemUserSession.User.Rights);
                Log.WriteText("------------------------------------------InitLocals completed -------------------------------------------");
            }
        }

        [CSN("ServerOptions")]
        public ServerOptionList ServerOptions { get; set; }
        public void LoadServerOptions()
        {
            if (ServerOptions == null)
                ServerOptions = GetOptions(null);
        }

        private void UpdateAccessRights(List<AccessRightDictionaryItem> accessRights)
        {
            for (int i = 0; i < accessRights.Count; i++)
            {
                accessRights[i] = (AccessRightDictionaryItem)ProgramContext.HemCommunicator.DictionaryCache[HemDictionaryNames.AccessRight, accessRights[i].Id];
            }
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

        /// <summary>
        /// Выполняет вход пользователя в один из доступных ему филиалов
        /// </summary>
        /// <param name="departmentment"></param>
        /// <param name="userSession"></param>
        public void LoginDepartment(DepartmentDictionaryItem departmentment, HemUserSession userSession)
        {
            var request = new { Department = departmentment };
            LoadObject(request, null, XMLConst.XML_METHOD_LOGIN_DEPARTMENT, true, true, userSession);
        }

        /// <summary>
        /// Возвращает список доноров, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<Donor> FindDonors(DonorJournalFilter filter, HemUserSession userSession)
        {
            var response = new { Result = new List<Donor>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_FIND_DONORS, true, true, userSession);
            return response.Result;
        }

        /// <summary>
        /// Получить список настроек сервера
        /// </summary>
        /// <returns></returns>
        public ServerOptionList GetOptions(HemUserSession userSession)
        {
            ServerOptionList userOptions = new ServerOptionList();
            LoadObject(new Object(), userOptions, XMLConst.XML_METHOD_LOAD_SERVER_OPTIONS, false, true, userSession);
            Log.WriteText("Server options received");
            return userOptions;
        }

        /// <summary>
        /// Получает подробную информацию о доноре
        /// </summary>
        /// <param name="donorId"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Donor DonorGet(int donorId, HemUserSession userSession)
        {
            var response = new Donor();
            LoadObject(new { Id = new ObjectRef(donorId) }, response, XMLConst.XML_METHOD_GET_DONOR_INFO, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Получает подробную информацию о реципиенте
        /// </summary>
        /// <param name="donorId"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Recipient RecipientGet(int RecipientId, HemUserSession userSession)
        {
            var response = new Recipient();
            LoadObject(new { Id = new ObjectRef(RecipientId) }, response, XMLConst.XML_METHOD_GET_DONOR_INFO, false, true, userSession);
            return response;
        }

        /// <summary>
        /// Сохраняет информацию о доноре
        /// </summary>
        /// <param name="donor"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public DonorSaveResult DonorSave(Donor donor, HemUserSession userSession)
        {
            var response = new DonorSaveResult();
            LoadObject(donor, response, XMLConst.XML_METHOD_SAVE_DONOR, true, true, userSession);
            return response;
        }

        /// <summary>
        /// Сохраняет информацию о реципиента
        /// </summary>
        /// <param name="donor"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public DonorSaveResult RecipientSave(Recipient recipient, HemUserSession userSession)
        {
            var response = new DonorSaveResult();
            LoadObject(recipient, response, XMLConst.XML_METHOD_SAVE_DONOR, true, true, userSession);
            return response;
        }

        /// <summary>
        /// Возвращает список продуктов, удовлетворяющих фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public List<Product> FindProducts(ProductJournalFilter filter, HemUserSession userSession)
        {
            var response = new { Result = new List<Product>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_FIND_PRODUCTS, true, true, userSession);

            //foreach (Product product in response.Result)
            //{
            //    product.BloodParametesAsString = GetDisplayGroupAndResus(product.BloodParameters);
            //}
            return response.Result;
        }

        /// <summary>
        /// Получает подробную информацию о продукте
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Product ProductGet(int productId, HemUserSession userSession)
        {
            List<ObjectRef> ids = new List<ObjectRef>();
            ids.Add(new ObjectRef(productId));
            return ProductsGet(ids, userSession).First();
        }

        public List<Product> ProductsGet(List<ObjectRef> ids, HemUserSession userSession)
        {
            var response = new { Result = new List<Product>() };
            LoadObject(new { Ids = ids }, response, XMLConst.XML_METHOD_GET_PRODUCTS_INFO, false, true, userSession);
            return response.Result;
        }
        /// <summary>
        /// Получает краткую информацию о продукте
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Product ProductShortGet(int productId, HemUserSession userSession)
        {
            List<ObjectRef> ids = new List<ObjectRef>();
            ids.Add(new ObjectRef(productId));
            return ProductsShortGet(ids, userSession).First();
        }

        public List<Product> ProductsShortGet(List<ObjectRef> ids, HemUserSession userSession)
        {
            var response = new { Result = new List<Product>() };
            LoadObject(new { Ids = ids }, response, XMLConst.XML_METHOD_GET_PRODUCTS_SHORT_INFO, false, true, userSession);
            return response.Result;
        }

        public Transfusion TransfusionGet(Int32 Id, HemUserSession userSession)
        {
            Transfusion result = new Transfusion();
            LoadObject(new { Transfusion = new ObjectRef(Id) }, result, XMLConst.XML_METHOD_GET_TRANSFUSION, false, true, userSession);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transfusionTemplateId">Шаблон трансфузии</param>
        /// <param name="RequestId">Id заявки на трансфузию</param>
        /// <param name="RecipientId">Id реципиента. По умолчанию проставляй 0, если же нет заявки на трансфузию, то его обязательно нужно указать</param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Int32 TransfusionCreate(Int32 transfusionTemplateId, Int32 RequestId, Int32 RecipientId, HemUserSession userSession)
        {
            ObjectRef response = new ObjectRef();
            var request = new Object();
            request = new { Template = new ObjectRef(transfusionTemplateId), Recipient = new ObjectRef(RecipientId), Request = new ObjectRef(RequestId) };
            //if (RequestId != 0)
            //    request = new { Template = new ObjectRef(transfusionTemplateId), Request = new ObjectRef(RequestId) };
            //else
            //    request = new { Template = new ObjectRef(transfusionTemplateId), Recipient = new ObjectRef(RecipientId) };
            LoadObject(request, response, XMLConst.XML_METHOD_CREATE_TRANSFUSION, false, true, userSession);
            return response.Id;
        }

        public Int32 TransfusionSave(Transfusion transfusion, HemUserSession userSession)
        {
            ObjectRef response = new ObjectRef();
            LoadObject(transfusion, response, XMLConst.XML_METHOD_SAVE_TRANSFUSION, false, true, userSession);
            return response.Id;
        }

        public List<TransfusionJournalRow> TransfusionJournal(TransfusionJournalFilter filter, HemUserSession userSession)
        {
            var response = new { Result = new List<TransfusionJournalRow>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_LOAD_TRANSFUSION_JOURNAL, false, true, userSession);
            return response.Result;
        }

        public Int32 SavePatientCard(PatientCard patientCard, HemUserSession userSession)
        {
            ObjectRef response = new ObjectRef();
            LoadObject(patientCard, response, XMLConst.XML_METHOD_SAVE_PATIENT_CARD, false, true, userSession);
            return response.Id;
        }

        public PatientCard GetPatientCard(Int32 Id, HemUserSession userSession)
        {
            PatientCard result = new PatientCard();
            LoadObject(new { Id = Id }, result, XMLConst.XML_METHOD_GET_PATIENT_CARD, false, true, userSession);
            return result;
        }

        public void DonorManageRecipients(Donor donor, List<Donor> recipients, HemUserSession userSession)
        {
            var request = new { Donor = donor, Recipients = recipients };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_DONOR_MANAGE_RECIPIENTS, false, true, userSession);

        }

        public void DonorManageVolunteers(Donor donor, List<Donor> personalDonors, HemUserSession userSession)
        {
            var request = new { Donor = donor, PersonalDonors = personalDonors };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_DONOR_MANAGE_PERSONAL_DONORS, false, true, userSession);
        }

        public Int32 TransfusionRequestSave(TransfusionRequest request, HemUserSession userSession)
        {
            Int32 result = 0;
            LoadObject(request, result, XMLConst.XML_METHOD_SAVE_TRANSFUSION_REQUEST, false, true, userSession);
            return result;
        }

        public TransfusionRequest TransfusionRequestGet(Int32 Id, HemUserSession userSession)
        {
            TransfusionRequest request = new TransfusionRequest();
            request.Id = Id;
            TransfusionRequest result = new TransfusionRequest();
            LoadObject(new { TransfusionRequest = new ObjectRef(Id) }, result, XMLConst.XML_METHOD_GET_TRANSFUSION_REQUEST, false, true, UserSession);
            return result;
        }

        public void DoctorApproveTransfusionRequest(Int32 Id, Boolean Approved, HemUserSession userSession)
        {

            var request = new { transfusionRequest = new ObjectRef(Id), approved = Approved };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_DOCTOR_APPROVE_TRANSFUSION_REQUEST, false, true, UserSession);
        }

        public void CancelTransfusionRequest(Int32 Id, String Comments, HemUserSession userSession)
        {

            var request = new { transfusionRequest = new ObjectRef(Id), comments = Comments };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_CANCEL_TRANSFUSION_REQUEST, false, true, UserSession);
        }

        public List<TransfusionRequestJournalRow> TransfusionRequestJournal(TransfusionRequestJournalFilter filter, HemUserSession userSession)
        {
            var response = new { Result = new List<TransfusionRequestJournalRow>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_LOAD_TRANSFUSION_REQUEST_JOURNAL, false, true, userSession);
            return response.Result;
        }

        public HemServerResponse ObservationSave(Int32 TransfusionId, MedicalObservation observation, HemUserSession userSession)
        {
            var request = new { Transfusion = new ObjectRef(TransfusionId), Observation = observation };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_OBSERVATION_SAVE, false, true, userSession);
            return result;
        }

        public void ReceiveProduct(Int32 ProductId, HemUserSession userSession)
        {
            var request = new { Product = new ObjectRef(ProductId) };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_PRODUCT_RECEIVE, false, true, userSession);
        }

        public HemServerResponse ReceiveProducts(List<String> nrs, HemUserSession userSession)
        {
            var request = new { productNrs = nrs };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_PRODUCTS_RECEIVE, false, true, userSession);
            return result;
        }

        public HemServerResponse ReceiveProducts(List<ObjectRef> Products, HemUserSession userSession)
        {
            var request = new { products = Products };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_PRODUCTS_RECEIVE, false, true, userSession);
            return result;
        }

        public HemServerResponse ProductsReturn(List<String> nrs, HemUserSession userSession)
        {
            var request = new { products = nrs };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_PRODUCTS_RETURN, false, true, userSession);
            return result;
        }

        public HemServerResponse TransfusionExecute(Int32 TransfusionId, TransfusionResultDictionaryItem tresult, HemUserSession userSession)
        {
            var request = new { Transfusion = new ObjectRef(TransfusionId), Result = new ObjectRef(tresult.Id) };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_TRANSFUSION_EXECUTE, false, true, userSession);
            return result;
        }

        public List<Product> GetVacantProducts(Int32 HospitalDepartmentId, HemUserSession userSession)
        {
            List<Product> result = new List<Product>();
            var request = new { HospitalDepartment = new ObjectRef(HospitalDepartmentId) };
            LoadObject(request, result, XMLConst.XML_METHOD_VACANT_PRODUCTS_GET, false, true, userSession);
            return result;
        }

        public void TransfusionAddProduct(Int32 TransfusionId, Int32 ProductId, HemUserSession userSession)
        {
            var request = new { Transfusion = new ObjectRef(TransfusionId), Product = new ObjectRef(ProductId) };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_TRANSFUSION_ADD_PRODUCT, false, true, userSession);
        }

        public void TransfusionRemoveProduct(Int32 TransfusionId, Int32 ProductId, HemUserSession userSession)
        {
            var request = new { Transfusion = new ObjectRef(TransfusionId), Product = new ObjectRef(ProductId) };
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_TRANSFUSION_REMOVE_PRODUCT, false, true, userSession);
        }

        public void NotifyHemState(HemStateNotifyRequest request, HemUserSession userSession)
        {
            Object result = new Object();
            LoadObject(request, result, XMLConst.XML_METHOD_NOTIFY, false, true, userSession);
        }

        /// <summary>
        /// Получить свободный внутренний номер для заявки
        /// </summary>
        /// <returns></returns>
        public String GetFreeNr(HemUserSession userSession)
        {
            TransfusionRequestNrResponse responce = new TransfusionRequestNrResponse();
            LoadObject(new Object(), responce, XMLConst.XML_METHOD_GET_FREE_TRANSFUSION_REQUEST_NR, false, true, userSession);
            return responce.TransfusionRequestNr;
        }

        public List<TreatmentJournalRow> TreatmentJournal(TreatmentJournalFilter filter, HemUserSession userSession)
        {
            var response = new { Result = new List<TreatmentJournalRow>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_GET_TREATMENT_JOURNAL, false, true, userSession);
            return response.Result;
        }

        public List<TreatmentRequestJournalRow> TreatmentRequestJournal(TreatmentRequestJournalFilter filter, HemUserSession userSession)
        {
            var response = new { Result = new List<TreatmentRequestJournalRow>() };
            LoadObject(filter, response, XMLConst.XML_METHOD_GET_TREATMENT_REQUEST_JOURNAL, false, true, userSession);
            return response.Result;
        }

        public TreatmentRequest TreatmentRequestGet(Int32 Id, HemUserSession userSession)
        {
            TreatmentRequest result = new TreatmentRequest();
            var request = new { treatmentRequest = new ObjectRef(Id) };
            LoadObject(request, result, XMLConst.XML_METHOD_GET_TREATMENT_REQUEST, false, true, userSession);
            return result;
        }

        public Int32 TreatmentRequestSave(TreatmentRequest request, HemUserSession userSession)
        {
            Int32 result = 0;
            //ObjectRef response = new ObjectRef();
            LoadObject(request, result, XMLConst.XML_METHOD_SAVE_TREATMENT_REQUEST, false, true, userSession);
            return result;
        }

        public HemServerResponse TreatmentRequestCancel(TreatmentRequest treatmentRequest, HemUserSession userSession)
        {
            var request = new { treatmentRequest = new ObjectRef(treatmentRequest.Id) };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_CANCEL_TREATMENT_REQUEST, false, true, userSession);
            return result;
        }

        public HemServerResponse ApparatusData(ApparatusData data, HemUserSession userSession)
        {
            HemServerResponse result = new HemServerResponse();
            LoadObject(data, result, XMLConst.XML_METHOD_APPARATUS_DATA, false, true, userSession);
            return result;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <param name="patient">Пациент (если передается заявка, ставить null)</param>
        /// <param name="template">Шаблон процедуры</param>
        /// <param name="accessType">Тип венозного доступа</param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public Int32 TreatmentCreate(TreatmentRequest request, int patientId, TreatmentTemplateDictionaryItem template, VeinAccessTypeDictionaryItem accessType, HemUserSession userSession)
        {
            ObjectRef response = new ObjectRef();
            //Int32 result = 0;

            if (request != null)
                LoadObject(new { treatmentRequest = new ObjectRef(request.Id) }, response, XMLConst.XML_METHOD_CREATE_TREATMENT, false, true, userSession);
            else
                LoadObject(new { patient = new ObjectRef(patientId), treatmentTemplate = new ObjectRef(template.Id), veinAccessType = new ObjectRef(accessType.Id) }, response, XMLConst.XML_METHOD_CREATE_TREATMENT, false, true, userSession);

            return response.Id;
        }

        public Treatment TreatmentGet(Int32 Id, HemUserSession userSession)
        {
            Treatment result = new Treatment();
            var request = new { treatment = new ObjectRef(Id) };
            LoadObject(request, result, XMLConst.XML_METHOD_GET_TREATMENT, false, true, userSession);
            return result;
        }

        public HemServerResponse TreatmentStart(Treatment treatment, HemUserSession userSession)
        {
            var request = new { treatment = new ObjectRef(treatment.Id) };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_START_TREATMENT, false, true, userSession);
            return result;
        }

        public HemServerResponse TreatmentCancel(Treatment treatment, HemUserSession userSession)
        {
            var request = new { treatment = new ObjectRef(treatment.Id) };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_CANCEL_TREATMENT, false, true, userSession);
            return result;
        }

        public HemServerResponse TreatmentFinish(Treatment treatment, HemUserSession userSession)
        {
            var request = new { treatment = new ObjectRef(treatment.Id) };
            HemServerResponse result = new HemServerResponse();
            LoadObject(request, result, XMLConst.XML_METHOD_FINISH_TREATMENT, false, true, userSession);
            return result;
        }

        /// <summary>
        /// Сохраняет информацию о лечебной процедуре
        /// </summary>
        /// <returns></returns>
        public TreatmentSaveResult TreatmentSave(Treatment treatment, HemUserSession userSession)
        {
            var response = new TreatmentSaveResult();
            LoadObject(treatment, response, XMLConst.XML_METHOD_SAVE_TREATMENT, true, true, userSession);
            return response;
        }
        /// <summary>
        /// Создает складскую операцию
        /// </summary>
        /// <param name="storageOperation"></param>
        /// <param name="userSession"></param>
        /// <returns></returns>
        public HemServerResponse StorageOperationSave(StorageOperation storageOperation, HemUserSession userSession)
        {
            HemServerResponse response = new HemServerResponse();
            LoadObject(storageOperation, response, XMLConst.XML_METHOD_STORAGE_OPERATION_SAVE, false, true, userSession);
            return response;
        }

        public object GetPersonHistory(string patientCode, HemUserSession userSession)
        {
            var request = new { personCode = patientCode };
            var response = new PersonHistory();
            LoadObject(request, response, XMLConst.XML_METHOD_PERSON_HISTORY, false, true, userSession);
            return response;
        }

        public TransfusionRequestNrResponse TransfusionRequestAdditionalSave(int volume, ObjectRef parentTransfusionRequest, HemUserSession userSession)
        {
            TransfusionRequestNrResponse response = new TransfusionRequestNrResponse();
            LoadObject(new { volume, parentTransfusionRequest }, response, XMLConst.XML_METHOD_TRANSFUSION_REQUEST_ADDITIONAL_SAVE, false, true, userSession);
            return response;
        }

    }
}