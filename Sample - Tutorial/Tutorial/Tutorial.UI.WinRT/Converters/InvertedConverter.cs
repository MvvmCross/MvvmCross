using System;
using System.Globalization;
using Cirrious.MvvmCross.Converters;

namespace Tutorial.UI.WinRT.Converters
{
    public class InvertedConverter : MvxBaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}