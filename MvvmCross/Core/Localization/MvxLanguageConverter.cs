// MvxLanguageConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Localization
{
    using System;
    using System.Globalization;

    using MvvmCross.Platform.Converters;

    public class MvxLanguageConverter
        : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var binder = value as IMvxLanguageBinder;
            if (binder == null)
                return null;

            if (parameter == null)
                return null;

            return binder.GetText(parameter.ToString());
        }
    }
}