using ru.novolabs.SuperCore.HemDictionary;
using System;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    [Serializable]
    public class BloodParameterValue : BaseParameterValue
    {
        [SendToServer(false)]
        [CSN("CreateByDefault")]
        public Boolean CreateByDefault { get; set; }

        [SendToServer(false)]
        [CSN("Required")]
        public Boolean Required { get; set; }

        [SendToServer(false)]
        [CSN("Date")]
        public DateTime? Date { get; set; }

        [SendToServer(false)]
        [CSN("Expired")]
        public Boolean Expired { get; set; }

        [SendToServer(false)]
        [CSN("_Value")]
        public new String _Value
        {
            get
            {
                switch (this.Parameter.FieldType)
                {
                    default:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_BOOLEAN:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_STRING:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_NUMERIC:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_DATETIME:
                        return this.Value;
                    case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                        string val = "";
                        foreach (ObjectRef v in Values)
                        {
                            val += v.Id.ToString() + ",";
                        }
                        return val;
                    case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                        return this.Reference.Id.ToString();
                        
                }
            }
            set
            {
                if (this.Parameter != null)
                    switch (this.Parameter.FieldType)
                    {

                        default:
                        case BloodParameterFieldType.HEM_FIELD_TYPE_BOOLEAN:
                        case BloodParameterFieldType.HEM_FIELD_TYPE_STRING:
                        case BloodParameterFieldType.HEM_FIELD_TYPE_NUMERIC:
                        case BloodParameterFieldType.HEM_FIELD_TYPE_DATETIME:
                            Value = value;
                            break;
                        case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
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
                        case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                            this.Reference = (UserDictionaryValue)((UserDirectoryDictionary)ProgramContext.Dictionaries[HemDictionaryNames.UserDirectory]).GetByReference(typeof(UserDictionaryValue), Int32.Parse(Value));
                                //new ObjectRef() { Id = Int32.Parse(value) };
                            break;

                    }
            }
        }
    }
}