// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color
{
    [Preserve(AllMembers = true)]
    public class MvxARGBValueConverter : MvxColorValueConverter<string>
    {
        protected override MvxColor Convert(string value, object parameter, CultureInfo culture)
            => MvxColor.ParseHexString(value, assumeArgb: true);
    }
}
