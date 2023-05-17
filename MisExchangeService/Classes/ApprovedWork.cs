using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore;

namespace ru.novolabs.MisExchangeService.Classes
{
    public class ApprovedWork
    {
        public ApprovedWork()
        {
            Request = new ObjectRef();
        }

        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Request")]
        public ObjectRef Request { get; set; }
    }

    public class ApprovedWorks
    {
        public ApprovedWorks()
        {
            Works = new List<ApprovedWork>();
        }

        [CSN("Works")]
        public List<ApprovedWork> Works { get; set; }
    }
}
