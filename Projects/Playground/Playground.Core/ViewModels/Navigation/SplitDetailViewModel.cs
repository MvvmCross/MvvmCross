// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailViewModel : MvxViewModel
    {
        public SplitDetailViewModel()
        {
            ShowChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitDetailNavViewModel>());
            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootBViewModel>());
        }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }
        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public string ContentText => "Text for the Content Area";

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }
    }
}
