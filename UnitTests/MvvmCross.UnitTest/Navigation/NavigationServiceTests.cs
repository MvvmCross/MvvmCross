// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters.Hints;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using MvvmCross.UnitTest.Mocks.ViewModels;
using MvvmCross.ViewModels;
using Xunit;

namespace MvvmCross.UnitTest.Navigation
{
    [Collection("MvxTest")]
    public class NavigationServiceTests
    {
        private readonly NavigationTestFixture _fixture;

        public NavigationServiceTests(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();
            AdditionalSetup(fixture);
        }

        protected Mock<NavigationMockDispatcher> MockDispatcher { get; set; }

        protected Mock<IMvxViewModelLoader> MockLoader { get; set; }


        private void AdditionalSetup(MvxTestFixture fixture)
        {
            MockLoader = new Mock<IMvxViewModelLoader>();
            MockLoader.Setup(
                l => l.LoadViewModel(It.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                       {
                           var vm = new SimpleTestViewModel();
                           vm.Prepare();
                           vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                           return vm;
                       });
            MockLoader.Setup(
                l => l.LoadViewModel(It.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleResultTestViewModel)), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                       {
                           var vm = new SimpleResultTestViewModel();
                           vm.Prepare();
                           vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                           return vm;
                       });
            MockLoader.Setup(
                l => l.LoadViewModel<string>(It.IsAny<MvxViewModelRequest>(), It.IsAny<string>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                      {
                          var vm = new SimpleParameterTestViewModel();
                          vm.Prepare();
                          vm.Prepare("");
                          vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                          return vm;
                      });
            MockLoader.Setup(
                l => l.ReloadViewModel(It.IsAny<IMvxViewModel>(), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                      {
                          var vm = new SimpleTestViewModel();
                          vm.Prepare();
                          vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                          return vm;
                      });
            MockLoader.Setup(
                l => l.ReloadViewModel<string>(It.IsAny<IMvxViewModel<string>>(), It.IsAny<string>(), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                      {
                          var vm = new SimpleParameterTestViewModel();
                          vm.Prepare();
                          vm.Prepare("");
                          vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                          return vm;
                      });

            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            var navigationService = new MvxNavigationService(null, MockLoader.Object)
            {
                ViewDispatcher = MockDispatcher.Object,
            };
            fixture.Ioc.RegisterSingleton<IMvxNavigationService>(navigationService);
            fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        [Fact]
        public async Task Test_NavigateNoBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            await navigationService.Navigate<SimpleTestViewModel>();

            MockLoader.Verify(loader => loader.LoadViewModel(It.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleTestViewModel))),
                Times.Once);
        }

        [Fact]
        public async Task Test_NavigateWithBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new Mock<SimpleTestViewModel>();

            var bundle = new MvxBundle();
            bundle.Write(new { hello = "world" });

            await navigationService.Navigate(mockVm.Object, bundle);

            //TODO: fix NavigationService not allowing parameter values in request and only presentation values
            //mockVm.Verify(vm => vm.Init(It.Is<string>(s => s == "world")), Times.Once);
        }

        [Fact]
        public async Task Test_NavigateViewModelInstance()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new Mock<SimpleTestViewModel>();

            await navigationService.Navigate(mockVm.Object);

            MockLoader.Verify(loader => loader.ReloadViewModel(It.Is<SimpleTestViewModel>(val => mockVm.Object == val), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelInstanceRequest>(t => t.ViewModelInstance == mockVm.Object)),
                Times.Once);

            Assert.NotEmpty(MockDispatcher.Object.Requests);
        }

        [Fact]
        public async Task Test_NavigateWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var parameter = "hello";
            await navigationService.Navigate<SimpleParameterTestViewModel, string>(parameter);

