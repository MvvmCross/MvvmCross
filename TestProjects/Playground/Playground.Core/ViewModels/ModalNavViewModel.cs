using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalNavViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public ModalNavViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.CloseAsync(this));

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.NavigateAsync<ChildViewModel>());

            ShowNestedModalCommand = new MvxAsyncCommand(async () => await _navigationService.NavigateAsync<NestedModalViewModel>());
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
