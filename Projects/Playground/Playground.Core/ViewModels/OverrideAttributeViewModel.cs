// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class OverrideAttributeViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public OverrideAttributeViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SecondChildViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; private set; }
    }
}