            MockLoader.Verify(loader => loader.LoadViewModel<string>(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)), It.Is<string>(val => val == parameter), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel))),
                Times.Once);

            Assert.NotEmpty(MockDispatcher.Object.Requests);
        }

        [Fact]
        public async Task Test_NavigateViewModelInstanceWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new Mock<SimpleParameterTestViewModel>();

            var parameter = "hello";
            await navigationService.Navigate<string>(mockVm.Object, parameter);

            MockLoader.Verify(loader => loader.ReloadViewModel<string>(It.Is<SimpleParameterTestViewModel>(val => mockVm.Object == val), It.Is<string>(val => val == parameter), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelInstanceRequest>(t => t.ViewModelInstance == mockVm.Object)),
                Times.Once);

            Assert.NotEmpty(MockDispatcher.Object.Requests);
        }

        [Fact]
        public async Task Test_NavigateTypeOfNoBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            await navigationService.Navigate(typeof(SimpleTestViewModel));

            MockLoader.Verify(loader => loader.LoadViewModel(It.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleTestViewModel))),
                Times.Once);
        }

        [Fact]
        public async Task Test_NavigateTypeOfWithBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var bundle = new MvxBundle();
            bundle.Write(new { hello = "world" });

            await navigationService.Navigate(typeof(SimpleTestViewModel), presentationBundle: bundle);

            //TODO: fix NavigationService not allowing parameter values in request and only presentation values
            //mockVm.Verify(vm => vm.Init(It.Is<string>(s => s == "world")), Times.Once);
        }

        [Fact]
        public async Task Test_NavigateTypeOfWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var parameter = "hello";
            await navigationService.Navigate<string>(typeof(SimpleParameterTestViewModel), parameter);

            MockLoader.Verify(loader => loader.LoadViewModel<string>(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)), It.Is<string>(val => val == parameter), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel))),
                Times.Once);

            Assert.NotEmpty(MockDispatcher.Object.Requests);
        }

        [Fact]
        public async Task Test_NavigateForResult()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var result = await navigationService.Navigate<SimpleResultTestViewModel, bool>();

            MockLoader.Verify(loader => loader.LoadViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleResultTestViewModel)), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleResultTestViewModel))),
                Times.Once);

            Assert.NotEmpty(MockDispatcher.Object.Requests);
            Assert.True(result);
        }

        [Fact]
        public async Task Test_NavigateCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeNavigate = 0;
            int afterNavigate = 0;
            navigationService.BeforeNavigate += (sender, e) => beforeNavigate++;
            navigationService.AfterNavigate += (sender, e) => afterNavigate++;

            var tasks = new Task[]
            {
                navigationService.Navigate<SimpleTestViewModel>(),
                navigationService.Navigate<SimpleTestViewModel>(new MvxBundle()),
                navigationService.Navigate<SimpleResultTestViewModel, bool>(),
                navigationService.Navigate<SimpleResultTestViewModel, bool>(new MvxBundle()),
                navigationService.Navigate<SimpleParameterTestViewModel, string>("hello"),
                navigationService.Navigate<SimpleParameterTestViewModel, string>("hello", new MvxBundle())
            };
            await Task.WhenAll(tasks);
            await Task.Delay(200);

            Assert.Equal(6, beforeNavigate);
            Assert.Equal(6, afterNavigate);
        }

        [Fact]
        public async Task Test_CloseCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeClose = 0;
            int afterClose = 0;
            navigationService.BeforeClose += (sender, e) => beforeClose++;
            navigationService.AfterClose += (sender, e) => afterClose++;

            var tasks = new Task[]
            {
                navigationService.Close(new SimpleTestViewModel()),
                navigationService.Close<bool>(new SimpleResultTestViewModel(), false)
            };
            await Task.WhenAll(tasks);

            Assert.Equal(2, beforeClose);
            Assert.Equal(2, afterClose);
        }

        [Fact]
        public void Test_ChangePresentationCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeChangePresentation = 0;
            int afterChangePresentation = 0;
            navigationService.BeforeChangePresentation += (sender, e) => beforeChangePresentation++;
            navigationService.AfterChangePresentation += (sender, e) => afterChangePresentation++;

            navigationService.ChangePresentation(new MvxClosePresentationHint(new SimpleTestViewModel()));

            Assert.Equal(1, beforeChangePresentation);
            Assert.Equal(1, afterChangePresentation);
        }
    }
}
