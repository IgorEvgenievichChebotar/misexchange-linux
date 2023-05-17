using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{

    public class AnamnesisParameter : DictionaryItem
    {

        //цвет фона
        [CSN("BgColor")]
        public Int32 BgColor { get; set; }

        //цвет шрифта
        [CSN("FontColor")]
        public Int32 FontColor { get; set; }

        //внешний код
        [CSN("ExternalCode")]
        public new String ExternalCode { get; set; }

        //Ранг
        [CSN("Rank")]
        public Int32 Rank { get; set; }

        [CSN("Strict")]
        public Boolean Strict { get; set; }

        [CSN("NeedTime")]
        public Boolean NeedTime { get; set; }

        private UserDirectoryDictionaryItem userDirectory = new UserDirectoryDictionaryItem();
        [CSN("UserDirectory")]
        public UserDirectoryDictionaryItem UserDirectory
        {
            get { return userDirectory; }
            set { userDirectory = value; }
        }

        [CSN("FieldType")]
        public Int32 FieldType { get; set; }

        [CSN("DefaultValue")]
        public String DefaultValue { get; set; }


    }

    public class AnamnesisParameterGroup : DictionaryItem
    {
        private List<AnamnesisParameter> parameters = new List<AnamnesisParameter>();
        [CSN("Parameters")]
        public List<AnamnesisParameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        //цвет фона
        [CSN("BgColor")]
        public Int32 BgColor { get; set; }

        //цвет шрифта
        [CSN("FontColor")]
        public Int32 FontColor { get; set; }

        //внешний код
        [CSN("ExternalCode")]
        public new String ExternalCode { get; set; }

        //Ранг
        [CSN("Rank")]
        public Int32 Rank { get; set; }
    }

    public class AnamnesisParameterGroupDictionaryClass : DictionaryClass<AnamnesisParameterGroup>
    {
        // Methods
        public AnamnesisParameterGroupDictionaryClass(string DictionaryName)
            : base(DictionaryName)
        {
            base.name = DictionaryName;
        }

        public override object GetByReference(Type type, int objRef)
        {
            if (type.Equals(typeof(AnamnesisParameterGroup)))
            {
                return base.GetByReference(type, objRef);
            }
            else if (type.Equals(typeof(AnamnesisParameterGroup)))
            {
                foreach (AnamnesisParameterGroup paramGroup in Elements)
                {
                    foreach (AnamnesisParameter param in paramGroup.Parameters)
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

        public AnamnesisParameter GetParameterByCode(String code)
        {
            foreach (AnamnesisParameterGroup parameterGroup in Elements)
            {
                foreach (AnamnesisParameter parameter in parameterGroup.Parameters)
                {
                    if (parameter.Code.Equals(code))
                        return parameter;
                }
            }
            return null;
        }

    }

}
