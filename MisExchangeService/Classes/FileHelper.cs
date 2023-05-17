using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using ru.novolabs.SuperCore;

namespace ru.novolabs.MisExchange.Classes
{
    public class FileHelper
    {
        /// <summary>
        /// Путь к файлу сборки
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private HelperSettings helperSettings { get; set; }
        public Encoding Encoding { get; private set; }
        private FileHelper()
        { }
        public FileHelper(HelperSettings helperSettings)
        {
            this.helperSettings = helperSettings;
            this.Encoding = GetEncoding(helperSettings.EncodingCodePage);
        }
        private Encoding GetEncoding(string codePage)
        {
            codePage = codePage.ToLower();
            if (codePage == "utf-8")
                return Encoding.UTF8;
            if (codePage == "utf-8notbom")
                return new UTF8Encoding(false);
            if (codePage == "utf-16" || codePage == "utf-16le")
                return Encoding.Unicode;
            if (codePage == "utf-16be")
                return Encoding.BigEndianUnicode;
            Int32 codePageNum;
            if (Int32.TryParse(codePage, out codePageNum))
                return Encoding.GetEncoding(codePageNum);
            return Encoding.GetEncoding(codePage);
        }


        public List<string> GetSourceDirectories()
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
            return sourceDirectories;
        }
        public void SaveFileToArchive(string fileName)
        {
            string archivePath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                archivePath = GetCorrectPath(helperSettings.ArchivePath);
            else
                archivePath = GetDirectoryPathForGroup(Path.GetDirectoryName(fileName), "Archive");
            Directory.CreateDirectory(archivePath);

            string destinationFileName = Path.Combine(archivePath, Path.GetFileNameWithoutExtension(fileName) + "_(" + DateTime.Now.ToString("HH-mm-ss-fff") + ")" + Path.GetExtension(fileName));
            if (File.Exists(destinationFileName))
                File.Replace(fileName, destinationFileName, null, true);
            else
                File.Copy(fileName, destinationFileName);

            File.Delete(fileName);
        }
        public void SaveFileToErrors(string errorsAndWarnings, string fileName)
        {
            string errorPath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                errorPath = GetCorrectPath(helperSettings.ErrorPath);
            else
                errorPath = GetDirectoryPathForGroup(Path.GetDirectoryName(fileName), "Errors");
            Directory.CreateDirectory(errorPath);

            String errFileName = (Path.Combine(errorPath, Path.GetFileNameWithoutExtension(fileName) + "_(" + DateTime.Now.ToString("HH-mm-ss-fff") + ")" + Path.GetExtension(fileName)));
            string content = errorsAndWarnings + "\n\r" + File.ReadAllText(fileName, Encoding);
            File.WriteAllText(errFileName, content);
            File.Delete(fileName);
        }
        public void SaveResultToFile(ExchangeDTOs.Result result, object content = null)
        {
            var backupPath = helperSettings.BackupPath;
            if (content == null)
                content = result;
            if (!String.IsNullOrEmpty(backupPath))
            {
                if (!Path.IsPathRooted(backupPath))
                    backupPath = Path.Combine(AssemblyDirectory, backupPath);

                BackupResult(result, Encoding, content);
            }

            String tempFileName = GetResultFileName(result);
            String destFileName;

            File.WriteAllText(tempFileName, content.Serialize(Encoding).RemoveNilElements(System.Xml.Linq.LoadOptions.PreserveWhitespace), Encoding);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
            Log.WriteText(String.Format("Сохранён результат по заявке № {0}.", result.RequestCode));
        }
        public string GetResultsPath(ExchangeDTOs.Result result)
        {
            string resultPath;
            if (helperSettings.FileGroupingMode == FileGroupingMode.None)
                resultPath = GetCorrectPath(helperSettings.ResultsPath);
            else
                resultPath = GetResultsPathForGroup(GetGroupCode(result));

            return resultPath;
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
        // Формирование имени файла результата
        private String GetResultFileName(ExchangeDTOs.Result result)
        {
            String resultsPath = GetResultsPath(result);
            String newfileName;
            newfileName = result.RequestCode + "-Result.~";
            return Path.Combine(resultsPath, newfileName);
        }

        private string GetGroupCode(ExchangeDTOs.Result result)
        {
            if (helperSettings.FileGroupingMode == FileGroupingMode.ByHospital)
                return result.HospitalCode;
            else
                return String.Empty;
        }

        private string GetCorrectPath(string path)
        {
            string result = path;
            if (!Path.IsPathRooted(result))
                result = Path.Combine(AssemblyDirectory, path);

            return result;
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

        // Формирование имени файла результата для папки резервной копии
        private string GetBackupfileName(ExchangeDTOs.Result result)
        {
            String backupPath = helperSettings.BackupPath;
            if (!Path.IsPathRooted(backupPath))
                backupPath = Path.Combine(AssemblyDirectory, backupPath);

            String newfileName;
            newfileName = result.RequestCode + "-Result.~";

            return Path.Combine(backupPath, newfileName);
        }

        private void BackupResult(ExchangeDTOs.Result result, Encoding encoding, object content = null)
        {
            String tempFileName = GetBackupfileName(result);
            String destFileName;
            if (content == null)
                content = result;
            File.WriteAllText(tempFileName, content.Serialize(encoding), encoding);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
        }
    }
}
