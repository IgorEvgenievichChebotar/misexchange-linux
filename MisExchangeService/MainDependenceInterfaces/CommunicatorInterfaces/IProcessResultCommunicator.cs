using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces
{
    public interface IProcessResultCommunicator
    {
        List<ExternalResult> GetRequestsResults(List<ObjectRef> requestIds);
        List<ExternalResult> ResendCloseEvents(List<ObjectRef> requestIds);
    }
}
