using ru.novolabs.MisExchangeService.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes.Communication
{
    public interface ISoapServiceCommunicator<T> where T: ExchangeHelper3
    {
        void ProcessService(T instance);
    }
}
