// MvxNativeValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Wpf.Converters
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