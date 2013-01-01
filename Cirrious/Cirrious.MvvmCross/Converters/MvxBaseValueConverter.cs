// MvxBaseValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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