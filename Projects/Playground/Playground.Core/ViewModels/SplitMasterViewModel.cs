// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitMasterViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitMasterViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenDetailCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitDetailViewModel>());

            OpenDetailNavCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitDetailNavViewModel>());

            ShowRootViewModel = new MvxAsyncCommand(async () => await _navigationService.Navigate<RootViewModel>());
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
