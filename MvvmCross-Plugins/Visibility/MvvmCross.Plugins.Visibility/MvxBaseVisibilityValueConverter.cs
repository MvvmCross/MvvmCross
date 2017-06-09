// MvxBaseVisibilityValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Visibility
{
    public abstract class MvxBaseVisibilityValueConverter<T>
        : MvxBaseVisibilityValueConverter
    {
        protected sealed override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            return Convert((T)value, parameter, culture);
        }

        protected abstract MvxVisibility Convert(T value, object parameter, CultureInfo culture);
    }

    public abstract class MvxBaseVisibilityValueConverter
        : MvxValueConverter
    {
        private IMvxNativeVisibility _nativeVisiblity;

        private IMvxNativeVisibility NativeVisibility => _nativeVisiblity ?? (_nativeVisiblity = Mvx.Resolve<IMvxNativeVisibility>());

        protected abstract MvxVisibility Convert(object value, object parameter, CultureInfo culture);

        public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mvx = Convert(value, parameter, culture);
            return NativeVisibility.ToNative(mvx);
        }

        public sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}