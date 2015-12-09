// MvxBindingExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.ExtensionMethods;
using Cirrious.CrossCore.IoC;
using System;
using System.Globalization;

namespace Cirrious.MvvmCross.Binding.ExtensionMethods
{
    public static class MvxBindingExtensions
    {
        public static bool ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(this IMvxEditableTextView mvxEditableTextView, object target, object value)
        {
            if (value == null)
                return false;

            // specifically for int, double, float and decimal we do some special comparisons
            // to prevent the user losing trailing periods, leading minus signs, leading zeroes and trailing zeros
            var valueType = value.GetType();
            if (valueType == typeof(int) ||
                valueType == typeof(double) ||
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
            return result.ConvertToBooleanCore();
        }

        public static object MakeSafeValue(this Type propertyType, object value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            var autoConverter = MvxBindingSingletonCache.Instance.AutoValueConverters.Find(value.GetType(),
                                                                                            propertyType);
            if (autoConverter != null)
            {
                return autoConverter.Convert(value, propertyType, null, CultureInfo.CurrentUICulture);
            }

            return propertyType.MakeSafeValueCore(value);
        }
    }
}