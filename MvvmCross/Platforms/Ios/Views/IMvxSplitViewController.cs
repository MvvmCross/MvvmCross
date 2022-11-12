// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute);
    }
}
