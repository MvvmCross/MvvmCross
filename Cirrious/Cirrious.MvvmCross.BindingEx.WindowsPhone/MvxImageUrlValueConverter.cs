// MvxImageUrlValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Media.Imaging;
using Cirrious.CrossCore.Converters;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone
{
    public class MvxImageUrlValueConverter : MvxValueConverter<string, BitmapImage>
    {
        protected override BitmapImage Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
        }
    }
}