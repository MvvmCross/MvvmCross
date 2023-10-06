// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Converters
{
#nullable enable
    public abstract class MvxValueConverter
        : IMvxValueConverter
    {
        public virtual object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return MvxBindingConstant.UnsetValue;
        }

        public virtual object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return MvxBindingConstant.UnsetValue;
        }
    }

    public abstract class MvxValueConverter<TFrom, TTo>
        : IMvxValueConverter
    {
        public object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                GetLog()?.Log(LogLevel.Error, $"Failed to Convert from {typeof(TFrom)} to {typeof(TTo)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TTo Convert(TFrom value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return ConvertBack((TTo)value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                GetLog()?.Log(LogLevel.Error, $"Failed to Convert from {typeof(TFrom)} to {typeof(TTo)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom ConvertBack(TTo value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        private static ILogger? GetLog() => MvxLogHost.GetLog<MvxValueConverter>();
    }

    public abstract class MvxValueConverter<TFrom>
        : IMvxValueConverter
    {
        public object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return Convert((TFrom)value, targetType, parameter, culture);
            }
            catch (Exception e)
            {
                GetLog()?.Log(LogLevel.Error, $"Failed to Convert from {typeof(TFrom)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual object Convert(TFrom value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            try
            {
                return TypedConvertBack(value, targetType, parameter, culture)!;
            }
            catch (Exception e)
            {
                GetLog()?.Log(LogLevel.Error, $"Failed to ConvertBack to {typeof(TFrom)} with Exception: {e}");
                return MvxBindingConstant.UnsetValue;
            }
        }

        protected virtual TFrom TypedConvertBack(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }

        private static ILogger? GetLog() => MvxLogHost.GetLog<MvxValueConverter>();
    }
#nullable restore
}
