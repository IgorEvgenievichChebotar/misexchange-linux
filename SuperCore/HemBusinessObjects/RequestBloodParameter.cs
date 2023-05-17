using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class RequestBloodParameter
    {
        public RequestBloodParameter()
        {
            Values = new List<ObjectRef>();
        }

        [CSN("Parameter")]
        public ObjectRef Parameter { get; set; }
        [CSN("Values")]
        public List<ObjectRef> Values { get; set; }
    }
}
