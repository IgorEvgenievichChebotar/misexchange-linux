using ru.novolabs.SuperCore;
using System;

namespace ru.novolabs.lims.drivers
{
    public class EquipmentConnectingSettings
    {
        public EquipmentConnectingSettings()
        {
            Equipment = new ObjectRef();
        }
        [CSN("Equipment")]
        public ObjectRef Equipment { get; set; }
        [CSN("Connected")]
        public Boolean Connected { get; set; }
    }
}
