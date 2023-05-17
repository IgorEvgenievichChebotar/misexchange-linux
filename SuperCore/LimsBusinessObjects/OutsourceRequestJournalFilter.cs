using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class OutsourceRequestJournalFilter
    {
        public OutsourceRequestJournalFilter()
        {
            States = new List<int>();
            Outsourcers = new RefSet();
        }
        [CSN("DateFrom")]
        public DateTime? DateFrom {get; set;}
        [CSN("DateTill")]
        public DateTime? DateTill {get; set;}
        [CSN("Priority")]
        public Int32? Priority {get; set;}
        [CSN("States")]
        public List<Int32> States {get; set;}
        [CSN("Outsourcers")]
        public RefSet Outsourcers {get; set;}
    }
}
