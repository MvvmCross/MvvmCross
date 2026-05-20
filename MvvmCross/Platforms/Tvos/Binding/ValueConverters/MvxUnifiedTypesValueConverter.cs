// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MvvmCross.Converters;
using ObjCRuntime;

namespace MvvmCross.Platforms.Tvos.Binding.ValueConverters
{
    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming.")]
    internal class MvxUnifiedTypesValueConverter
        : MvxValueConverter
    {
        //dictionary of supported unified type conversions
        internal static readonly IReadOnlyDictionary<Type, Type> UnifiedTypeConversions;

        static MvxUnifiedTypesValueConverter()
        {
            var initDictionary = new Dictionary<Type, Type>()
                {
                    { typeof(float), typeof(nfloat) },
                    { typeof(int), typeof(nint) },
                    { typeof(uint), typeof(nuint) }
                };

            UnifiedTypeConversions = new ReadOnlyDictionary<Type, Type>(initDictionary);
        }

        public override object Convert(
            object value,
            Type? targetType,
            object? parameter,
            CultureInfo? culture)
        {
            //return original value if converter not used to convert to unified value type from proper managed value type
            var valueType = value.GetType();
            if (!UnifiedTypeConversions.TryGetValue(valueType, out var nativeType))
                return value;

            var nativeValue = Activator.CreateInstance(nativeType, value);
            return nativeValue ?? MvxBindingConstant.UnsetValue;
        }

        public override object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            //unified types already implement proper conversion with IConvertible interface support
            return targetType != null
                ? System.Convert.ChangeType(value, targetType, culture)
                : MvxBindingConstant.UnsetValue;
        }
    }
}
