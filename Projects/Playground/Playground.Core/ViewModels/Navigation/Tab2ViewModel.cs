// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab2ViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public Tab2ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowRootViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; private set; }

        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }
    }
}
