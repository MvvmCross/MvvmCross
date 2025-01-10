// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Result;
using Playground.Core.Models;
using Playground.Core.Services;
using Playground.Core.ViewModels.Bindings;
using Playground.Core.ViewModels.Navigation;
using Playground.Core.ViewModels.Samples;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxNavigationResultAwaitingViewModel<SampleModel>
    {
        private readonly IMvxViewModelLoader _mvxViewModelLoader;

        private int _counter = 2;

        private string _welcomeText = "Default welcome";

        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder("Playground.Core", "Text"); }
        }

        public RootViewModel(
                ILoggerFactory logProvider,
                IMvxNavigationService navigationService,
                IMvxViewModelLoader mvxViewModelLoader,
                IMvxResultViewModelManager resultViewModelManager)
            : base(logProvider, navigationService, resultViewModelManager)
        {
            _mvxViewModelLoader = mvxViewModelLoader;

            ShowChildCommand = new MvxAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

            ShowModalCommand = new MvxAsyncCommand(Navigate);

            ShowModalNavCommand =
                new MvxAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

            ShowPagesCommand = new MvxAsyncCommand(() => NavigationService.Navigate<PagesRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SplitRootViewModel>());

            ShowNativeCommand = new MvxAsyncCommand(() => NavigationService.Navigate<NativeViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(() => NavigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand =
                new MvxAsyncCommand(() => NavigationService.Navigate<MixedNavFirstViewModel>());

            ShowDictionaryBindingCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<DictionaryBindingViewModel>());

            ShowCollectionViewCommand =
                new MvxAsyncCommand(() => NavigationService.Navigate<CollectionViewModel, CollectionViewParameter>(new CollectionViewParameter(50)));

            ShowSharedElementsCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<SharedElementRootChildViewModel>());

            ShowCustomBindingCommand =
                new MvxAsyncCommand(() => NavigationService.Navigate<CustomBindingViewModel>());

            ShowFluentBindingCommand =
                new MvxAsyncCommand(() => NavigationService.Navigate<FluentBindingViewModel>());

            RegisterAndResolveWithReflectionCommand = new MvxAsyncCommand(RegisterAndResolveWithReflection);
            RegisterAndResolveWithNoReflectionCommand = new MvxAsyncCommand(RegisterAndResolveWithNoReflection);

            ShowViewModelWithResult = new MvxAsyncCommand(DoShowChildWithResult);

            _counter = 3;

            TriggerVisibilityCommand =
                new MvxCommand(() => IsVisible = !IsVisible);

            FragmentCloseCommand = new MvxAsyncCommand(() => NavigationService.Navigate<FragmentCloseViewModel>());
        }

        private Task DoShowChildWithResult()
        {
            return NavigationService.NavigateRegisteringToResult<ChildWithResultViewModel, SampleModel, SampleModel>(this,
                ResultViewModelManager, new SampleModel("Hello from Root!", 1.337m));
        }

        public MvxNotifyTask MyTask { get; set; }

        public IMvxAsyncCommand ShowChildCommand { get; }

        public IMvxAsyncCommand ShowModalCommand { get; }

        public IMvxAsyncCommand ShowModalNavCommand { get; }

        public IMvxAsyncCommand ShowCustomBindingCommand { get; }

        public IMvxAsyncCommand ShowTabsCommand { get; }

        public IMvxAsyncCommand ShowPagesCommand { get; }

        public IMvxAsyncCommand ShowSplitCommand { get; }

        public IMvxAsyncCommand ShowOverrideAttributeCommand { get; }

        public IMvxAsyncCommand ShowNativeCommand { get; }

        public IMvxAsyncCommand ShowSheetCommand { get; }

        public IMvxAsyncCommand ShowWindowCommand { get; }

        public IMvxAsyncCommand ShowMixedNavigationCommand { get; }

        public IMvxAsyncCommand ShowDictionaryBindingCommand { get; }

        public IMvxAsyncCommand ShowCollectionViewCommand { get; }

        public IMvxAsyncCommand ShowListViewCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<ListViewModel>());

        public IMvxAsyncCommand ShowBindingsViewCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<BindingsViewModel>());

        public IMvxAsyncCommand ShowCodeBehindViewCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<CodeBehindViewModel>());

        public IMvxAsyncCommand ShowNavigationCloseCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<NavigationCloseViewModel>());

        public IMvxAsyncCommand ShowContentViewCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<ParentContentViewModel>());

        public IMvxAsyncCommand ShowConvertersCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<ConvertersViewModel>());

        public IMvxAsyncCommand ShowNewWindowCommand =>
            new MvxAsyncCommand(() => NavigationService.Navigate<NewWindowViewModel>());
        public IMvxAsyncCommand ShowRegionCommand =>
            new MvxAsyncCommand(() => this.NavigationService.Navigate<RegionViewModel>(this));

        public IMvxAsyncCommand ShowSharedElementsCommand { get; }

        public IMvxAsyncCommand ShowFluentBindingCommand { get; }

        public IMvxAsyncCommand RegisterAndResolveWithReflectionCommand { get; }

        public IMvxAsyncCommand RegisterAndResolveWithNoReflectionCommand { get; }

        public IMvxCommand TriggerVisibilityCommand { get; }

        public IMvxCommand FragmentCloseCommand { get; }
        public IMvxAsyncCommand ShowLocationCommand { get; }

        public MvxAsyncCommand ShowViewModelWithResult { get; set; }

        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public string WelcomeText
        {
            get => _welcomeText;
            set
            {
                ShouldLogInpc(true);
                SetProperty(ref _welcomeText, value);
                ShouldLogInpc(false);
            }
        }

        public string TimeToRegister { get; set; }

        public string TimeToResolve { get; set; }

        public string TotalTime { get; set; }

        public override Task Initialize()
        {
            Log.LogWarning("Testing log");

            return base.Initialize();
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            MyTask = MvxNotifyTask.Create(
                async () =>
                {
                    await Task.Delay(300);

                    WelcomeText = "Welcome to MvvmCross!";

                    throw new Exception("Boom!");
                }, exception => { });
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

        private Task Navigate()
        {
            return NavigationService.Navigate<ModalViewModel>();
        }

        private async Task RegisterAndResolveWithReflection()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Mvx.IoCProvider.RegisterTypesWithReflection();
            var registered = stopwatch.ElapsedTicks;
            for (int i = 0; i < 20; i++)
            {
                Mvx.IoCProvider.ResolveTypes();
            }
            stopwatch.Stop();
            var total = stopwatch.ElapsedTicks;
            var resolved = total - registered;

            TimeToRegister = $"Time to register using reflection - {registered}";
            TimeToResolve = $"Time to resolve using reflection - {resolved}";
            TotalTime = $"Total time using reflection - {total}";
            await RaiseAllPropertiesChanged();
        }

        private async Task RegisterAndResolveWithNoReflection()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Mvx.IoCProvider.RegisterTypesWithNoReflection();
            var registered = stopwatch.ElapsedTicks;
            for (int i = 0; i < 20; i++)
            {
                Mvx.IoCProvider.ResolveTypes();
            }
            stopwatch.Stop();
            var total = stopwatch.ElapsedTicks;
            var resolved = total - registered;

            TimeToRegister = $"Time to register - NO reflection - {registered}";
            TimeToResolve = $"Time to resolve - NO reflection - {resolved}";
            TotalTime = $"Total time - NO reflection - {total}";
            await RaiseAllPropertiesChanged();
        }

        public override bool ResultSet(IMvxResultSettingViewModel<SampleModel> viewModel, SampleModel result)
        {
            Log.LogInformation("Got Result {@Result} from {ViewModel}", result, viewModel.GetType().Name);
            return true;
        }
    }
}
