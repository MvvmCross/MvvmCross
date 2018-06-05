// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels.Bindings;

namespace Playground.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private string _bindableText = "I'm bound!";

        private int _counter = 2;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel>());

            ShowModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());

            ShowModalNavCommand =
                new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitRootViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () =>
                await _navigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand =
                new MvxAsyncCommand(async () => await _navigationService.Navigate<MixedNavFirstViewModel>());

            ShowCustomBindingCommand =
                new MvxAsyncCommand(async () => await _navigationService.Navigate<CustomBindingViewModel>());

            _counter = 3;
        }

        public IMvxAsyncCommand ShowChildCommand { get; }

        public IMvxAsyncCommand ShowModalCommand { get; }

        public IMvxAsyncCommand ShowModalNavCommand { get; }

        public IMvxAsyncCommand ShowTabsCommand { get; }

        public IMvxAsyncCommand ShowCustomBindingCommand { get; }

        public IMvxAsyncCommand ShowSplitCommand { get; }

        public IMvxAsyncCommand ShowOverrideAttributeCommand { get; }

        public IMvxAsyncCommand ShowSheetCommand { get; }

        public IMvxAsyncCommand ShowWindowCommand { get; }

        public IMvxAsyncCommand ShowMixedNavigationCommand { get; }

        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("MvxBindingsExample", "Text");

        public string BindableText
        {
            get => _bindableText;
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
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
    }
}
