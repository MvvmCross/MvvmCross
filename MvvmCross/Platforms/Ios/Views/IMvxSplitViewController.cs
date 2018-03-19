// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platform.Ios.Views
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, bool wrapInNavigationController);

        void ShowDetailView(UIViewController viewController, bool wrapInNavigationController);

        bool CloseChildViewModel(IMvxViewModel viewModel);
    }
}
