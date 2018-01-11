// MvxVisibilityValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;
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