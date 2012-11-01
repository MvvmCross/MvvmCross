using System;
using System.Globalization;
using Cirrious.MvvmCross.Converters;

namespace TwitterSearch.UI.WinRT.Converters
{
    public class HighQualityTwitterValueConverter 
        : MvxBaseValueConverter
    {
        public override object Convert(object value, Type type, object parmeter, CultureInfo cultureInfo)
        {
            return ((string) value).Replace("_normal", string.Empty);
        }
    }
}
