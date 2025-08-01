// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using MvvmCross.IoC;

namespace MvvmCross.Base
{
#nullable enable
    public static class MvxCoreExtensions
    {
        // core implementation of ConvertToBoolean
        [UnconditionalSuppressMessage("Trimming", "IL2072:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' requirements",
            Justification = "The types returned by Nullable.GetUnderlyingType on a type with DynamicallyAccessedMemberTypes.PublicParameterlessConstructor are safe to process")]
        public static bool ConvertToBooleanCore<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(this T? result)
        {
            if (EqualityComparer<T?>.Default.Equals(result, default))
                return false;

            var s = result as string;
            if (s != null)
                return !string.IsNullOrEmpty(s);

            if (result is bool x)
                return x;

            var resultType = result!.GetType();
            if (resultType.GetTypeInfo().IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        // core implementation of MakeSafeValue
        public static object? MakeSafeValueCore(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] this Type propertyType,
            object? value)
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
                    var s = value as string;
                    safeValue =
                        s != null ?
                            Enum.Parse(propertyType, s, true) :
                            Enum.ToObject(propertyType, value);
                }
                else if (propertyType.GetTypeInfo().IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    safeValue =
                        underlyingType == typeof(bool) ?
                            value.ConvertToBooleanCore() :
                            ErrorMaskedConvert(value, underlyingType, CultureInfo.CurrentUICulture);
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
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // pokemon - mask the error
                return value;
            }
        }
    }
#nullable restore
}
