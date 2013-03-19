using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;

namespace Tutorial.UI.WinRT.Converters
{
    public class TypeToNameConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Type;
            return type.Name;
        }
    }
}
