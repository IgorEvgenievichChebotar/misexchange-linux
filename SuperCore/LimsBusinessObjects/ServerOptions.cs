using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
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
        [CSN("Elements")]
        public List<ServerOption> Elements
        {
            get { return elements; }
            set { elements = value; }
        }

        public ServerOption GetServerOption(String Name)
        {
            foreach (ServerOption option in Elements)
            {
                if (option.Code.Equals(Name))
                    return option;
            }
            return null;
        }

        public bool OptionValueEquals(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(value);
            ServerOption option = GetServerOption(name);
            if (option == null)
                return false;
            return option.Value.ToLower().Equals(value.ToLower());
        }
    }
}
