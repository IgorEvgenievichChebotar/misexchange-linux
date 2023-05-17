using System.Collections.Generic;
using System.Xml;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LisBusinessObjects.Exchange.ServerEventsParameters;

namespace ru.novolabs.MisExchangeService.Processors
{
    [ProcessorName(LisProcessorsNames.RequestClose)]
    public class RequestsClosedProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            ClosedRequests contentObject = new ClosedRequests();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            List<ObjectRef> ids = contentObject.Ids;
            ResultSaver.StoreRequestsResults(ids);

            RequestDone = true;
        }
    }
}
