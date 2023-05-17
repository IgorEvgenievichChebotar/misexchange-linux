using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.MisExchange
{
    public class DeliveredBiomaterials
    {
        public DeliveredBiomaterials()
        {
            BiomaterialsDelivered = new List<DeliveredBiomaterial>();
        }

        [CSN("BiomaterialsDelivered")]
        public List<DeliveredBiomaterial> BiomaterialsDelivered { get; set; }
    }

    public class DeliveredBiomaterial
    {
        public DeliveredBiomaterial()
        {
            //Application = new ObjectRef();
        }

        [CSN("Application")]
        public ObjectRef Application { get; set; }
        [CSN("ApplicationNumber")]
        public string ApplicationNumber { get; set; }
        [CSN("Specimen")]
        public ObjectRef Specimen { get; set; }
        [CSN("SpecimenNumber")]
        public string SpecimenNumber { get; set; }
        [CSN("DeliveryDate")]
        public DateTime DeliveryDate { get; set; }
    }
}
