using System;

namespace ru.novolabs.ExchangeDTOs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength : Attribute
    {
        public MaxLength(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }
}
