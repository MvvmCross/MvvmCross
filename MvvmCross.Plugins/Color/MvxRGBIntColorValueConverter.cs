// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace MvvmCross.Plugin.Color
{
    [Preserve(AllMembers = true)]
    public class MvxRGBIntColorValueConverter : MvxColorValueConverter<int>
    {
        protected override System.Drawing.Color Convert(int value, object parameter, CultureInfo culture)
        {
            MvxHexParser.ParseRGBInteger(value, out int red, out int green, out int blue);

            var color = System.Drawing.Color.FromArgb(red, green, blue);

            return color;
        }
    }
}
