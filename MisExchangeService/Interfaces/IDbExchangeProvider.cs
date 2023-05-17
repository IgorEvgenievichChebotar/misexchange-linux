using ru.novolabs.ExchangeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Interfaces
{
    public interface IDbExchangeProvider
    {
        void SaveToExchangeDb(Request requestDTO, Int32 requestLisId);

        Int32 GetRequestLisId(string requestCode);
    }
}
