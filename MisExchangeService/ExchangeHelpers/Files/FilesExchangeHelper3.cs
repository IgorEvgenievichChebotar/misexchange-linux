using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.HelperDependencies;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ru.novolabs.MisExchange.ExchangeHelpers.Files
{
    // В каждом ExchangeHelper-е необходимо переопределить 4 метода, отвечающих за специфическую обработку данных:
    //
    // - SpecificProcessNewData() - специфическим образом обрабатывает новые входящие данные. В реализации этого метода нужно вызвать унаследованный метод ProcessRequestsData()
    // - SpecificFilterRequestResults() - cпецифическим образом фильтрует результаты по заявкам
    // - SpecificStoreResults() - специфическим образом сохраняет список результатов
    // - SpecificExportDictionaries() - специфическим образом экспортирует во внешнюю систему справочники

    [Obsolete("This is old version of FileHelper, please, prefer FileExchangeHelper3New")]
    [ExchangeHelperName("ExchangeHelper3_Files")]
    class FilesExchangeHelper3 : ExchangeHelper3
    {
        private HelperSettings helperSettings = null;

        public FilesExchangeHelper3()
        {
            helperSettings = File.ReadAllText(Path.Combine(FileHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
        }

        private string GetCorrectPath(string path)
        {
            string result = path;
            if (!Path.IsPathRooted(result))
                result = Path.Combine(FileHelper.AssemblyDirectory, path);

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

        private void ProcessFile(String fileName)
        {
            ExchangeDTOs.Request requestDTO = null;
            try
            {
                requestDTO = File.ReadAllText(fileName, Encoding.Unicode).Deserialize<ExchangeDTOs.Request>(Encoding.Unicode);
                ProcessRequest(requestDTO);
                CacheRequestMessage(requestDTO, null);
                SaveFileToArchive(fileName);
            }
            catch (NlsServerException ex)
            {
                CacheRequestMessage(requestDTO, ex.ToString());
                Log.WriteError(ex.ToString());
                SaveFileToErrors(ex.Message, fileName);
            }
            catch (CustomDataCheckException ex)
            {
                CacheRequestMessage(requestDTO, ex.ToString());
                Log.WriteError(ex.ToString());
                SaveFileToErrors(String.Join(";\n", ex.Errors), fileName);
            }
            catch (Exception ex)
            {
                if (requestDTO != null)
                {
                    CacheRequestMessage(requestDTO, ex.ToString());
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

        private void CacheResultMessage(ru.novolabs.ExchangeDTOs.Result result, string errorStr)
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
                //errorPath = Path.Combine(Path.GetDirectoryName(fileName), "Errors");
                errorPath = GetDirectoryPathForGroup(Path.GetDirectoryName(fileName), "Errors");
            Directory.CreateDirectory(errorPath);

            String errFileName = (Path.Combine(errorPath, Path.GetFileName(fileName)));
            string content = errorsAndWarnings + "\n\r" + File.ReadAllText(fileName, Encoding.Unicode);
            File.WriteAllText(errFileName, content);
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

        /*  private void SaveRequestToFileWithErrors(Object request, String errorsAndWarnings, string fileName)
          {
              string content = errorsAndWarnings;
              content += "\n\r" + request.Serialize(Encoding.Unicode);
              SaveFileToErrors(fileName, content);
          }*/

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

        protected override bool IsFilterRequestResults { get { return false; } }

        // Формирование имени файла результата для папки резервной копии
        private string GetBackupfileName(ExchangeDTOs.Result result)
        {
            String backupPath = helperSettings.BackupPath;
            if (!Path.IsPathRooted(backupPath))
                backupPath = Path.Combine(FileHelper.AssemblyDirectory, backupPath);

            String newfileName;
            newfileName = result.RequestCode + "-Result.~";

            return Path.Combine(backupPath, newfileName);
        }

        private void BackupResult(ExchangeDTOs.Result result, Encoding encoding)
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

        // Формирование имени файла результата
        private String GetResultFileName(ExchangeDTOs.Result result)
        {
            String resultsPath = GetResultsPath(result);
            String newfileName;
            newfileName = result.RequestCode + "-Result.~";
            return Path.Combine(resultsPath, newfileName);
        }

        private string GetResultsPath(ExchangeDTOs.Result result)
        {
            string resultPath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                resultPath = GetCorrectPath(helperSettings.ResultsPath);
            else
                resultPath = GetResultsPathForGroup(GetGroupCode(result));

            return resultPath;
        }

        private string GetGroupCode(ExchangeDTOs.Result result)
        {
            if (helperSettings.FileGroupingMode == FileGroupingMode.ByHospital)
                return result.HospitalCode;
            else
                return String.Empty;
        }

        public void SaveResultToFile(ExchangeDTOs.Result result)
        {
            var backupPath = helperSettings.BackupPath;
            if (!String.IsNullOrEmpty(backupPath))
            {
                if (!Path.IsPathRooted(backupPath))
                    backupPath = Path.Combine(FileHelper.AssemblyDirectory, backupPath);

                BackupResult(result, Encoding.Unicode);
            }

            String tempFileName = GetResultFileName(result);
            String destFileName;

            File.WriteAllText(tempFileName, result.Serialize(Encoding.Unicode).RemoveNilElements(System.Xml.Linq.LoadOptions.PreserveWhitespace), Encoding.Unicode);
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
                string resultsPath = GetResultsPath(result);
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
                    CacheResultMessage(result,errorStr );
                    Log.WriteError(errorStr);
                }
            }
        }
    }
}