using System;

namespace Cirrious.MvvmCross.Converters.Color
{
    public abstract class MvxBaseColorConverter : MvxBaseValueConverter
    {
        protected abstract MvxColor Convert(object value, object parameter, System.Globalization.CultureInfo culture);

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, parameter, culture).ToNative();
        }
    }
}