using System;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.Conference.Core.Converters
{
    public class TimeAgoValueConverter
        : MvxValueConverter
          
    {
        private IMvxTextProvider _textProvider;
        private IMvxTextProvider TextProvider
        {
            get
            {
                if (_textProvider == null)
                {
                    _textProvider = Mvx.Resolve<IMvxTextProvider>();
                }
                return _textProvider;
            }
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var when = (DateTime)value;

            string whichFormat;
            int valueToFormat;
			
			if (when == DateTime.MinValue)
			{
				whichFormat = "TimeAgo.Never";
                valueToFormat = 0;
			}
			else
			{
			    var whenUtc = when.ToUniversalTime();
                var difference = (DateTime.UtcNow - whenUtc).TotalSeconds;
	            if (difference < 30.0)
	            {
	                whichFormat = "TimeAgo.JustNow";
	                valueToFormat = 0;
	            }
	            else if (difference < 100.0)
	            {
	                whichFormat = "TimeAgo.SecondsAgo";
	                valueToFormat = (int)difference;
	            }
	            else if (difference < 3600.0)
	            {
	                whichFormat = "TimeAgo.MinutesAgo";
	                valueToFormat = (int) (difference/60);
	            }
	            else if (difference < 24 * 3600)
	            {
	                whichFormat = "TimeAgo.HoursAgo";
	                valueToFormat = (int) (difference/(3600));
	            }
	            else
	            {
	                whichFormat = "TimeAgo.DaysAgo";
	                valueToFormat = (int) (difference/(3600*24));
	            }
			}
            var format = TextProvider.GetText(Constants.GeneralNamespace, Constants.Shared, whichFormat);
            return string.Format(format, valueToFormat);
        }
    }
}