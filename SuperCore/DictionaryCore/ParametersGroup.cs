using System;
using System.Collections.Generic;
using System.Text;
using FicExchange.Core;

namespace FicExchange.Dictionary
{

    public class Parameter:  DictionaryItem
    {

        private ObjectRef userDirectory = new ObjectRef();

        public ObjectRef UserDirectory
        {
            get { return userDirectory; }
            set { userDirectory = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ParameterGroup:  DictionaryItem
    {
        private int listType = 0;
        private List<Parameter> parameters = new List<Parameter>();

        public List<Parameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }


        public int ListType
        {
            get { return listType; }
            set { listType = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
