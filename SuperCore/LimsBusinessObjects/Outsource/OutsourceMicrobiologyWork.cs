using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    
    public class OutsourceMicrobiologyWork
    {
        public OutsourceMicrobiologyWork()
        {
            MicroorganismResults = new List<OutsourceMicroorganismResult>();
        }

        /// <summary>
        /// Результаты поиска микроорганизмов
        /// </summary>
        [CSN("MicroorganismResults")]
        public List<OutsourceMicroorganismResult> MicroorganismResults { get; set; }
        
        [CSN("DateCompleted")]
        public DateTime DateCompleted { get; set; }

        [CSN("Test")]
        public ObjectRef Test { get; set; }

        [SendToServer(false)]
        [CSN("Name")]
        public String Name { get; set; }

       // [SendToServer(false)]
        [CSN("Code")]
        public String Code { get; set; }
    }
}
