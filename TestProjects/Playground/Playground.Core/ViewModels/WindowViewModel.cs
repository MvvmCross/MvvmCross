using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class WindowViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public WindowViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.CloseAsync(this));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
