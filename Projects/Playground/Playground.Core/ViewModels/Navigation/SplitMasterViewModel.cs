// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitMasterViewModel : MvxViewModel
    {
        public SplitMasterViewModel()
        {
            OpenDetailCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitDetailViewModel>());

            OpenDetailNavCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitDetailNavViewModel>());

            ShowRootViewModel = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>());
        }

        public string PaneText => "Text for the Master Pane";

        public IMvxAsyncCommand OpenDetailCommand { get; private set; }

        public IMvxAsyncCommand OpenDetailNavCommand { get; private set; }

        public IMvxAsyncCommand ShowRootViewModel { get; private set; }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }
    }
}
