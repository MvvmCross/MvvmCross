// MvxColorValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;
using System;

namespace MvvmCross.Plugins.Color
{
    public abstract class MvxColorValueConverter
        : MvxValueConverter
    {
        private IMvxNativeColor _nativeColor;

        private IMvxNativeColor NativeColor => _nativeColor ?? (_nativeColor = Mvx.Resolve<IMvxNativeColor>());

        protected abstract MvxColor Convert(object value, object parameter, System.Globalization.CultureInfo culture);

        public sealed override object Convert(object value, Type targetType, object parameter,
                                       System.Globalization.CultureInfo culture)
        {
            return NativeColor.ToNative(Convert(value, parameter, culture));
        }
    }

    public abstract class MvxColorValueConverter<T>
        : MvxColorValueConverter
    {
        protected sealed override MvxColor Convert(object value, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert((T)value, parameter, culture);
        }

        protected abstract MvxColor Convert(T value, object parameter, System.Globalization.CultureInfo culture);
    }
}