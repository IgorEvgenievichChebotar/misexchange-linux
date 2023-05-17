using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Encryption
{
    public class Crypt
    {
        
        byte[] sessionKey;
        Aes aes;
        const String PublicKey = "<RSAKeyValue><Modulus>invW0RP9FE/qVnuANmTAKXNi5U5DGMXlKRoGOiq6MVSzp9IlL5cetKutLI7B0QygFgR/OzIGlA7G2oO6+YpHLQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        //keysize in byte
        const Int32 keysize = 16;
        public Crypt()
        {
            CreateAESKey();
        }

        ~Crypt()
        {
            aes.Dispose();
        }

        private void CreateAESKey()
        {
            aes = Aes.Create();
            aes.KeySize = keysize * 8;
            aes.GenerateKey();
            sessionKey = aes.Key;
        }

        public byte[] GetSessionKey()
        {
            return GetEncryptedKey();
        }

        private byte[] GetEncryptedKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicKey);
            byte[] encryptedSessionKey = rsa.Encrypt(sessionKey, false);
            return encryptedSessionKey;
        }

        public byte[] EncryptMessage(byte[] msg)
        {
            aes = Aes.Create();
            aes.Key = sessionKey;
            byte[] encryptedMessage = null;
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(msg, 0, msg.Length);
                        cs.FlushFinalBlock();
                        encryptedMessage = ms.ToArray();
                        ms.Close();
                        cs.Close();
                    }
                }
            }
            return encryptedMessage;            
        }

        public byte[] DecryptMessage(byte[] msg)
        {
            aes = Aes.Create();
            aes.Key = sessionKey;
            byte[] decryptedMessage = new byte[msg.Length];
            int byteCount = 0;
            using (ICryptoTransform encryptor = aes.CreateDecryptor())
            {
                using (MemoryStream ms = new MemoryStream(msg))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cs.Read(decryptedMessage, 0, decryptedMessage.Length);
                        ms.Close();
                        cs.Close();
                    }
                }
            }
            return decryptedMessage;            
        }

        private byte[] StringToByteArray(String msg)
        {
            byte[] result = new byte[msg.Length];
            for (int i = 0; i < msg.Length; i++)
            {
                result[i] = (byte)msg[i];
            }
            return result;
        }
    }
}
