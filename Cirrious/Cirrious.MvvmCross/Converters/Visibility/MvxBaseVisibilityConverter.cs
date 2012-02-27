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
            return (visibility == MvxVisibility.Visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
#endif
#if MONOTOUCH
            return visibility;
#endif
#if MonoDroid
            return (visibility == MvxVisibility.Visible) ? global::Android.Views.ViewStates.Visible : global::Android.Views.ViewStates.Gone;
#endif
        }

        public abstract MvxVisibility ConvertToMvxVisibility(object value, object parameter, CultureInfo culture);

        public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mvx = ConvertToMvxVisibility(value, parameter, culture);
            return NativeVisibility(mvx);
        }

        public sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}