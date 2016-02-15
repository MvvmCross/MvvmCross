// MvxBooleanInverterValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.ValueConverters
{
    public class MvxBooleanInverterValueConverter : MvxValueConverter<bool, bool>
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture) => !value;

        protected override bool ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture) => !value;
    }
}
