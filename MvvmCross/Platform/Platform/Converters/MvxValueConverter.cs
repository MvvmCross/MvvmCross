// MvxValueConverter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;

namespace MvvmCross.Platform.Converters
{
    public abstract class MvxValueConverter
        : IMvxValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MvxBindingConstant.UnsetValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MvxBindingConstant.UnsetValue;
        }
    }

    public abstract class MvxValueConverter<TFrom, TTo>
        : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Mvx.TaggedError("MvxValueConverter",
                    $"Failed to Convert from {typeof (TFrom)} to {typeof (TTo)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TTo Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ConvertBack((TTo)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Mvx.TaggedError("MvxValueConverter",
                    $"Failed to ConvertBack from {typeof(TTo)} to {typeof(TFrom)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
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
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Mvx.TaggedError("MvxValueConverter",
                    $"Failed to Convert from {typeof(TFrom)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual object Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return TypedConvertBack(value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                Mvx.TaggedError("MvxValueConverter",
                    $"Failed to ConvertBack to {typeof(TFrom)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom TypedConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}