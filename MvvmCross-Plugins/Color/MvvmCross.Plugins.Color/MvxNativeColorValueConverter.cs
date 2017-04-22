// MvxNativeColorValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;
using System.Globalization;

namespace MvvmCross.Plugins.Color
{
    [Preserve(AllMembers = true)]
	public class MvxNativeColorValueConverter : MvxColorValueConverter<MvxColor>
    {
        protected override MvxColor Convert(MvxColor value, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}