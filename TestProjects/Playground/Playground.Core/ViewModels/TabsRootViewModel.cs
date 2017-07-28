using System.Threading.Tasks;
﻿using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class TabsRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public TabsRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            await _navigationService.Navigate<Tab1ViewModel, string>("test").ConfigureAwait(false);
            await _navigationService.Navigate<Tab2ViewModel>().ConfigureAwait(false);
            await _navigationService.Navigate<Tab3ViewModel>().ConfigureAwait(false);
        }
    }
}
