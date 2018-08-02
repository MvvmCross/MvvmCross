// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.Logging;
using Xamarin.Forms;

namespace MvvmCross.Forms.Converters
{
    public class MvxNativeValueConverter
        : IValueConverter
    {
        protected IMvxValueConverter Wrapped { get; }

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            Wrapped = wrapped;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = Wrapped.Convert(value, targetType, parameter, culture);
            return MapIfSpecialValue(toReturn);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = Wrapped.ConvertBack(value, targetType, parameter, culture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            MvxLog.Instance.Trace("DoNothing and UnsetValue is not available in Xamarin.Forms - returning empty object instead");
            if (toReturn == MvxBindingConstant.DoNothing || toReturn == MvxBindingConstant.UnsetValue)
            {
                return new object();
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
