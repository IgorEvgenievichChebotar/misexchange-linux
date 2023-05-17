using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class PersonHistory
    {
        public PersonHistory()
        {
            Result = new List<PersonHistoryResponseListItem>();
        }
        
        [CSN("Result")]
        public List<PersonHistoryResponseListItem> Result { get; set; }

        public class PersonHistoryResponseListItem
        {
            public PersonHistoryResponseListItem()
            {
                Works = new List<WorkShort>();
            }

            [DateTimeFormat(@"dd/MM/yyyy")]
            [CSN("Date")]
            public DateTime? Date { get; set; }

            [CSN("Works")]
            public List<WorkShort> Works { get; set; }
        }
    }

    public class WorkShort
    {
        [CSN("Work")]
        public WorkDataShort Work { get; set; }
        [CSN("Value")]
        public string Value { get; set; }
        [CSN("UnitName")]
        public string UnitName { get; set; }

        public WorkShort()
        { }
    }

    public class WorkDataShort
    {
        [CSN("Test")]
        public ObjectRef Test { get; set; }
        [CSN("Rank")]
        public int Rank { get; set; }
        [CSN("Code")]
        public string Code { get; set; }
        [CSN("Mnemonics")]
        public string Mnemonics { get; set; }
        [CSN("Name")]
        public string Name { get; set; }

        public WorkDataShort()
        { }
    }
}
