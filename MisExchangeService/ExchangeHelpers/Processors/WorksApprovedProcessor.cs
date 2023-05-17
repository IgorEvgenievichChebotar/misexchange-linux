using System.Xml;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LisBusinessObjects.Exchange.ServerEventsParameters;

namespace ru.novolabs.MisExchangeService.Processors
{
    [ProcessorName(LisProcessorsNames.WorkApprove)]
    public class WorksApprovedProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            ApprovedWorks contentObject = new ApprovedWorks();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            RefSet requestsIds = new RefSet();
            foreach (ApprovedWork work in contentObject.Works)
            {                
                requestsIds.Add(work.Request);
            }

            ResultSaver.StoreRequestsResults(requestsIds);
  
            RequestDone = true;
        }
    }
}
