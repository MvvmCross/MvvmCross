// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels.Bindings;

namespace Playground.Core.ViewModels
{
    public class MainViewModel : MvxNavigationViewModel
    {
        private string _bindableText = "I'm bound!";

        private int _counter = 2;

        public MainViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>());

            ShowModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalViewModel>());

            ShowModalNavCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitRootViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<MixedNavFirstViewModel>());

            ShowCustomBindingCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CustomBindingViewModel>());

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
                SetProperty(ref _bindableText, value);
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
