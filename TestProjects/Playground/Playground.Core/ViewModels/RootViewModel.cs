using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public RootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel>());

            ShowModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());

            ShowModalNavCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitRootViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<WindowViewModel>());
        }

        private string _hello = "Hello MvvmCross";
        public string Hello
        {
            get { return _hello; }
            set { SetProperty(ref _hello, value); }
        }

        private int _counter = 2;
        public int Counter
        {
            get { return _counter; }
            set { SetProperty(ref _counter, value); }
        }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public IMvxAsyncCommand ShowModalCommand { get; private set; }

        public IMvxAsyncCommand ShowModalNavCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand ShowSplitCommand { get; private set; }

        public IMvxAsyncCommand ShowOverrideAttributeCommand { get; private set; }

        public IMvxAsyncCommand ShowSheetCommand { get; private set; }

        public IMvxAsyncCommand ShowWindowCommand { get; private set; }
    }
}
