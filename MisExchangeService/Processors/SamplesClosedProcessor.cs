using System.Xml;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange.ServerEventsParameters;

namespace ru.novolabs.MisExchange.Processors
{
    [ProcessorName(LisProcessorsNames.SampleClose)]
    class SamplesClosedProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            ClosedSamples contentObject = new ClosedSamples();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            ResultSaver.StoreRequestsResults(contentObject.Ids);

            RequestDone = true;
        }
    }
}
