using ru.novolabs.Common;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    class ExchangeCacheHelper: IExchangeCacheHelper
    {
        public void SaveRequest(Common.RequestObjectStatus reqObjS)
        {
            Cache2DbManager.Instance.SaveRequest(reqObjS);
        }

        public void SaveResult(Common.ResultObjectStatus resObjS)
        {
            Cache2DbManager.Instance.SaveResult(resObjS);
        }

        public ExchangeDTOs.Result GetResult(string requestCode, StatusObjectCache objectStatus)
        {
            return Cache2DbManager.Instance.GetResultByCode(requestCode, objectStatus);
        }
    }
}
