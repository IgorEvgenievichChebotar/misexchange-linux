using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses;
using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.DictionaryExport;
using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.ResearchResult;
using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.Service;
using ru.novolabs.MisExchange.HelperDependencies;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork
{
    [ExchangeHelperName("ExchangeHelper3_Medwork")]
    class MedworkExchangeHelper3 : ExchangeHelper3
    {
        private HelperSettings helperSettings = new HelperSettings();

        public MedworkExchangeHelper3(IDictionaryCache dictionaryCache)
        {
            helperSettings = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
            MedworkParser.HelperSettings = helperSettings;
        }

        private string GetCorrectPath(string path)
        {
            string result = path;
            if (!Path.IsPathRooted(result))
                result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            return result;
        }

        protected override void SpecificProcessNewData()
        {
            List<string> sourceDirectories = new List<string>();
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
            {
                sourceDirectories.Add(GetCorrectPath(helperSettings.SourcePath));
            }
            else
            {
                helperSettings.FileGroupDirectioriesList.ForEach(d => sourceDirectories.Add(GetCorrectPath(d.SourcePath)));
            }

            foreach (string requestsPath in sourceDirectories)
            {
                if (!Directory.Exists(requestsPath))
                {
                    Log.WriteError("Source directory [{0}] not exist", requestsPath);
                    return;
                }
                try
                {
                    String[] files = Directory.GetFiles(requestsPath, "*.xml");
                    foreach (String fileName in files)
                    {
                        ProcessFile(fileName);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Невозможно обработать директорию [{0}]. Причина: {1}", requestsPath, ex.ToString()));
                }
            }
        }

        private void saveAcknowledgment(String requestCode, String status, String comment)
        {
            String requestAcknowledgmentPath = GetCorrectPath(helperSettings.RequestAcknowledgmentPath);
            if (!Directory.Exists(requestAcknowledgmentPath))
                Directory.CreateDirectory(requestAcknowledgmentPath);

            String acknowledgmentFileName = requestCode + "-Acknowledgment.xml";
            Acknowledgment acknowledgment = new Acknowledgment() { RequestCode = requestCode, Status = status, Comment = comment};
            File.WriteAllText(Path.Combine(requestAcknowledgmentPath, acknowledgmentFileName), acknowledgment.Serialize(Encoding.UTF8));
        }

        private void ProcessFile(String fileName)
        {
            ExchangeDTOs.Request requestDTO = null;
            try
            {
                requestDTO = File.ReadAllText(fileName, Encoding.UTF8).Deserialize<ExchangeDTOs.Request>(Encoding.UTF8);
                ProcessRequest(requestDTO);
                CacheRequestMessage(requestDTO, null);
                SaveFileToArchive(fileName);
                saveAcknowledgment(requestDTO.RequestCode, StatusTypes.STATUS_SUCCESS, String.Empty);
            }
            catch (NlsServerException ex)
            {
                CacheRequestMessage(requestDTO, ex.ToString());
                Log.WriteError(ex.ToString());
                SaveFileToErrors(ex.Message, fileName);
                saveAcknowledgment(requestDTO.RequestCode, StatusTypes.STATUS_ERROR, ex.ToString());
            }
            catch (CustomDataCheckException ex)
            {
                CacheRequestMessage(requestDTO, ex.ToString());
                Log.WriteError(ex.ToString());
                SaveFileToErrors(String.Join(";\n", ex.Errors), fileName);
                saveAcknowledgment(requestDTO.RequestCode, StatusTypes.STATUS_ERROR, ex.ToString());
            }
            catch (Exception ex)
            {
                if (requestDTO != null)
                {
                    CacheRequestMessage(requestDTO, ex.ToString());
                    saveAcknowledgment(requestDTO.RequestCode, StatusTypes.STATUS_ERROR, ex.ToString());
                }
                SaveFileToErrors(ex.Message, fileName);
                Log.WriteError(String.Format("Невозможно обработать файл \"{0}\": {1}", fileName, ex.ToString()));
            }
        }
        #region For Test Purposes
        private void CacheRequestMessage(ru.novolabs.ExchangeDTOs.Request request, string errorStr)
        {
            if (helperSettings.IsEnabledMessageCaching)
            {
                Common.StatusObjectCache status = String.IsNullOrEmpty(errorStr) ? Common.StatusObjectCache.Completed : Common.StatusObjectCache.Error;                
                StoreRequest(request, status , errorStr, false);           
            }
        
        }

        private void CacheResultMessage(ExchangeDTOs.Result result, string errorStr)
        {
            if (helperSettings.IsEnabledMessageCaching)
            {
                Common.StatusObjectCache status = String.IsNullOrEmpty(errorStr) ? Common.StatusObjectCache.Completed : Common.StatusObjectCache.Error;
                StoreResult(result, status, errorStr, false);
            }

        }
        #endregion

        private void SaveFileToErrors(string errorsAndWarnings, string fileName)
        {
            string errorPath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                errorPath = GetCorrectPath(helperSettings.ErrorPath);
            else
                errorPath = GetDirectoryPathForGroup(Path.GetDirectoryName(fileName), "Errors");
            Directory.CreateDirectory(errorPath);

            String errFileName = (Path.Combine(errorPath, Path.GetFileName(fileName)));
            File.Copy(fileName, errFileName, true);
            File.Delete(fileName);
        }

        private string GetResultsPathForGroup(string groupCode)
        {
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
            {
                return helperSettings.ResultsPath;
            }
            else
            {
                FileGroupDirectories dirs = helperSettings.FileGroupDirectioriesList.Find(g => g.GroupCode == groupCode);
                if (dirs != null)
                    return dirs.ResultsPath;
                else
                    return String.Empty;
            }
        }


        private string GetDirectoryPathForGroup(string sourcePath, string directoryType)
        {
            string result = String.Empty;
            FileGroupDirectories dirs = helperSettings.FileGroupDirectioriesList.Find(g => g.SourcePath == sourcePath);
            if (dirs != null)
            {
                switch (directoryType)
                {
                    case "Errors":
                        result = dirs.ErrorPath;
                        break;
                    case "Archive":
                        result = dirs.ArchivePath;
                        break;
                    case "Results":
                        result = dirs.ResultsPath;
                        break;
                    default:
                        break;
                }
            }
            if (String.IsNullOrEmpty(result))
                result = Path.Combine(sourcePath, directoryType);

            return result;
        }

        private void SaveFileToArchive(string fileName)
        {
            string archivePath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                archivePath = GetCorrectPath(helperSettings.ArchivePath);
            else
                archivePath = GetDirectoryPathForGroup(Path.GetDirectoryName(fileName), "Archive");
            Directory.CreateDirectory(archivePath);

            string destinationFileName = Path.Combine(archivePath, Path.GetFileName(fileName));
            if (File.Exists(destinationFileName))
                File.Replace(fileName, destinationFileName, null, true);
            else
                File.Copy(fileName, destinationFileName);

            File.Delete(fileName);
        }

        //protected override bool IsFilterRequestResults { get { return false; } } // TO-DO: Выяснить, нужна ли фильтрация

        // Формирование имени файла результата для папки резервной копии
        private string GetBackupfileName(String requestCode)
        {
            String backupPath = helperSettings.BackupPath;
            if (!Path.IsPathRooted(backupPath))
                backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backupPath);

            String newfileName;
            newfileName = requestCode + "-Result.~";

            return Path.Combine(backupPath, newfileName);
        }

        private void BackupResult(Result result, Encoding encoding)
        {
            String tempFileName = GetBackupfileName(result.RequestCode);
            String destFileName;

            File.WriteAllText(tempFileName, result.Serialize(encoding), encoding);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
        }

        // Формирование имени файла результата
        private String GetResultFileName(Result result)
        {
            String resultsPath = GetResultsPath(result.HospitalCode);
            String newfileName;
            newfileName = result.RequestCode + "-Result.~";
            return Path.Combine(resultsPath, newfileName);
        }

        private string GetResultsPath(String resultHospitalCode)
        {
            string resultPath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                resultPath = GetCorrectPath(helperSettings.ResultsPath);
            else
                resultPath = GetResultsPathForGroup(GetGroupCode(resultHospitalCode));

            return resultPath;
        }

        private string GetGroupCode(String resultHospitalCode)
        {
            if (helperSettings.FileGroupingMode == FileGroupingMode.ByHospital)
                return resultHospitalCode;
            else
                return String.Empty;
        }

        public void SaveResultToFile(ExchangeDTOs.Result result)
        {
            Result medworkResult = MedworkParser.PrepareResult(result);

            var backupPath = helperSettings.BackupPath;
            if (!String.IsNullOrEmpty(backupPath))
            {
                if (!Path.IsPathRooted(backupPath))
                    backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backupPath);

                BackupResult(medworkResult, Encoding.Unicode);
            }

            String tempFileName = GetResultFileName(medworkResult);
            String destFileName;

            File.WriteAllText(tempFileName, medworkResult.Serialize(Encoding.UTF8), Encoding.UTF8);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
            Log.WriteText(String.Format("Сохранён результат по заявке № {0}.", result.RequestCode));
        }

        public override void SpecificStoreResults(List<ExchangeDTOs.Result> results)
        {
            foreach (ExchangeDTOs.Result result in results)
            {
                string resultsPath = GetResultsPath(result.HospitalCode);
                if (Directory.Exists(resultsPath))
                {
                    try
                    {
                        SaveResultToFile(result);
                        CacheResultMessage(result, null);
                    }
                    catch (Exception ex)
                    {
                        CacheResultMessage(result, ex.ToString());
                        Log.WriteError(String.Format("Ошибка при сохранении ответа по заявке №{0}:{1}", result.RequestCode, ex.Message));
                    }
                }
                else
                {
                    string errorStr = String.Format("Ошибка при сохранении ответов по заявкам: Директория для ответов [{0}] не существует", resultsPath);
                    CacheResultMessage(result,errorStr);
                    Log.WriteError(errorStr);
                }
            }
        }
    }
}