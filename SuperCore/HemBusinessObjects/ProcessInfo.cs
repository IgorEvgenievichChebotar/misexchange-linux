using System;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ProcessInfo
    {
        public ProcessInfo()
        {
            ProcessTemplate = new ObjectRef();
            Process = new ObjectRef();        
        }

        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("StartDate")]
        public DateTime StartDate { get; set; }
        [CSN("EndDate")]
        public DateTime EndDate { get; set; }
        [CSN("ProcessTemplate")]
        public ObjectRef ProcessTemplate { get; set; }
        [CSN("Process")]
        public ObjectRef Process { get; set; }
    }
}