using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailNavViewModel : MvxViewModel
    {

        private readonly IMvxNavigationService _navigationService;

        public SplitDetailNavViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.Close(this));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
