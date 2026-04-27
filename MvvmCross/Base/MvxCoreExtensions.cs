// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

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

        /// <summary>
        /// Returns the default value for <paramref name="type"/>:
        /// <c>null</c> for reference types and nullable value types,
        /// or a new zero-initialised instance for non-nullable value types.
        /// </summary>
        public static object? CreateDefault(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] this Type? type)
        {
            if (type == null)
                return null;

            if (!type.GetTypeInfo().IsValueType)
                return null;

            if (Nullable.GetUnderlyingType(type) != null)
                return null;

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Enumerates the types defined in <paramref name="assembly"/>, silently swallowing
        /// <see cref="ReflectionTypeLoadException"/> and logging partial failures at Warning level.
        /// </summary>
        [RequiresUnreferencedCode("Calls Assembly.GetTypes() which may not preserve all types during trimming. " +
            "Use explicit type registration instead of assembly scanning for trim-compatible code.")]
        public static IEnumerable<Type> ExceptionSafeGetTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                var logger = MvvmCross.Logging.MvxLogHost.Default;
                logger?.LogWarning(e,
                    "ReflectionTypeLoadException masked during loading of {AssemblyName}", assembly.FullName);

                foreach (var loaderException in e.LoaderExceptions ?? Array.Empty<Exception?>())
                {
                    if (loaderException != null)
                        logger?.LogWarning(loaderException, "Failed to load type");
                }

                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();

                return Enumerable.Empty<Type>();
            }
        }
    }
#nullable restore
}
