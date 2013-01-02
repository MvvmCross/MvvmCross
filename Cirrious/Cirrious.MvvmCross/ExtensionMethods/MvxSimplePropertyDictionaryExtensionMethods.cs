// MvxSimplePropertyDictionaryExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxSimplePropertyDictionaryExtensionMethods
    {
        public static IDictionary<string, string> ToSimpleStringPropertyDictionary(
            this IDictionary<string, object> input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            return input.ToDictionary(x => x.Key, x => x.Value == null ? null : x.Value.ToString());
        }

        public static IDictionary<string, string> ToSimplePropertyDictionary(this object input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            var propertyInfos = from property in input.GetType()
                                                      .GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                                     BindingFlags.FlattenHierarchy)
                                where property.CanRead
                                select property;

            return propertyInfos.ToDictionary(x => x.Name, x => input.GetPropertyValueAsString(x));
        }

        public static string GetPropertyValueAsString(this object input, PropertyInfo propertyInfo)
        {
            try
            {
                var value = propertyInfo.GetValue(input, new object[] {});
                if (value == null)
                    return null;

                return value.ToString();
            }
            catch (Exception suspectedMethodAccessException)
            {
                throw suspectedMethodAccessException.MvxWrap(
                    "Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1");
            }
        }
    }
}