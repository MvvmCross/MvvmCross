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
                TValue x = dict[value];
                return x;
            }
            throw new ArgumentException($"Could not cast {parameter.GetType().ToString()} to {nameof(Dictionary<TKey, TValue>)}");
        }
    }
}
