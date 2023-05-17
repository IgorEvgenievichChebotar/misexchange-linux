using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace ru.novolabs.SuperCore.Crypto
{
    /// <summary>
    /// Cryptor with pairs encrypt-decrypt methods
    /// </summary>
    public class Cryptor
    {
        public enum CryptWorkModes
        {
            Old = 0,
            New = 1
        }
        byte[] sessionKey;
        byte[] encryptedSessionKey;
        byte[] initializationVector;
        const String PublicKey = "<RSAKeyValue><Modulus>j/n7H1Urq3WiENHiYCHjRjM4qewXepHW9dv2SiW4ZHcQWCqW/gOZR/4cN88X3jbQe1oPYJ+nvGcRuQD3p47dww==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public Cryptor(bool useStaticKey = false, CryptWorkModes workMode = CryptWorkModes.Old)
        {
            if (useStaticKey)
            {
                if (workMode == CryptWorkModes.Old)
                    CreateStaticAESKey();
                else
                    CreateStaticAESKey_New();
            }
            else
                CreateAESKey();
        }

        private void CreateAESKey()
        {
            Aes aes = Aes.Create();
            aes.KeySize = 16 * 8;

            aes.GenerateKey();

            sessionKey = aes.Key;
            initializationVector = aes.IV;
        }
        private void CreateStaticAESKey()
        {            
            sessionKey = new byte[] { 119, 66, 15, 118, 62, 222, 210, 231, 195, 182, 162, 171, 178, 61, 4, 160 };
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            initializationVector = new byte[] { 0x54, 0x52, 0x55, 0x53, 0x54, 0x4e, 0x4f, 0x31, 0x4d, 0x52, 0x4d, 0x55, 0x4c, 0x44, 0x45, 0x52 };
        }

        private void CreateStaticAESKey_New()
        {
            sessionKey = new byte[] { 52, 203, 35, 102, 167, 228, 183, 47, 252, 29, 9, 167, 91, 22, 85, 196 };
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            initializationVector = new byte[] { 0x45, 0x25, 0x55, 0x35, 0x45, 0x5e, 0x3d, 0x43, 0x5d, 0x25, 0x2d, 0x44, 0x3c, 0x22, 0x54, 0x25 };
        }
        public byte[] GetInitializationVector()
        {
            return initializationVector;
        }

        public byte[] GetSessionKey()
        {
            return GetEncryptedKey();
        }

        private byte[] GetEncryptedKey()
        {
            if ((encryptedSessionKey == null) || (encryptedSessionKey.Length == 0))
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                InitRSAFromXMLString(rsa, PublicKey);
                encryptedSessionKey = rsa.Encrypt(sessionKey, false);
            }            

            // Возвращаем копию массива, содержащего зашифрованный ключ
            byte[] result = new byte[encryptedSessionKey.Length];
            Array.Copy(encryptedSessionKey, result, encryptedSessionKey.Length);
            return result;
        }

        private void InitRSAFromXMLString(RSACryptoServiceProvider rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public byte[] EncryptMessage(byte[] msg)
        {
            Aes aes = Aes.Create();
            aes.Key = sessionKey;
            aes.IV = initializationVector;
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
                    }
                }
            }
            return encryptedMessage;
        }

        public byte[] DecryptMessage(byte[] msg)
        {
            Aes aes = Aes.Create();
            aes.Key = sessionKey;
            aes.IV = initializationVector;

            byte[] decryptedMessage = new byte[msg.Length];
            int byteCount = 0;
            using (ICryptoTransform encryptor = aes.CreateDecryptor())
            {
                using (MemoryStream ms = new MemoryStream(msg))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cs.Read(decryptedMessage, 0, decryptedMessage.Length);
                    }
                }
            }
            return decryptedMessage;
        }


        public byte[] EncryptMessage(byte[] msg, byte[] Salt)
        {
            Aes aes = Aes.Create();
            aes.Key = sessionKey;
            aes.IV = initializationVector;
            byte[] encryptedMessage = null;
            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, Salt))
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
       
        public byte[] DecryptMessage(byte[] msg, byte[] Salt)
        {
            //msg = Convert.FromBase64String("4FFXBmm+6259sYKtVQ9fF+lHnb44sj+gNl/TwvQ0o6ukbrU/GRIto5MitLI5Plnz0wjSxQaO71T0c5dU+kwIDO1w5OtG2AxAAyOzHuANTccsxmMLmNJtfHvMig5R14NEpDWVITHeftCjJ/okeoJF2P8NN/q8Vil6J12ktzhnRRMjzofA9ydogDQ9UWC09FBZ");
            //Salt = Convert.FromBase64String("1cSfQC5zmpXVKDVpW5mQgA==");
            Aes aes = Aes.Create();
            aes.Key = sessionKey;
            //aes.Key = Convert.FromBase64String("9tCsFpTB+06OqmISP/JTlQ==");
            //aes.IV = initializationVector;
            aes.IV = GetSalt(Salt);
            aes.Padding = PaddingMode.PKCS7;
            byte[] decryptedMessage = new byte[msg.Length];
            int byteCount = 0;
            using (ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                using (MemoryStream ms = new MemoryStream(msg))
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                    {

                        int b;
                        while ((b = cs.ReadByte()) != -1)
                        {
                            decryptedMessage[byteCount] = (byte)b;
                            byteCount++;
                        }
                        //byteCount = cs.Read(decryptedMessage, 0, decryptedMessage.Length);
                        //cs.FlushFinalBlock();
                    }
                }
            }
            return decryptedMessage;
        }

        private byte[] GetSalt(byte[] Salt)
        {
            byte[] IV = new byte[initializationVector.Length];
            for (int i = 0; i < Salt.Length; i++)
                IV[i] = Salt[i];
            return IV;
        }
        /// <summary>
        /// Метод попытки дешифровать строку.
        /// </summary>
        /// <param name="base64">Base64 строка с набором AES-зашифрованных байт</param>
        /// <returns>Если строка не зашифрована, то вернёт её. Иначе вернёт расшифрованную строку</returns>
        public string TryDecryptString(string base64)
        {
            try
            {
                byte[] base64Bytes = Convert.FromBase64String(base64);
                if (base64Bytes == null || base64Bytes.Length == 0)
                    return base64;
                byte[] decrypted = DecryptMessage(base64Bytes, initializationVector);
                if (decrypted.Length == 0)
                    return base64;
                return Encoding.Default.GetString(decrypted).TrimEnd('\0');
            }
            catch
            {
                return base64;
            }
        }
    }
}
