// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
                l => l.LoadViewModel<SimpleParameter>(It.IsAny<MvxViewModelRequest>(), It.IsAny<SimpleParameter>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                      {
                          var vm = new SimpleParameterTestViewModel();
                          vm.Prepare();
                          vm.Prepare(new SimpleParameter { Hello = "" });
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
                l => l.ReloadViewModel<SimpleParameter>(It.IsAny<IMvxViewModel<SimpleParameter>>(), It.IsAny<SimpleParameter>(), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                      .Returns(() =>
                      {
                          var vm = new SimpleParameterTestViewModel();
                          vm.Prepare();
                          vm.Prepare(new SimpleParameter { Hello = "" });
                          vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                          return vm;
                      });

            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            var navigationService = new MvxNavigationService(MockLoader.Object, MockDispatcher.Object, fixture.Ioc);
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

            var mockVm = new ViewModelMock<SimpleTestViewModel>();

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

            var mockVm = new ViewModelMock<SimpleTestViewModel>();

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
            await navigationService.Navigate<SimpleParameterTestViewModel, SimpleParameter>(new SimpleParameter { Hello = parameter });

            MockLoader.Verify(loader => loader.LoadViewModel<SimpleParameter>(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)), It.Is<SimpleParameter>(val => val.Hello == parameter), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
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

            var mockVm = new ViewModelMock<SimpleParameterTestViewModel>();

            var parameter = new SimpleParameter { Hello = "hello" };
            await navigationService.Navigate<SimpleParameter>(mockVm.Object, parameter);

            MockLoader.Verify(loader => loader.ReloadViewModel<SimpleParameter>(It.Is<SimpleParameterTestViewModel>(val => mockVm.Object == val), It.Is<SimpleParameter>(val => val.Hello == parameter.Hello), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
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

            var parameter = new SimpleParameter { Hello = "hello" };
            await navigationService.Navigate<SimpleParameter>(typeof(SimpleParameterTestViewModel), parameter);

            MockLoader.Verify(loader => loader.LoadViewModel<SimpleParameter>(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)), It.Is<SimpleParameter>(val => val.Hello == parameter.Hello), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()),
                              Times.Once);

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel))),
                Times.Once);

            Assert.NotEmpty(MockDispatcher.Object.Requests);
        }

        [Fact]
        public async Task Test_NavigateCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeNavigate = 0;
            int afterNavigate = 0;
            navigationService.WillNavigate += (sender, e) => beforeNavigate++;
            navigationService.DidNavigate += (sender, e) => afterNavigate++;

            var tasks = new Task[]
            {
                navigationService.Navigate<SimpleTestViewModel>(),
                navigationService.Navigate<SimpleTestViewModel>(new MvxBundle()),
                navigationService.Navigate<SimpleParameterTestViewModel, SimpleParameter>(new SimpleParameter { Hello = "hello" }),
                navigationService.Navigate<SimpleParameterTestViewModel, SimpleParameter>(new SimpleParameter { Hello = "hello" }, new MvxBundle())
            };
            await Task.WhenAll(tasks);
            await Task.Delay(200);

            Assert.Equal(tasks.Length, beforeNavigate);
            Assert.Equal(tasks.Length, afterNavigate);
        }

        [Fact]
        public async Task Test_CloseCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeClose = 0;
            int afterClose = 0;
            navigationService.WillClose += (sender, e) => beforeClose++;
            navigationService.DidClose += (sender, e) => afterClose++;

            var tasks = new Task[]
            {
                navigationService.Close(new SimpleTestViewModel())
            };
            await Task.WhenAll(tasks);

            Assert.Equal(tasks.Length, beforeClose);
            Assert.Equal(tasks.Length, afterClose);
        }

        [Fact]
        public void Test_ChangePresentationCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeChangePresentation = 0;
            int afterChangePresentation = 0;
            navigationService.WillChangePresentation += (sender, e) => beforeChangePresentation++;
            navigationService.DidChangePresentation += (sender, e) => afterChangePresentation++;

            navigationService.ChangePresentation(new MvxClosePresentationHint(new SimpleTestViewModel()));

            Assert.Equal(1, beforeChangePresentation);
            Assert.Equal(1, afterChangePresentation);
        }
    }
}
