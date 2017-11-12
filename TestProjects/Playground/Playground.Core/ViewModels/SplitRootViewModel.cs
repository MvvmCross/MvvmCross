using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

namespace Playground.Core.ViewModels
{
    public class SplitRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowInitialMenuCommand = new MvxAsyncCommand(ShowInitialViewModels);
            ShowDetailCommand = new MvxAsyncCommand(ShowDetailViewModel);
        }

        public IMvxAsyncCommand ShowInitialMenuCommand { get; private set; }

        public IMvxAsyncCommand ShowDetailCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            await _navigationService.Navigate<SplitMasterViewModel>();
        }

        private async Task ShowDetailViewModel()
        {
            await _navigationService.Navigate<SplitDetailViewModel>();
        }
    }
}
