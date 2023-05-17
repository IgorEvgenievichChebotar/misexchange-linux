using System;
using System.Text;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.Core.HardwareWork;

namespace ru.novolabs.SuperCore.HardwareWork.WMI
{
    public class SimpleCodec : AbstractCodec
    {
        public override String GetClientId()
        {
            string id = GetMachineId();
            var clientIdSuffix = (string)ProgramContext.Settings["clientIdSuffix", false];
            if (!String.IsNullOrEmpty(clientIdSuffix))
                id += EncodeString(clientIdSuffix);
            return id;
        }

        public override String GetCPUId()
        {
            return String.Join("_", NlsWMIProvider.GetWMIClassPropertiesInfo(propNames: new string[] { "DeviceID", "Name", "ProcessorId" }, className: "Win32_Processor"));
        }

        public override String GetBIOSId()
        {
            return String.Join("_", NlsWMIProvider.GetWMIClassPropertiesInfo(propNames: new string[] { "Version", "Name", "ReleaseDate" }, className: "Win32_BIOS"));
        }

        public override String GetMachineName()
        {
            return Environment.MachineName;
        }

        public override String GetHDDId()
        {
            return String.Join("_",
                NlsWMIProvider.GetWMIClassPropertiesInfo(
                propNames: new string[] { "Index", "Model", "FirmwareRevision", "SerialNumber" },
                className: "Win32_DiskDrive",
                conditions: new string[] { "MediaType like \"Fixed%hard%disk%media\"" }
                ));
        }
    }
}