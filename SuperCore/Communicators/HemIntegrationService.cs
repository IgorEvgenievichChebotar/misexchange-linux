using System;
using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.Core.HardwareWork;
using ru.novolabs.SuperCore.HardwareWork.WMI;
using ru.novolabs.SuperCore.HemDictionary;

namespace ru.novolabs.SuperCore.Communicators
{
    public class HemIntegrationService: HemCommunicator
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса LisIntegrationService
        /// </summary>
        public HemIntegrationService() : base() { }

        public void Login()
        {
            String externalSystemCode = (String)ProgramContext.Settings["externalSystemCode"];
            Object loggingLevel = ProgramContext.Settings["loggingLevel", false];
            try
            {
                ServerAddress = (string)ProgramContext.Settings["serverAddress"];
            }
            catch (SettingNotFoundException)
            {
                ServerAddress = (string)ProgramContext.Settings["hemServerAddress"];                 
            }
            String sLoggingLevel = (loggingLevel != null) ? (String)loggingLevel : SystemLoggingLevels.LOGIN_LEVEL_OFF;
            String login = (string)ProgramContext.Settings["hemLogin"];
            String password = (string)ProgramContext.Settings["hemPassword"];
            BaseUserSession userSession;

            Login(login, password.GetMD5Hash(), ServerAddress, out userSession, new SimpleCodec());
            this.UserSession = userSession;
            LoadDictionaries(userSession);
        }

        protected override Type DictionaryCacheType()
        {
            return typeof(HemDictionaryCache);    
        }
    }
}
