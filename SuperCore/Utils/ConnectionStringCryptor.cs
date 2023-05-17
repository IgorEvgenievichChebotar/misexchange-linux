using ru.novolabs.SuperCore.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore
{
    public static class ConnectionStringCryptor
    {
        public static string DisplayingConnectionString(string connectionStr)
        {
            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionStr);
                return EncryptDecrpytSqlBuilder(sqlBuilder, (_,__)=>"***");
            }
            catch (ArgumentException)
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder(connectionStr);
                return EncryptDecryptOleDbBuilder(builder, (_,__)=>"***");
            }       
        }
        private static string EncryptFullConnectionStringEx(string connectionStr)
        {
            Cryptor cryptor = new Cryptor(true);
            return cryptor.EncryptUtf8Str(connectionStr);
        }
        private static string DecryptFullConnectionStringEx(string connectionStr)
        {
            Cryptor cryptor = new Cryptor(true);
            return cryptor.DecryptUtf8Str(connectionStr);
        }

        public static string EncryptFullConnectionString(string connectionStr)
        {
                if (IsEncrypted(connectionStr))
                    return connectionStr;
                return EncryptFullConnectionStringEx(connectionStr);
        }
        public static string DecryptFullConnectionString(string connectionStr)
        {
            int recursionCount = 0;
            Func<string, string> action = null;
            action = (connStr) =>
            {
                if (recursionCount > 1)
                    throw new InvalidOperationException(
                        String.Format("Cannot validate string [{0}] or [{1}] its decrypted version as a connection string",
                        connectionStr, connStr));
                if (!IsEncrypted(connStr))
                    return connStr;
                recursionCount++;
                return action(DecryptFullConnectionStringEx(connStr));
            };
            return action(connectionStr);
        }
        private static bool IsEncrypted(string connectionStr)
        {
            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionStr);
                if (String.IsNullOrEmpty(sqlBuilder.ToString()))
                    return true;
                return false;
            }
            catch{}
            try
            {
                OleDbConnectionStringBuilder oleBuilder = new OleDbConnectionStringBuilder(connectionStr);
                if (String.IsNullOrEmpty(oleBuilder.ToString()))
                    return true;
                return false;               
            }
            catch{  }
            return true;
        }

        public static string EncryptCredentialConnectionString(string connectionStr)
        {
            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionStr);
                return EncryptDecrpytSqlBuilder(sqlBuilder, CryptorExtension.EncryptUtf8Str);
            }
            catch (ArgumentException)
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder(connectionStr);
                return EncryptDecryptOleDbBuilder(builder, CryptorExtension.EncryptUtf8Str);
            }
        }
        public static string DecryptCredentialConnectionString(string connectionStr)
        {
            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionStr);
                return EncryptDecrpytSqlBuilder(sqlBuilder,CryptorExtension.DecryptUtf8Str);
            }
            catch (ArgumentException)
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder(connectionStr);
                return EncryptDecryptOleDbBuilder(builder, CryptorExtension.DecryptUtf8Str);
            }
        }
        private static string EncryptDecrpytSqlBuilder(SqlConnectionStringBuilder builder, Func<Cryptor, string,string> cryptFunc)
        {
            Cryptor cryptor = new Cryptor(true);

            builder.UserID = cryptFunc(cryptor, builder.UserID);
            builder.Password = cryptFunc(cryptor, builder.Password);
            return builder.ConnectionString;
        }
        private static string EncryptDecryptOleDbBuilder(OleDbConnectionStringBuilder builder, Func<Cryptor, string, string> cryptFunc)
        {
            Cryptor cryptor = new Cryptor(true);
            object userIdObj;
            object passwordObj;
            List<string> listUserId = new List<string>() { "user id", "uid" };
            List<string> listPassword = new List<string>() { "password", "pwd" };
            string userIdStr = (from userId in listUserId
                                let keyStr = GetKey(builder.Keys, userId)
                                where keyStr != null
                                select keyStr).FirstOrDefault();
            string passwordStr = (from password in listPassword
                                  let keyStr = GetKey(builder.Keys, password)
                                  where keyStr != null
                                  select keyStr).FirstOrDefault();
            if (builder.TryGetValue(userIdStr, out userIdObj))
            {
                builder[userIdStr] = cryptFunc(cryptor, (string)builder[userIdStr]);
            }
            if (builder.TryGetValue(passwordStr, out passwordObj))
            {
                builder[passwordStr] = cryptFunc(cryptor, (string)builder[passwordStr]);
            }
            return builder.ConnectionString;
        }
        private static string GetKey(ICollection keys, string key)
        {
            key = key.Replace("  "," ").Trim();
            foreach (var keyCol in keys)
            {
                string keyStr = (string)keyCol;
                if (String.Equals(key,keyStr,StringComparison.CurrentCultureIgnoreCase))
                {
                    return keyStr;
                }
            }
            return null;
        }
    }
}
