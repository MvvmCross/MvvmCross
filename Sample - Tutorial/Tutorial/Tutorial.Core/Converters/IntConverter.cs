using Cirrious.CrossCore.Converters;

namespace Tutorial.Core.Converters
{
    public class IntConverter
        : MvxValueConverter
    {
        public override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var strValue = (int)value;
            return strValue.ToString(System.Globalization.CultureInfo.CurrentUICulture);
        }

        public override object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var strValue = (string)value;
            return int.Parse(strValue, System.Globalization.CultureInfo.CurrentUICulture);
        }
    }
}