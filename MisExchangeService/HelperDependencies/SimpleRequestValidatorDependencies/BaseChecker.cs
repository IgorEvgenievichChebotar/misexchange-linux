using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies
{
    class BaseChecker
    {
        protected List<ErrorTriplet> errors;
        public BaseChecker(List<ErrorTriplet> errors)
        {
            this.errors = errors;        
        }
        public virtual void CheckIsMandatoryNull<T>(T propertyValue, string objectName, string propertyName)
        {
            if (!CheckIsMandatoryNull(propertyValue))
            {
                errors.Add(new ErrorTriplet { ObjectName = objectName, PropertyName = propertyName, ErrorValidationType = ErrorValidationType.IsMandatory });
            }
        }
        public virtual void CheckMaxLength(string propertyValue, int maxLength, string objectName, string propertyName)
        {
            if (!CheckMaxLength(propertyValue, maxLength))
            {
                errors.Add(new ErrorTriplet { ObjectName = objectName, PropertyName = propertyName, ErrorValidationType = ErrorValidationType.MaxLength, ControlValue = maxLength.ToString() });
            }
        }
        public virtual void CheckMinValue<T>(T propertyValue, int minValue, string objectName, string propertyName)
        {
            if (!CheckMinValue(propertyValue, minValue))
            {
                errors.Add(new ErrorTriplet { ObjectName = objectName, PropertyName = propertyName, ErrorValidationType = ErrorValidationType.MinValue, ControlValue = minValue.ToString() });
            }
        }
        public virtual void CheckMaxValue<T>(T propertyValue, int maxValue, string objectName, string propertyName)
        {
            if (!CheckMaxValue(propertyValue, maxValue))
            {
                errors.Add(new ErrorTriplet { ObjectName = objectName, PropertyName = propertyName, ErrorValidationType = ErrorValidationType.MaxValue, ControlValue = maxValue.ToString() });
            }
        }

        public bool CheckMaxValue<T>(T propertyValue, int maxValue)
        {
            int parseIntVal;
            Type type = typeof(T);

            if (propertyValue == null)
            {
                return true;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsNumber(type.GetGenericArguments()[0]))
            {
                return Convert.ToInt32(propertyValue) <= maxValue;
            }

            if (IsNumber(type))
            {
                return Convert.ToInt32(propertyValue) <= maxValue;
            }
            else if (type == typeof(String) && Int32.TryParse(Convert.ToString(propertyValue), out parseIntVal))
            {
                return parseIntVal <= maxValue;
            }
            return true;
        }
        protected bool CheckMinValue<T>(T propertyValue, int minValue)
        {
            int parseIntVal;
            Type type = typeof(T);

            if (propertyValue == null)
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsNumber(type.GetGenericArguments()[0]))
            {
                return Convert.ToInt32(propertyValue) >= minValue;
            }

            if (IsNumber(type))
            {
                return Convert.ToInt32(propertyValue) >= minValue;
            }
            else if (type == typeof(String) && Int32.TryParse(Convert.ToString(propertyValue), out parseIntVal))
            {
                return parseIntVal >= minValue;
            }
            return true;

        }
        protected bool CheckMaxLength(string propertyValue, int maxLenght)
        {
            return String.IsNullOrEmpty(propertyValue) || propertyValue.Length <= maxLenght;
        }
        protected bool CheckIsMandatoryNull<T>(T propertyValue)
        {
            if (typeof(T).GetInterface("IList") != null)
            {
                return ((IList)propertyValue).Count > 0;
            }
            bool resultNull = !(typeof(T) == typeof(String) && String.IsNullOrEmpty(Convert.ToString(propertyValue)) || propertyValue == null);
            return resultNull;
        }

        private bool IsNumber(Type type)
        {
            return (type.IsPrimitive && type != typeof(Boolean)) || type == typeof(Decimal);

        }
    }
}
