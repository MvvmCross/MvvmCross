// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Tests;
using MvvmCross.UnitTest.Mocks.Dispatchers;
using MvvmCross.UnitTest.Mocks.TestViewModels;
using MvvmCross.UnitTest.Mocks.ViewModels;
using MvvmCross.UnitTest.Stubs;
using MvvmCross.ViewModels;
using NSubstitute;
using Xunit;

[assembly: MvxNavigation(typeof(ViewModelA), @"https?://mvvmcross.com/blog")]
[assembly: MvxNavigation(typeof(ViewModelB), @"mvx://test/\?id=00000000000000000000000000000000$")]
[assembly: MvxNavigation(typeof(ViewModelC), @"mvx://test/\?id=(?<id>[A-Z0-9]{32})$")]
[assembly: MvxNavigation(typeof(SimpleRoutingFacade), @"mvx://facade/\?id=(?<vm>a|b)")]

namespace MvvmCross.UnitTest.Navigation
{
    [Collection("MvxTest")]
    public class RoutingServiceTests
    {
        protected NavigationMockDispatcher MockDispatcher;
        protected IMvxNavigationService RoutingService;
        private readonly NavigationTestFixture _fixture;

        public RoutingServiceTests(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();

            // ReSharper disable once AssignNullToNotNullAttribute
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            AdditionalSetup(fixture);
        }

        private void AdditionalSetup(MvxTestFixture fixture)
        {
            var mockLocator = Substitute.For<IMvxViewModelLocator>();
            mockLocator.Load(
                    Arg.Any<Type>(),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>())
                .Returns(new ViewModelMock<SimpleTestViewModel>().Object);
            mockLocator.Reload(
                    Arg.Any<IMvxViewModel>(),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxBundle>(),
                    Arg.Any<IMvxNavigateEventArgs>())
                .Returns(new ViewModelMock<SimpleTestViewModel>().Object);

            var mockCollection = Substitute.For<IMvxViewModelLocatorCollection>();
            mockCollection.FindViewModelLocator(Arg.Any<MvxViewModelRequest>())
                          .Returns(mockLocator);

            fixture.Ioc.RegisterSingleton(mockLocator);

            var loader = new MvxViewModelLoader(mockCollection);
            MockDispatcher = new NavigationMockDispatcher();
            var navigationService = RoutingService = new MvxNavigationService(loader, MockDispatcher, fixture.Ioc);
            RoutingService.LoadRoutes([typeof(RoutingServiceTests).Assembly]);
            fixture.Ioc.RegisterSingleton(navigationService);
            fixture.Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        [Fact]
        public async Task TestFailAsync()
        {
            var url = "mvx://fail/?id=" + Guid.NewGuid();

            var canNavigate = await RoutingService.CanNavigate(url);
            Assert.False(canNavigate);

            await Assert.ThrowsAsync<MvxException>(() => RoutingService.Navigate(url));

            Assert.Empty(MockDispatcher.Requests);
        }

        [Fact]
        public async Task TestDirectMatchRegexAsync()
        {
            await RoutingService.Navigate("mvx://test/?id=" + Guid.Empty.ToString("N"),
                cancellationToken: TestContext.Current.CancellationToken);

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(ViewModelB), MockDispatcher.Requests[0].ViewModelType);
        }

        [Fact]
        public async Task TestRegexWithParametersAsync()
        {
            await RoutingService.Navigate("mvx://test/?id=" + Guid.NewGuid().ToString("N"),
                cancellationToken: TestContext.Current.CancellationToken);

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(ViewModelC), MockDispatcher.Requests[0].ViewModelType);
        }

        [Fact]
        public async Task TestFacadeAsync()
        {
            await RoutingService.Navigate("mvx://facade/?id=a",
                cancellationToken: TestContext.Current.CancellationToken);

            Assert.Single(MockDispatcher.Requests);
            Assert.Equal(typeof(ViewModelA), MockDispatcher.Requests[0].ViewModelType);
        }
    }
}
