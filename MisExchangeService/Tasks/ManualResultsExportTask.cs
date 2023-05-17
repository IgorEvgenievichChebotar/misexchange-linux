using ru.novolabs;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisExchange.User.Tasks
{
    public class ManualResultsExportTask: ru.novolabs.SuperCore.Task
    {
        private bool IsEnabled;
        private bool isOneRequest;
        private string fileNameIds;
        System.Threading.Tasks.Task _task = null;

        private IProcessResultSettings Settings { get; set; }
        public ManualResultsExportTask()
        {
            Settings = GAP.ResultSettings;
            fileNameIds = Settings.FileNameIds;
            if (!String.IsNullOrEmpty(fileNameIds))
            {
                IsEnabled = true;
            }
            isOneRequest = Settings.IsOneRequestResultExport;
        }

        public override void Execute()
        {
            if (!IsAbleToProcess())
                return;
            if (_task == null || _task.IsCompleted)
                _task = System.Threading.Tasks.Task.Factory.StartNew(ProcessTask, TaskCreationOptions.LongRunning);
        }

        private void ProcessTask()
        {
            try
            {
                string fileContent = File.ReadAllText(fileNameIds);
                IEnumerable<string> ids = fileContent.Deserialize<ManualExportResult>(Encoding.UTF8).Ids.Replace("\r\n", "").Split(new char[] { ',' }).Select(s => s.Trim());
                if (isOneRequest)
                    SendAsOneRequest(ids);
                else
                    SendAsMultiplyRequests(ids);
                File.Move(fileNameIds, fileNameIds + "~~~");
            }
            catch (Exception ex)
            {
                GAP.Logger.WriteError("There was an excepiton in ManualResultsExportTask:\r\n{0}", ex.ToString());          
            }
        }
        private bool IsAbleToProcess()
        {     
            if (!IsEnabled)
                return false;
            if (!File.Exists(fileNameIds))
                return false;
            return true;
        }

        private void SendAsOneRequest(IEnumerable<string> idsStr)
        {
            var ids = BuildObjectRefList(idsStr);
            ResultSaver.StoreRequestsResults(ids);        
        }
        private void SendAsMultiplyRequests(IEnumerable<string> idsStr)
        {
            var idsTemp = BuildObjectRefList(idsStr);
            var idsList = idsTemp.Select(obj => new List<ObjectRef>() { obj });
            Parallel.ForEach(idsList, ids => ResultSaver.StoreRequestsResults(ids));                       
        }
        private List<ObjectRef> BuildObjectRefList(IEnumerable<string> idsStr)
        {
            Predicate<string> isNumeric = s =>
            {
                int temp;
                return Int32.TryParse(s, out temp);
            };
            var idsStrValid = idsStr.AsParallel().Where(s => isNumeric(s));
            var idsStrNotValid = idsStr.Except(idsStrValid);
            if (idsStrNotValid.Count() > 0)
                GAP.Logger.WriteError("There are some not valid objectRefs in file: [{0}] in ManualExportResult:\r\n{1}", fileNameIds, String.Join("\r\n", idsStrNotValid));

            List<ObjectRef> ids = idsStrValid.AsParallel().Select(s => new ObjectRef(Int32.Parse(s))).ToList();
            return ids;        
        }   
        public class ManualExportResult
        {
            public string Ids { get; set; }      
        }
    }
}
