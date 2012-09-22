using System;
using System.Globalization;
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.WinRT.Platform.Converters;

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

    public class NativeHighQualityTwitterValueConverter : MvxNativeValueConverter<HighQualityTwitterValueConverter>
    {}

}
