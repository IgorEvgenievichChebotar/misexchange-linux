using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.MisExchange.ExchangeHelpers.ExportDictionariesHelperAbstractFile
{
    abstract class ExportDictionariesHelperAbstractFile : ExportDictionariesHelper
    {
        protected ExportDirectoryHelperSettings helperSettings;

        protected ExportDictionariesHelperAbstractFile()
        {
            string exportSettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionaryExportSettings.xml");
            if (!File.Exists(exportSettingsFile))
                throw new Exception(String.Format("File [{0}] not found", exportSettingsFile));
            helperSettings = File.ReadAllText(exportSettingsFile, Encoding.UTF8).Deserialize<ExportDirectoryHelperSettings>(Encoding.UTF8);
        }

        public override void DoExport()
        {
            if (String.IsNullOrEmpty(helperSettings.SourcePath))
            {
                return;
            }
            if (!Directory.Exists(GetCorrectPath(helperSettings.SourcePath)))
            {
                Log.WriteError(String.Format("Директория [{0}] не существует", GetCorrectPath(helperSettings.SourcePath)));
                return;
            }
            try
            {
                String[] files = Directory.GetFiles(GetCorrectPath(helperSettings.SourcePath), "*.xml");
                foreach (String fileName in files)
                {
                    ProcessFile(fileName);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Невозможно экспортировать справочники. Причина: {0} \r\n {1}", ex.Message, ex.StackTrace));
            }
        }

        protected virtual void ProcessDictionary(string dictionary, string exportFileName)
        {
            switch (dictionary)
            {
                case LimsDictionaryNames.Target:
                    DefaultDictionaryExporter.ExportTargetDictionary(exportFileName);
                    break;
                case LimsDictionaryNames.Test:
                    DefaultDictionaryExporter.ExportTestDictionary(exportFileName);
                    break;
                case LimsDictionaryNames.Biomaterial:
                    DefaultDictionaryExporter.ExportBiomaterialDictionary(exportFileName);
                    break;
            }
        }

        private void ProcessFile(String fileName)
        {
            DirectoryExportRequest request = null;
            try
            {
                request = File.ReadAllText(fileName, Encoding.Unicode).Deserialize<DirectoryExportRequest>(Encoding.Unicode);
                DictionariesExportHelper.RefreshDictionaries(request.Directories);
                foreach (String directory in request.Directories)
                {
                    ProcessDictionaryEx(directory);
                }

                SaveFileToArchive(fileName);
            }
            catch (NlsServerException ex)
            {
                Log.WriteError(ex.ToString());
                SaveFileToErrors(ex.Message, fileName);
            }
        }
        private void ProcessDictionaryEx(string dictionary)
        {
            string dictionaryExportPath = GetCorrectPath(helperSettings.Output);
            if (!Directory.Exists(dictionaryExportPath))
                Directory.CreateDirectory(dictionaryExportPath);

            String exportFileName;
            if (!String.IsNullOrEmpty(dictionaryExportPath))
            {
                exportFileName = Path.Combine(dictionaryExportPath, dictionary + ".xml");
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(FileHelper.AssemblyDirectory, "Export"));
                exportFileName = Path.Combine(FileHelper.AssemblyDirectory, "Export", dictionary + ".xml");
            }
            ProcessDictionary(dictionary, exportFileName);

            Log.WriteText(String.Format("Dictionary \"{0}\" export succeeded ", dictionary));
        }
        private string GetCorrectPath(string path)
        {
            string result = path;
            if (!Path.IsPathRooted(result))
                result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            return result;
        }
        private void SaveFileToArchive(string fileName)
        {
            String archivePath = GetCorrectPath(helperSettings.Archive);
            if (!Directory.Exists(archivePath))
                Directory.CreateDirectory(archivePath);

            string destinationFileName = Path.Combine(archivePath, Path.GetFileName(fileName));
            if (File.Exists(destinationFileName))
                File.Replace(fileName, destinationFileName, null, true);
            else
                File.Copy(fileName, destinationFileName);

            File.Delete(fileName);
        }
        private void SaveFileToErrors(string errorsAndWarnings, string fileName)
        {
            String errorPath = GetCorrectPath(helperSettings.Errors); 
            if (!Directory.Exists(errorPath))
               Directory.CreateDirectory(errorPath);

            String errFileName = (Path.Combine(errorPath, Path.GetFileName(fileName)));
            string content = errorsAndWarnings + "\n\r" + File.ReadAllText(fileName, Encoding.Unicode);
            File.WriteAllText(errFileName, content);
            File.Delete(fileName);
        }

    }
}
