using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IMvxAsyncCommand TryToCloseNewViewModelCommand => new MvxAsyncCommand(TryToCloseNewViewModelAsync);
        public IMvxAsyncCommand OpenChildThenCloseThisCommand => new MvxAsyncCommand(CloseThisAndOpenChildAsync);


        private async Task TryToCloseNewViewModelAsync()
        {
            await _mvxNavigationService.Close(new SecondChildViewModel());
        }

        private async Task CloseThisAndOpenChildAsync()
        {
            await _mvxNavigationService.Navigate<SecondChildViewModel>();
            await _mvxNavigationService.Close(this);
        }

    }
}
