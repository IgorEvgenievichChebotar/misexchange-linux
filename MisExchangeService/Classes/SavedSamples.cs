using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange.ServerEventsParameters
{
    public class SavedSamples
    {
        public SavedSamples()
        {
            Ids = new List<ObjectRef>();
        }

        [CSN("Ids")]
        public List<ObjectRef> Ids { get; set; }
    }
}