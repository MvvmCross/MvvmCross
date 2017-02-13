using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.Dispatchers;
using MvvmCross.Test.Mocks.TestViewModels;
using MvvmCross.Test.Stubs;
using NUnit.Framework;

[assembly: MvxNavigation(typeof(ViewModelA), @"https?://mvvmcross.com/blog")]
[assembly: MvxNavigation(typeof(ViewModelB), @"mvx://test/\?id=00000000000000000000000000000000$")]
[assembly: MvxNavigation(typeof(ViewModelC), @"mvx://test/\?id=(?<id>[A-Z0-9]{32})$")]
[assembly: MvxNavigation(typeof(SimpleRoutingFacade), @"mvx://facade/\?id=(?<vm>a|b)")]

namespace MvvmCross.Test.Navigation
{
    [TestFixture]
    public class RoutingServiceTests
        : MvxIoCSupportingTest
    {
        protected Mock<NavigationMockDispatcher> MockDispatcher;
        protected IMvxNavigationService RoutingService;

        [OneTimeSetUp]
        public static void SetupFixture()
        {
            MvxNavigationService.LoadRoutes(new[] { typeof(RoutingServiceTests).Assembly });
        }

        [SetUp]
        public void SetupTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            Debug.Listeners.Clear();
            Debug.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener()); 

            Setup();
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher.Object);
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher.Object);

            SetupRoutings();
        }

        protected void SetupRoutings()
        {
            RoutingService = new MvxNavigationService(MockDispatcher.Object);
        }

        [Test]
        public async Task TestFailAsync()
        {
            var url = "mvx://fail/?id=" + Guid.NewGuid();

            Assert.That(RoutingService.CanRoute(url), Is.False);
            await RoutingService.RouteAsync(url);

            MockDispatcher.Verify(x => x.ShowViewModel(It.IsAny<MvxViewModelRequest>()), Times.Never);
        }

        [Test]
        public async Task TestDirectMatchRegexAsync()
        {
            await RoutingService.RouteAsync("mvx://test/?id=" + Guid.Empty.ToString("N"));

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelB))),
                Times.Once);
        }


        [Test]
        public async Task TestRegexWithParametersAsync()
        {
            await RoutingService.RouteAsync("mvx://test/?id=" + Guid.NewGuid().ToString("N"));

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelC))),
                Times.Once);
        }


        [Test]
        public async Task TestFacadeAsync()
        {
            await RoutingService.RouteAsync("mvx://facade/?id=a");

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelA))),
                Times.Once);
        }

    }
}
