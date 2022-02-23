// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Mac.Views
{
    public interface IMvxTabViewController
    {
        void ShowTabView(NSViewController viewController, string tabTitle);

        bool CloseTabView(IMvxViewModel viewModel);
    }
}
