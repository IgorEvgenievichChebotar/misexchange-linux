using System;
using System.IO;
using System.Text;
using SuperCore.External;
using SuperCore.BusinessObjectsLisExchange;

namespace SuperCore.Core
{
    public static class FileHelper
    {
        public static Boolean ReadRequestFromFile(String fileName, out ExternalRequest request)
        {
            return ReadRequestFromFile(fileName, Encoding.UTF8, out request);
        }

        public static Boolean ReadRequestFromFile(String fileName, Encoding encoding, out ExternalRequest request)
        {
            Boolean result = false;
            request = new ExternalRequest(); // Приходится обязательно создать экземпляр заявки, т.к. это out-параметр
            try
            {
                request = File.ReadAllText(fileName, encoding).Deserialize<ExternalRequest>(encoding);
                result = true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.ToString());
                try
                {
                    String newFileText = ex.ToString() + "\n" + File.ReadAllText(fileName, encoding);
                    File.WriteAllText((Path.Combine(ServiceContext.LisSettings.ErrorPath, Path.GetFileName(fileName))), newFileText);
                }
                catch { }
            }
            return result;
        }

        public static void SaveRequestToFile(ExternalRequest request, String fileName)
        {
            SaveRequestToFile(request, fileName, Encoding.Unicode);
        }
        
        public static void SaveRequestToFile(ExternalRequest request, String fileName, Encoding encoding)
        {            
            File.WriteAllText(fileName, request.Serialize(encoding), encoding);
        }

        public static void SaveFileToArchive(String fileName)
        {
            String sourceFileName = Path.Combine(ServiceContext.LisSettings.SourcePath, Path.GetFileName(fileName));
            String destinationFileName = Path.Combine(ServiceContext.LisSettings.ArchivePath, Path.GetFileName(fileName));
            
            if (File.Exists(destinationFileName))
                File.Replace(sourceFileName, destinationFileName, null, true);
            else
                File.Copy(sourceFileName, destinationFileName);
        }

        public static void SaveRequestToFileWithErrors(ExternalRequest request, String fileName)
        {
            request.Errors.ForEach(error => Log.WriteError(error.Message));
            FileHelper.SaveRequestToFile(request, (Path.Combine(ServiceContext.LisSettings.ErrorPath, Path.GetFileName(fileName))));
        }

        public static void SaveRequestToFileWithWarnings(ExternalRequest request, String fileName)
        {
            request.Warnings.ForEach(warning => Log.WriteError(warning.Message));
            FileHelper.SaveRequestToFile(request, (Path.Combine(ServiceContext.LisSettings.WarningPath, Path.GetFileName(fileName))));
        }        

        // Формирование имени файла результата
        public static String GetNewfileName(ExternalResult result)
        { 
            String newfileName;
            newfileName = result.RequestCode + "-Result.~";

            return Path.Combine(ServiceContext.LisSettings.ResultsPath, newfileName);              
        }

        public static void SaveResultToFile(ExternalResult result)
        {
           SaveResultToFile(result, Encoding.Unicode); 
        }

        public static void SaveResultToFile(ExternalResult result, Encoding encoding)
        {
            if (!ServiceContext.LisSettings.BackupPath.Equals(String.Empty))
                BackupResult(result); // Создание резервной копии результата по заявке
            
            String tempFileName = GetNewfileName(result);
            String destFileName;
            
            File.WriteAllText(tempFileName, result.Serialize(encoding), encoding);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
            Log.WriteText(String.Format("Сохранён результат по заявке № {0}.", result.RequestCode));
        }

        // Формирование имени файла результата для папки резервной копии
        public static String GetBackupfileName(ExternalResult result)
        {
            String newfileName;
            newfileName = result.RequestCode + "-Result.~";

            return Path.Combine(ServiceContext.LisSettings.BackupPath, newfileName);
        }

        public static void BackupResult(ExternalResult result)
        {
            BackupResult(result, Encoding.Unicode);
        }

        public static void BackupResult(ExternalResult result, Encoding encoding)
        {
            String tempFileName = GetBackupfileName(result);
            String destFileName;

            File.WriteAllText(tempFileName, result.Serialize(encoding), encoding);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
        }

        public static Boolean ReadRequestFilterFromFile(String fileName, out ExternalRequestFilter externalFilter)
        {
            Boolean result = false;
            externalFilter = new ExternalRequestFilter(); // Приходится обязательно создать экземпляр фильтра заявок, т.к. это out-параметр
            try
            {
                externalFilter = File.ReadAllText(fileName).Deserialize<ExternalRequestFilter>();
                result = true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                String NewFileText = ex.Message + "\n" + File.ReadAllText(fileName);
                File.WriteAllText((Path.Combine(ServiceContext.LisSettings.ErrorPath, Path.GetFileName(fileName))), NewFileText);
            }
            return result;
        }

        public static void SaveRequestFilterToFile(ExternalRequestFilter filter, String fileName)
        {
            SaveRequestFilterToFile(filter, fileName, Encoding.Unicode);
        } 
        
        public static void SaveRequestFilterToFile(ExternalRequestFilter filter, String fileName, Encoding encoding)
        {
            File.WriteAllText(fileName, filter.Serialize(encoding), encoding);
        }
        
        public static void SaveRequestFilterToFileWithErrors(ExternalRequestFilter filter, String fileName)
        {
            filter.Errors.ForEach(error => Log.WriteError(error.Message));
            FileHelper.SaveRequestFilterToFile(filter, (Path.Combine(ServiceContext.LisSettings.ErrorPath, Path.GetFileName(fileName))));
        }

        public static Boolean ReadEventFromFile(String fileName, out ExternalEvent externalEvent)
        {
            Boolean result = false;
            externalEvent = new ExternalEvent(); // Приходится обязательно создать экземпляр события, т.к. это out-параметр

            try
            {
                externalEvent = File.ReadAllText(fileName).Deserialize<ExternalEvent>();
                result = true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                String NewFileText = ex.Message + "\n" + File.ReadAllText(fileName);
                File.WriteAllText((Path.Combine(ServiceContext.LisSettings.ErrorPath, Path.GetFileName(fileName))), NewFileText);
            }
            
            return result;
        }
    }
}
