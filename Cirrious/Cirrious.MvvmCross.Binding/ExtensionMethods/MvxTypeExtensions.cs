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
                    if (value != null)
                    {
                        safeValue = value.ToString();
                    }
                }
                else if (propertyType.IsEnum)
                {
                    if (value == null)
                        safeValue = propertyType.CreateDefault();
                    else if (value is string)
                        safeValue = Enum.Parse(propertyType, (string) value, true);
                    else
                        safeValue = Enum.ToObject(propertyType, value);
                }
                else if (propertyType.IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType);
                    if (underlyingType == null)
                    {
                        if (value == null)
                            safeValue = Activator.CreateInstance(propertyType);
                        else
                            safeValue = Convert.ChangeType(value, propertyType, CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        if (value != null)
                            safeValue = Convert.ChangeType(value, underlyingType, CultureInfo.CurrentUICulture);
                        else
                            safeValue = null;
                    }
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