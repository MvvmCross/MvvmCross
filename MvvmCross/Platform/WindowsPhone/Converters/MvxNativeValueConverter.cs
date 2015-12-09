// MvxNativeValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.WindowsPhone.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using MvvmCross.Platform.Converters;

    public class MvxNativeValueConverter
        : IValueConverter
    {
        private readonly IMvxValueConverter _wrapped;

        protected IMvxValueConverter Wrapped => this._wrapped;

        public MvxNativeValueConverter(IMvxValueConverter wrapped)
        {
            this._wrapped = wrapped;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = this._wrapped.Convert(value, targetType, parameter, culture);
            return MapIfSpecialValue(toReturn);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toReturn = this._wrapped.ConvertBack(value, targetType, parameter, culture);
            return MapIfSpecialValue(toReturn);
        }

        private static object MapIfSpecialValue(object toReturn)
        {
            if (toReturn == MvxBindingConstant.DoNothing)
            {
                Mvx.Trace("DoNothing does not have an equivalent in SL WinPhone - returning UnsetValue instead");
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

        public MvxNativeValueConverter() : base(new T())
        {
        }
    }
}