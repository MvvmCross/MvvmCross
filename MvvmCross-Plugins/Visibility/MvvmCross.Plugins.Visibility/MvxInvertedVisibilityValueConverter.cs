// MvxInvertedVisibilityValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Visibility
{
    [Preserve(AllMembers = true)]
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            var visibility = !IsATrueValue(value, parameter, true);
            return visibility ? MvxVisibility.Visible : MvxVisibility.Collapsed;
        }
    }
}