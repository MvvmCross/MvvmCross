// MvxValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;

namespace Cirrious.CrossCore.Converters
{
    public abstract class MvxValueConverter
        : IMvxValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class MvxValueConverter<TFrom, TTo>
        : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TFrom)value, targetType, parameter, culture);
        }

        protected virtual TTo Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TTo)value, targetType, parameter, culture);
        }

        protected virtual TFrom ConvertBack(TTo value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class MvxValueConverter<TFrom>
        : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TFrom)value, targetType, parameter, culture);
        }

        protected virtual object Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TypedConvertBack(value, targetType, parameter, culture);
        }

        protected virtual TFrom TypedConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}