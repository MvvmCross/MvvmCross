using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
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
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Debug.Listeners.Clear();
            Debug.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener()); 

            Setup();
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>())).Returns(() => new NavigationServiceTests.SimpleTestViewModel());
            mockLocator.Setup(
                m => m.Reload(It.IsAny<IMvxViewModel>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>())).Returns(() => new NavigationServiceTests.SimpleTestViewModel());

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            Ioc.RegisterSingleton(mockLocator.Object);

            var loader = new MvxViewModelLoader(mockCollection.Object);
            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            var navigationService = RoutingService = new MvxNavigationService(null, loader)
            {
                ViewDispatcher = MockDispatcher.Object,
            };
            Ioc.RegisterSingleton(navigationService);
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        [Test]
        public async Task TestFailAsync()
        {
            var url = "mvx://fail/?id=" + Guid.NewGuid();

            var canNavigate = await RoutingService.CanNavigate(url);
            Assert.That(canNavigate, Is.False);

            Assert.CatchAsync(async () =>
            {
                await RoutingService.Navigate(url);
            });

            MockDispatcher.Verify(x => x.ShowViewModel(It.IsAny<MvxViewModelRequest>()), Times.Never);
        }

        [Test]
        public async Task TestDirectMatchRegexAsync()
        {
            await RoutingService.Navigate("mvx://test/?id=" + Guid.Empty.ToString("N"));

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelB))),
                Times.Once);
        }

        [Test]
        public async Task TestRegexWithParametersAsync()
        {
            await RoutingService.Navigate("mvx://test/?id=" + Guid.NewGuid().ToString("N"));

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelC))),
                Times.Once);
        }

        [Test]
        public async Task TestFacadeAsync()
        {
            await RoutingService.Navigate("mvx://facade/?id=a");

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(ViewModelA))),
                Times.Once);
        }
    }
}
