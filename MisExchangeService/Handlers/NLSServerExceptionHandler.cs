using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Handlers
{
    static class NLSServerExceptionHandler
    {
        static List<int> LISServerErrorCodes = new List<int>()
        {
            883,
            906
        };

        public static string GetErrorMessage(int errorCode, string errorStr, string defaultErrrorStr)
        {
            int foundErrorCode = LISServerErrorCodes.Find(code => code == errorCode);
            if (foundErrorCode == default(int))
            {
                return defaultErrrorStr;            
            }
            string replacementErrorCodeStr = "LIS-" + foundErrorCode;
            return errorStr.Replace(replacementErrorCodeStr, "").TrimStart(new char[] { '.', ' ' });
        
        }

    }
}
