using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowInitialMenuCommand = new MvxAsyncCommand(ShowInitialViewModel);
            ShowDetailCommand = new MvxAsyncCommand(ShowDetailViewModel);
        }

        public IMvxAsyncCommand ShowInitialMenuCommand { get; private set; }

        public IMvxAsyncCommand ShowDetailCommand { get; private set; }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            await ShowInitialViewModel();
            await ShowDetailViewModel();
        }

        private async Task ShowInitialViewModel()
        {
            await _navigationService.Navigate<SplitMasterViewModel>();
        }

        private async Task ShowDetailViewModel()
        {
            await _navigationService.Navigate<SplitDetailViewModel>();
        }
    }
}
