// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Platform;
using MvvmCross.Platform.ExtensionMethods;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Visibility
{
    [Preserve(AllMembers = true)]
    public class MvxVisibilityValueConverter : MvxBaseVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            bool visible = value.ConvertToBooleanCore();
            bool hide = parameter.ConvertToBooleanCore();

            if (!visible)
            {
                return hide ? MvxVisibility.Hidden : MvxVisibility.Collapsed;
            }

            return MvxVisibility.Visible;
        }
    }
}
