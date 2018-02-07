// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugin.Color
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
