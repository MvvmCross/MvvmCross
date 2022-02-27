// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using MvvmCross.Converters;
using MvvmCross.Logging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MvvmCross.Platforms.Uap.Converters
{
    public class MvxNativeValueConverter
        : IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        protected IMvxValueConverter Wrapped => _wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            _wrapped = wrapped;
        }

        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = _wrapped.Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            var toReturn = _wrapped.ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            if (toReturn == MvxBindingConstant.DoNothing)
            {
                MvxLogHost.GetLog<MvxNativeValueConverter>()?.Log(
                    LogLevel.Trace, "DoNothing does not have an equivalent in WinRT - returning UnsetValue instead");
                return DependencyProperty.UnsetValue;
            }

            if (toReturn == MvxBindingConstant.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            return toReturn;
        }
    }

    public class MvxNativeValueConverter<T>
        : MvxNativeValueConverter
        where T : IMvxValueConverter, new()
    {
        protected new T Wrapped => (T)base.Wrapped;

        public MvxNativeValueConverter()
            : base(new T())
        {
        }
    }
}
