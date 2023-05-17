using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ServerOption
    {
        private String name;
        private String value;

        [CSN("Name")]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [CSN("Value")]
        public String Value
        {
            get { return value; }
            set { this.value = value; }
        }


    }

    public class ServerOptionList
    {
        private List<ServerOption> result = new List<ServerOption>();

        [Unnamed]
        [CSN("Result")]
        public List<ServerOption> Result
        {
            get { return result; }
            set { result = value; }
        }

        public ServerOption GetServerOption(String Name)
        {
            foreach (ServerOption option in result)
            {
                if (option.Name.Equals(Name))
                    return option;
            }
            return null;
        }
    }
}
