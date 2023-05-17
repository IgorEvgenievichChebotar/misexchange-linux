using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public static class BloodParameterFieldType
    {
        public const Int32 HEM_FIELD_TYPE_UNKNOWN = 0;
        public const Int32 HEM_FIELD_TYPE_STRING = 1;
        public const Int32 HEM_FIELD_TYPE_NUMERIC = 2;
        public const Int32 HEM_FIELD_TYPE_BOOLEAN = 3;
        public const Int32 HEM_FIELD_TYPE_DATETIME = 4;
        public const Int32 HEM_FIELD_TYPE_ENUMERATION = 5;
        public const Int32 HEM_FIELD_TYPE_SET = 6;
        public const Int32 HEM_FIELD_TYPE_SEX = 8;
    }

    public class BloodParameterItem : Parameter
    {
        [CSN("Group")]
        public BloodParameterGroup Group { get; set; }

        [CSN("HistoryRestriction")]
        public Int32 HistoryRestriction { get; set; }

        [CSN("CreateByDefaultInBatch")]
        public Boolean CreateByDefaultInBatch { get; set; }

        [CSN("DonorEditRank")]
        public Int32 DonorEditRank { get; set; }

        [CSN("ProductEditRank")]
        public Int32 ProductEditRank { get; set; }

        [CSN("BloodFieldType")]
        public Int32 BloodFieldType { get; set; }

        [CSN("VisitRestriction")]
        public Int32 VisitRestriction { get; set; }

        [CSN("RequiredInBatch")]
        public Boolean RequiredInBatch { get; set; }

        [CSN("BatchEditRank")]
        public Int32 BatchEditRank { get; set; }

        [CSN("SendToLIS")]
        public Boolean SendToLIS { get; set; }

        [CSN("SendByCode")]
        public Boolean SendByCode { get; set; }

        [CSN("DefaultValue")]
        public String DefaultValue { get; set; }


    }

    public class BloodParameterGroup : DictionaryItem
    {
        private List<BloodParameterItem> parameters = new List<BloodParameterItem>();
        [CSN("Parameters")]
        public List<BloodParameterItem> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }

    public class BloodParameterGroupDictionaryClass : DictionaryClass<BloodParameterGroup>
    {
        // Methods
        public BloodParameterGroupDictionaryClass(string DictionaryName)
            : base(DictionaryName)
        {
            base.name = DictionaryName;
        }

        public override object GetByReference(Type type, int objRef)
        {
            if (type.Equals(typeof(BloodParameterGroup)))
            {
                return base.GetByReference(type, objRef);
            }
            else if (type.Equals(typeof(BloodParameterItem)))
            {
                foreach (BloodParameterGroup paramGroup in Elements)
                {
                    foreach (BloodParameterItem param in paramGroup.Parameters)
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

        public BloodParameterItem GetBloodParameterByCode(String code, Boolean ShowRemoved = false)
        {
            
            foreach (BloodParameterGroup parameterGroup in Elements)
            {
                foreach (BloodParameterItem parameter in parameterGroup.Parameters)
                {
                    if (!parameter.Removed || ShowRemoved)
                        if (parameter.Code.Equals(code))
                            return parameter;
                }
            }
            return null;
        }


        public BloodParameterItem GetBloodParameterById(Int32 Id)
        {
            foreach (BloodParameterGroup parameterGroup in Elements)
            {
                foreach (BloodParameterItem parameter in parameterGroup.Parameters)
                {
                    if (parameter.Id.Equals(Id))
                        return parameter;
                }
            }
            return null;
        }

        public List<BloodParameterItem> GetAllBloodParameters(Boolean Removed = false)
        {
            List<BloodParameterItem> result = new List<BloodParameterItem>();
            foreach (BloodParameterGroup parameterGroup in Elements)
            {
                if ((Removed) || (!parameterGroup.Removed))
                    foreach (BloodParameterItem parameter in parameterGroup.Parameters)
                    {
                        if ((Removed) || (!parameter.Removed))
                            result.Add(parameter);
                    }
            }
            return result;
        }

        public List<DictionaryItem> GetAllBloodParametersAsDictionaryItems(Boolean Removed = false)
        {
            List<DictionaryItem> result = new List<DictionaryItem>();
            foreach (BloodParameterGroup parameterGroup in Elements)
            {
                if ((Removed) || (!parameterGroup.Removed))
                    foreach (BloodParameterItem parameter in parameterGroup.Parameters)
                    {
                        if ((Removed) || (!parameter.Removed))
                        result.Add(new DictionaryItem() { Name = parameter.Name, Id = parameter.Id, Code = parameter.Code });
                    }
            }
            return result;
        }
    }

}
