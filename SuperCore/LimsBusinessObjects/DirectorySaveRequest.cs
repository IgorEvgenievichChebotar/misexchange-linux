using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class DirectorySaveRequest
    {
        [CSN("Directory")]
         public String Directory {get; set;}
        [CSN("Element")]
         public Object Element { get; set; }
    }
}
