using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Cachers
{
    public class FileRequestsCacherSettings
    {
        public FileRequestsCacherSettings()
        {
        }

        public String WorkingDirectory { get; set; }

        public String ErrorDirectory { get; set; }
    }
}
