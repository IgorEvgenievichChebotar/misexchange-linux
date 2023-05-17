
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Notification
{
    class DbCreator
    {
        const string DbCheckerStr = "SELECT 1 FROM master.dbo.sysdatabases WHERE name = @name";
        public string DbConnectionStr { get; set; }
        public string DbMasterConnectionStr { get; set; }
        string DbName { get; set; }
        public DbCreator(Dictionary<string,string> settings)
        {
            SqlConnectionStringBuilder scsbMaster = new SqlConnectionStringBuilder();
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.InitialCatalog = settings["DbInitialCatalog"];
            scsbMaster.InitialCatalog = "master";
            scsbMaster.DataSource =scsb.DataSource = settings["DbDataSource"];
            scsbMaster.UserID = scsb.UserID = ConnectionStringHelper.GetCredentialString(settings["DbUserID"]);
            scsbMaster.Password = scsb.Password = ConnectionStringHelper.GetCredentialString(settings["DbPassword"]);
            DbConnectionStr = scsb.ToString();
            DbName = scsb.InitialCatalog;

            DbMasterConnectionStr = scsbMaster.ToString();
        }
        public void Execute()
        {
            if (!Check())
                CreateDb();
        
        }
        private bool Check()
        {
            using (SqlConnection connection = new SqlConnection(DbMasterConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(DbCheckerStr, connection);
                SqlParameter param = new SqlParameter();
                param.SqlDbType = System.Data.SqlDbType.VarChar;
                param.ParameterName = "@name";
                param.Size = 4000;
                param.Value = DbName;
                command.Parameters.Add(param);
                bool result = command.ExecuteScalar() != null;
                return result;

            }
        }
        private void CreateDb()
        {
           // using (TransactionScope scope = new TransactionScope())
            using (SqlConnection connection = new SqlConnection(DbMasterConnectionStr))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("CREATE DATABASE " + DbName, connection);
                command.ExecuteNonQuery();

                command = new SqlCommand("CREATE TABLE " + DbName + ".dbo.Mail_message_sender (id BIGINT IDENTITY(1,1), from_str VARCHAR(4000), to_str VARCHAR(4000)" +
                    ", subject_str VARCHAR(4000), body_str NVARCHAR(MAX), credential_id BIGINT, count INT, date_create datetime, date_stamp datetime, state INT, CONSTRAINT PK_mail_sender PRIMARY KEY (id))", connection);
                command.ExecuteNonQuery();
              //  scope.Complete();

                command = new SqlCommand("CREATE TABLE " + DbName + ".dbo.Mail_credentials (id BIGINT IDENTITY(1,1), login VARCHAR(4000), password VARCHAR(4000), date_create datetime, date_stamp datetime, CONSTRAINT PK_mail_credentials PRIMARY KEY (id) )", connection);
                command.ExecuteNonQuery();

                command = new SqlCommand("ALTER TABLE " + DbName + ".dbo.Mail_message_sender ADD CONSTRAINT FK_message_credentials FOREIGN KEY (credential_id) " +
                    "REFERENCES " + DbName + ".dbo.Mail_credentials(id)", connection);
                command.ExecuteNonQuery();
            }            
        }
    }
}

