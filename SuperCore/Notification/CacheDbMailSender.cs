using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ru.novolabs.SuperCore.Notification
{
    class CacheDbMailSender
    {
        public static string dbConnectionStr { get; set; }
        public CacheDbMailSender()
        {

        }
        #region Store methods
        public void StoreMailSender(MailDTO mailDTO, int count)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(dbConnectionStr))
                {
                    connection.Open();
                    long? credentialId = 0;
                    credentialId = GetCredentialMailId(mailDTO.Credential, connection);
                    if (credentialId == null)
                    {
                        SaveCredential(mailDTO.Credential, connection);
                        credentialId = GetCredentialMailId(mailDTO.Credential, connection);                      
                    }
                    mailDTO.Credential.Id = credentialId;
                    if (GetMailSenderId(mailDTO, connection) == null)
                    {
                        SaveMailSender(mailDTO, count, connection);
                    }
                    transactionScope.Complete();
                }
            }
        }

        private void SaveMailSender(MailDTO mailDTO, int count, SqlConnection connection)
        {
          //  using (SqlConnection connection = new SqlConnection(dbConnectionStr))
          //  {
               // connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Mail_message_sender(from_str, to_str, subject_str, body_str, " +
                    "credential_id, count, state, date_create, date_stamp) VALUES(@from_str, @to_str, @subject_str, @body_str, @credential_id, @count, @state, @date_create, @date_stamp)", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@from_str";
                param.Value = mailDTO.FromStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@to_str";
                param.Value = mailDTO.ToStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@subject_str";
                param.Value = mailDTO.SubjectStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.NVarChar;
                param.Size = -1;
                param.ParameterName = "@body_str";
                param.Value = mailDTO.BodyStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.BigInt;
                param.ParameterName = "@credential_id";
                param.Value = mailDTO.Credential.Id;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.ParameterName = "@count";
                param.Value = count;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.ParameterName = "@state";
                param.Value = MailSenderState.InProgress;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.DateTime;
                param.ParameterName = "@date_create";
                param.Value = DateTime.Now;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.DateTime;
                param.ParameterName = "@date_stamp";
                param.Value = DateTime.Now;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            
           // }
        
        }
        private void SaveCredential(MailDTO.CredentialInfo credentialInfo, SqlConnection connection)
        {
           // using (SqlConnection connection = new SqlConnection(dbConnectionStr))
           // {
              //  connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Mail_credentials(login, password) VALUES(@login, @password)", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@login";
                param.Value = credentialInfo.Login;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@password";
                param.Value = credentialInfo.Password;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();

           // }
 
        
        }
        

        private long? GetMailSenderId(MailDTO mailDTO, SqlConnection connection)
        {
            //using (SqlConnection connection = new SqlConnection(dbConnectionStr))
           // {
            //    connection.Open();
                SqlCommand command = new SqlCommand("SELECT id FROM Mail_message_sender WHERE from_str = @from_str AND to_str = @to_str " + 
                    "AND subject_str = @subject_str AND body_str = @body_str AND credential_id = @credential_id AND state = @state", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@from_str";
                param.Value = mailDTO.FromStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@to_str";
                param.Value = mailDTO.ToStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@subject_str";
                param.Value = mailDTO.SubjectStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.NVarChar;
                param.Size = -1;
                param.ParameterName = "@body_str";
                param.Value = mailDTO.BodyStr;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.BigInt;
                param.ParameterName = "@credential_id";
                param.Value = mailDTO.Credential.Id;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.ParameterName = "@state";
                param.Value = MailSenderState.InProgress;
                command.Parameters.Add(param);

                object id = command.ExecuteScalar();
                if (id == null || id == DBNull.Value)
                {
                    return null;
                }
                return Convert.ToInt64(id);

            //}
        
        }
        private long? GetCredentialMailId(MailDTO.CredentialInfo credentialInfo, SqlConnection connection)
        {
           // using (SqlConnection connection = new SqlConnection(dbConnectionStr))
           // {
             //   connection.Open();
                SqlCommand command = new SqlCommand("SELECT id FROM Mail_credentials WHERE login = @login AND password = @password", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@login";
                param.Value = credentialInfo.Login;
                command.Parameters.Add(param);
                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.Size = 4000;
                param.ParameterName = "@password";
                param.Value = credentialInfo.Password;
                command.Parameters.Add(param);

                object id = command.ExecuteScalar();
                if (id == null || id == DBNull.Value)
                {
                    return null;                
                }
                return Convert.ToInt64(id);
            
           // }
        
        }

        #endregion

        #region Process methods
        public void ProcessMailSender(Func<MailDTO,bool> func)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MMS.id AS id, MMS.from_str AS from_str, MMS.to_str AS to_str, MMS.subject_str AS subject_str, MMS.body_str AS body_str, " +
                    "MMS.credential_id AS credential_id, MMS.count AS count, MCR.login AS login, MCR.password AS password " +
                    "FROM Mail_message_sender MMS " +
                    "JOIN Mail_credentials MCR ON MCR.id = MMS.credential_id " +
                    "WHERE MMS.state = @state", connection);

                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.ParameterName = "@state";
                param.Value = MailSenderState.InProgress;
                command.Parameters.Add(param);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            MailDTO mailDTO = new MailDTO();
                            mailDTO.Credential = new MailDTO.CredentialInfo();
                            mailDTO.FromStr = reader.GetString(reader.GetOrdinal("from_str"));
                            mailDTO.ToStr = reader.GetString(reader.GetOrdinal("to_str"));
                            mailDTO.SubjectStr = reader.GetString(reader.GetOrdinal("subject_str"));
                            mailDTO.BodyStr = reader.GetString(reader.GetOrdinal("body_str"));
                            mailDTO.Id = reader.GetInt64(reader.GetOrdinal("id"));
                            mailDTO.Credential.Id = reader.GetInt64(reader.GetOrdinal("credential_id"));
                            mailDTO.Credential.Login = reader.GetString(reader.GetOrdinal("login"));
                            mailDTO.Credential.Password = reader.GetString(reader.GetOrdinal("password"));
                            int count = reader.GetInt32(reader.GetOrdinal("count"));

                            bool isSendMail = func(mailDTO);
                            if (!isSendMail && count > 1)
                            {
                               UpdateMailSender(mailDTO.Id.Value);
                            }
                            else if (!isSendMail && count == 1)
                            {
                                UpdateMailSender(mailDTO.Id.Value, MailSenderState.NotSendWithError);                            
                            }
                            else
                            {
                                DeleteMailSender(mailDTO.Id.Value);
                            }
                            scope.Complete();
                        }

                    }
                }
            }
        
        }

        private void UpdateMailSender(long mailId)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Mail_message_sender SET count = count-1, date_stamp = @date_stamp  WHERE id = @id", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.BigInt;
                param.ParameterName = "@id";
                param.Value = mailId;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.DateTime;
                param.ParameterName = "@date_stamp";
                param.Value = DateTime.Now;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();

            }
        }
        private void UpdateMailSender(long mailId, MailSenderState state)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Mail_message_sender SET count = 0, state = @state, date_stamp = @date_stamp  WHERE id = @id", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.BigInt;
                param.ParameterName = "@id";
                param.Value = mailId;
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.Int;
                param.ParameterName = "@state";
                param.Value = state.GetHashCode();
                command.Parameters.Add(param);

                param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.DateTime;
                param.ParameterName = "@date_stamp";
                param.Value = DateTime.Now;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();

            }
        }
        private void DeleteMailSender(long mailId)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Mail_message_sender WHERE id = @id", connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.BigInt;
                param.ParameterName = "@id";
                param.Value = mailId;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();

            }

        }
        #endregion
    }
    public class MailDTO
    {

        public class CredentialInfo
        {
            public long? Id { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
        public long? Id { get; set; }
        public string FromStr { get; set; }
        public string ToStr { get; set; }
        public string SubjectStr { get; set; }
        public string BodyStr { get; set; }
        public CredentialInfo Credential { get; set; }
    
    }
    enum MailSenderState
    {
        InProgress = 0,
        NotSendWithError = 1
    
    }
}
