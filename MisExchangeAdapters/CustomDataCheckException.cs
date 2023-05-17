using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange
{
    public class CustomDataCheckException : Exception
    {
        public CustomDataCheckException(List<string> errors)
        {
            Errors = errors;
            foreach (String error in Errors)
                Log.WriteError(error);
        }

        public List<string> Errors { get; set; }

        public override string Message
        {
            get
            {
                return String.Join("\r\n", Errors.ToArray());
            }
        }
    }
}
