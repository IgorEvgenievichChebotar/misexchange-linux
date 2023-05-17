using ru.novolabs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces
{
    public interface IExchangeCacheHelper
    {
        void SaveRequest(RequestObjectStatus reqObjS);
        void SaveResult(ResultObjectStatus resObjS);
        ExchangeDTOs.Result GetResult(string requestCode, StatusObjectCache objectStatus);
    }
}
