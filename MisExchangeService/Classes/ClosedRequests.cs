using System.Collections.Generic;
using ru.novolabs.SuperCore;

namespace ru.novolabs.MisExchangeService.Classes
{
    public class ClosedRequests
    {
        public ClosedRequests()
        {
            Ids = new List<ObjectRef>();
        }

        [CSN("Ids")]
        public List<ObjectRef> Ids { get; set; }
    }
}
