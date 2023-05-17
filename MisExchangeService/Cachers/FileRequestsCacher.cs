using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ru.novolabs.MisExchange.Classes;

namespace ru.novolabs.MisExchange.Cachers
{
    public static class FileRequestsCacher
    {

        public static void Init()
        {
            String requestsCacherFile = ProgramContext.Settings["requestsCacherSettings"].ToString();
            if (!Path.IsPathRooted(requestsCacherFile))
            {
                requestsCacherFile = Path.Combine(FileHelper.AssemblyDirectory, requestsCacherFile);
            }
            requestSettings = File.ReadAllText(requestsCacherFile, Encoding.UTF8).Deserialize<FileRequestsCacherSettings>(Encoding.UTF8);
            if (!Directory.Exists(Path.GetPathRoot(requestSettings.WorkingDirectory)))
                Directory.CreateDirectory(requestSettings.WorkingDirectory);
            if (!Directory.Exists(Path.GetPathRoot(requestSettings.ErrorDirectory)))
                Directory.CreateDirectory(requestSettings.ErrorDirectory);
        }

        static FileRequestsCacherSettings requestSettings { get; set; }


        public static void SaveToError(ExchangeDTOs.Request request, Exception ex)
        {
            String fileName = "request" + request.RequestCode;
            String errFileName = (Path.Combine(requestSettings.ErrorDirectory, Path.GetFileName(fileName)));
            string content = ex.Message + "\n\r" + request.Serialize(Encoding.Unicode).RemoveNilElements();
            File.WriteAllText(errFileName, content);
        }

        public static List<ExchangeDTOs.Request> ReadFromCache()
        {
            List<ExchangeDTOs.Request> requests = new List<ExchangeDTOs.Request>();

            String[] files = Directory.GetFiles(requestSettings.WorkingDirectory);
            foreach (String file in files)
            {
                try
                {
                    ExchangeDTOs.Request request = File.ReadAllText(file, Encoding.UTF8).Deserialize<ExchangeDTOs.Request>(Encoding.UTF8);
                    requests.Add(request);
                }
                catch (Exception ex)
                {
                    Log.WriteError("Не удается прочитать файл {0} : {1} \r\n {2}", file, ex.Message, ex.StackTrace);
                }
            }

            return requests;
        }

        public static void SaveToCache(ExchangeDTOs.Request request)
        {
            String fileName = "request" + request.RequestCode;
            string reqFileName = (Path.Combine(requestSettings.WorkingDirectory, Path.GetFileName(fileName)));
            string content = request.Serialize(Encoding.Unicode).RemoveNilElements();
            File.WriteAllText(reqFileName, content);
        }

        public static void SaveToCache(List<ExchangeDTOs.Request> requests)
        {
            foreach (ExchangeDTOs.Request request in requests)
            {
                SaveToCache(request);
            }
        }

        public static List<ExchangeDTOs.Request> ExchangeCache(List<ExchangeDTOs.Request> requests)
        {
            SaveToCache(requests);
            requests = ReadFromCache();
            return requests;
        }
    }
}
