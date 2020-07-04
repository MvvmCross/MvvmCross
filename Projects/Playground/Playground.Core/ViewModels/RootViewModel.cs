// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Plugin.Network.Rest;
using MvvmCross.ViewModels;
using Playground.Core.Models;
using Playground.Core.Services;
using Playground.Core.ViewModels.Bindings;
using Playground.Core.ViewModels.Location;
using Playground.Core.ViewModels.Navigation;
using Playground.Core.ViewModels.Samples;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxNavigationViewModel
    {
        private readonly IMvxViewModelLoader _mvxViewModelLoader;

        private int _counter = 2;

        private string _welcomeText = "Default welcome";

        public IMvxLanguageBinder TextSource
        {
            get { return new MvxLanguageBinder("Playground.Core", "Text"); }
        }

        public RootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IMvxViewModelLoader mvxViewModelLoader) : base(logProvider, navigationService)
        {
            _mvxViewModelLoader = mvxViewModelLoader;
            try
            {
                var messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
                var str = messenger.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            ShowChildCommand = new MvxAsyncCommand(async () =>
            {
                var result = await NavigationService.Navigate<ChildViewModel, SampleModel, SampleModel>(new SampleModel
                {
                    Message = "Hey",
                    Value = 1.23m
                });
                var testIfReturn = result;
            });

            ShowModalCommand = new MvxAsyncCommand(Navigate);

            ShowModalNavCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>().ConfigureAwait(false));

            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(false));

            ShowPagesCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<PagesRootViewModel>().ConfigureAwait(false));

            ShowSplitCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitRootViewModel>().ConfigureAwait(false));

            ShowNativeCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<NativeViewModel>().ConfigureAwait(false));

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<OverrideAttributeViewModel>().ConfigureAwait(false));

            ShowSheetCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SheetViewModel>().ConfigureAwait(false));

            ShowWindowCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<WindowViewModel>().ConfigureAwait(false));

            ShowMixedNavigationCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<MixedNavFirstViewModel>().ConfigureAwait(false));

            ShowDictionaryBindingCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<DictionaryBindingViewModel>().ConfigureAwait(false));

            ShowCollectionViewCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CollectionViewModel>().ConfigureAwait(false));

            ShowSharedElementsCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<SharedElementRootChildViewModel>().ConfigureAwait(false));

            ShowCustomBindingCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CustomBindingViewModel>().ConfigureAwait(false));

            ShowFluentBindingCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<FluentBindingViewModel>().ConfigureAwait(false));

            RegisterAndResolveWithReflectionCommand = new MvxAsyncCommand(RegisterAndResolveWithReflection);
            RegisterAndResolveWithNoReflectionCommand = new MvxAsyncCommand(RegisterAndResolveWithNoReflection);

            _counter = 3;

            TriggerVisibilityCommand =
                new MvxCommand(() => IsVisible = !IsVisible);

            FragmentCloseCommand = new MvxAsyncCommand(() => NavigationService.Navigate<FragmentCloseViewModel>());

            ShowLocationCommand = new MvxAsyncCommand(() => NavigationService.Navigate<LocationViewModel>());
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
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ListViewModel>().ConfigureAwait(false));

        public IMvxAsyncCommand ShowBindingsViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<BindingsViewModel>().ConfigureAwait(false));

        public IMvxAsyncCommand ShowCodeBehindViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<CodeBehindViewModel>().ConfigureAwait(false));

        public IMvxAsyncCommand ShowNavigationCloseCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<NavigationCloseViewModel>().ConfigureAwait(false));

        public IMvxAsyncCommand ShowContentViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ParentContentViewModel>().ConfigureAwait(false));

        public IMvxAsyncCommand ConvertersCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ConvertersViewModel>().ConfigureAwait(false));

        public IMvxAsyncCommand ShowSharedElementsCommand { get; }

        public IMvxAsyncCommand ShowFluentBindingCommand { get; }

        public IMvxAsyncCommand RegisterAndResolveWithReflectionCommand { get; }

        public IMvxAsyncCommand RegisterAndResolveWithNoReflectionCommand { get; }

        public IMvxCommand TriggerVisibilityCommand { get; }

        public IMvxCommand FragmentCloseCommand { get; }
        public IMvxAsyncCommand ShowLocationCommand { get; }

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

        public override async Task Initialize()
        {
            Log.Warn(() => "Testing log");

            await base.Initialize();

            // Uncomment this to demonstrate use of StartAsync for async first navigation
            // await Task.Delay(5000);

            _mvxViewModelLoader.LoadViewModel(MvxViewModelRequest.GetDefaultRequest(typeof(ChildViewModel)),
                new SampleModel
                {
                    Message = "From locator",
                    Value = 2
                },
                null);

            await MakeRequest();
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

        private async Task Navigate()
        {
            try
            {
                await NavigationService.Navigate<ModalViewModel>();
            }
            catch (Exception)
            {
            }
        }

        public async Task<MvxRestResponse> MakeRequest()
        {
            try
            {
                var request = new MvxRestRequest("http://github.com/asdsadadad");
                if (Mvx.IoCProvider.TryResolve(out IMvxRestClient client))
                {
                    var task = client.MakeRequestAsync(request);

                    var result = await task;

                    return result;
                }
            }
            catch (WebException webException)
            {
            }
            return default(MvxRestResponse);
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
    }
}
