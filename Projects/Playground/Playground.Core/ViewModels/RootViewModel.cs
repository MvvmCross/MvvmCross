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
                var result = await NavigationService.Navigate<ChildViewModel, SampleModel, SampleModel>(
                    new SampleModel
                    {
                        Message = "Hey",
                        Value = 1.23m
                    }).ConfigureAwait(false);
                var testIfReturn = result;
            });

            ShowModalCommand = new MvxAsyncCommand(async () => await Navigate().ConfigureAwait(true));

            ShowModalNavCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>().ConfigureAwait(true));

            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(true));

            ShowPagesCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<PagesRootViewModel>().ConfigureAwait(true));

            ShowSplitCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitRootViewModel>().ConfigureAwait(true));

            ShowNativeCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<NativeViewModel>().ConfigureAwait(true));

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<OverrideAttributeViewModel>().ConfigureAwait(true));

            ShowSheetCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SheetViewModel>().ConfigureAwait(true));

            ShowWindowCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<WindowViewModel>().ConfigureAwait(true));

            ShowMixedNavigationCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<MixedNavFirstViewModel>().ConfigureAwait(true));

            ShowDictionaryBindingCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<DictionaryBindingViewModel>().ConfigureAwait(true));

            ShowCollectionViewCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CollectionViewModel>().ConfigureAwait(true));

            ShowSharedElementsCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<SharedElementRootChildViewModel>().ConfigureAwait(true));

            ShowCustomBindingCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CustomBindingViewModel>().ConfigureAwait(true));

            ShowFluentBindingCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<FluentBindingViewModel>().ConfigureAwait(true));

            RegisterAndResolveWithReflectionCommand = new MvxAsyncCommand(
                async () => await RegisterAndResolveWithReflection().ConfigureAwait(true));
            RegisterAndResolveWithNoReflectionCommand = new MvxAsyncCommand(
                async () => await RegisterAndResolveWithNoReflection().ConfigureAwait(true));

            _counter = 3;

            TriggerVisibilityCommand =
                new MvxCommand(() => IsVisible = !IsVisible);

            FragmentCloseCommand = new MvxAsyncCommand(
                async () => await NavigationService.Navigate<FragmentCloseViewModel>().ConfigureAwait(true));

            ShowLocationCommand = new MvxAsyncCommand(
                async () => await NavigationService.Navigate<LocationViewModel>().ConfigureAwait(true));
        }

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
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ListViewModel>().ConfigureAwait(true));

        public IMvxAsyncCommand ShowBindingsViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<BindingsViewModel>().ConfigureAwait(true));

        public IMvxAsyncCommand ShowCodeBehindViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<CodeBehindViewModel>().ConfigureAwait(true));

        public IMvxAsyncCommand ShowNavigationCloseCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<NavigationCloseViewModel>().ConfigureAwait(true));

        public IMvxAsyncCommand ShowContentViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ParentContentViewModel>().ConfigureAwait(true));

        public IMvxAsyncCommand ConvertersCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ConvertersViewModel>().ConfigureAwait(true));

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

        public override async ValueTask Initialize()
        {
            Log.Warn(() => "Testing log");

            await base.Initialize().ConfigureAwait(false);

            // Uncomment this to demonstrate use of StartAsync for async first navigation
            // await Task.Delay(5000);

            await _mvxViewModelLoader.LoadViewModel(MvxViewModelRequest.GetDefaultRequest(typeof(ChildViewModel)),
                new SampleModel
                {
                    Message = "From locator",
                    Value = 2
                },
                null).ConfigureAwait(false);

            await MakeRequest().ConfigureAwait(false);
        }

        public override ValueTask ViewAppearing()
        {
            return base.ViewAppearing();
        }

        protected override async ValueTask SaveStateToBundle(IMvxBundle? bundle)
        {
            await base.SaveStateToBundle(bundle).ConfigureAwait(false);

            bundle!.Data["MyKey"] = _counter.ToString();
        }

        protected override async ValueTask ReloadFromBundle(IMvxBundle state)
        {
            await base.ReloadFromBundle(state).ConfigureAwait(false);

            _counter = int.Parse(state.Data["MyKey"]);
        }

        private async Task Navigate()
        {
            try
            {
                await NavigationService.Navigate<ModalViewModel>().ConfigureAwait(true);
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

                    var result = await task.ConfigureAwait(true);

                    return result;
                }
            }
            catch (WebException webException)
            {
            }
            return default(MvxRestResponse);
        }

        private Task RegisterAndResolveWithReflection()
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
            return RaiseAllPropertiesChanged();
        }

        private Task RegisterAndResolveWithNoReflection()
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
            return RaiseAllPropertiesChanged();
        }
    }
}
