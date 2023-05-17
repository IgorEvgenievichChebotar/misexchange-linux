using ru.novolabs.MisExchange.MainDependenceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    class LoggerNull : ILogger
    {
        private string loggerName;
        public LoggerNull()
        {
            loggerName = this.GetType().Name;
        }
        private void RegisterCall(string methodName, params object[] paramList)
        {
            if (paramList == null)
                paramList = Enumerable.Empty<string>().ToArray();
            paramList = paramList.Select(p => p ?? String.Empty).ToArray();
            string paramsStr = paramList.Length < 1 ? String.Empty : String.Join("; ", paramList);
            string format = String.Format("Logger: [{0}]; Called Method: [{1}]; Arguments: [{2}]", loggerName, methodName, paramsStr);
            System.Diagnostics.Debug.WriteLine(format);       
        }
        public void Init()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName);          
        }

        public void WriteText(string message)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, message);            
        }

        public void WriteText(string formatMessage, object arg0)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, arg0);            
        }

        public void WriteText(string formatMessage, object arg0, object arg1)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, arg0, arg1);            
        }

        public void WriteText(string formatMessage, object arg0, object arg1, object arg2)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, arg0, arg1, arg2);            
        }

        public void Debug(string message, DateTime? dateTime = null)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, message, dateTime);            
        }

        public void WriteError(string message)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, message);            
        }

        public void WriteError(string formatMessage, object arg0)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, formatMessage, arg0);           
        }

        public void WriteError(string formatMessage, object arg0, object arg1)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, formatMessage, arg0, arg1);             
        }

        public void WriteError(string formatMessage, object arg0, object arg1, object arg2)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            RegisterCall(methodName, formatMessage, arg0, arg1, arg2);        
        }
    }
}
