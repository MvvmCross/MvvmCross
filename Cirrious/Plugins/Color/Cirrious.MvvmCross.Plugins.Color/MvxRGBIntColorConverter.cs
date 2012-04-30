using System.Globalization;

namespace Cirrious.MvvmCross.Plugins.Color
{
    public class MvxRGBIntColorConverter : MvxBaseColorConverter
    {
        protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
        {
            return new MvxColor((int)value);
        }
    }
}