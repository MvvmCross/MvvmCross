#region Copyright
// <copyright file="MvxLanguageBinderConverter.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Interfaces.Localization;

namespace Cirrious.MvvmCross.Converters
{
    public class MvxLanguageBinderConverter 
        : MvxBaseValueConverter
    {
        #region Implementation of IValueConverter

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var binder = value as IMvxLanguageBinder;
            if (binder == null)
                return null;

            if (parameter == null)
                return null;

            return binder.GetText(parameter.ToString());
        }

        #endregion
    }
}
