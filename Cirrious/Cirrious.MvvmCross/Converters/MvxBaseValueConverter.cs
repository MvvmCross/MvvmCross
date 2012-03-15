#region Copyright
// <copyright file="MvxBaseValueConverter.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.Converters
{
    public abstract class MvxBaseValueConverter
        : IMvxValueConverter
    {
        #region IMvxValueConverter Members

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

#if NETFX_CORE
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
#warning language ignored
            return Convert(value, targetType, parameter, CultureInfo.CurrentUICulture);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
#warning language ignored
            return ConvertBack(value, targetType, parameter, CultureInfo.CurrentUICulture);
        }
#endif
    }
}