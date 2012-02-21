using System;
using System.Globalization;
using Cirrious.MvvmCross.Interfaces.Converters;

namespace Cirrious.MvvmCross.Converters
{
    public abstract class MvxBaseValueConverter
        : IMvxValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}