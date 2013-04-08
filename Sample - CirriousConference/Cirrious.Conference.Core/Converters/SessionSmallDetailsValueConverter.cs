using System;
using Cirrious.Conference.Core.Models.Raw;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Localization;

namespace Cirrious.Conference.Core.Converters
{
    public class SessionSmallDetailsValueConverter
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
            var session = (Session) value;
            var format = TextProvider.GetText(Constants.GeneralNamespace, Constants.Shared, (string)parameter);
            return string.Format(format, session.Type, session.Level, session.Where, session.When);
        }
    }
}