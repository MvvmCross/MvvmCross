using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class NavigationCloseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public NavigationCloseViewModel(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public IMvxAsyncCommand OpenChildThenCloseThisCommand => new MvxAsyncCommand(CloseThisAndOpenChildAsync);

        public IMvxAsyncCommand TryToCloseNewViewModelCommand => new MvxAsyncCommand(TryToCloseNewViewModelAsync);

        private async ValueTask CloseThisAndOpenChildAsync()
        {
            await _mvxNavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(false);
            await _mvxNavigationService.Close(this).ConfigureAwait(false);
        }

        private ValueTask TryToCloseNewViewModelAsync()
        {
            return new ValueTask(_mvxNavigationService.Close(Mvx.IoCProvider.Resolve<SecondChildViewModel>()).AsTask());
        }
    }
}
