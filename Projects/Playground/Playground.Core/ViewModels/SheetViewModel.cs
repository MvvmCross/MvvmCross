using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

namespace Playground.Core.ViewModels
{
    public class SheetViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SheetViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(CloseSheet);
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        private async Task CloseSheet()
        {
            await _navigationService.Close(this);
        }
    }
}
