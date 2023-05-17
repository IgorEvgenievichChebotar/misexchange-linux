using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.Common
{
    public class RequestWasAlreadyAcceptedException : Exception
    {
        const string formatStr = "Request [{0}] had been already accepted early";
        public RequestWasAlreadyAcceptedException(string requestCode)
            : base(String.Format(formatStr,requestCode))
        { }
    }
}
