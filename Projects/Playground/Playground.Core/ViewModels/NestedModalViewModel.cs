// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class NestedModalViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public NestedModalViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
