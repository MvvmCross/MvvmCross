// MvxWpfVisibility.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.UI;

namespace MvvmCross.Plugins.Visibility.Wpf
{
    public class MvxWpfVisibility : IMvxNativeVisibility
    {
        #region Implementation of IMvxNativeVisibility

        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible
                       ? System.Windows.Visibility.Visible
                       : System.Windows.Visibility.Collapsed;
        }

        #endregion Implementation of IMvxNativeVisibility
    }
}