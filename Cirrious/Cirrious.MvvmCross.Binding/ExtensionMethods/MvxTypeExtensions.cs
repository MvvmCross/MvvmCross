// MvxTypeExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;

namespace Cirrious.MvvmCross.Binding.ExtensionMethods
{
    public static class MvxTypeExtensions
    {
        public static object MakeSafeValue(this Type propertyType, object value)
        {
            if (value != null)
            {
                var autoConverter = MvxBindingSingletonCache.Instance.AutoValueConverters.Find(value.GetType(),
                                                                                               propertyType);
                if (autoConverter != null)
                {
                    return autoConverter.Convert(value, propertyType, null, CultureInfo.CurrentUICulture);
                }
            }

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