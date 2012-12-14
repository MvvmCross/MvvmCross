using System;
using System.Globalization;
using System.Windows.Data;
using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Wpf.Platform.Converters
{
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

        #endregion
    }
}
