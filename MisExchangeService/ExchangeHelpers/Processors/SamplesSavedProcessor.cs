using System.Xml;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LisBusinessObjects.Exchange.ServerEventsParameters;

namespace ru.novolabs.MisExchangeService.Processors
{
    [ProcessorName(LisProcessorsNames.SampleSave)]
    public class SamplesSavedProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            SavedSamples contentObject = new SavedSamples();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            ResultSaver.StoreRequestsResults(contentObject.Ids);

            RequestDone = true;
        }
    }
}
