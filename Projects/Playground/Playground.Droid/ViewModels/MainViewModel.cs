using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace Playground.Droid.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowFirstViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<FirstViewModel>());
        }

        public IMvxAsyncCommand ShowFirstViewModelCommand { get; private set; }
    }
}
