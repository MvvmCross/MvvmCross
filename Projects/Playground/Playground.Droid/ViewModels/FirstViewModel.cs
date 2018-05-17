using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using StarWarsSample.Droid.ViewModels;

namespace Playground.Droid.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public FirstViewModel(
            IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenSecondViewCommand = new MvxAsyncCommand(OpenSecondView);
        }

        public IMvxCommand OpenSecondViewCommand { get; private set; }

        private async Task OpenSecondView()
        {
            await _navigationService.Navigate<SecondViewModel>();
        }
    }
}
