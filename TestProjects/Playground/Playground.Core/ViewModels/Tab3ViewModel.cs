using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

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
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; private set; }

        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }
    }
}
