using System;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.Dispatchers;
using NUnit.Framework;

namespace MvvmCross.Test.Navigation
{
    [TestFixture]
    public class NavigationServiceTests
        : MvxIoCSupportingTest
    {
        protected Mock<NavigationMockDispatcher> MockDispatcher { get; set; }

        [SetUp]
        public void SetupTest()
        {
            Setup();

            SetInvariantCulture();
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>())).Returns(() => new SimpleTestViewModel());
            mockLocator.Setup(
                m => m.Reload(It.IsAny<IMvxViewModel>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>())).Returns(() => new SimpleTestViewModel());

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            Ioc.RegisterSingleton(mockLocator.Object);

            var loader = new MvxViewModelLoader(mockCollection.Object);
            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            var navigationService = new MvxNavigationService(null, loader)
            {
                ViewDispatcher = MockDispatcher.Object,
            };
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationService);
            Ioc.RegisterSingleton<IMvxStringToTypeParser>(new MvxStringToTypeParser());
        }

        [Test]
        public async Task Test_NavigateNoBundle()
        {
            var navigationService = Ioc.Resolve<IMvxNavigationService>();

            await navigationService.Navigate<SimpleTestViewModel>();

            MockDispatcher.Verify(
                x => x.ShowViewModel(It.Is<MvxViewModelRequest>(t => t.ViewModelType == typeof(SimpleTestViewModel))),
                Times.Once);
        }

        [Test]
        public async Task Test_NavigateWithBundle()
        {
            var navigationService = Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new Mock<SimpleTestViewModel>();

            var bundle = new MvxBundle();
            bundle.Write(new { hello = "world" });

            await navigationService.Navigate(mockVm.Object, bundle);

            mockVm.Verify(vm => vm.Initialize(), Times.Once);
            //mockVm.Verify(vm => vm.Init(), Times.Once);

            //TODO: fix NavigationService not allowing parameter values in request and only presentation values
            //mockVm.Verify(vm => vm.Init(It.Is<string>(s => s == "world")), Times.Once);
        }

        [Test]
        public async Task Test_NavigateViewModelInstance()
        {
            var navigationService = Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new Mock<SimpleTestViewModel>();

            await navigationService.Navigate(mockVm.Object);

            mockVm.Verify(vm => vm.Initialize(), Times.Once);
            //mockVm.Verify(vm => vm.Init(), Times.Once);
            Assert.IsTrue(MockDispatcher.Object.Requests.Count > 0);
        }

        public class SimpleTestViewModel : MvxViewModel
        {
            public virtual void Init()
            {
            }

            public virtual void Init(string hello)
            {
            }
        }
    }
}
