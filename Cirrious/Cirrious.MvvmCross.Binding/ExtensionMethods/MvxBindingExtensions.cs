// MvxBindingExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.Binding.ExtensionMethods
{
    public static class MvxBindingExtensions
    {
        public static bool ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(this IMvxEditableTextView mvxEditableTextView, object target, object value)
        {
            if (value == null)
                return false;

            // specifically for double, float and decimal we do some special comparisons
            // to prevent the user losing trailing periods, leading minus signs and trailing zeros
            var valueType = value.GetType();
            if (valueType == typeof(double) ||
                valueType == typeof(float) ||
                valueType == typeof(decimal))
            {
                var currentValue = mvxEditableTextView.CurrentText;
                if (currentValue == null)
                    return false;

                try
                {
                    var equivalentCurrentValue = valueType.MakeSafeValue(currentValue);
                    if (equivalentCurrentValue.Equals(value))
                        return true;
                }
                catch (FormatException)
                {
                    // format problem - so they are definitely not equivalent
                    return false;
                }
            }

            return false;
        }

        public static bool ConvertToBoolean(this object result)
        {
            if (result == null)
                return false;

            if (result is string)
                return !string.IsNullOrEmpty((string)result);

            if (result is bool)
                return (bool)result;

            var resultType = result.GetType();
            if (resultType.IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        public static object MakeSafeValue(this Type propertyType, object value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

			var autoConverter = MvxBindingSingletonCache.Instance.AutoValueConverters.Find (value.GetType(),
				                                                                            propertyType);
			if (autoConverter != null) 
            {
				return autoConverter.Convert (value, propertyType, null, CultureInfo.CurrentUICulture);
			}

            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                if (propertyType == typeof (string))
                {
                    safeValue = value.ToString();
                }
                else if (propertyType.IsEnum)
                {
                    if (value is string)
                        safeValue = Enum.Parse(propertyType, (string) value, true);
                    else
                        safeValue = Enum.ToObject(propertyType, value);
                }
                else if (propertyType.IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    if (underlyingType == typeof(bool))
                        safeValue = value.ConvertToBoolean();
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

        private static object ErrorMaskedConvert(object value, Type type, CultureInfo cultureInfo)
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