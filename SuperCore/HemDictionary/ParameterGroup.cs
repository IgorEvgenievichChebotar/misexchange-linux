using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class ParameterGroupDictionaryItem : DictionaryItem
    {
        public List<ParameterClass> parameters = new List<ParameterClass>();

        public ParameterClass Find(int Id)
        {
            foreach (ParameterClass ParameterClass in parameters)
            {
                if (ParameterClass.Id == Id)
                {
                    return ParameterClass;
                }
            }
            return (ParameterClass)null;
        }
    }
   
    public class ParameterGroup:  DictionaryItem
    {
        private string externalCode = string.Empty;
        [CSN("ExternalCode")]
        public new string ExternalCode
        {
            get { return externalCode; }
            set { externalCode = value; }
        }
        private int listType = 0;
        private List<Parameter> parameters = new List<Parameter>();

        public Parameter Find(int Id)
        {
            foreach (Parameter Parameter in parameters)
            {
                if (Parameter.Id == Id)
                {
                    return Parameter;
                }
            }
            return null;
        }
        [CSN("Parameters")]
        public List<Parameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        [CSN("ListType")]
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

    public class ParameterGroupDictionary<Class> : DictionaryClass<Class> where Class : ParameterGroup
    {
        // Methods
        public ParameterGroupDictionary(string DictionaryName): base(DictionaryName)
        {
            base.name = DictionaryName;
        }

        public override object GetByReference(Type type, int objRef)
        {
            if (type.Equals(typeof(ParameterGroup)))
            {
            return base.GetByReference(type, objRef);
            }
            else if (type.Equals(typeof(Parameter)))
            {
                foreach(ParameterGroup paramGroup in Elements)
                {
                    foreach(Parameter param in paramGroup.Parameters)
                    {
                        if (param.Id == objRef)
                        {
                            return param;
                        }
                    }
                }
            }
            return null;
        }

        public Parameter GetParameterByExternalCode(String extCode)
        {
            foreach (ParameterGroup parameterGroup in Elements)
            {
                foreach (Parameter parameter in parameterGroup.Parameters)
                {
                    if (extCode.ToLower().Equals(parameter.ExternalCode.ToLower()))
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public Parameter GetParameterByCode(String code, Int32 ListType = -1)
        {
            foreach (ParameterGroup parameterGroup in Elements)
            {
                if (ListType == -1 || (parameterGroup.ListType == ListType && ListType != -1))
                {
                    foreach (Parameter parameter in parameterGroup.Parameters)
                    {
                        if (parameter.Code.Equals(code))
                            return parameter;
                    }
                }
            }
            return null;
        }

        public Parameter GetParameterById(Int32 Id)
        {
            foreach (ParameterGroup parameterGroup in Elements)
            {
                foreach (Parameter parameter in parameterGroup.Parameters)
                {
                    if (parameter.Id.Equals(Id))
                        return parameter;
                }
            }
            return null;
        }

        public string GetParameterGroupName(Int32 Id)
        {
            foreach (ParameterGroup parameterGroup in Elements)
            {
                foreach (Parameter parameter in parameterGroup.Parameters)
                {
                    if (parameter.Id.Equals(Id))
                        return parameterGroup.Name;
                }
            }
            return null;
        }

        public List<DictionaryItem> GetAllParametersAsDictionaryItems(Boolean Removed = false, Int32 ListType = -1)
        {
            List<DictionaryItem> result = new List<DictionaryItem>();
            foreach (ParameterGroup parameterGroup in Elements)
            {
                if ((Removed && parameterGroup.Removed) || (!parameterGroup.Removed))
                    if (ListType == -1 || (ListType != -1 && ListType == parameterGroup.ListType))
                    {
                        foreach (Parameter parameter in parameterGroup.Parameters)
                        {
                            if ((Removed && parameter.Removed) || (!parameter.Removed))
                                result.Add(new DictionaryItem() { Name = parameter.Name, Id = parameter.Id, Code = parameter.Code });
                        }
                    }
            }
            return result;
        }
    }

    
}
