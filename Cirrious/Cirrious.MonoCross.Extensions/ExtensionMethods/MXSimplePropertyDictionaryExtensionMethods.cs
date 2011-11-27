using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MonoCross.Extensions.Conventions;

namespace Cirrious.MonoCross.Extensions.ExtensionMethods
{
    public static class MXSimplePropertyDictionaryExtensionMethods
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
            var value = propertyInfo.GetGetMethod().Invoke(input, new object[] {});
            if (value == null)
                return MXConventionConstants.NullParameterValue ;

#warning Really need to be careful here - what if some numpty wants to pass an empty string in?
            return value.ToString();
        }
    }
}