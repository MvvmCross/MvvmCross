// MvxWindowsPhoneVisibility.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;

namespace MvvmCross.Plugins.Visibility.WindowsPhone
{
    public class MvxWindowsPhoneVisibility : IMvxNativeVisibility
    {
        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible
                       ? System.Windows.Visibility.Visible
                       : System.Windows.Visibility.Collapsed;
        }
    }
}