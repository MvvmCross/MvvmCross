using System;
using System.Globalization;
using Cirrious.MvvmCross.Converters;

namespace Tutorial.UI.WindowsMetro.Converters
{
    public class TypeToNameConverter : MvxBaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Type;
            return type.Name;
        }
    }
}
