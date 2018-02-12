// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Hints;

namespace Playground.Core.ViewModels
{
    public class Tab3ViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public Tab3ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowRootViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));

            ShowPageOneCommand = new MvxCommand(() => _navigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel))));
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; private set; }

        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }

        public IMvxCommand ShowPageOneCommand { get; private set; }
    }
}
