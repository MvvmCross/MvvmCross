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
                var typedParameters = (Tuple<IDictionary<TKey, TValue>, TValue>)parameter;

                if (typedParameters.Item1.ContainsKey(value))
                {
                    return typedParameters.Item1[value];
                }

                return typedParameters.Item2;
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException($"Dictionary Converter expected a parameter of type \"{typeof(Tuple<IDictionary<TKey, TValue>, TValue>)}\" but received type \"{parameter.GetType()}\"", nameof(parameter), ex);
            }
        }
    }
}
