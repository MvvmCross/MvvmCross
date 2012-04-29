#region Copyright
// <copyright file="MvxBaseColorConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Converters;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Converters.Color
{
    public abstract class MvxBaseColorConverter
        : MvxBaseValueConverter, IMvxServiceConsumer<IMvxNativeColor>
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

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return NativeColor.ToNative(Convert(value, parameter, culture));
        }
    }
}