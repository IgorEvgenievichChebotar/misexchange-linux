using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces
{
    public interface ILogger
    {
        void Init();

        void WriteText(String message);

        void WriteText(string formatMessage, Object arg0);

        void WriteText(string formatMessage, Object arg0, Object arg1);

        void WriteText(string formatMessage, Object arg0, Object arg1, Object arg2);

        void Debug(string message, DateTime? dateTime = null);

        void WriteError(String message);

        void WriteError(string formatMessage, Object arg0);

        void WriteError(string formatMessage, Object arg0, Object arg1);

        void WriteError(string formatMessage, Object arg0, Object arg1, Object arg2);
    }
}
