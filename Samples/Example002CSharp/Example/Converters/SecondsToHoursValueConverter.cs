using System;
using System.Globalization;
using Xamarin.Forms;

namespace Example.Converters
{
    public class MinutesToHoursMinutesValueConverter 
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = TimeSpan.FromMinutes((int)value);

            return string.Format("{0} hours {1} minutes", t.Hours, t.Minutes);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
