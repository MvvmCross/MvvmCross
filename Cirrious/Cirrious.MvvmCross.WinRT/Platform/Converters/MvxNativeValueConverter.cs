#region Copyright

// <copyright file="MvxNativeValueConverter.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Interfaces.Converters;
using Windows.UI.Xaml.Data;

namespace Cirrious.MvvmCross.WinRT.Platform.Converters
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