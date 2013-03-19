using System;
using Cirrious.CrossCore.Converters;

namespace Tutorial.Core.Converters
{
    public class IntToFloatConverter
        : MvxValueConverter
    {
        public override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var intValue = (int)value;
            return (float)intValue;
        }

        public override object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var floatValue = (float)value;
            return (int)Math.Round(floatValue);
        }
    }
}