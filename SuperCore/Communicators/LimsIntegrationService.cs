using System;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.Core;

namespace ru.novolabs.SuperCore
{
    public class LimsIntegrationService : LimsCommunicator
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса LisIntegrationService
        /// </summary>
        public LimsIntegrationService() : base() { }

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
                ServerAddress = (string)ProgramContext.Settings["lisServerAddress"];                 
            }
            String sLoggingLevel = (loggingLevel != null) ? (String)loggingLevel : SystemLoggingLevels.LOGIN_LEVEL_OFF;
            BaseUserSession userSession;
            SystemLogin(externalSystemCode, sLoggingLevel, out userSession);
            LoadDictionaries(userSession);
        }

        protected override Type DictionaryCacheType()
        {
            return typeof(LimsDictionaryCache);    
        }
    }
}
