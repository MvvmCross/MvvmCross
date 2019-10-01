// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility
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

        private IMvxNativeVisibility NativeVisibility => _nativeVisiblity ?? (_nativeVisiblity = Mvx.IoCProvider.Resolve<IMvxNativeVisibility>());

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
