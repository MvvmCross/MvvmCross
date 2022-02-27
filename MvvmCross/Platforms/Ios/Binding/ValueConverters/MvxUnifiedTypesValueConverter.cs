// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Ios.Binding.ValueConverters
{
    internal class MvxUnifiedTypesValueConverter
        : MvxValueConverter
    {
        //dictionary of supported unified type conversions
        internal static readonly IReadOnlyDictionary<Type, Type> UnifiedTypeConversions;

        static MvxUnifiedTypesValueConverter()
        {
            var initDictionary = new Dictionary<Type, Type>
                {
                    { typeof(float), typeof(nfloat) },
                    { typeof(int), typeof(nint) },
                    { typeof(uint), typeof(nuint) }
                };

            UnifiedTypeConversions = new ReadOnlyDictionary<Type, Type>(initDictionary);
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //actually value cannot be null if converter is being used by auto converter registry and was
            //registered with proper source/target types for unified (value types which are non nullable)
            //but keep it for sanity in case converter is used outside of auto converters scope
            if (value == null)
                return targetType.CreateDefault();

            //return original value if converter not used to convert to unified value type from proper managed value type
            var valueType = value.GetType();
            if (!UnifiedTypeConversions.ContainsKey(valueType))
                return value;

            var nativeType = UnifiedTypeConversions[valueType];
            var nativeValue = Activator.CreateInstance(nativeType, value);

            return nativeValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //unified types already implement proper conversion with IConvertible interface support
            return System.Convert.ChangeType(value, targetType, culture);
        }
    }
}
