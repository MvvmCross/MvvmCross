#region Copyright
// <copyright file="MvxBaseVisibilityConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Globalization;

#if MonoDroid
using Android.Views;
#endif

namespace Cirrious.MvvmCross.Converters.Visibility
{
    public abstract class MvxBaseVisibilityConverter 
        : MvxBaseValueConverter
    {
        private object NativeVisibility(MvxVisibility visibility)
        {
#if NETFX_CORE
            return (visibility == MvxVisibility.Visible) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
#endif
#if WINDOWS_PHONE 
            return (visibility == MvxVisibility.Visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
#endif
#if MONOTOUCH
			return visibility == MvxVisibility.Visible ? true : false;
#endif
#if MonoDroid
            return (visibility == MvxVisibility.Visible) ? ViewStates.Visible : ViewStates.Gone;
#endif
#if WINDOWS_CONSOLE
            return visibility == MvxVisibility.Visible ? true : false;
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