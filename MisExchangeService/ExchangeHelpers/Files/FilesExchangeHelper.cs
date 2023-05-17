using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchangeService.Adapters;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files
{
    // В каждом ExchangeHelper-е необходимо переопределить 6 методов, отвечающих за специфическую обработку данных:
    //
    // - SpecificProcessNewData() - специфическим образом обрабатывает новые входящие данные. В реализации этого метода нужно вызвать унаследованный метод ProcessRequestsData()
    // - SpecificGetRequests() - специфическим образом получает список заявок из внешней системы
    // - SpecificFinishRequestsProcessing() - выполняет специфические действия после обработки новых заявок. Например, оповещение внешней системы об успешной обработке группы заявок, обработка ошибок
    // - SpecificFilterRequestResults() - cпецифическим образом фильтрует результаты по заявкам
    // - SpecificStoreResults() - специфическим образом сохраняет список результатов
    // - SpecificExportDictionaries() - специфическим образом экспортирует во внешнюю систему справочники

    [ExchangeHelperName("ExchangeHelper_Files_old")]
    class FilesExchangeHelper : ExchangeHelper
    {
        private HelperSettings helperSettings = null;

        public FilesExchangeHelper()
        {
            helperSettings = File.ReadAllText(Path.Combine(FileHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
        }

        protected override void SpecificProcessNewData()
        {
            ProcessRequestsData();
            // ProcessFilters()
            // etc.
        }

        protected override void SpecificGetRequests(object[] parameters, out List<ExternalRequest> requests)
        {
            string requestsPath = helperSettings.SourcePath;
            if (!Path.IsPathRooted(requestsPath))
                requestsPath = Path.Combine(FileHelper.AssemblyDirectory, requestsPath);

            requests = new List<ExternalRequest>();

            try
            {
                // Проверяем, существует ли директория.
                if (Directory.Exists(requestsPath))
                {
                    try
                    {
                        // Получаем список файлов c расширением .xml в директории "SourcePath"
                        String[] files = Directory.GetFiles(requestsPath, "*.xml");
                        foreach (String fileName in files)
                        {
                            try
                            {
                                // Следующие две строки пригодятся в случае, если понадобится использовать классы, соответствующие версии протокола обмена 2.0 и старый запрос create-requests
                                //var requestDTO = File.ReadAllText(fileName, Encoding.Unicode).Deserialize<ExchangeDTOs.Request>(Encoding.Unicode);
                                //var request = new FilesRequestAdapter().ReadDTO(requestDTO);
                                
                                var request = File.ReadAllText(fileName, Encoding.Unicode).Deserialize<ExternalRequest>(Encoding.Unicode);
                                request.ExtraInfo = new object[] { fileName/*, requestDTO*/ };
                                requests.Add(request);
                            }
                            catch (Exception ex)
                            {
                                Log.WriteError(String.Format("Невозможно обработать файл \"{0}\": {1}", fileName, ex.ToString()));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Невозможно обработать директорию с заявками: {0}", ex.ToString()));
                    }
                }
                else
                    Log.WriteError("Source directory not exist");
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("The process failed: {0}", ex.Message));
            }
            finally { }
        }

        protected override void SpecificFinishRequestsProcessing(List<ExternalRequest> requests)
        {
            foreach (var request in requests)
            {
                var extraInfo = (object[])request.ExtraInfo;
                string fileName = (string)extraInfo[0];
                //var requestDTO = (ExchangeDTOs.Request)extraInfo[1];

                // Обрабатываем ошибки, предупреждения
                var errorsAndWarnings = new List<ErrorMessage>();
                errorsAndWarnings.AddRange(request.Errors.Union(request.Warnings));

                if ((ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_DEBUG) && (errorsAndWarnings.Count == 0))
                    Log.WriteText(String.Format("Requests  \"{0}\"  successfully created.", String.Join(", ", requests.Select(r => r.InternalNr))));

                if (errorsAndWarnings.Count == 0)
                {
                    // Копируем заявку в архив;
                    SaveFileToArchive(fileName);
                }
                else
                {
                    // Помещаем файл с сообщениями об ошибках и xml-описанием заявки в директорию с ошибками
                    SaveRequestToFileWithErrors(request, errorsAndWarnings, fileName);
                }
                if ((ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_DEBUG) && (request.Errors.Count == 0))
                    Log.WriteText(String.Format("Request  \"{0}\"  successfully created.", request.InternalNr));
                // Удаляем файл из Source папки 
                File.Delete(fileName);
            }
        }

        private void SaveRequestToFileWithErrors(ExternalRequest request, List<ErrorMessage> errorsAndWarnings, string fileName)
        {
            String errorPath = helperSettings.ErrorPath;
            if (!Path.IsPathRooted(errorPath))
                errorPath = Path.Combine(FileHelper.AssemblyDirectory, errorPath);
            if (!Directory.Exists(errorPath))
            {
                errorPath = Path.Combine(FileHelper.AssemblyDirectory, "Errors");
                Directory.CreateDirectory(errorPath);
            }
            errorsAndWarnings.ForEach(error => Log.WriteError(error.Message));
            fileName = (Path.Combine(errorPath, Path.GetFileName(fileName)));
            string contents = String.Join("\n", errorsAndWarnings.Select(e => e.Message));
            contents += "\n\r" + request.Serialize(Encoding.Unicode);
            File.WriteAllText(fileName, contents);
        }

        // Следующая версия функции пригодятися в случае, если понадобится использовать классы, соответствующие версии протокола обмена 2.0 и старый запрос create-requests
        //private void SaveRequestToFileWithErrors(ExchangeDTOs.Request request, List<ErrorMessage> errorsAndWarnings, string fileName)
        //{
        //    String errorPath = helperSettings.ErrorPath;
        //    if (!Directory.Exists(errorPath))
        //    {
        //        errorPath = Path.Combine(Application.StartupPath, "Errors");
        //        Directory.CreateDirectory(errorPath);
        //    }
        //    errorsAndWarnings.ForEach(error => Log.WriteError(error.Message));
        //    fileName = (Path.Combine(errorPath, Path.GetFileName(fileName)));
        //    string contents = String.Join("\n", errorsAndWarnings.Select(e => e.Message));
        //    contents += "\n\r" + request.Serialize(Encoding.Unicode);
        //    File.WriteAllText(fileName, contents); 
        //}

        private void SaveFileToArchive(string fileName)
        {
            String sourcePath = helperSettings.SourcePath;
            if (!Path.IsPathRooted(sourcePath))
                sourcePath = Path.Combine(FileHelper.AssemblyDirectory, sourcePath);

            String archivePath = helperSettings.ArchivePath;
            if (!Path.IsPathRooted(archivePath))
                archivePath = Path.Combine(FileHelper.AssemblyDirectory, archivePath);
            Directory.CreateDirectory(archivePath);

            String sourceFileName = Path.Combine(sourcePath, Path.GetFileName(fileName));
            String destinationFileName = Path.Combine(archivePath, Path.GetFileName(fileName));

            if (File.Exists(destinationFileName))
                File.Replace(sourceFileName, destinationFileName, null, true);
            else
                File.Copy(sourceFileName, destinationFileName);
        }

        protected override void SpecificFilterRequestResults(List<ObjectRef> requestIds)
        {
            /* nop */
        }

        protected override void SpecificStoreResults(List<ExternalResult> results)
        {
            string resultsPath = helperSettings.ResultsPath;
            if (!Path.IsPathRooted(resultsPath))
                resultsPath = Path.Combine(FileHelper.AssemblyDirectory, resultsPath);

            if (Directory.Exists(resultsPath))
            {
                foreach (ExternalResult result in results)
                {
                    try
                    {
                        var resultDTO = (ExchangeDTOs.Result)(new ExternalResultAdapterBase().WriteDTO(result));
                        SaveResultToFile(resultDTO);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Ошибка при сохранении ответа по заявке №{0}:{1}", result.RequestCode, ex.Message));
                    }
                }
            }
            else
                Log.WriteError("Ошибка при сохранении ответов по заявкам: Директория для ответов не существует");
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

            File.WriteAllText(tempFileName, result.Serialize(Encoding.Unicode).RemoveNilElements(), Encoding.Unicode);
            destFileName = Path.ChangeExtension(tempFileName, ".xml");

            if (File.Exists(destFileName))
                File.Replace(tempFileName, destFileName, null, true);
            else
                File.Copy(tempFileName, destFileName);

            File.Delete(tempFileName);
            Log.WriteText(String.Format("Сохранён результат по заявке № {0}.", result.RequestCode));
        }

        // Формирование имени файла результата
        private String GetResultFileName(ExchangeDTOs.Result result)
        {
            String resultsPath = helperSettings.ResultsPath;
            if (!Path.IsPathRooted(resultsPath))
                resultsPath = Path.Combine(FileHelper.AssemblyDirectory, resultsPath);
            String newfileName;
            newfileName = result.RequestCode + "-Result.~";

            return Path.Combine(resultsPath, newfileName);
        }

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

//        protected override void SpecificExportDictionaries()
//        {
//            // todo: Отказаться от использования этого метода в пользу специфического ExportDictionariesHelper-а
//            //DictionariesExportHelper.ExportDictionaries();
//            /*DictionariesExportHelper.ExportDictionary(LisDictionaryNames.Biomaterial);
//DictionariesExportHelper.ExportDictionary(LisDictionaryNames.Employee);
//DictionariesExportHelper.ExportServiceDictionaryExtended();*/
//        }
    }
}