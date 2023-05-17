using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;

namespace ru.novolabs.MisExchangeService.Classes
{
    public class MisExchangeServerCommunicator : LimsIntegrationService
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NewLisIntegrationServiceCommunicator
        /// </summary>
        public MisExchangeServerCommunicator() : base() { }

        protected override Type DictionaryCacheType()
        {
            return typeof(MisExchangeDictionaryCache);
        }
    }
}