using ru.novolabs.MisExchange;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace ru.novolabs.MisExchangeService.Classes
{
    internal abstract class ExchangeHelper
    {
        // В каждом ExchangeHelper-е необходимо переопределить 6 методов, отвечающих за специфическую обработку данных:
        //
        // - SpecificProcessNewData() - специфическим образом обрабатывает новые входящие данные. В реализации этого метода нужно вызвать унаследованный метод ProcessRequestsData()
        // - SpecificGetRequests() - специфическим образом получает список заявок из внешней системы
        // - SpecificFinishRequestsProcessing() - выполняет специфические действия после обработки новых заявок. Например, оповещение внешней системы об успешной обработке группы заявок, обработка ошибок
        // - SpecificFilterRequestResults() - cпецифическим образом фильтрует результаты по заявкам
        // - SpecificStoreResults() - специфическим образом сохраняет список результатов
        // - SpecificExportDictionaries() - специфическим образом экспортирует во внешнюю систему справочники

        /// <summary>
        /// Специфическим образом обрабатывает новые входящие данные. В реализации этого метода нужно вызвать унаследованный метод ProcessRequestsData()
        /// </summary>
        abstract protected void SpecificProcessNewData();
        /// <summary>
        /// Специфическим образом получает список заявок из внешней системы
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="requests"></param>
        /// <returns></returns>
        abstract protected void SpecificGetRequests(object[] parameters, out List<ExternalRequest> requests);
        /// <summary>
        /// Выполняет специфические действия после обработки новых заявок. Например, оповещение внешней системы об успешной обработке группы заявок, обработка ошибок
        /// </summary>
        /// <param name="results"></param>
        abstract protected void SpecificFinishRequestsProcessing(List<ExternalRequest> requests);
        /// <summary>
        /// Специфическим образом фильтрует результаты по заявкам
        /// </summary>
        /// <param name="requestIds"></param>
        abstract protected void SpecificFilterRequestResults(List<ObjectRef> requestIds);
        /// <summary>
        /// Специфическим образом сохраняет список результатов
        /// </summary>
        /// <param name="results"></param>
        abstract protected void SpecificStoreResults(List<ExternalResult> results);
        ///// <summary>
        ///// Специфическим образом экспортирует во внешнюю систему справочники
        ///// </summary>
        //abstract protected void SpecificExportDictionaries();

        const String REQUEST_CODE = "?REQUEST_CODE";
        const String BARCODE = "?BARCODE";
        const String REQUEST_ID = "?REQUEST_ID";
        const String LIS_ID = "?LIS_ID";

        private OdbcCommand SelectRequestLisIdCommand { get; set; }
        private OdbcCommand SelectRequestIdCommand { get; set; }
        private OdbcCommand InsertRequestIdCommand { get; set; }
        private OdbcCommand UpdateRequestIdCommand { get; set; }

        private OdbcCommand SelectSampleIdCommand { get; set; }
        private OdbcCommand DeleteSampleCommand { get; set; }
        private OdbcCommand InsertSampleIdCommand { get; set; }

        public ExchangeHelper()
        {
            SelectRequestLisIdCommand = new OdbcCommand(@"SELECT lis_id as RequestLisId FROM dbo.Request WHERE request_code = ?");
            SelectRequestLisIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);

            SelectRequestIdCommand = new OdbcCommand(@"SELECT id as RequestId FROM dbo.Request WHERE request_code = ?");
            SelectRequestIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);

            InsertRequestIdCommand = new OdbcCommand(@"INSERT INTO dbo.Request (request_code, lis_id) VALUES (?,?)");
            InsertRequestIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);
            InsertRequestIdCommand.Parameters.Add(LIS_ID, OdbcType.Numeric);

            UpdateRequestIdCommand = new OdbcCommand(@"UPDATE dbo.Request SET lis_id = ? WHERE request_code = ?");
            UpdateRequestIdCommand.Parameters.Add(LIS_ID, OdbcType.Numeric);
            UpdateRequestIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);

            SelectSampleIdCommand = new OdbcCommand(@"SELECT lis_id FROM dbo.Sample WHERE barcode = ?");
            SelectSampleIdCommand.Parameters.Add(BARCODE, OdbcType.VarChar);

            DeleteSampleCommand = new OdbcCommand(@"DELETE FROM dbo.Sample WHERE request_id = (SELECT id FROM dbo.Request WHERE lis_id = ?)");
            DeleteSampleCommand.Parameters.Add(LIS_ID, OdbcType.Numeric);

            InsertSampleIdCommand = new OdbcCommand(@"INSERT INTO dbo.Sample (barcode, request_id, lis_id) VALUES (?, ?, ?)");
            InsertSampleIdCommand.Parameters.Add(BARCODE, OdbcType.VarChar);
            InsertSampleIdCommand.Parameters.Add(REQUEST_ID, OdbcType.Numeric);
            InsertSampleIdCommand.Parameters.Add(LIS_ID, OdbcType.Numeric);
        }

        private Int32 GetRequestLisId(String requestCode)
        {
            SelectRequestLisIdCommand.Parameters[REQUEST_CODE].Value = requestCode;
            Object requestLisId = SelectRequestLisIdCommand.ExecuteScalar();
            return (requestLisId == null) ? 0 : (Int32)requestLisId;
        }

        private Int32 GetRequestId(String requestCode)
        {
            SelectRequestIdCommand.Parameters[REQUEST_CODE].Value = requestCode;
            Object requestId = SelectRequestIdCommand.ExecuteScalar();
            return (requestId == null) ? 0 : (Int32)requestId;
        }

        private Int32 GetSampleLisId(String barcode)
        {
            SelectSampleIdCommand.Parameters[BARCODE].Value = barcode;
            Object sampleLisId = SelectSampleIdCommand.ExecuteScalar();
            return (sampleLisId == null) ? 0 : (Int32)sampleLisId;
        }

        protected void ProcessRequestsData(params object[] parameters)
        {
            List<ExternalRequest> requests;
            // Получаем данные по заявке
            SpecificGetRequests(parameters, out requests);
            foreach (var request in requests)
            {
                // Выясняем lis_id заявки. Если lis_id == 0, то создаём новую заявку, иначе - изменяем существующую
                request.Id = GetRequestLisId(request.RequestCode);

                try
                {
                    RequestPrepareHelper.PrepareRequest(request);
                    bool? setting = (bool?)ProgramContext.Settings["useUniqueSampleBarcoding", false];
                    bool useUniqueSampleBarcoding = setting != null ? setting.Value : false;

                    // Если НЕ используется индивидуальная нумерация пробирок
                    if (!useUniqueSampleBarcoding)
                    {
                        List<RequestId> RequestIds;
                        // Коллекция request.Errors содержит критические ошибки, не позволяющие создать заявку. Поэтому, если
                        // она не пуста, то запрос серверному приложению не посылаем
                        if ((request.Errors.Count > 0) || (request.Warnings.Count > 0))
                            continue;

                        ProgramContext.LisCommunicator.CreateRequest(request.ToBaseRequest(), out RequestIds, null);
                        request.Id = RequestIds.Find(req => req.InternalNr.Equals(request.InternalNr)).Id;
                    }
                    // Если используется индивидуальная нумерация пробирок, то существует возможность редактирования проб.
                    // Для этого нужно инициализировать id созданных ранее проб
                    else
                    {
                        if (useUniqueSampleBarcoding)
                        {
                            foreach (ExternalRequestSample sample in request.Samples)
                            {
                                if (!String.IsNullOrEmpty(sample.Barcode))
                                    sample.Id = GetSampleLisId(sample.Barcode);
                                else
                                    throw new ApplicationException("При использовании индивидуальной нумерация пробирок ШТРИХ-КОД пробирки ДОЛЖЕН БЫТЬ УКАЗАН ОБЯЗАТЕЛЬНО");
                            }

                            // Посылаем запрос на создание/изменение заявки
                            request.Id = ((MisExchangeServerCommunicator)ProgramContext.LisCommunicator).CreateRequest2(request);
                        }
                    }

                    // Сохраняем информацию о заявке в служебной БД
                    if (request.Id > 0)
                    {
                        if (GetRequestLisId(request.RequestCode) == 0)
                        {
                            InsertRequestIdCommand.Parameters[REQUEST_CODE].Value = request.RequestCode;
                            InsertRequestIdCommand.Parameters[LIS_ID].Value = request.Id;
                            InsertRequestIdCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            UpdateRequestIdCommand.Parameters[REQUEST_CODE].Value = request.RequestCode;
                            UpdateRequestIdCommand.Parameters[LIS_ID].Value = request.Id;
                            UpdateRequestIdCommand.ExecuteNonQuery();
                        }

                        // Если используется индивидуальная нумерация пробирок, нужно сохранить lis_id проб
                        if (useUniqueSampleBarcoding)
                        {
                            // Если ранее были созданы пробы для данной заявки, то их необходимо удалить
                            DeleteSampleCommand.Parameters[LIS_ID].Value = request.Id;
                            DeleteSampleCommand.ExecuteNonQuery();
                            List<BaseSample> samples = ProgramContext.LisCommunicator.RequestSamples(request.Id, null);
                            foreach (BaseSample sample in samples)
                            {
                                InsertSampleIdCommand.Parameters[BARCODE].Value = sample.InternalNr;
                                InsertSampleIdCommand.Parameters[LIS_ID].Value = sample.Id;
                                InsertSampleIdCommand.Parameters[REQUEST_ID].Value = GetRequestId(request.RequestCode);
                                InsertSampleIdCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (ExternalRequestCheckException) { /* nop */}
                catch (Exception ex)
                {
                    ErrorMessage e = new ErrorMessage();
                    e.Message = ex.Message;
                    request.Errors.Add(e);
                }
            }

            SpecificFinishRequestsProcessing(requests);
        }

        public void ProcessNewData()
        {
            // Получаем соединение со служебной БД на время обработки новых данных
            using (OdbcConnection serviceDBConnection = GAP.ServiceDBManager.GetConnection())
            {
                InsertRequestIdCommand.Connection = serviceDBConnection;
                UpdateRequestIdCommand.Connection = serviceDBConnection;
                SelectRequestLisIdCommand.Connection = serviceDBConnection;
                SelectRequestIdCommand.Connection = serviceDBConnection;

                SelectSampleIdCommand.Connection = serviceDBConnection;
                DeleteSampleCommand.Connection = serviceDBConnection;
                InsertSampleIdCommand.Connection = serviceDBConnection;

                serviceDBConnection.Open();

                try
                {
                    SpecificProcessNewData();
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Ошибка при обработке новых данных от МИС: {0}", ex.ToString()));
                }
            }
        }

        public void FilterRequestResults(List<ObjectRef> requestIds)
        {
            SpecificFilterRequestResults(requestIds);
        }

        public void StoreRequestsResults(List<ExternalResult> results)
        {
            results.ForEach(result =>
                {
                    ResultPrepareHelperOld.PrepareResult(result);
                    if (result.SampleResults.Count == 0)
                        Log.WriteText("Warning: в заявке № {0} не было ни одной работы, либо все работы были отфильтрованы в соответствии с настройками", result.RequestCode);
                });
            SpecificStoreResults(results);
        }

        protected void ProcessRequestFilter(ExternalRequestFilter extRequestFilter)
        {
            var errors = new List<string>();

            List<BaseRequest> requests = new List<BaseRequest>();
            RegistrationJournalFilter requestFilter = extRequestFilter.ToRequestJournalFilter();
            if (extRequestFilter.Errors.Count > 0)
            {
                extRequestFilter.Errors.ForEach(em => errors.Add(em.Message));
                throw new CustomDataCheckException(errors);
            }
            try
            {
                requests = ProgramContext.LisCommunicator.RegistrationJournal(requestFilter, null);
                Log.WriteText(String.Format("{0} requests found by filter", requests.Count));
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                throw new CustomDataCheckException(errors);
            }

            List<ObjectRef> requestIds = new List<ObjectRef>();
            foreach (BaseRequest request in requests)
            {
                if (!request.Removed && !requestIds.Exists(r => r.Id == request.Id))
                    requestIds.Add(new ObjectRef(request.Id));
            }

            FilterRequestResults(requestIds);
            Log.WriteText(String.Format("filtered requests count = [{0}]", requestIds.Count));
            if (requestIds.Count > 0)
            {
                List<ExternalResult> results = ProgramContext.LisCommunicator.GetRequestsResults(requestIds, null);
                StoreRequestsResults(results);
            }
        }
    }

    internal class RequestIdsFilter
    {
        const String LIS_ID = "?LIS_ID";

        public RequestIdsFilter()
        {
            SelectRequestCountCommand = new OdbcCommand(@"SELECT count(id) FROM dbo.Request WHERE lis_id = ?");
            SelectRequestCountCommand.Parameters.Add(LIS_ID, OdbcType.BigInt);
        }

        OdbcCommand SelectRequestCountCommand { get; set; }

        private Boolean RequestExists(Int32 Id)
        {
            SelectRequestCountCommand.Parameters[LIS_ID].Value = Id;
            Object requestCount = SelectRequestCountCommand.ExecuteScalar();
            if (((Int32)requestCount) == 0)
                return false;
            else
                return true;
        }

        public void Filter(List<ObjectRef> requestIds)
        {
            RefSet result = new RefSet();

            // Получаем соединение со служебной БД
            using (OdbcConnection serviceDBConnection = GAP.ServiceDBManager.GetConnection())
            {
                SelectRequestCountCommand.Connection = serviceDBConnection;
                serviceDBConnection.Open();

                for (int i = requestIds.Count - 1; i >= 0; i--)
                {
                    // Закоменчено только для локальной версии
                    /*if (!RequestExists(requestIds[i].Id))
                    {
                        Log.WriteText(String.Format("Request (id = {0}) skipped, because it is not from current external system", requestIds[i].Id));
                        requestIds.RemoveAt(i);
                    }*/
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class ExchangeHelperName : Attribute
    {
        private string helperName = String.Empty;

        public string Name
        {
            get { return helperName; }
        }

        public ExchangeHelperName(string exchangeHelperName)
        {
            this.helperName = exchangeHelperName;
        }
    }
}