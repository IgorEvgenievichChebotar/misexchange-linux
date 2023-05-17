using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;

namespace MisExchangeAdapters.Result
{
    public abstract class ResultAdapter
    {
        public abstract Object WriteDTO(ExternalResult result);
    }

    public abstract class Result3Adapter
    {
        public abstract Object WriteDTO(ru.novolabs.ExchangeDTOs.Result result);
    }
}
