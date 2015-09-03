// MvxRGBIntColorValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;
using Cirrious.CrossCore.UI;

namespace Cirrious.MvvmCross.Plugins.Color
{
    public class MvxRGBIntColorValueConverter : MvxColorValueConverter<int>
    {
        protected override MvxColor Convert(int value, object parameter, CultureInfo culture)
        {
            return new MvxColor(value, 255);
        }
    }
}