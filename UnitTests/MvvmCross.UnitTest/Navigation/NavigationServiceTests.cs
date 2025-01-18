// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters.Hints;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using MvvmCross.UnitTest.Mocks.ViewModels;
using MvvmCross.ViewModels;
using NSubstitute;
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

        protected NavigationMockDispatcher MockDispatcher { get; set; }

        protected IMvxViewModelLoader MockLoader { get; set; }


        private void AdditionalSetup(MvxTestFixture fixture)
        {
            MockLoader = Substitute.For<IMvxViewModelLoader>();
            MockLoader
                .LoadViewModel(
                    Arg.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)),
                    Arg.Any<IMvxBundle>(), Arg.Any<IMvxNavigateEventArgs>())
                .Returns(CreateSimpleTestViewModel());
            MockLoader
                .LoadViewModel<SimpleParameter>(Arg.Any<MvxViewModelRequest>(), Arg.Any<SimpleParameter>(), Arg.Any<IMvxBundle>(), Arg.Any<IMvxNavigateEventArgs>())
                .Returns(CreateSimpleParameterTestViewModel());
            MockLoader
                .ReloadViewModel(Arg.Any<IMvxViewModel>(), Arg.Any<MvxViewModelRequest>(), Arg.Any<IMvxBundle>(), Arg.Any<IMvxNavigateEventArgs>())
                .Returns(CreateSimpleTestViewModel());
            MockLoader
                .ReloadViewModel<SimpleParameter>(Arg.Any<IMvxViewModel<SimpleParameter>>(), Arg.Any<SimpleParameter>(), Arg.Any<MvxViewModelRequest>(), Arg.Any<IMvxBundle>(), Arg.Any<IMvxNavigateEventArgs>())
                .Returns(CreateSimpleParameterTestViewModel());

            MockDispatcher = new NavigationMockDispatcher();
            var navigationService = new MvxNavigationService(MockLoader, MockDispatcher, fixture.Ioc);
            fixture.Ioc.RegisterSingleton<IMvxNavigationService>(navigationService);
            fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        private static SimpleParameterTestViewModel CreateSimpleParameterTestViewModel()
        {
            var vm = new SimpleParameterTestViewModel();
            vm.Prepare();
            vm.Prepare(new SimpleParameter { Hello = "" });
            vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
            return vm;
        }

        private static SimpleTestViewModel CreateSimpleTestViewModel()
        {
            var vm = new SimpleTestViewModel();
            vm.Prepare();
            vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
            return vm;
        }

        [Fact]
        public async Task Test_NavigateNoBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            await navigationService.Navigate<SimpleTestViewModel>();

            MockLoader
                .Received()
                .LoadViewModel(
                    Arg.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)),
                    Arg.Any<IMvxBundle>(), Arg.Any<IMvxNavigateEventArgs>());

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(SimpleTestViewModel), MockDispatcher.Requests[0].ViewModelType);
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
            //mockVm.Verify(vm => vm.Init(Arg.Is<string>(s => s == "world")), Times.Once);
        }

        [Fact]
        public async Task Test_NavigateViewModelInstance()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new ViewModelMock<SimpleTestViewModel>();

            await navigationService.Navigate(mockVm.Object, cancellationToken: TestContext.Current.CancellationToken);

            MockLoader
                .Received()
                .ReloadViewModel(
                    Arg.Is<SimpleTestViewModel>(val => mockVm.Object == val),
                    Arg.Any<MvxViewModelRequest>(), Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(mockVm.Object.GetType(), MockDispatcher.Requests[0].ViewModelType);
        }

        [Fact]
        public async Task Test_NavigateWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var parameter = "hello";
            await navigationService.Navigate<SimpleParameterTestViewModel, SimpleParameter>(
                new SimpleParameter { Hello = parameter }, cancellationToken: TestContext.Current.CancellationToken);

            MockLoader
                .Received()
                .LoadViewModel<SimpleParameter>(
                    Arg.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)),
                    Arg.Is<SimpleParameter>(val => val.Hello == parameter),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(SimpleParameterTestViewModel), MockDispatcher.Requests[0].ViewModelType);
        }

        [Fact]
        public async Task Test_NavigateViewModelInstanceWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new ViewModelMock<SimpleParameterTestViewModel>();

            var parameter = new SimpleParameter { Hello = "hello" };
            await navigationService.Navigate<SimpleParameter>(
                mockVm.Object, parameter, cancellationToken: TestContext.Current.CancellationToken);

            MockLoader
                .Received()
                .ReloadViewModel<SimpleParameter>(
                    Arg.Is<SimpleParameterTestViewModel>(val => mockVm.Object == val),
                    Arg.Is<SimpleParameter>(val => val.Hello == parameter.Hello),
                    Arg.Any<MvxViewModelRequest>(),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(mockVm.Object.GetType(), MockDispatcher.Requests[0].ViewModelType);
        }

        [Fact]
        public async Task Test_NavigateTypeOfNoBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            await navigationService.Navigate(typeof(SimpleTestViewModel));

            MockLoader
                .Received()
                .LoadViewModel(
                    Arg.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(SimpleTestViewModel), MockDispatcher.Requests[0].ViewModelType);
        }

        [Fact(Skip = "Need to fix this")]
        public async Task Test_NavigateTypeOfWithBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var bundle = new MvxBundle();
            bundle.Write(new { hello = "world" });

            await navigationService.Navigate(typeof(SimpleTestViewModel), presentationBundle: bundle);

            //TODO: fix NavigationService not allowing parameter values in request and only presentation values
            //mockVm.Verify(vm => vm.Init(Arg.Is<string>(s => s == "world")), Times.Once);
        }

        [Fact]
        public async Task Test_NavigateTypeOfWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var parameter = new SimpleParameter { Hello = "hello" };
            await navigationService.Navigate<SimpleParameter>(typeof(SimpleParameterTestViewModel), parameter);

            MockLoader
                .Received()
                .LoadViewModel<SimpleParameter>(
                    Arg.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)),
                    Arg.Is<SimpleParameter>(val => val.Hello == parameter.Hello),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(SimpleParameterTestViewModel), MockDispatcher.Requests[0].ViewModelType);
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
