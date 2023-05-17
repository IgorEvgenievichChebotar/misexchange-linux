using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System.Xml;

namespace ru.novolabs.MisExchange.Processors
{
    [ProcessorName(LisProcessorsNames.RequestSave)]
    public class RequestsSavedProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            SavedRequests contentObject = new SavedRequests();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            ResultSaver.StoreRequestsResults(contentObject.Ids);

            RequestDone = true;
        }
    }
}
