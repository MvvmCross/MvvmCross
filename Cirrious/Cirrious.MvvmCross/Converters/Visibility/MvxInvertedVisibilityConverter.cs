using System.Globalization;

namespace Cirrious.MvvmCross.Converters.Visibility
{
    public class MvxInvertedVisibilityConverter : MvxBaseVisibilityConverter
    {
        public override MvxVisibility ConvertToMvxVisibility(object value, object parameter, CultureInfo culture)
        {
            var visibility = (bool) value;
            return visibility ? MvxVisibility.Collapsed : MvxVisibility.Visible;
        }
    }
}