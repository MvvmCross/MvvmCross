using System.Threading.Tasks;
using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;

namespace Playground.Core.ViewModels
{
    public class TabsRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public TabsRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(_navigationService.NavigateAsync<Tab1ViewModel, string>("test"));
            tasks.Add(_navigationService.NavigateAsync<Tab2ViewModel>());
            tasks.Add(_navigationService.NavigateAsync<Tab3ViewModel>());
            await Task.WhenAll(tasks);
        }
    }
}
