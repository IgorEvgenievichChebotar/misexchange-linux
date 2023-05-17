using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class WorklistJournalFilter : BaseFilter
    {
        public WorklistJournalFilter()
        {
            States = new List<ObjectRef>();
            Users = new List<ObjectRef>();
        }

        public override void Clear()
        {
            base.Clear();
            WorklistDef = null;
            DateFrom = null;
            DateTill = null;
            States.Clear();
            Code = null;
            SampleNr = null;
            Users.Clear();
            SendRemote = WorklistSendRemoteState.ALL;
        }
        [CSN("WorklistDef")]
        public WorklistDefDictionaryItem WorklistDef { get; set; }
        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        [CSN("States")]
        public List<ObjectRef> States { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("SampleNr")]
        public String SampleNr { get; set; }
        [CSN("Users")]
        public List<ObjectRef> Users { get; set; }
        [CSN("SendRemote")]
        public Int32 SendRemote { get; set; }
    }
}