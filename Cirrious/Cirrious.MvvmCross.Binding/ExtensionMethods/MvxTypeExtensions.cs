using System;
using System.Globalization;

namespace Cirrious.MvvmCross.Binding.ExtensionMethods
{
    public static class MvxTypeExtensions
    {
        public static object MakeSafeValue(this Type propertyType, object value)
        {
            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                if (propertyType.IsValueType && propertyType.IsGenericType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType);
                    safeValue = Convert.ChangeType(value, underlyingType, CultureInfo.CurrentUICulture);
                }
                else if (propertyType == typeof (string))
                {
                    if (value != null)
                    {
                        safeValue = value.ToString();
                    }
                }
                else
                {
                    safeValue = Convert.ChangeType(value, propertyType, CultureInfo.CurrentUICulture);
                }
            }
            return safeValue;
        }
    }
}