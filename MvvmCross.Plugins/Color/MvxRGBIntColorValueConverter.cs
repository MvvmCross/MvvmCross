// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Base;
using MvvmCross.Base.UI;

namespace MvvmCross.Plugin.Color
{
    [Preserve(AllMembers = true)]
	public class MvxRGBIntColorValueConverter : MvxColorValueConverter<int>
    {
        protected override MvxColor Convert(int value, object parameter, CultureInfo culture)
        {
            return new MvxColor(value, 255);
        }
    }
}
