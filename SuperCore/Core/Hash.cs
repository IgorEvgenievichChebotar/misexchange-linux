using System;
using System.Security.Cryptography;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public static class HashHelper
    {
        public static String GetMD5Hash(this String input)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();

            foreach (Byte b in provider.ComputeHash(Encoding.GetEncoding(1251).GetBytes(input)))
                sb.Append(b.ToString("x2").ToLower());

            return sb.ToString().ToUpper();
        }

        public static String GetMD5Hash(this byte[] input)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();

            foreach (Byte b in provider.ComputeHash(input))
                sb.Append(b.ToString("x2").ToLower());

            return sb.ToString().ToUpper();
        }
    }
}
