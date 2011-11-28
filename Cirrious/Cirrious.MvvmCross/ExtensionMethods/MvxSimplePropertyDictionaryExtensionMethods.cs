using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Conventions;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxSimplePropertyDictionaryExtensionMethods
    {
        public static IDictionary<string,string> ToSimplePropertyDictionary(this object input)
        {
            if (input == null)
                return new Dictionary<string, string>();

            var propertyInfos = from property in input.GetType()
                                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty)
                                where property.CanRead
                                select property;

            return propertyInfos.ToDictionary(x => x.Name, x => input.GetPropertyValueAsString(x));
        }

        public static string GetPropertyValueAsString(this object input, PropertyInfo propertyInfo)
        {
            try
            {
                var value = propertyInfo.GetGetMethod().Invoke(input, new object[] { });
                if (value == null)
                    return null;

#warning Really need to be careful here - what if some numpty wants to pass an empty string in?
                return value.ToString();
            }
            catch (MethodAccessException methodAccessException)
            {
                throw methodAccessException.MvxWrap("Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1");
            }
        }
    }
}