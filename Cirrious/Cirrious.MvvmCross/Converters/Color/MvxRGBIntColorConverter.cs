using System.Globalization;

namespace Cirrious.MvvmCross.Converters.Color
{
    public class MvxRGBIntColorConverter : MvxBaseColorConverter
    {
        protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
        {
            return new MvxColor((int)value);
        }
    }
}