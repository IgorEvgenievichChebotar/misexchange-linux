using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    class LoggerProd : ILogger
    {
        public LoggerProd(IFileLoggerSettings settings)
        {
            Settings = settings;       
        }
        private IFileLoggerSettings Settings { get; set; }
        public void Debug(string message, DateTime? dateTime = default(DateTime?))
        {
            Log.Debug(message, dateTime);
        }

        public void Init()
        {
            String logFile = Settings.LogFile;//["logFile", false];
            if (String.IsNullOrEmpty(logFile))
                logFile = Path.Combine(FileHelper.AssemblyDirectory, "logFile.log");
            else if (Path.GetPathRoot(logFile).Equals(String.Empty))
                logFile = Path.Combine(FileHelper.AssemblyDirectory, (logFile.ToLower().Contains(".log")) ? logFile : logFile + ".log");
            ExportLog.InitializeLogging(logFile);
        }

        public void WriteError(string message)
        {
            Log.WriteError(message);
        }

        public void WriteError(string formatMessage, object arg0)
        {
            Log.WriteError(formatMessage, arg0);
        }

        public void WriteError(string formatMessage, object arg0, object arg1)
        {
            Log.WriteError(formatMessage,arg0,arg1);
        }

        public void WriteError(string formatMessage, object arg0, object arg1, object arg2)
        {
            Log.WriteError(formatMessage,arg0,arg1,arg2);
        }

        public void WriteText(string message)
        {
            Log.WriteText(message);
        }

        public void WriteText(string formatMessage, object arg0)
        {
            Log.WriteText(formatMessage,arg0);
        }

        public void WriteText(string formatMessage, object arg0, object arg1)
        {
            Log.WriteText(formatMessage, arg0, arg1);
        }

        public void WriteText(string formatMessage, object arg0, object arg1, object arg2)
        {
            Log.WriteText(formatMessage, arg0, arg1, arg2);
        }
    }
}
