using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavMasterRootContentViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MixedNavMasterRootContentViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());
        }

        public IMvxAsyncCommand ShowModalCommand { get; private set; } 
    }
}
