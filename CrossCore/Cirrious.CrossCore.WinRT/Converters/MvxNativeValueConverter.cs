// MvxNativeValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Windows.UI.Xaml.Data;

namespace Cirrious.CrossCore.WindowsStore.Converters
{
    public class MvxNativeValueConverter<T>
        : IValueConverter
        where T : IMvxValueConverter, new()
    {
        private readonly T _wrapped = new T();

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            return _wrapped.Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // note - Language ignored here!
            return _wrapped.ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
        }

        #endregion
    }
}