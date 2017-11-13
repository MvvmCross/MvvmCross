// MvxWindowsPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Windows.UI.Xaml;

namespace MvvmCross.Binding.Uwp
{
    public static class MvxWindowsPropertyBindingExtensions
    {
        public static string BindVisible(this FrameworkElement frameworkElement)
            => MvxWindowsPropertyBinding.FrameworkElement_Visible;

        public static string BindCollapsed(this FrameworkElement frameworkElement)
            => MvxWindowsPropertyBinding.FrameworkElement_Collapsed;

        public static string BindHidden(this FrameworkElement frameworkElement)
            => MvxWindowsPropertyBinding.FrameworkElement_Hidden;
    }
}
