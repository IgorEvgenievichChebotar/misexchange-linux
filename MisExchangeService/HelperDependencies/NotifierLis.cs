using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.Notification;
using System;
using System.IO;

namespace ru.novolabs.MisExchange.Classes
{
    class NotifierLis : INotifierLis
    {
        public NotifierLis(IProcessResultSettings settings)
        {
            Settings = settings;        
        }
        IProcessResultSettings Settings { get; set; }
        public void NotifyLisServer(string requestId, string state)
        {
            try
            {
                string xmlTemplate = "";
                string xmlTemplateFile = Path.Combine(FileHelper.AssemblyDirectory, "notificationTemplate.xml");
                try
                {
                    if (File.Exists(xmlTemplateFile))
                        xmlTemplate = File.ReadAllText(xmlTemplateFile);
                }
                catch { }

                if (String.IsNullOrEmpty(xmlTemplate))
                    return;

                String xml = NotificationTemplateProcessing.ProcessStringTemplateFromMisExchange(xmlTemplate, requestId, state);
                NotifyLis(xml);
            }
            catch (Exception ex)
            {
                GAP.Logger.WriteError("Не удалось оповестить сервер. Ошибка: {0}", ex.ToString());
            }
        }

        private void NotifyLis(String xml)
        {
            GAP.Logger.WriteText("NotifyLis");
            GAP.Logger.WriteText("Notification text:\r\n\r\n{0}", xml);
           // String serverAddress = ProgramContext.LisCommunicator.GetServerAddress();

            LisNotifierOld.NotifyLis(xml, Settings.ServerAddress);
        }
    }
}
