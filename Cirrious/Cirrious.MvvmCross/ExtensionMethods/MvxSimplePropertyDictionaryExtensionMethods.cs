#region Copyright
// <copyright file="MvxSimplePropertyDictionaryExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxSimplePropertyDictionaryExtensionMethods
    {
        public static IDictionary<string, string> ToSimplePropertyDictionary(this object input)
        {
            if (input == null)
                return new Dictionary<string, string>();
			
            var propertyInfos = from property in input.GetType()
#if NETFX_CORE
                                .GetTypeInfo().DeclaredProperties
#else

                                    .GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                                   BindingFlags.FlattenHierarchy | BindingFlags.GetProperty)
#endif
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
#if NETFX_CORE
            finally
            {
                
            }
#else
            catch (MethodAccessException methodAccessException)
            {
                throw methodAccessException.MvxWrap(
                    "Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1");
            }
#endif
        }
    }
}