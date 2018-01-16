using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.ViewModels.Hints;

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
