using System.Globalization;

namespace Cirrious.MvvmCross.Plugins.Color
{
    public class MvxSimpleColorConverter : MvxBaseColorConverter
    {
        protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
        {
            return (MvxColor)value;
        }
    }
}