using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvvmCross.Platform.Converters
{
    public class MvxDictionaryValueConverter<TKey, TValue> : MvxValueConverter<TKey, TValue>
    {
        protected override TValue Convert(TKey value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var typedParameters = (Tuple<IDictionary<TKey, TValue>, TValue, bool>)parameter;

                if (typedParameters.Item1.ContainsKey(value))
                {
                    return typedParameters.Item1[value];
                }
                else if (typedParameters.Item3)
                {
                    return typedParameters.Item2;
                }

                throw new KeyNotFoundException($"Could not find key {value?.ToString()} for {typeof(MvxDictionaryValueConverter<TKey, TValue>)}.");
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException($"Dictionary Converter expected a parameter of type \"{typeof(Tuple<IDictionary<TKey, TValue>, TValue, bool>)}\" but received type \"{parameter.GetType()}\"", nameof(parameter), ex);
            }
        }
    }
}
