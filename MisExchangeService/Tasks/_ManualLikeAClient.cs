using ru.novolabs;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisExchange.User.Tasks
{
    /// <summary>
    /// Send resend-close-event like a Client. This class is only for perfomancy profiling
    /// </summary>
    public class _ManualLikeAClient : ru.novolabs.SuperCore.Task
    {
        private bool isEnabled;
        private bool isOneRequest;
        private string fileNameIds;

        private IProcessResultSettings Settings { get; set; }
        private IProcessResultCommunicator Communicator { get; set; }

        public _ManualLikeAClient()
        {
            Settings = GAP.ResultSettings;
            Communicator = GAP.ResultCommunicator;
            fileNameIds = Settings.__fileNameIds;
            if (!String.IsNullOrEmpty(fileNameIds))
            {
                isEnabled = true;
            }
            isOneRequest = Settings.__isOneRequestResultExport;
        }

        public override void Execute()
        {
            if (!IsAbleToProcess())
                return;
            string fileContent = File.ReadAllText(fileNameIds);
            IEnumerable<string> ids = fileContent.Deserialize<ManualExportResult>(Encoding.UTF8).Ids.Replace("\r\n", "").Split(new char[] { ',' }).Select(s => s.Trim());
            if (isOneRequest)
                SendAsOneRequest(ids);
            else
                SendAsMultiplyRequests(ids);
            string tempFileName = fileNameIds + "~~~";
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);
            File.Move(fileNameIds, tempFileName);
        }
        private bool IsAbleToProcess()
        {
            if (!isEnabled)
                return false;
            if (!File.Exists(fileNameIds))
                return false;
            return true;
        }

        private void SendAsOneRequest(IEnumerable<string> idsStr)
        {
            var ids = BuildObjectRefList(idsStr);
            Send(ids);
        }
        private void SendAsMultiplyRequests(IEnumerable<string> idsStr)
        {
            var idsTemp = BuildObjectRefList(idsStr);
            var idsList = idsTemp.Select(obj => new List<ObjectRef>() { obj });
            Parallel.ForEach(idsList, ids => Send(ids));
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
                GAP.Logger.WriteError("There are some not valid objectRefs in file: [{0}] in _ManualLikeAClient:\r\n{1}", fileNameIds, String.Join("\r\n", idsStrNotValid));

            List<ObjectRef> ids = idsStrValid.AsParallel().Select(s => new ObjectRef(Int32.Parse(s))).ToList();
            return ids;
        }
        public class ManualExportResult
        {
            public string Ids { get; set; }
        }
        public List<ExternalResult> Send(List<ObjectRef> requestIds)
        {
            return Communicator.ResendCloseEvents(requestIds);
        }
    }
}
