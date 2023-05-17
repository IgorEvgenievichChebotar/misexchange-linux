using ru.novolabs.SuperCore.Crypto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public static class ConnectionStringHelper
    {
        public static string GetConnStr()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MisExchangeCacheCnStr"].ConnectionString;
            return GetConnStr(connStr);
        }
        public static string GetConnStr(string connStr)
        {
            bool isCryptedCredentialsAndConnStrs = (Nullable<Boolean>)ProgramContext.Settings["isCryptedCredentialsAndConnStrs", false] ?? false;
            bool isFullConnectionStringCrypted = (Nullable<Boolean>)ProgramContext.Settings["isFullConnectionStringCrypted", false] ?? false;
            if (!isCryptedCredentialsAndConnStrs)
                return connStr;
            if (isFullConnectionStringCrypted)
                return ConnectionStringCryptor.DecryptFullConnectionString(connStr);
            connStr = ConnectionStringCryptor.DecryptCredentialConnectionString(connStr);
            return connStr;         
        }
        /// <summary>
        /// Get passwords and login to lims server in depend of crypted options
        /// </summary>
        /// <param name="credentialStr"></param>
        /// <returns></returns>
        public static string GetCredentialString(string credentialStr)
        {
            bool isCryptedCredentialsAndConnStrs = (Nullable<Boolean>)ProgramContext.Settings["isCryptedCredentialsAndConnStrs", false] ?? false;
            if (!isCryptedCredentialsAndConnStrs)
                return credentialStr;
            Cryptor cryptor = new Cryptor(true);
            return cryptor.DecryptUtf8Str(credentialStr);
        }
    }
}
