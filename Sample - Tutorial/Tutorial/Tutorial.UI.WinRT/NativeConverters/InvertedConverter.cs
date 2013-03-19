using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;

namespace Tutorial.UI.WinRT.Converters
{
    public class InvertedConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}