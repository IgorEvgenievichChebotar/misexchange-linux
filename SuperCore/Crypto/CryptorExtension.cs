using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.Crypto
{
    /// <summary>
    /// Consists extension methods for Cryptor
    /// </summary>
    public static class CryptorExtension
    {
        public static byte[] EncryptMessage(this Cryptor cryptor, String msg, Encoding encoding)
        {
            byte[] b_msg = encoding.GetBytes(msg);
            return cryptor.EncryptMessage(b_msg);
        }

        public static String DecryptMessage(this Cryptor cryptor, byte[] msg, Encoding encoding)
        {
            byte[] result = cryptor.DecryptMessage(msg);
            return ByteArrayToString(result, encoding);
        }

        public static byte[] EncryptMessage(this Cryptor cryptor, String msg, byte[] Salt, Encoding encoding)
        {
            byte[] b_msg = encoding.GetBytes(msg);
            return cryptor.EncryptMessage(b_msg, Salt);
        }

        public static String DecryptMessage(this Cryptor cryptor, byte[] msg, byte[] Salt, Encoding encoding)
        {
            byte[] result = cryptor.DecryptMessage(msg, Salt);
            return ByteArrayToString(result, encoding);
        }

        public static string EncryptUtf8Str(this Cryptor cryptor, string str)
        {
            return Convert.ToBase64String(cryptor.EncryptMessage(Encoding.UTF8.GetBytes(str)));
        }
        public static string DecryptUtf8Str(this Cryptor cryptor, string str)
        {
            return Encoding.UTF8.GetString(cryptor.DecryptMessage(Convert.FromBase64String(str))).TrimEnd('\0');
        }
        private static String ByteArrayToString(byte[] msg, Encoding encoding)
        {
            return encoding.GetString(msg);
        }
    }
}
