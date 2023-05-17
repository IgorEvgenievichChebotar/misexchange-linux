using System;


namespace ru.novolabs.ExchangeDTOs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldProps : Attribute
    {
        public FieldProps(Boolean mandatory = false, Int32 maxLength = -1, Int32 minValue = Int32.MinValue,  Int32 maxValue = Int32.MaxValue ) 
        {
            Mandatory = mandatory;
            MaxLength = maxLength == -1? null: (Int32?) maxLength;
            MinValue = minValue == Int32.MinValue? null: (Int32?) minValue;
            MaxValue = maxValue == Int32.MaxValue? null: (Int32?) maxValue;
        }

        public Int32? MaxLength { get; private set; }
        public Boolean Mandatory { get; private set; }
        public Int32? MinValue { get; private set; }
        public Int32? MaxValue { get; private set; }
    }

}