using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.ExchangeDTOs
{
    
    public class DirectoryExportRequest
    {
        public DirectoryExportRequest()
        { 
            Directories = new List<String>();
        }

        public List<String> Directories { get; set; }
    }
}
