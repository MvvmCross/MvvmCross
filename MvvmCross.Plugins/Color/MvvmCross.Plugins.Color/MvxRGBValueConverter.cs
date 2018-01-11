// MvxRGBValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Color
{
    [Preserve(AllMembers = true)]
	public class MvxRGBValueConverter : MvxColorValueConverter<string>
    {
        protected override MvxColor Convert(string value, object parameter, CultureInfo culture)
            => MvxColor.ParseHexString(value);
    }
}