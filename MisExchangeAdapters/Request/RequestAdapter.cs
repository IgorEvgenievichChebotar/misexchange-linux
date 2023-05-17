using System;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;

namespace ru.novolabs.MisExchangeService.Adapters
{
    public abstract class RequestAdapter
    {
        public abstract ExternalRequest ReadDTO(object obj);
        public abstract Object WriteDTO(ExternalRequest request);
    }

    public abstract class Request3Adapter
    {
        public abstract ExchangeDTOs.Request ReadDTO(object obj);
        public abstract Object WriteDTO(ExchangeDTOs.Request request);
    }
}
