// MvxBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.ValueConverters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.IoC;

    internal class MvxUnifiedTypesValueConverter
        : MvxValueConverter
    {
        //dictionary of supported unified type conversions
        internal static readonly IReadOnlyDictionary<Type, Type> UnifiedTypeConversions;

        static MvxUnifiedTypesValueConverter()
        {
            var initDictionary = new Dictionary<Type, Type>()
                {
                    {typeof (float), typeof (nfloat)},
                    {typeof (int), typeof (nint)},
                    {typeof (uint), typeof (nuint)}
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