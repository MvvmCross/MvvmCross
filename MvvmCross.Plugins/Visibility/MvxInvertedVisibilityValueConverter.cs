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
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            bool hide = parameter.ConvertToBooleanCore();
            switch (base.Convert(value, parameter, culture))
            {
                case MvxVisibility.Visible:
                    if (hide)
                    {
                        return MvxVisibility.Hidden;
                    }
                    else
                    {
                        return MvxVisibility.Collapsed;
                    }
                default:
                    return MvxVisibility.Visible;
            }
        }
    }
}
