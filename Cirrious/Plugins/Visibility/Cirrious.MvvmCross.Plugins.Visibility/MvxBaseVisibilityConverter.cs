// MvxBaseVisibilityConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Visibility
{
    public abstract class MvxBaseVisibilityConverter
        : MvxBaseValueConverter
        , IMvxServiceConsumer
    {
        private IMvxNativeVisibility _nativeVisiblity;

        private IMvxNativeVisibility NativeVisibility
        {
            get
            {
                if (_nativeVisiblity == null)
                {
					_nativeVisiblity = this.GetService<IMvxNativeVisibility>();
                }

                return _nativeVisiblity;
            }
        }

        public abstract MvxVisibility ConvertToMvxVisibility(object value, object parameter, CultureInfo culture);

        public override sealed object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mvx = ConvertToMvxVisibility(value, parameter, culture);
            return NativeVisibility.ToNative(mvx);
        }

        public override sealed object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}