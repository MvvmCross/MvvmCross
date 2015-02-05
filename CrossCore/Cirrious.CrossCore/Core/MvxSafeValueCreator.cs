using System;
using System.Globalization;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;

namespace Cirrious.CrossCore.Core
{
    public class MvxSafeValueCreator : IMvxSafeValueCreator
    {
        // core implementation of ConvertToBoolean
        public bool ConvertToBooleanCore(object result)
        {
            if (result == null)
                return false;

            if (result is string)
                return !string.IsNullOrEmpty((string)result);

            if (result is bool)
                return (bool)result;

            var resultType = result.GetType();
            if (resultType.GetTypeInfo().IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        // core implementation of MakeSafeValue
        public object MakeSafeValueCore(Type propertyType, object value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                if (propertyType == typeof(string))
                {
                    safeValue = value.ToString();
                }
                else if (propertyType.GetTypeInfo().IsEnum)
                {
                    if (value is string)
                        safeValue = Enum.Parse(propertyType, (string)value, true);
                    else
                        safeValue = Enum.ToObject(propertyType, value);
                }
                else if (propertyType.GetTypeInfo().IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    if (underlyingType == typeof(bool))
                        safeValue = ConvertToBooleanCore(value);
                    else
                        safeValue = ErrorMaskedConvert(value, underlyingType, CultureInfo.CurrentUICulture);
                }
                else
                {
                    safeValue = ErrorMaskedConvert(value, propertyType, CultureInfo.CurrentUICulture);
                }
            }
            return safeValue;
        }

        private object ErrorMaskedConvert(object value, Type type, CultureInfo cultureInfo)
        {
            try
            {
                return Convert.ChangeType(value, type, cultureInfo);
            }
            catch (Exception)
            {
                // pokemon - mask the error
                return value;
            }
        }
    }
}
