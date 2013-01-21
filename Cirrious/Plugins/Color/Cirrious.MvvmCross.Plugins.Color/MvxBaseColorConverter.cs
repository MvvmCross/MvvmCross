// MvxBaseColorConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Color
{
    public abstract class MvxBaseColorConverter
        : MvxBaseValueConverter
		, IMvxServiceConsumer
    {
        private IMvxNativeColor _nativeColor;

        private IMvxNativeColor NativeColor
        {
            get
            {
                if (_nativeColor == null)
                {
					_nativeColor = this.GetService<IMvxNativeColor>();
                }

                return _nativeColor;
            }
        }

        protected abstract MvxColor Convert(object value, object parameter, System.Globalization.CultureInfo culture);

        public override object Convert(object value, Type targetType, object parameter,
                                       System.Globalization.CultureInfo culture)
        {
            return NativeColor.ToNative(Convert(value, parameter, culture));
        }
    }
}