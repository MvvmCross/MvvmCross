// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.UI;
using WpfVisibility = System.Windows.Visibility;

namespace MvvmCross.Plugin.Visibility.Platforms.Wpf
{
    public class MvxWpfVisibility : IMvxNativeVisibility
    {
        public object ToNative(MvxVisibility visibility)
        {
            return visibility == MvxVisibility.Visible
                       ? WpfVisibility.Visible
                       : WpfVisibility.Collapsed;
        }
    }
}
