// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows;

namespace MvvmCross.Platforms.Wpf.Binding
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
