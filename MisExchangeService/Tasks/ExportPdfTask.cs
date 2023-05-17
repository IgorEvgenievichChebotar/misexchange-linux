using ru.novolabs;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MisExchange.User.Tasks
{
   /* public class ExportPdfTask : Task
    {
        const String NAME = "NAME";
        const String PDF = "PDF";

        private String reportPath = null;
        private String reportArchivePath = null;
        private String reportErrorPath = null;
        private String reportTableName = null;
        private String reportDBConnectionString = null;
        private OdbcCommand InsertCommand = null;
        private OdbcCommand SelectCommand = null;
        private OdbcCommand UpdateCommand = null;

        public ExportPdfTask()
        {
            try
            {

                reportPath = (String)ProgramContext.Settings["reportPath"];
                reportArchivePath = (String)ProgramContext.Settings["reportArchivePath"];
                reportErrorPath = (String)ProgramContext.Settings["reportErrorPath"];
                reportTableName = (String)ProgramContext.Settings["reportTableName"];
                reportDBConnectionString = (String)ProgramContext.Settings["reportDBConnectionString"];

                if (String.IsNullOrEmpty(reportPath) || String.IsNullOrEmpty(reportArchivePath) || String.IsNullOrEmpty(reportTableName) ||
                     String.IsNullOrEmpty(reportDBConnectionString) || String.IsNullOrEmpty(reportErrorPath))
                {
                    Log.WriteError("Не заданы параметры обработки отчетов");
                    return;
                }

                InsertCommand = new OdbcCommand(String.Format("Insert into {0} (Name, pdf) values (?, ?)", reportTableName));
                SelectCommand = new OdbcCommand(String.Format("Select count(*) from {0} where Name = ?", reportTableName));
                UpdateCommand = new OdbcCommand(String.Format("Update {0} set pdf = ? where Name = ?", reportTableName));

                InsertCommand.Parameters.Add(NAME, OdbcType.VarChar);
                InsertCommand.Parameters.Add(PDF, OdbcType.VarChar);

                SelectCommand.Parameters.Add(NAME, OdbcType.VarChar);

                UpdateCommand.Parameters.Add(PDF, OdbcType.NVarChar);
                UpdateCommand.Parameters.Add(NAME, OdbcType.NVarChar);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }
        }

        public override void Execute()
        {
            try
            {


                if (String.IsNullOrEmpty(reportPath) || String.IsNullOrEmpty(reportArchivePath) || String.IsNullOrEmpty(reportTableName) ||
                     String.IsNullOrEmpty(reportDBConnectionString) || String.IsNullOrEmpty(reportErrorPath))
                {
                    return;
                }

                ProcessReports();

            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }
        }

        private void ProcessReports()
        {
           String[] files = Directory.GetFiles(reportPath, "*.pdf");

           using (OdbcConnection dbConnection = new OdbcConnection(reportDBConnectionString))
           {
               
               foreach (String fileName in files)
               {
                   try
                   {
                       String pdf = GetFileText(fileName);
                       if (String.IsNullOrEmpty(pdf))
                       {
                           Log.WriteError(String.Format("Файл {0} не содержит данных", fileName));
                           MoveFileToErrors(fileName);
                           continue;
                       }
                       SaveFileToDb(Path.GetFileNameWithoutExtension(fileName), pdf, dbConnection);
                       MoveFileToArchive(fileName);
                       Log.WriteText(String.Format("Файл {0} успешно обработан", fileName));
                   }
                   catch (Exception ex)
                   {
                       if (String.IsNullOrEmpty(fileName))
                           Log.WriteError(String.Format("Не удалось обработать файл {0}", fileName));
                       MoveFileToErrors(fileName);
                       Log.WriteError(ex.Message);
                   }
               }
           }
        }

        private string GetFileText(string fileName)
        {
            byte[] bytes = File.ReadAllBytes(fileName);
            return BytesToStr(bytes);
        }

        private void MoveFileToArchive(string fileName)
        {
            try
            {
                string newFileDestination = Path.Combine(reportArchivePath, Path.GetFileName(fileName));
                if (File.Exists(newFileDestination))
                {
                    File.Delete(newFileDestination);
                }
                File.Copy(fileName, newFileDestination);
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }
        } 

        private void SaveFileToDb(String name, string pdf, OdbcConnection dbConnection)
        {
            InsertCommand.Connection = dbConnection;
            SelectCommand.Connection = dbConnection;
            UpdateCommand.Connection = dbConnection;

            try
            {
                if (!(dbConnection.State == System.Data.ConnectionState.Open))
                {
                    dbConnection.Open();
                }

                //SelectCommand.CommandText = "Select Name, Pdf from PdfTable";
                //OdbcDataReader r = SelectCommand.ExecuteReader();
                //DataTable result = new DataTable();
                //string col0 = r.GetDataTypeName(0);
                //string col1 = r.GetDataTypeName(1);
                //result.Load(r);

                //OdbcDataAdapter dataAdapter = new OdbcDataAdapter(SelectCommand.CommandText, dbConnection);
                //DataTable result = new DataTable();
                //dataAdapter.Fill(result);

                
                SelectCommand.Parameters[NAME].Value = name;
                Int32 rowCount = (int) SelectCommand.ExecuteScalar();

            if (rowCount > 0)
            {
                UpdateCommand.Parameters[NAME].Value = name;
                UpdateCommand.Parameters[PDF].Value = pdf;
                Object odbcUpdate = UpdateCommand.ExecuteNonQuery();
            }
            else
            {
                InsertCommand.Parameters[NAME].Value = name;
                InsertCommand.Parameters[PDF].Value = pdf;
                Object odbcInsert = InsertCommand.ExecuteNonQuery();
            }
                
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }

           
        }

        private void MoveFileToErrors(string fileName)
        {
            try
            {
                string newFileDestination = Path.Combine(reportErrorPath, Path.GetFileName(fileName));
                if (File.Exists(newFileDestination))
                {
                    File.Delete(newFileDestination);
                }
                File.Copy(fileName, newFileDestination);
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
            }
        }

        private string BytesToStr(byte[] bytes)
        {
            try
            {
                string encodedData = Convert.ToBase64String(bytes);
                return encodedData;

                //MemoryStream memStream = new MemoryStream();
                //BinaryFormatter binFormatter = new BinaryFormatter();
                //binFormatter.Serialize(memStream, bytes);
                //memStream.Seek(0, 0);
                //StreamReader strReader = new StreamReader(memStream);
                //string serializedData = strReader.ReadToEnd();
                //return serializedData;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                return null;
            }
        }


    }*/
}