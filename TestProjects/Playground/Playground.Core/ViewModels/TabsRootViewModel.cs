using System.Threading.Tasks;
using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using MvvmCross.Platform.Platform;

namespace Playground.Core.ViewModels
{
    public class TabsRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public TabsRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
            ShowTabsRootBCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootBViewModel>());
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsRootBCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(_navigationService.Navigate<Tab1ViewModel, string>("test"));
            tasks.Add(_navigationService.Navigate<Tab2ViewModel>());
            tasks.Add(_navigationService.Navigate<Tab3ViewModel>());
            await Task.WhenAll(tasks);
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                if (_itemIndex == value) return;
                _itemIndex = value;
                MvxTrace.Trace("Tab item changed to {0}", _itemIndex.ToString());
                RaisePropertyChanged(() => ItemIndex);
            }
        }
    }
}
