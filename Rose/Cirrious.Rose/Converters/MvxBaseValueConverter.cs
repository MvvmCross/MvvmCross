// MvxBaseValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using Cirrious.CrossCore.Interfaces.Converters;

namespace Cirrious.CrossCore.Converters
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
    }
}