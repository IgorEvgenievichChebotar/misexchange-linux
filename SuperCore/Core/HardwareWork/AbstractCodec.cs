using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Core.HardwareWork
{
    public abstract class AbstractCodec
    {
        public String GetClientIdView()
        {
            return GetClientId() + ',' + EncodeString(GetMachineName());
        }

        public virtual String GetMachineId()
        {
            return EncodeString(GetCPUId()) + ',' + EncodeString(GetBIOSId()) + ',' + EncodeString(GetHDDId());
        }

        public abstract String GetClientId();

        public abstract String GetCPUId();

        public abstract String GetBIOSId();

        public abstract String GetMachineName();


        public abstract String GetHDDId();

        protected String EncodeString(string str)
        {
            if (String.IsNullOrEmpty(str)) return String.Empty;
            byte[] data = Encoding.Default.GetBytes(str);
            return BitConverter.ToString(data).Replace("-", String.Empty);
        }

        protected String DecodeString(string str)
        {
            if (str.Length % 2 > 0) throw new ArgumentException("Incorrect source string length");
            byte[] bytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i += 2)
            {
                string hexChars = String.Concat(str[i], str[i + 1]);
                bytes[i / 2] = Byte.Parse(hexChars, System.Globalization.NumberStyles.HexNumber);
            }
            return Encoding.Default.GetString(bytes);
        }
    }
}
