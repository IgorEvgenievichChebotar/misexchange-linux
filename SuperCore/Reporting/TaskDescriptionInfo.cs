using System;
using System.Globalization;
using System.Linq;

namespace ru.novolabs.SuperCore.Reporting
{
    public class TaskDescriptionInfo
    {
        public String Name { get { return TaskDescription.Name; } }
        public TaskDescription TaskDescription { get; set; }
        
    }
}