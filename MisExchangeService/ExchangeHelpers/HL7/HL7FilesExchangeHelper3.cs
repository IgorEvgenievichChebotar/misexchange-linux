using MisExchangeAdapters.Parser.HL7;
using ru.novolabs.HL7.Files;
using ru.novolabs.MisExchange.HelperDependencies;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.HL7
{
    [ExchangeHelperName("ExchangeHelper3_HL7Files")]
    class HL7FilesExchangeHelper3: ExchangeHelper3
    {
        private HelperSettings helperSettings = null;
        private string cacheDirectoryPath;
        private static Encoding EncodingUTF8NotBOM = new UTF8Encoding(false);
        public HL7FilesExchangeHelper3()
        {
            helperSettings 
                = File.ReadAllText(Path.Combine(PathHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
           // HL7MessageParser.HelperSetting = helperSettings;
            cacheDirectoryPath = Path.Combine(PathHelper.AssemblyDirectory, "CacheMessages");
            if (!Directory.Exists(cacheDirectoryPath))
            {
                Directory.CreateDirectory(cacheDirectoryPath);
            }
        }

        private String GetCorrectPath(String path)
        {
            String result = path;
            if (!Path.IsPathRooted(result))
                result = Path.Combine(PathHelper.AssemblyDirectory, path);
            return result;
        }

        protected override void SpecificProcessNewData()
        {
            List<String> sourceDirectories = new List<string>();
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
            {
                sourceDirectories.Add(GetCorrectPath(helperSettings.SourcePath));
            }
            else
            {
                helperSettings.FileGroupDirectioriesList.ForEach(d => sourceDirectories.Add(GetCorrectPath(d.SourcePath)));
            }

            foreach (String requestsPath in sourceDirectories)
            {
                try
                {
                 //   String[] files = Directory.GetFiles(requestsPath, "*.xml");
                    String[] files = Directory.GetFiles(requestsPath);
                    foreach (String fileName in files)
                    {
                        ExchangeDTOs.Request requestDTO = null;
                        try
                        {
                            String message = File.ReadAllText(fileName, Encoding.UTF8);
                            requestDTO = HL7MessageParser.BuildDTORequest(message);
                            requestDTO.Patient.Code = helperSettings.PatientCodePrefix + requestDTO.Patient.Code;
                            requestDTO.HospitalCode = helperSettings.HospitalCode;
                            requestDTO.HospitalName = helperSettings.HospitalName;
                            ProcessRequest(requestDTO);
                            SaveFileToArchive(fileName,requestDTO.RequestCode);
                        }
                        catch (NlsServerException ex)
                        {
                            Log.WriteError(ex.ToString());
                            SaveFileToErrors(ex.Message, fileName);
                        }
                        catch (CustomDataCheckException ex)
                        {
                            Log.WriteError(ex.ToString());
                            SaveFileToErrors(String.Join(";\n", ex.Errors), fileName);
                        }
                        catch (Exception ex)
                        {
                            SaveFileToErrors(ex.Message, fileName);
                            Log.WriteError(String.Format("Невозможно обработать файл \"{0}\": {1}", fileName, ex.ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error in SpecificProcessNewData: {0}", ex.ToString()));
                }
            }
        }

        private void SaveFileToErrors(String errorsAndWarnings, String fileName)
        {
            String errorPath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                errorPath = GetCorrectPath(helperSettings.ErrorPath);
            else
                errorPath = GetDirectoryPathForGroup(Path.GetDirectoryName(fileName), "Errors");
            Directory.CreateDirectory(errorPath);

            String errFileName = (Path.Combine(errorPath, Path.GetFileName(fileName)));
            String content = errorsAndWarnings + "\n\r" + File.ReadAllText(fileName, EncodingUTF8NotBOM);
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

        private void SaveFileToArchive(string fileName, string requestCode)
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
            CacheMessageContentInFile(requestCode, destinationFileName);
            File.Delete(fileName);
        }

        private void CacheMessageContentInFile(string requestCode, string archiveFileName)
        {
            string fileNameCache = Path.Combine(cacheDirectoryPath, "RC-" + requestCode + ".cch");
            File.Copy(archiveFileName, fileNameCache);
        }

        private string GetBackupfileName(ExchangeDTOs.Result result)
        {
            String backupPath = helperSettings.BackupPath;
            if (!Path.IsPathRooted(backupPath))
                backupPath = Path.Combine(PathHelper.AssemblyDirectory, backupPath);

            String newfileName;
            newfileName = result.RequestCode + "-Result.~";

            return Path.Combine(backupPath, newfileName);
        }

        private void BackupResult(ExchangeDTOs.Result result, string dataCache, Encoding encoding)
        {
            String tempFileName = GetBackupfileName(result);
            String destFileName;

            List<String> res = HL7MessageParser.BuildHL7Results(result,dataCache);
            String strRes = String.Join("\r", res);
            File.WriteAllText(tempFileName, strRes, encoding);

            //File.WriteAllText(tempFileName, result.Serialize(encoding), encoding);
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

        public void SaveResultToFile(ExchangeDTOs.Result result, string dataCache)
        {
            var backupPath = helperSettings.BackupPath;
            if (!String.IsNullOrEmpty(backupPath))
            {
                if (!Path.IsPathRooted(backupPath))
                    backupPath = Path.Combine(PathHelper.AssemblyDirectory, backupPath);

                BackupResult(result, dataCache, EncodingUTF8NotBOM);
            }

            String tempFileName = GetResultFileName(result);
            String destFileName;
            result.Patient.Code = result.Patient.Code.Substring(helperSettings.PatientCodePrefix.Length);
            List<String> res = HL7MessageParser.BuildHL7Results(result, dataCache);
            String strRes = String.Join("\r", res);
            File.WriteAllText(tempFileName, strRes, EncodingUTF8NotBOM);
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
                        string fileCacheName = Path.Combine(cacheDirectoryPath, "RC-" + result.RequestCode + ".cch");
                        string data = null;
                        if (File.Exists(fileCacheName))
                        {
                            data = File.ReadAllText(fileCacheName, Encoding.UTF8);
                        }
                        SaveResultToFile(result, data);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Ошибка при сохранении ответа по заявке №{0}:{1}", result.RequestCode, ex.ToString()));
                    }
                }
                else
                    Log.WriteError("Ошибка при сохранении ответов по заявкам: Директория для ответов [{0}] не существует", resultsPath);
            }
        }
    }
}
