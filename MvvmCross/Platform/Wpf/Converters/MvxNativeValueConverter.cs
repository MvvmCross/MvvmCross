// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platform.Wpf.Converters
{
    /*
     * obsolete - use CrossCore version
    public class MvxNativeValueConverter<T>
        : IValueConverter
        where T : IMvxValueConverter, new()
    {
        private readonly T _wrapped = new T();

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _wrapped.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _wrapped.ConvertBack(value, targetType, parameter, culture);
        }

        #endregion Implementation of IValueConverter
    }
    */
}
