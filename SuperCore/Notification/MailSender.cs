using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Timers;
//using System.Windows.Forms;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.Crypto;
using System.Diagnostics;

namespace ru.novolabs.SuperCore.Notification
{
    public class MailSender
    {

        private CacheDbMailSender cacheDbMailSender { get; set; }
        private MailSenderSettings mailSenderSettings { get; set; }

        public MailSender()        
        {
            mailSenderSettings = File.ReadAllText(Path.Combine(PathHelper.AssemblyDirectory, "MailSenderSettings.xml"), Encoding.UTF8).Deserialize<MailSenderSettings>(Encoding.UTF8);
            cacheDbMailSender = new CacheDbMailSender();
            Dictionary<string, string> dbConnecionSettings = new Dictionary<string, string>();
            dbConnecionSettings.Add("DbInitialCatalog", mailSenderSettings.DbInitialCatalog);
            dbConnecionSettings.Add("DbDataSource", mailSenderSettings.DbDataSource);
            dbConnecionSettings.Add("DbUserID", mailSenderSettings.DbUserID);
            dbConnecionSettings.Add("DbPassword", mailSenderSettings.DbPassword);
            DbCreator dbCreator = new DbCreator(dbConnecionSettings);
            dbCreator.Execute();

            CacheDbMailSender.dbConnectionStr = dbCreator.DbConnectionStr;
             _timer = new System.Timers.Timer(mailSenderSettings.TimerInterval*1000);
             _timer.Elapsed += (sender, e) => BackgroundSend();
             _timer.AutoReset = false;
             _timer.Start();
        }

        System.Timers.Timer _timer;
        /// <summary>
        /// Send mail and cached it for background sending if there was some errors in foreground
        /// Note: To pass credentials you must use the following code  
        ///    Cryptor cryptor = new Cryptor(true)
        ///    mailDTO.Credential.Login = Convert.ToBase64String(cryptor.EncryptMessage(Encoding.UTF8.GetBytes("mail"),cryptor.GetInitializationVector()));
        ///    mailDTO.Credential.Password = Convert.ToBase64String(cryptor.EncryptMessage(Encoding.UTF8.GetBytes("password"), cryptor.GetInitializationVector()));
        /// </summary>
        /// <param name="mailDTO"></param>
        /// <returns>If the sending was successfull</returns>
        public bool Send(MailDTO mailDTO)
        {
            MailMessage message = new MailMessage(mailDTO.FromStr, mailDTO.ToStr, mailDTO.SubjectStr, mailDTO.BodyStr);

            SmtpClient smtpClient = new SmtpClient(mailSenderSettings.Host, mailSenderSettings.Port);
            smtpClient.UseDefaultCredentials = false;
            Cryptor cryptor = new Cryptor(true);
           // string credentialUserDecrypted = Encoding.UTF8.GetString(cryptor.DecryptMessage(Convert.FromBase64String(mailDTO.Credential.Login), cryptor.GetInitializationVector()));
           // string credentialPasswordDecrypted = Encoding.UTF8.GetString(cryptor.DecryptMessage(Convert.FromBase64String(mailDTO.Credential.Password), cryptor.GetInitializationVector()));
             string credentialUserDecrypted = ConnectionStringHelper.GetCredentialString(mailDTO.Credential.Login);
             string credentialPasswordDecrypted = ConnectionStringHelper.GetCredentialString(mailDTO.Credential.Password);
            smtpClient.Credentials = new NetworkCredential(credentialUserDecrypted, credentialPasswordDecrypted);
            try
            {
                Log.WriteText("Sending message {0}",mailDTO.Serialize(Encoding.UTF8));
                smtpClient.Send(message);
                Log.WriteText("Message was send");
            }
            catch (Exception ex)
            {
                Log.WriteError("Cannot send message. Exception:\r\n{0}",ex.ToString());
                cacheDbMailSender.StoreMailSender(mailDTO, mailSenderSettings.Count);
                return false;
            }
            return true;

        }
        private void BackgroundSend()
        {
            try
            {
                cacheDbMailSender.ProcessMailSender(Send);
            }
            catch (Exception ex)
            {
                Log.WriteText("There was exception occered in background sending message of MailSender. Exception:\r\n{0}", ex.ToString());
            }
            finally
            {
                _timer.Start();
            }       
        }   
    }
    public class MailSenderSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public int TimerInterval { get; set; }

        public int Count { get; set; }
        public string DbInitialCatalog { get; set; }
        public string DbDataSource { get; set; }
        public string DbUserID { get; set; }
        public string DbPassword { get; set; }
    
    
    }
}
