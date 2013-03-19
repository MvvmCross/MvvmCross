using System;
using Cirrious.CrossCore.Converters;

namespace SimpleDroid.Converters
{
    public class StringLengthValueConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
                return 0;
            return stringValue.Length;
        }
    }
}