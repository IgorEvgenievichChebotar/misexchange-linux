using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class BaseParameterValue : BaseObject
    {
        public BaseParameterValue()
        {
            Parameter = new Parameter();
            Values = new List<ObjectRef>();
        }

        /// <summary>
        /// Указатель на пользовательский справочник из которого взяты значения
        /// </summary>
        [CSN("Parameter")]
        public Parameter Parameter { get; set; }

        public Parameter GetFullParameter()
        {
            if (this.GetType().Equals(typeof(BloodParameterValue)))
            {
                return (Parameter)((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterById(Parameter.Id);
            }
            else
                if (this.GetType().Equals(typeof(ParameterValue)))
                {
                    return (Parameter)((ParameterGroupDictionary<ParameterGroup>)ProgramContext.Dictionaries[HemDictionaryNames.ParameterGroup]).GetParameterById(Parameter.Id);
                }
            return null;
        }

        [CSN("Value")]
        public String Value { get; set; }

        [CSN("Values")]
        public List<ObjectRef> Values { get; set; }



        private UserDictionaryValue reference;
        /// <summary>
        /// Указатель на элемент справочника выбранного значения
        /// </summary>
        [CSN("Reference")]
        [SendAsRef(true)]
        public UserDictionaryValue Reference {
            get
            {
                if (reference == null)
                    return new UserDictionaryValue() { Id = 0 };
                return reference;
            }
            set
            {
                reference = value;
            }
        }

        [SendToServer(false)]
        [CSN("ExternalCode")]        
        public String ExternalCode { get; set; }


        [SendToServer(false)]
        [CSN("_Value")]
        public String _Value
        {
            get
            {
                switch (this.Parameter.FieldType)
                {
                    default:
                    case ParameterTypes.TYPE_BOOLEAN:
                        return this.Value;
                    case ParameterTypes.TYPE_SET:
                        String val = "";
                        foreach (ObjectRef v in Values)
                        {
                            val += v.Id.ToString() + ",";
                        }
                        return val;
                    case ParameterTypes.TYPE_ENUM:
                        if (this.Reference == null)
                            return null;
                        return this.Reference.Id.ToString();
                }
            }
            set
            {
                if (this.Parameter != null)
                    switch (this.Parameter.FieldType)
                    {
                        default:
                        case ParameterTypes.TYPE_BOOLEAN:
                            Value = value;
                            break;
                        case ParameterTypes.TYPE_SET:
                            String[] Vals = value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (String val in Vals)
                            {
                                Boolean contained = false;
                                foreach (ObjectRef r in this.Values)
                                {
                                    if (r.Id == Int32.Parse(val))
                                        contained = true;
                                }
                                if (!contained)
                                    this.Values.Add(new ObjectRef() { Id = Int32.Parse(val) });
                            }
                            break;
                        case ParameterTypes.TYPE_ENUM:
                            this.Reference = (UserDictionaryValue)((UserDirectoryDictionary)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory]).GetByReference(typeof(UserDictionaryValue), Int32.Parse(value));
                                //new ObjectRef() { Id = Int32.Parse(value) };
                            break;
                    }
            }
        }
    }
}