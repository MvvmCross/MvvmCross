using System;
using Cirrious.Conference.Core.Models.Raw;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Localization.Interfaces;

namespace Cirrious.Conference.Core.Converters
{
    public class SessionSmallDetailsValueConverter
        : MvxBaseValueConverter
          , IMvxConsumer
    {
        private IMvxTextProvider _textProvider;
        private IMvxTextProvider TextProvider
        {
            get
            {
                if (_textProvider == null)
                {
                    _textProvider = this.GetService<IMvxTextProvider>();
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