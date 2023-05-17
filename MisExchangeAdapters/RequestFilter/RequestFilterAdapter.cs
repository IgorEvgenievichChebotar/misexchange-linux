using System;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;

namespace ru.novolabs.MisExchangeService.Adapters
{
    public abstract class RequestFilterAdapter
    {
        public abstract ExternalRequestFilter ReadDTO(object obj);
        //public abstract Object WriteDTO(ExternalRequestFilter request);
    }
}
