using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.IoC;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;
using NUnit.Framework;
using TwitterSearch.Test.Mocks;

namespace TwitterSearch.Test
{
    public class BaseIoCSupportingTest
    {
        private IMvxIoCProvider _ioc;

        protected IMvxIoCProvider Ioc
        {
            get { return _ioc; }
        }

        [TestFixtureSetUp]
        public virtual void Setup()
        {
            // fake set up of the IoC
            MvxSingleton.ClearAllSingletons();
            _ioc = new MvxSimpleIoCServiceProvider();
            var serviceProvider = new MvxServiceProvider(_ioc);
            _ioc.RegisterServiceInstance<IMvxServiceProviderRegistry>(serviceProvider);
            _ioc.RegisterServiceInstance<IMvxServiceProvider>(serviceProvider);
            _ioc.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            MvxTrace.Initialize();
        }


        protected MockMvxViewDispatcher CreateMockNavigation()
        {
            var mockNavigation = new MockMvxViewDispatcher();
            var mockNavigationProvider = new MockMvxViewDispatcherProvider();
            mockNavigationProvider.Dispatcher = mockNavigation;
            Ioc.RegisterServiceInstance<IMvxViewDispatcherProvider>(mockNavigationProvider);
            return mockNavigation;
        }
    }
}