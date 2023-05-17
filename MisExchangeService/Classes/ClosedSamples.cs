using ru.novolabs.SuperCore;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange.ServerEventsParameters
{
    public class ClosedSamples
    {
        public ClosedSamples()
        {
            Ids = new List<ObjectRef>();
        }

        [CSN("Ids")]
        public List<ObjectRef> Ids { get; set; }
    }
}
