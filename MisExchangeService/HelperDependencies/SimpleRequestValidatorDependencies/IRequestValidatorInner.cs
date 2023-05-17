using ru.novolabs.ExchangeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies
{
    public interface IRequestValidatorInner
    {
        void CheckData(Request request);
    }
}
