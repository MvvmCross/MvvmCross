// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace MvvmCross.Core
{
#nullable enable
    public static class MvxSimplePropertyDictionaryExtensions
    {
        public static IDictionary<string, string> ToSimpleStringPropertyDictionary(
            this IDictionary<string, object> input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            return input.ToDictionary(x => x.Key, x => x.Value?.ToStringInvariant() ?? string.Empty);
        }

        public static IDictionary<string, string>? SafeGetData(this IMvxBundle? bundle)
        {
            return bundle?.Data;
        }

        public static void Write(this IDictionary<string, string> data, object toStore)
        {
            if (toStore == null)
                return;

            var propertyDictionary = toStore.ToSimplePropertyDictionary();
            foreach (var kvp in propertyDictionary)
            {
                data[kvp.Key] = kvp.Value;
            }
        }

        public static T Read<T>(this IDictionary<string, string> data)
            where T : new()
        {
            return (T)data.Read(typeof(T));
        }

        public static object Read(this IDictionary<string, string> data, Type type)
        {
            var t = Activator.CreateInstance(type);

            foreach (var propertyInfo in type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite))
            {
                if (!data.TryGetValue(propertyInfo.Name, out var textValue))
                    continue;

                var typedValue = MvxSingletonCache.Instance?.Parser.ReadValue(
                    textValue, propertyInfo.PropertyType, propertyInfo.Name);
                if (typedValue != null)
                    propertyInfo.SetValue(t, typedValue, Array.Empty<object>());
            }

            return t;
        }

        public static IEnumerable<object> CreateArgumentList(
            this IDictionary<string, string> data, IEnumerable<ParameterInfo> requiredParameters, string? debugText)
        {
            var argumentList = new List<object>();
            foreach (var requiredParameter in requiredParameters)
            {
                var argumentValue = data.GetArgumentValue(requiredParameter, debugText);
                if (argumentValue != null)
                    argumentList.Add(argumentValue);
            }
            return argumentList;
        }

        public static object? GetArgumentValue(this IDictionary<string, string> data, ParameterInfo requiredParameter, string? debugText)
        {
            string? parameterValue;
            if (data == null ||
                !data.TryGetValue(requiredParameter.Name, out parameterValue))
            {
                if (requiredParameter.IsOptional)
                {
                    return Type.Missing;
                }

                MvxLog.Instance?.Trace(
                    $"Missing parameter for call to {debugText} - missing parameter {requiredParameter.Name} - asssuming null - this may fail for value types!");

                parameterValue = string.Empty;
            }

            return MvxSingletonCache.Instance?.Parser.ReadValue(
                parameterValue, requiredParameter.ParameterType, requiredParameter.Name);
        }

        public static IDictionary<string, string> ToSimplePropertyDictionary(this object input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            if (input is IDictionary<string, string> inputDict)
                return inputDict;

            var propertyInfos =
                from property in input.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                where property.CanRead
                select new
                {
                    CanSerialize =
                    MvxSingletonCache.Instance?.Parser.TypeSupported(property.PropertyType) ?? false,
                    Property = property
                };

            var dictionary = new Dictionary<string, string>();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.CanSerialize)
                {
                    dictionary[propertyInfo.Property.Name] = input.GetPropertyValueAsString(propertyInfo.Property);
                }
                else
                {
                    MvxLog.Instance?.Trace(
                        "Skipping serialization of property {0} - don't know how to serialize type {1} - some answers on http://stackoverflow.com/questions/16524236/custom-types-in-navigation-parameters-in-v3",
                        propertyInfo.Property.Name,
                        propertyInfo.Property.PropertyType.Name);
                }
            }
            return dictionary;
        }

        public static string GetPropertyValueAsString(this object input, PropertyInfo propertyInfo)
        {
            try
            {
                var value = propertyInfo.GetValue(input, Array.Empty<object>());
                return value?.ToStringInvariant() ?? string.Empty;
            }
            catch (Exception suspectedMethodAccessException)
            {
                throw suspectedMethodAccessException.MvxWrap(
                    "Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1");
            }
        }

        private static string ToStringInvariant(this object value)
        {
            switch (value)
            {
                case DateTime dateTime: return dateTime.ToString("o", CultureInfo.InvariantCulture);
                case double doubleValue: return doubleValue.ToString("r", CultureInfo.InvariantCulture);
                case float floatValue: return floatValue.ToString("r", CultureInfo.InvariantCulture);
                case IFormattable formattableValue: return formattableValue.ToString(null, CultureInfo.InvariantCulture);
                default: return value.ToString();
            }
        }
    }
#nullable restore
}
