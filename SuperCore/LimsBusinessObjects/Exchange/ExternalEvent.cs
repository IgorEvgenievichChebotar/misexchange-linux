using System;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    public class ExternalEvent
    {
        [CSN("Type")]
        public String Type { get; set; }
        [CSN("Content")]
        public Object Content { get; set; }
    }
}