using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class FindAutoWorklistsFilter
    {
        public FindAutoWorklistsFilter()
        {
            Equipments = new List<ObjectRef>();
            States = new List<int>();
        }

        [CSN("Equipments")]
        public List<ObjectRef> Equipments { get; set; }
        [CSN("States")]
        public List<int> States { get; set; }


        ///// <summary>
        ///// ссылка на заявку. (Указывается в случае, если авто-рабочий список создавался по заявке - в запросе create-requests-xxx)
        ///// </summary>
        //[CSN("Request")]
        //public ObjectRef Request { get; set; }
    }
}
