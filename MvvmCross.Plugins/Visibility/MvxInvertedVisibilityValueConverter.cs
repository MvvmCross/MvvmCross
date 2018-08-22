// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Base;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility
{
    [Preserve(AllMembers = true)]
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            bool hide = parameter.ConvertToBooleanCore();
            switch (base.Convert(value, parameter, culture))
            {
                case MvxVisibility.Visible when hide:
                    return MvxVisibility.Hidden;
                case MvxVisibility.Visible when !hide:
                    return MvxVisibility.Collapsed;
                default:
                    return MvxVisibility.Visible;
            }
        }
    }
}
