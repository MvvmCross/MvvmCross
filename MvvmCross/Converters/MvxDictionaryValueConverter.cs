// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvvmCross.Converters
{
#nullable enable
    public class MvxDictionaryValueConverter<TKey, TValue> : MvxValueConverter<TKey, TValue>
        where TKey : notnull
    {
        protected override TValue Convert(TKey value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter), $"Dictionary Converter expected a parameter of type \"{typeof(Tuple<IDictionary<TKey, TValue>, TValue, bool>)}\" but received null");

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
#nullable restore
}
