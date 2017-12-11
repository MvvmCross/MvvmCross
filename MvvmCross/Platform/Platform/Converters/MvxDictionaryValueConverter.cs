using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvvmCross.Platform.Converters
{
    public class MvxDictionaryValueConverter<TKey, TValue> : MvxValueConverter<TKey, TValue>
    {
        protected override TValue Convert(TKey value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Dictionary<TKey, TValue> dict)
            {
                if (dict.ContainsKey(value))
                {
                    return dict[value];
                }
                else
                {
                    throw new KeyNotFoundException($"Could not find key {value.ToString()} for {typeof(MvxDictionaryValueConverter<TKey, TValue>).Name}.");
                }
            }
            throw new ArgumentException($"Could not cast {parameter.GetType().Name} to {typeof(Dictionary<TKey, TValue>).Name}");
        }
    }
}
