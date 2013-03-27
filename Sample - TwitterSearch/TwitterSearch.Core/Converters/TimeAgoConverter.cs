using System;
using Cirrious.CrossCore.Converters;

namespace TwitterSearch.Core.Converters
{
    public class TimeAgoConverter
        : MvxValueConverter<DateTime>     
    {
        protected override object Convert(DateTime value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var difference = (DateTime.UtcNow - value).TotalSeconds;

            string whichFormat;
            int valueToFormat;
            if (difference < 30.0)
            {
                whichFormat = "Just now";
                valueToFormat = 0;
            }
            else if (difference < 100.0)
            {
                whichFormat = "{0}s ago";
                valueToFormat = (int)difference;
            }
            else if (difference < 3600.0)
            {
                whichFormat = "{0}m ago";
                valueToFormat = (int) (difference/60);
            }
            else if (difference < 24 * 3600)
            {
                whichFormat = "{0}h ago";
                valueToFormat = (int) (difference/(3600));
            }
            else
            {
                whichFormat = "{0}d ago";
                valueToFormat = (int) (difference/(3600*24));
            }

            return string.Format(whichFormat, valueToFormat);
        }
    }
}