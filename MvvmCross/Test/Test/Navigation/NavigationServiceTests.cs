using System;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Test.Core;
using MvvmCross.Test.Mocks.Dispatchers;
using MvvmCross.Test.Mocks.ViewModels;
using NUnit.Framework;

namespace MvvmCross.Test.Navigation
{
    [TestFixture]
    public class NavigationServiceTests
        : MvxIoCSupportingTest
    {
        protected Mock<NavigationMockDispatcher> MockDispatcher { get; set; }

        protected Mock<IMvxViewModelLoader> MockLoader { get; set; }

        [SetUp]
        public void SetupTest()
        {
            Setup();

            SetInvariantCulture();
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            MockLoader = new Mock<IMvxViewModelLoader>();
            MockLoader.Setup(
                l => l.LoadViewModel(It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>()))
                      .Returns(() =>
                       {
                           var vm = new SimpleTestViewModel();
                           vm.Prepare();
                           vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                           return vm;
                       });
            MockLoader.Setup(
                l => l.ReloadViewModel(It.IsAny<IMvxViewModel>(), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>()))
                      .Returns(() =>
                      {
                          var vm = new SimpleTestViewModel();
                          vm.Prepare();
                          vm.InitializeTask = MvxNotifyTask.Create(() => vm.Initialize());
                          return vm;
                      });

            MockDispatcher = new Mock<NavigationMockDispatcher>(MockBehavior.Loose) { CallBase = true };
            var navigationService = new MvxNavigationService(null, MockLoader.Object)
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

            MockLoader.Verify(loader => loader.LoadViewModel(It.Is<MvxViewModelRequest>(val => val.ViewModelType == typeof(SimpleTestViewModel)), It.IsAny<IMvxBundle>()),
                              Times.Once);

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

            //TODO: fix NavigationService not allowing parameter values in request and only presentation values
            //mockVm.Verify(vm => vm.Init(It.Is<string>(s => s == "world")), Times.Once);
        }

        [Test]
        public async Task Test_NavigateViewModelInstance()
        {
            var navigationService = Ioc.Resolve<IMvxNavigationService>();

            var mockVm = new Mock<SimpleTestViewModel>();

            await navigationService.Navigate(mockVm.Object);

            MockLoader.Verify(loader => loader.ReloadViewModel(It.Is<SimpleTestViewModel>(val => mockVm.Object == val), It.IsAny<MvxViewModelRequest>(), It.IsAny<IMvxBundle>()),
                              Times.Once);
            Assert.IsTrue(MockDispatcher.Object.Requests.Count > 0);
        }
    }
}
