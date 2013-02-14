using System;
using Cirrious.MvvmCross.Converters;
using System.Globalization;

namespace Cirrious.MvvmCross.Binding.Droid.Converters
{
    public class MvxStringValueConverter : MvxBaseValueConverter
    {
        private static MvxStringValueConverter _inst;

        public static MvxStringValueConverter Instance {
            get {
                if (_inst == null) {
                    _inst = new MvxStringValueConverter();
                }
                return _inst;
            }
        }

        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value ?? string.Empty).ToString ();
        }
    }
}

