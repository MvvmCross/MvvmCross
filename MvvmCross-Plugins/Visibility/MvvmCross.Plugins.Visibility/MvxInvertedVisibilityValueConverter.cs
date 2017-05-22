// MvxInvertedVisibilityValueConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;
using System.Globalization;
using MvvmCross.Platform.ExtensionMethods;

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
                case (MvxVisibility.Visible):
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