using System;
using System.Linq;
using Cirrious.CrossCore.Converters;

namespace SimpleDroid.Converters
{
    public class StringReverseValueConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
                return string.Empty;
            return new string(stringValue.Reverse().ToArray());
        }
    }
}