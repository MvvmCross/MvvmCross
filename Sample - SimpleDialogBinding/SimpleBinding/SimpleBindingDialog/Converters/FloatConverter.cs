using Cirrious.MvvmCross.Converters;

namespace SimpleBindingDialog.Converters
{
    public class FloatConverter
        : MvxBaseValueConverter
    {
        public override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var strValue = (float)value;
            return strValue.ToString(System.Globalization.CultureInfo.CurrentUICulture);
        }

        public override object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var strValue = (string)value;
            return float.Parse(strValue, System.Globalization.CultureInfo.CurrentUICulture);
        }
    }
}