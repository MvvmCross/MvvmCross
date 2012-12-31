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
using Cirrious.MvvmCross.Converters;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Visibility
{
    public abstract class MvxBaseVisibilityConverter
        : MvxBaseValueConverter
          , IMvxServiceConsumer<IMvxNativeVisibility>
    {
        private IMvxNativeVisibility _nativeVisiblity;

        private IMvxNativeVisibility NativeVisibility
        {
            get
            {
                if (_nativeVisiblity == null)
                {
                    _nativeVisiblity = this.GetService();
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