using ru.novolabs.MisExchange;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using System.Xml;

namespace ru.novolabs.MisExchangeService.Processors
{
    [ProcessorName(LisProcessorsNames.BiomaterialDelivered)]
    public class BiomaterialDeliveredProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            DeliveredBiomaterials contentObject = new DeliveredBiomaterials();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            ResultSaver.ProcessDeliveredBiomaterials(contentObject.BiomaterialsDelivered);

            RequestDone = true;
        }
    }
}
