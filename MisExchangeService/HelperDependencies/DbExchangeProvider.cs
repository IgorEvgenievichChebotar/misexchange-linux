using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchangeService;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    class DbExchangeProvider : IDbExchangeProvider
    {
        const String REQUEST_CODE = "REQUEST_CODE";
        const String BARCODE = "BARCODE";
        const String REQUEST_ID = "REQUEST_ID";
        const String LIS_ID = "LIS_ID";

        const String SelectRequestLisIdCommandText = @"SELECT lis_id as RequestLisId FROM Request WHERE request_code = ?";
        const String InsertRequestIdCommandText = @"INSERT INTO Request (request_code, lis_id) VALUES (?,?)";
        const String UpdateRequestIdCommandText = @"UPDATE Request SET lis_id = ? WHERE request_code = ?";

        public void SaveToExchangeDb(ExchangeDTOs.Request requestDTO, Int32 requestLisId)
        {
            using (OdbcConnection serviceDBConnection = GAP.ServiceDBManager.GetConnection())
            {
                serviceDBConnection.Open();

                OdbcCommand InsertRequestIdCommand = new OdbcCommand(InsertRequestIdCommandText);
                InsertRequestIdCommand.Connection = serviceDBConnection;
                InsertRequestIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);
                InsertRequestIdCommand.Parameters.Add(LIS_ID, OdbcType.Numeric);

                OdbcCommand UpdateRequestIdCommand = new OdbcCommand(UpdateRequestIdCommandText);
                UpdateRequestIdCommand.Connection = serviceDBConnection;
                UpdateRequestIdCommand.Parameters.Add(LIS_ID, OdbcType.Numeric);
                UpdateRequestIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);


                // Сохраняем информацию о заявке в служебной БД
                if (requestLisId > 0)
                {
                    if (GetRequestLisId(requestDTO.RequestCode) == 0)
                    {
                        InsertRequestIdCommand.Parameters[REQUEST_CODE].Value = requestDTO.RequestCode;
                        InsertRequestIdCommand.Parameters[LIS_ID].Value = requestLisId;
                        InsertRequestIdCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        UpdateRequestIdCommand.Parameters[REQUEST_CODE].Value = requestDTO.RequestCode;
                        UpdateRequestIdCommand.Parameters[LIS_ID].Value = requestLisId;
                        UpdateRequestIdCommand.ExecuteNonQuery();
                    }

                    // Запрос CreateRequestXXX был реализован так, что серверу ВСЕГДА посылаются новые объекты Sample.
                    // Поэтому ранее задуманный код, подставляющий id сэмплов по их номерам закомментирован

                }

            }
        }
        public int GetRequestLisId(string requestCode)
        {
            Object requestLisId = null;
            using (OdbcConnection serviceDBConnection = GAP.ServiceDBManager.GetConnection())
            {
                serviceDBConnection.Open();
                OdbcCommand SelectRequestLisIdCommand = new OdbcCommand(SelectRequestLisIdCommandText);
                SelectRequestLisIdCommand.Connection = serviceDBConnection;
                SelectRequestLisIdCommand.Parameters.Add(REQUEST_CODE, OdbcType.VarChar);

                SelectRequestLisIdCommand.Parameters[REQUEST_CODE].Value = requestCode;
                requestLisId = SelectRequestLisIdCommand.ExecuteScalar();
            }
            return (requestLisId == null) ? 0 : (Int32)requestLisId;

        }
    }
}
