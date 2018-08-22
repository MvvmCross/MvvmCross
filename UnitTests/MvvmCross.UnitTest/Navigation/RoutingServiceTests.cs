// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
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
        protected Mock<NavigationMockDispatcher> MockDispatcher;
        protected IMvxNavigationService RoutingService;
        private readonly NavigationTestFixture _fixture;

        public RoutingServiceTests(NavigationTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearAll();

            MvxNavigationService.LoadRoutes(new[] { typeof(RoutingServiceTests).Assembly });
            // ReSharper disable once AssignNullToNotNullAttribute
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            AdditionalSetup(fixture);
        }

        private void AdditionalSetup(MvxTestFixture fixture)
        {
            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>())).Returns(() => new SimpleTestViewModel());
            mockLocator.Setup(
                m => m.Reload(It.IsAny<IMvxViewModel>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>())).Returns(() => new SimpleTestViewModel());

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            fixture.Ioc.RegisterSingleton(mockLocator.Object);

            var loader = new MvxViewModelLoader(mockCollection.Object);
            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            var navigationService = RoutingService = new MvxNavigationService(null, loader)
            {
                ViewDispatcher = MockDispatcher.Object,
            };
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

            MockDispatcher.Verify(x => x.ShowViewModel(It.IsAny<MvxViewModelRequest>()), Times.Never);
        }

        [Fact]
        public async Task TestDirectMatchRegexAsync()
        {
            await RoutingService.Navigate("mvx://test/?id=" + Guid.Empty.ToString("N"));

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelB))),
                Times.Once);
        }

        [Fact]
        public async Task TestRegexWithParametersAsync()
        {
            await RoutingService.Navigate("mvx://test/?id=" + Guid.NewGuid().ToString("N"));

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelC))),
                Times.Once);
        }

        [Fact]
        public async Task TestFacadeAsync()
        {
            await RoutingService.Navigate("mvx://facade/?id=a");

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelA))),
                Times.Once);
        }
    }
}
