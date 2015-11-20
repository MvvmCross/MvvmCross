// MvxSimplePropertyDictionaryExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.Platform
{
    public static class MvxSimplePropertyDictionaryExtensionMethods
    {
        public static IDictionary<string, string> ToSimpleStringPropertyDictionary(
            this IDictionary<string, object> input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            return input.ToDictionary(x => x.Key, x => x.Value?.ToString());
        }

        public static IDictionary<string, string> SafeGetData(this IMvxBundle bundle)
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
            var propertyList =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite);

            foreach (var propertyInfo in propertyList)
            {
                string textValue;
                if (!data.TryGetValue(propertyInfo.Name, out textValue))
                    continue;

                var typedValue = MvxSingletonCache.Instance.Parser.ReadValue(textValue, propertyInfo.PropertyType,
                                                                             propertyInfo.Name);
                propertyInfo.SetValue(t, typedValue, new object[0]);
            }

            return t;
        }

        public static IEnumerable<object> CreateArgumentList(this IDictionary<string, string> data,
                                                             IEnumerable<ParameterInfo> requiredParameters,
                                                             string debugText)
        {
            var argumentList = new List<object>();
            foreach (var requiredParameter in requiredParameters)
            {
                var argumentValue = data.GetArgumentValue(requiredParameter, debugText);
                argumentList.Add(argumentValue);
            }
            return argumentList;
        }

        public static object GetArgumentValue(this IDictionary<string, string> data, ParameterInfo requiredParameter, string debugText)
        {
            string parameterValue;
            if (data == null ||
                !data.TryGetValue(requiredParameter.Name, out parameterValue))
            {
                if (requiredParameter.IsOptional)
                {
                    return Type.Missing;
                }

                MvxTrace.Trace(
                    "Missing parameter for call to {0} - missing parameter {1} - asssuming null - this may fail for value types!",
                    debugText,
                    requiredParameter.Name);
                parameterValue = null;
            }

            var value = MvxSingletonCache.Instance.Parser.ReadValue(parameterValue, requiredParameter.ParameterType,
                                                                    requiredParameter.Name);
            return value;
        }

        public static IDictionary<string, string> ToSimplePropertyDictionary(this object input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            if (input is IDictionary<string, string>)
                return (IDictionary<string, string>)input;

            var propertyInfos = from property in input.GetType()
                                                      .GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                                     BindingFlags.FlattenHierarchy)
                                where property.CanRead
                                select new
                                {
                                    CanSerialize =
                                    MvxSingletonCache.Instance.Parser.TypeSupported(property.PropertyType),
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
                    Mvx.Trace(
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
                var value = propertyInfo.GetValue(input, new object[] { });

                return value?.ToString();
            }
            catch (Exception suspectedMethodAccessException)
            {
                throw suspectedMethodAccessException.MvxWrap(
                    "Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1");
            }
        }
    }
}