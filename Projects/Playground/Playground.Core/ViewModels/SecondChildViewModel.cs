// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SecondChildViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SecondChildViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowNestedChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<NestedChildViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
        }

        public IMvxAsyncCommand ShowNestedChildCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
