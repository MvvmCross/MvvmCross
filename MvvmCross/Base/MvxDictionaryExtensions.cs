#nullable enable
using System.Reflection;

namespace MvvmCross.Base;

public static class MvxDictionaryExtensions
{
    public static IDictionary<string, object> ToPropertyDictionary(this object? input)
    {
        if (input == null)
            return new Dictionary<string, object>();

        if (input is IDictionary<string, object> dict)
            return dict;

        var propertyInfos =
            input.GetType().GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(p => p.CanRead);

        var dictionary = new Dictionary<string, object>();
        foreach (var propertyInfo in propertyInfos)
        {
            var value = propertyInfo.GetValue(input);
            if (value != null)
                dictionary[propertyInfo.Name] = value;
        }
        return dictionary;
    }
}
