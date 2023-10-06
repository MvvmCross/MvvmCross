// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace MvvmCross.Plugin.Color
{
    [Preserve(AllMembers = true)]
    public class MvxARGBValueConverter : MvxColorValueConverter<string>
    {
        protected override System.Drawing.Color Convert(string value, object parameter, CultureInfo culture)
            => MvxHexParser.ColorFromHexString(value, assumeArgb: true);
    }
}
