using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MvvmCross.Converters;

namespace Playground.Core.Converters
{
    public class StringToLowerValueConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToLower();
        }
    }
}
