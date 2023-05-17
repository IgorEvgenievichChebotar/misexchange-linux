using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.WebCore
{
    public class WebServerReloadingException :Exception
    {
        public WebServerReloadingException(string message) : base(message) { }
    }
}
