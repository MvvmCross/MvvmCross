// MvxWindowsPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows;

#if WINDOWS_COMMON
namespace MvvmCross.BindingEx.WindowsCommon
#endif

#if WINDOWS_WPF
namespace MvvmCross.BindingEx.Wpf
#endif
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
