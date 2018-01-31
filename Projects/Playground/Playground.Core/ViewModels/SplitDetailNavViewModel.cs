// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailNavViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitDetailNavViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            MainMenuCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<MixedNavFirstViewModel>());
            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
        }

        public IMvxAsyncCommand MainMenuCommand { get; private set; }
        public IMvxAsyncCommand CloseCommand { get; private set; }

    }
}
