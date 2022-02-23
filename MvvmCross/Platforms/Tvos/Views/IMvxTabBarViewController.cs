// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public interface IMvxTabBarViewController
    {
        void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(IMvxViewModel viewModel);

        bool CloseTabViewModel(IMvxViewModel viewModel);

        bool CanShowChildView();
    }
}
