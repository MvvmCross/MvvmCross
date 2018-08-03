using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MvvmCross.Forms.Converters;

namespace Playground.Forms.UI.Converters
{
    public class StringToUpperValueConverter : MvxFormsValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToUpper();
        }
    }
}
