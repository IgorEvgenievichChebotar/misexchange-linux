using ru.novolabs.SuperCore;
using System.Collections.Generic;

namespace ru.novolabs.MisExchange.Classes
{
    public class SavedRequests
    {
        public SavedRequests()
        {
            Ids = new List<ObjectRef>();
        }

        [CSN("Ids")]
        public List<ObjectRef> Ids { get; set; }
    }
}
