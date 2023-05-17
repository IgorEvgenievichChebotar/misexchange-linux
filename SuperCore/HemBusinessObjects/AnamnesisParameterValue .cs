using System;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    [Serializable]
    public class AnamnesisParameterValue : BaseParameterValue
    {
        [SendToServer(false)]
        [CSN("CreateByDefault")]
        public Boolean CreateByDefault { get; set; }

        [SendToServer(false)]
        [CSN("Required")]
        public Boolean Required { get; set; }

        [SendToServer(false)]
        [CSN("Date")]
        public DateTime? Date { get; set; }

        [SendToServer(false)]
        [CSN("Expired")]
        public Boolean Expired { get; set; }
    }
}