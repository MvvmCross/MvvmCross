// MvxTypeExtensions.cs
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
    public static class MvxTypeExtensions
    {
        public static bool ConvertToBoolean(this object result)
        {
            if (result == null)
                return false;

            if (result is string)
                return string.IsNullOrEmpty((string)result);

            if (result is bool)
                return (bool)result;

            var resultType = result.GetType();
            if (resultType.IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return underlyingType.CreateDefault() != result;
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
                        safeValue = Convert.ChangeType(value, underlyingType, CultureInfo.CurrentUICulture);
                }
                else
                {
                    try
                    {
                        safeValue = Convert.ChangeType(value, propertyType, CultureInfo.CurrentUICulture);
                    }
                    catch (Exception)
                    {
                        // pokemon - mask the error
                    }
                }
            }
            return safeValue;
        }
    }
}