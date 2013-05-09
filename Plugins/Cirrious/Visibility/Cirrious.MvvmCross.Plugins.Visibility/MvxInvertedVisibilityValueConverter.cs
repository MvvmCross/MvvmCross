// MvxInvertedVisibilityValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Globalization;
using Cirrious.CrossCore.UI;

namespace Cirrious.MvvmCross.Plugins.Visibility
{
    public class MvxInvertedVisibilityValueConverter : MvxVisibilityValueConverter
    {
		protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture)
        {
            var visibility = !IsATrueValue(value, parameter, true);
            return visibility ? MvxVisibility.Visible : MvxVisibility.Collapsed;
        }
    }
}