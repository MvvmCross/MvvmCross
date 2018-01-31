// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Logging;
using Playground.Core.Models;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IMvxViewModelLoader _mvxViewModelLoader;

        private int _counter = 2;

        public RootViewModel(IMvxNavigationService navigationService, IMvxLogProvider logProvider, IMvxViewModelLoader mvxViewModelLoader)
        {
            _navigationService = navigationService;
            _mvxViewModelLoader = mvxViewModelLoader;

            logProvider.GetLogFor<RootViewModel>().Warn(() => "Testing log");

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel, SampleModel>(new SampleModel { Message = "Hey", Value = 1.23m }));

            ShowModalCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalViewModel>());

            ShowModalNavCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitRootViewModel>());

            ShowNativeCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<NativeViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<MixedNavFirstViewModel>());

            ShowDictionaryBindingCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<DictionaryBindingViewModel>());

            _counter = 3;
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        public override async System.Threading.Tasks.Task Initialize()
        {
            await base.Initialize();

            _mvxViewModelLoader.LoadViewModel<SampleModel>(MvxViewModelRequest.GetDefaultRequest(typeof(ChildViewModel)),
                                                           new SampleModel { Message = "From locator", Value = 2 },
                                                           null);
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

        public IMvxAsyncCommand ShowNativeCommand { get; private set; }

        public IMvxAsyncCommand ShowSheetCommand { get; private set; }

        public IMvxAsyncCommand ShowWindowCommand { get; private set; }

        public IMvxAsyncCommand ShowMixedNavigationCommand { get; private set; }

        public IMvxAsyncCommand ShowDictionaryBindingCommand { get; private set; }

        public IMvxAsyncCommand ShowListViewCommand => new MvxAsyncCommand(async () => await _navigationService.Navigate<ListViewModel>());

        public IMvxAsyncCommand ShowBindingsViewCommand => new MvxAsyncCommand(async () => await _navigationService.Navigate<BindingsViewModel>());

        public IMvxAsyncCommand ShowCodeBehindViewCommand => new MvxAsyncCommand(async () => await _navigationService.Navigate<CodeBehindViewModel>());
    }
}
