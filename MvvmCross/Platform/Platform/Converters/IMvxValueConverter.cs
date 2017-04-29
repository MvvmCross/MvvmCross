// IMvxValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;

namespace MvvmCross.Platform.Converters
{
    public interface IMvxValueConverter
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}