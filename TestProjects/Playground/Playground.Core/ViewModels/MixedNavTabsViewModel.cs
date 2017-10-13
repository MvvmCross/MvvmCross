using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavTabsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MixedNavTabsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }


        public override async void ViewAppearing()
        {
            await ShowInitialViewModels();
            base.ViewAppearing();
        }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(_navigationService.Navigate<MixedNavTab1ViewModel>());
            tasks.Add(_navigationService.Navigate<MixedNavTab2ViewModel>());
            //tasks.Add(_navigationService.Navigate<Tab1ViewModel, string>("test"));
            //tasks.Add(_navigationService.Navigate<Tab2ViewModel>());
            //tasks.Add(_navigationService.Navigate<Tab3ViewModel>());
            await Task.WhenAll(tasks);
        }
    }
}
