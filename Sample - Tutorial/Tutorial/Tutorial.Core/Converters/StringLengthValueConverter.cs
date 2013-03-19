using System;
using System.Collections.Generic;
using System.Text;
using Cirrious.CrossCore.Converters;

namespace Tutorial.Core.Converters
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