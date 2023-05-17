using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LisBusinessObjects
{
    public class ServerOption
    {
        private String code;
        private String value;

        [CSN("Code")]
        public String Code
        {
            get { return code; }
            set { code = value; }
        }

        [CSN("Value")]
        public String Value
        {
            get { return value; }
            set { this.value = value; }
        }

        [CSN("DefaultValue")]
        public String DefaultValue
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    public class ServerOptionList
    {

        private List<ServerOption> elements = new List<ServerOption>();

        [Unnamed]
        public List<ServerOption> Elements
        {
            get { return elements; }
            set { elements = value; }
        }
    }
}
