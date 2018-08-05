// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Plugin.Network.Rest;
using MvvmCross.ViewModels;
using Playground.Core.Models;
using Playground.Core.ViewModels.Bindings;
using Playground.Core.ViewModels.Samples;

namespace Playground.Core.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        private readonly IMvxViewModelLoader _mvxViewModelLoader;

        private int _counter = 2;

        private string _welcomeText = "Default welcome";

        public RootViewModel(IMvxViewModelLoader mvxViewModelLoader)
        {
            _mvxViewModelLoader = mvxViewModelLoader;
            try
            {
                var messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
                var str = messenger.ToString();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            ShowChildCommand = new MvxAsyncCommand(async () => {
                var result = await NavigationService.Navigate<ChildViewModel, SampleModel, SampleModel>(new SampleModel
                {
                    Message = "Hey",
                    Value = 1.23m
                });
                var testIfReturn = result;
            });

            ShowModalCommand = new MvxAsyncCommand(Navigate);

            ShowModalNavCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());

            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>());

            ShowSplitCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SplitRootViewModel>());

            ShowNativeCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<NativeViewModel>());

            ShowOverrideAttributeCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<OverrideAttributeViewModel>());

            ShowSheetCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SheetViewModel>());

            ShowWindowCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<WindowViewModel>());

            ShowMixedNavigationCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<MixedNavFirstViewModel>());

            ShowDictionaryBindingCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<DictionaryBindingViewModel>());

            ShowCollectionViewCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CollectionViewModel>());

            ShowSharedElementsCommand = new MvxAsyncCommand(async () =>
                await NavigationService.Navigate<SharedElementRootChildViewModel>());

            ShowCustomBindingCommand =
                new MvxAsyncCommand(async () => await NavigationService.Navigate<CustomBindingViewModel>());

            _counter = 3;
        }

        public MvxNotifyTask MyTask { get; set; }

        public IMvxAsyncCommand ShowChildCommand { get; }

        public IMvxAsyncCommand ShowModalCommand { get; }

        public IMvxAsyncCommand ShowModalNavCommand { get; }

        public IMvxAsyncCommand ShowCustomBindingCommand { get; }

        public IMvxAsyncCommand ShowTabsCommand { get; }

        public IMvxAsyncCommand ShowSplitCommand { get; }

        public IMvxAsyncCommand ShowOverrideAttributeCommand { get; }

        public IMvxAsyncCommand ShowNativeCommand { get; }

        public IMvxAsyncCommand ShowSheetCommand { get; }

        public IMvxAsyncCommand ShowWindowCommand { get; }

        public IMvxAsyncCommand ShowMixedNavigationCommand { get; }

        public IMvxAsyncCommand ShowDictionaryBindingCommand { get; }

        public IMvxAsyncCommand ShowCollectionViewCommand { get; }

        public IMvxAsyncCommand ShowListViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ListViewModel>());

        public IMvxAsyncCommand ShowBindingsViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<BindingsViewModel>());

        public IMvxAsyncCommand ShowCodeBehindViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<CodeBehindViewModel>());

        public IMvxAsyncCommand ShowContentViewCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<ParentContentViewModel>());

        public IMvxAsyncCommand ConvertersCommand =>
            new MvxAsyncCommand(async ()=> await NavigationService.Navigate<ConvertersViewModel>());

        public IMvxAsyncCommand ShowSharedElementsCommand { get; }

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
                if(Mvx.IoCProvider.TryResolve(out IMvxRestClient client))
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
    }
}
