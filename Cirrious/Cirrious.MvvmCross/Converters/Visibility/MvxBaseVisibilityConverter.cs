using System;
using System.Globalization;

namespace Cirrious.MvvmCross.Converters.Visibility
{
    public abstract class MvxBaseVisibilityConverter 
        : MvxBaseValueConverter
    {
        private object NativeVisibility(MvxVisibility visibility)
        {
#if WINDOWS_PHONE
            return (System.Windows.Visibility) (byte) visibility;
#endif
#if MONOTOUCH
            return visibility;
#endif
#if MonoDroid
            return (visibility == MvxVisibility.Visible) ? global::Android.Views.ViewStates.Visible : global::Android.Views.ViewStates.Gone;
#endif
        }

        public abstract MvxVisibility Convert(object value, object parameter, CultureInfo culture);

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NativeVisibility(Convert(value, parameter, culture));
        }
    }
}