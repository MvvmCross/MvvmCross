using System;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private int _counter = 2;

        public RootViewModel(IMvxNavigationService navigationService, IMvxLogProvider logProvider)
        {
            _navigationService = navigationService;

            logProvider.GetLogFor<RootViewModel>().Warn(() => "Testing log");

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel>());

            ShowModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());

            ShowModalNavCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitRootViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<MixedNavFirstViewModel>());

            _counter = 3;
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);

            bundle.Data["MyKey"] = _counter.ToString();
        }

        protected override void ReloadFromBundle(IMvxBundle state)
        {
            base.ReloadFromBundle(state);

            _counter = int.Parse(state.Data["MyKey"]);
        }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public IMvxAsyncCommand ShowModalCommand { get; private set; }

        public IMvxAsyncCommand ShowModalNavCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand ShowSplitCommand { get; private set; }

        public IMvxAsyncCommand ShowOverrideAttributeCommand { get; private set; }

        public IMvxAsyncCommand ShowSheetCommand { get; private set; }

        public IMvxAsyncCommand ShowWindowCommand { get; private set; }

        public IMvxAsyncCommand ShowMixedNavigationCommand { get; private set; }

        public IMvxAsyncCommand ShowListViewCommand => new MvxAsyncCommand(async () => await _navigationService.Navigate<ListViewModel>());

        public IMvxAsyncCommand ShowBindingsViewCommand => new MvxAsyncCommand(async () => await _navigationService.Navigate<BindingsViewModel>());

        public IMvxAsyncCommand ShowCodeBehindViewCommand => new MvxAsyncCommand(async () => await _navigationService.Navigate<CodeBehindViewModel>());
    }
}
