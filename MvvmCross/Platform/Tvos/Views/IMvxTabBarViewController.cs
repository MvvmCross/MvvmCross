// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Tvos.Views.Presenters.Attributes;
using UIKit;

namespace MvvmCross.Platform.Tvos.Views
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
