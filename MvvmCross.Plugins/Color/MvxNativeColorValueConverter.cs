// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace MvvmCross.Plugin.Color
{
    [Preserve(AllMembers = true)]
    public class MvxNativeColorValueConverter : MvxColorValueConverter<System.Drawing.Color>
    {
        protected override System.Drawing.Color Convert(System.Drawing.Color value, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
