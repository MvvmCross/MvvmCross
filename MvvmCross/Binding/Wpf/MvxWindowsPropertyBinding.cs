// MvxWindowsPropertyBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if WINDOWS_COMMON
namespace MvvmCross.BindingEx.WindowsCommon
#endif

#if WINDOWS_WPF
namespace MvvmCross.BindingEx.Wpf
#endif
{
    internal static class MvxWindowsPropertyBinding
    {
        public const string FrameworkElement_Visible = "Visible";
        public const string FrameworkElement_Collapsed = "Collapsed";
        public const string FrameworkElement_Hidden = "Hidden";
    }
}
