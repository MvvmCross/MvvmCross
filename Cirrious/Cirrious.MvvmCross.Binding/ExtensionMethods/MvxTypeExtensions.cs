#region Copyright

// <copyright file="MvxTypeExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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