using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab1ViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public Tab1ViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
        }

        public IMvxAsyncCommand OpenChildCommand { get; private set; }

        public IMvxAsyncCommand OpenModalCommand { get; private set; }

        public IMvxAsyncCommand OpenNavModalCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
