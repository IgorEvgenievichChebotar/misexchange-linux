using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Interfaces
{
    public interface IRequestValidatorFactory
    {
        IRequestValidatorInner CreateRequestValidator();
    }
}
