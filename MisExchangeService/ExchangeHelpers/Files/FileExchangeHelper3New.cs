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

    [ExchangeHelperName("ExchangeHelper3_FilesNew")]
    class FilesExchangeHelper3New : ExchangeHelper3
    {
        private HelperSettings helperSettings = null;
        private FileHelper FileHelper { get; set; }

        public FilesExchangeHelper3New()
        {
            helperSettings = File.ReadAllText(Path.Combine(FileHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
            FileHelper = new FileHelper(helperSettings);
        }

        protected override void SpecificProcessNewData()
        {
            var sourceDirectories = FileHelper.GetSourceDirectories();
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

        protected virtual void ProcessFile(String fileName)
        {
            ExchangeDTOs.Request requestDTO = null;
            try
            {
                requestDTO = File.ReadAllText(fileName, FileHelper.Encoding).Deserialize<ExchangeDTOs.Request>(FileHelper.Encoding);
                ProcessRequest(requestDTO);
                CacheRequestMessage(requestDTO, null);
                FileHelper.SaveFileToArchive(fileName);
            }
            catch (NlsServerException ex)
            {
                CacheRequestMessage(requestDTO, ex.ToString());
                Log.WriteError(ex.ToString());
                FileHelper.SaveFileToErrors(ex.Message, fileName);
            }
            catch (CustomDataCheckException ex)
            {
                CacheRequestMessage(requestDTO, ex.ToString());
                Log.WriteError(ex.ToString());
                FileHelper.SaveFileToErrors(String.Join(";\n", ex.Errors), fileName);
            }
            catch (Exception ex)
            {
                if (requestDTO != null)
                {
                    CacheRequestMessage(requestDTO, ex.ToString());
                }
                FileHelper.SaveFileToErrors(ex.Message, fileName);
                Log.WriteError(String.Format("Невозможно обработать файл \"{0}\": {1}", fileName, ex.ToString()));
            }
        }
        #region For Test Purposes
        protected void CacheRequestMessage(ru.novolabs.ExchangeDTOs.Request request, string errorStr)
        {
            if (helperSettings.IsEnabledMessageCaching)
            {
                Common.StatusObjectCache status = String.IsNullOrEmpty(errorStr) ? Common.StatusObjectCache.Completed : Common.StatusObjectCache.Error;
                StoreRequest(request, status, errorStr, false);
            }

        }

        protected void CacheResultMessage(ru.novolabs.ExchangeDTOs.Result result, string errorStr)
        {
            if (helperSettings.IsEnabledMessageCaching)
            {
                Common.StatusObjectCache status = String.IsNullOrEmpty(errorStr) ? Common.StatusObjectCache.Completed : Common.StatusObjectCache.Error;
                StoreResult(result, status, errorStr, false);
            }

        }
        #endregion

        protected override bool IsFilterRequestResults { get { return false; } }

        public override void SpecificStoreResults(List<ExchangeDTOs.Result> results)
        {
            foreach (ExchangeDTOs.Result result in results)
            {
                string resultsPath = FileHelper.GetResultsPath(result);
                if (Directory.Exists(resultsPath))
                {
                    try
                    {
                        FileHelper.SaveResultToFile(result);
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
                    CacheResultMessage(result, errorStr);
                    Log.WriteError(errorStr);
                }
            }
        }
    }
}