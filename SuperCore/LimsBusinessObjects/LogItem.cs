using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class LogItem : BaseObject
    {
        public LogItem()
        {
            //            
        }

        [CSN("ObjectType")]
        public int? ObjectType { get; set; }
        /// <summary>
        /// Ссылка на сотрудника
        /// </summary>
        [SendAsRef(true)]
        [CSN("Employee")]
        public EmployeeDictionaryItem Employee { get; set; }

        [CSN("ObjectId")]
        public int? ObjectId { get; set; }

        [CSN("ObjectNr")]
        public string ObjectNr { get; set; }

        [CSN("Action")]
        public string Action { get; set; }

        [CSN("Details")]
        public string Details { get; set; }

        [CSN("Date")]
        public DateTime? Date { get; set; }

        [CSN("Machine")]
        public string Machine { get; set; }        
    }
}
