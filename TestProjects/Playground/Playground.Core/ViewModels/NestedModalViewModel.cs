using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class NestedModalViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public NestedModalViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.CloseAsync(this));

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.NavigateAsync<TabsRootViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
