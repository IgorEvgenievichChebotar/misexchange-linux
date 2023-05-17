using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using ru.novolabs.SuperCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Windows.Forms;

namespace ru.novolabs
{
    public class ExportLog
    {
        private const string mainAppAppenderName = "mainAppAppender";
        private static ru.novolabs.SuperCore.Crypto.Cryptor cryptor;
        private static string GetCorrectPath(string path)
        {
            string result = path;
            if (!Path.IsPathRooted(result))
                //result = Path.Combine(Application.StartupPath, path);
                result = Path.Combine(PathHelper.AssemblyDirectory, path);

            return result;
        }
        
        public static void InitializeLogging(String logPath, bool checkPathCorrect = true, bool checkSettings = true)
        {
            if (checkPathCorrect)
            {
                logPath = GetCorrectPath(logPath);
            }
            cryptor = new SuperCore.Crypto.Cryptor(true);
            RollingFileAppender appender = new RollingFileAppender();
            String rotateLogs = "false";
            if (checkSettings && ProgramContext.Settings["rotateLogs", false] != null)
                rotateLogs = ProgramContext.Settings["rotateLogs", false].ToString();
            
            //Имя файла, в который будет сохраняться лог
            appender.File = logPath;
            appender.Name = mainAppAppenderName;
            appender.AppendToFile = true;

            //Максимальный размер файла, после которого будет создан новый файл (для RollingStyle - Size и Composite)
            appender.MaximumFileSize = "1000MB";

            if (rotateLogs != "true")
            {
                appender.StaticLogFileName = true;
                appender.MaxSizeRollBackups = 5;
                //Тип "перезаписи" файла
                appender.RollingStyle = RollingFileAppender.RollingMode.Once;
                //appender.DatePattern = "ddMMyyyy HH:mm'.log'";
            }
            else
            {
                //Максимальный размер файла, после которого будет создан новый файл (для RollingStyle - Size и Composite)
                appender.RollingStyle = RollingFileAppender.RollingMode.Date;
                //Шаблон даты, дописывается в конец имени файла (для RollingStyle - Date и Composite)
                appender.DatePattern = "yyyy-MM-dd'.log'";
                appender.StaticLogFileName = false;
                //appender.File = System.IO.Path.GetDirectoryName(logPath) + "\\";
                appender.File = PathHelper.AssemblyDirectory + "/logs/";
            }


            //Формат добавления записей в лог
            PatternLayout layout = new PatternLayout("(%d)  %m%n"); // %logger (%d)  %m%n
            layout.ActivateOptions();
            appender.Layout = layout;
            appender.ActivateOptions();

            bool repositoryExists = LogManager.GetAllRepositories().Any(rep => rep.Name == mainAppAppenderName);
            Hierarchy repository = repositoryExists ? (Hierarchy)LogManager.GetRepository(mainAppAppenderName) : (Hierarchy)LogManager.CreateRepository(mainAppAppenderName);
            repository.Root.Level = repository.LevelMap["ALL"];
            repository.Root.AddAppender(appender);
            BasicConfigurator.Configure(repository, appender);
        }

        static void Log(String prefix, String text)
        {
            string message = String.Format("[ThreadId: {0}] {1}", Thread.CurrentThread.ManagedThreadId.ToString(), prefix + text);
#if !DEBUG
            byte[] encr = cryptor.EncryptMessage(Encoding.Default.GetBytes(message), cryptor.GetInitializationVector());
            message = Convert.ToBase64String(encr);
#endif
            LogManager.GetLogger(mainAppAppenderName, mainAppAppenderName).Info(message);
        }

        public static void Shutdown()
        {
            LogManager.Shutdown();
        }

        public static void Error(String text)
        {
            Log("Error: ", text);
        }

        public static void Text(String text)
        {
            Log(String.Empty, text);
        }
    }

    public class Log
    {
        public static void WriteText(String message)
        {
            ExportLog.Text(message);

#if (DEBUG)
            string text = String.Format("[ThreadId: {0}] {1}", Thread.CurrentThread.ManagedThreadId.ToString(), message);
            Trace.WriteLine(String.Format("{0}.{1}: {2}", DateTime.Now.ToString(), DateTime.Now.Millisecond.ToString("D3"), text));
#endif
        }

        public static void WriteText(string formatMessage, Object arg0)
        {
            string message = String.Format(formatMessage, arg0);
            WriteText(message);
        }

        public static void WriteText(string formatMessage, Object arg0, Object arg1)
        {
            string message = String.Format(formatMessage, arg0, arg1);
            WriteText(message);
        }

        public static void WriteText(string formatMessage, Object arg0, Object arg1, Object arg2)
        {
            string message = String.Format(formatMessage, arg0, arg1, arg2);
            WriteText(message);
        }

        public static void Debug(string message, DateTime? dateTime = null)
        {
#if (DEBUG)
            if (dateTime == null)
                dateTime = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(dateTime.Value.ToString("dd.MM.yyyy HH:mm:ss.") + dateTime.Value.Millisecond.ToString("D3") + " " + message);
#endif
        }

        public static void WriteError(String message)
        {
            ExportLog.Error(message);

#if (DEBUG)
            System.Diagnostics.Debug.WriteLine("\n\r{0}.{1}: Error: {2}", DateTime.Now.ToString(), DateTime.Now.Millisecond.ToString("D3"), message);
#endif
        }

        public static void WriteError(string formatMessage, Object arg0)
        {
            string message = String.Format(formatMessage, arg0);
            WriteError(message);
        }

        public static void WriteError(string formatMessage, Object arg0, Object arg1)
        {
            string message = String.Format(formatMessage, arg0, arg1);
            WriteError(message);
        }

        public static void WriteError(string formatMessage, Object arg0, Object arg1, Object arg2)
        {
            string message = String.Format(formatMessage, arg0, arg1, arg2);
            WriteError(message);
        }
    }
}
