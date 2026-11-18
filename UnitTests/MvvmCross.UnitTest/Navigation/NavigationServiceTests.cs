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

namespace MvvmCross.UnitTest.Navigation
{
    [ClassDataSource<NavigationTestFixture>(Shared = SharedType.Keyed, Key = "MvxTest")]
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

        [Test]
        public async Task Test_NavigateNoBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            await navigationService.Navigate<SimpleTestViewModel>();

            MockLoader
                .Received()
                .LoadViewModel(
                    Arg.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)),
                    Arg.Any<IMvxBundle>(), Arg.Any<IMvxNavigateEventArgs>());

            await Assert.That(MockDispatcher.Requests).HasSingleItem();
            await Assert.That(MockDispatcher.Requests[0].ViewModelType).IsEqualTo(typeof(SimpleTestViewModel));
        }

        [Test]
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

        [Test]
        public async Task Test_NavigateViewModelInstance()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new ViewModelMock<SimpleTestViewModel>();

            await navigationService.Navigate(mockVm.Object, cancellationToken: TestContext.Current.Execution.CancellationToken);

            MockLoader
                .Received()
                .ReloadViewModel(
                    Arg.Is<SimpleTestViewModel>(val => mockVm.Object == val),
                    Arg.Any<MvxViewModelRequest>(), Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            await Assert.That(MockDispatcher.Requests).HasSingleItem();
            await Assert.That(MockDispatcher.Requests[0].ViewModelType).IsEqualTo(mockVm.Object.GetType());
        }

        [Test]
        public async Task Test_NavigateWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var parameter = "hello";
            await navigationService.Navigate<SimpleParameterTestViewModel, SimpleParameter>(
                new SimpleParameter { Hello = parameter }, cancellationToken: TestContext.Current.Execution.CancellationToken);

            MockLoader
                .Received()
                .LoadViewModel<SimpleParameter>(
                    Arg.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleParameterTestViewModel)),
                    Arg.Is<SimpleParameter>(val => val.Hello == parameter),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            await Assert.That(MockDispatcher.Requests).HasSingleItem();
            await Assert.That(MockDispatcher.Requests[0].ViewModelType).IsEqualTo(typeof(SimpleParameterTestViewModel));
        }

        [Test]
        public async Task Test_NavigateViewModelInstanceWithParameter()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new ViewModelMock<SimpleParameterTestViewModel>();

            var parameter = new SimpleParameter { Hello = "hello" };
            await navigationService.Navigate<SimpleParameter>(
                mockVm.Object, parameter, cancellationToken: TestContext.Current.Execution.CancellationToken);

            MockLoader
                .Received()
                .ReloadViewModel<SimpleParameter>(
                    Arg.Is<SimpleParameterTestViewModel>(val => mockVm.Object == val),
                    Arg.Is<SimpleParameter>(val => val.Hello == parameter.Hello),
                    Arg.Any<MvxViewModelRequest>(),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>());

            await Assert.That(MockDispatcher.Requests).HasSingleItem();
            await Assert.That(MockDispatcher.Requests[0].ViewModelType).IsEqualTo(mockVm.Object.GetType());
        }

        [Test]
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

            await Assert.That(MockDispatcher.Requests).HasSingleItem();
            await Assert.That(MockDispatcher.Requests[0].ViewModelType).IsEqualTo(typeof(SimpleTestViewModel));
        }

        [Test]
        [Skip("Need to fix this")]
        public async Task Test_NavigateTypeOfWithBundle()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            var bundle = new MvxBundle();
            bundle.Write(new { hello = "world" });

            await navigationService.Navigate(typeof(SimpleTestViewModel), presentationBundle: bundle);

            //TODO: fix NavigationService not allowing parameter values in request and only presentation values
            //mockVm.Verify(vm => vm.Init(Arg.Is<string>(s => s == "world")), Times.Once);
        }

        [Test]
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

            await Assert.That(MockDispatcher.Requests).HasSingleItem();
            await Assert.That(MockDispatcher.Requests[0].ViewModelType).IsEqualTo(typeof(SimpleParameterTestViewModel));
        }

        [Test]
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

            await Assert.That(beforeNavigate).IsEqualTo(tasks.Length);
            await Assert.That(afterNavigate).IsEqualTo(tasks.Length);
        }

        [Test]
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

            await Assert.That(beforeClose).IsEqualTo(tasks.Length);
            await Assert.That(afterClose).IsEqualTo(tasks.Length);
        }

        [Test]
        public async Task Test_ChangePresentationCallbacks()
        {
            var navigationService = _fixture.Ioc.Resolve<IMvxNavigationService>();

            int beforeChangePresentation = 0;
            int afterChangePresentation = 0;
            navigationService.WillChangePresentation += (sender, e) => beforeChangePresentation++;
            navigationService.DidChangePresentation += (sender, e) => afterChangePresentation++;

            await navigationService.ChangePresentation(new MvxClosePresentationHint(new SimpleTestViewModel()));

            await Assert.That(beforeChangePresentation).IsEqualTo(1);
            await Assert.That(afterChangePresentation).IsEqualTo(1);
        }
    }
}
