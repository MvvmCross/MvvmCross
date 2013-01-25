using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.IoC;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;
using NUnit.Framework;

namespace Cirrious.MvvmCross.Test.Core
{
    public class BaseIoCSupportingTest
    {
        private IMvxIoCProvider _ioc;

        protected IMvxIoCProvider Ioc
        {
            get { return _ioc; }
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            // fake set up of the IoC
            MvxSingleton.ClearAllSingletons();
            _ioc = new MvxSimpleIoCServiceProvider();
            var serviceProvider = new MvxServiceProvider(_ioc);
            _ioc.RegisterServiceInstance<IMvxServiceProviderRegistry>(serviceProvider);
            _ioc.RegisterServiceInstance<IMvxServiceProvider>(serviceProvider);
            _ioc.RegisterServiceInstance<IMvxTrace>(new TestTrace());
            MvxTrace.Initialize();
        }
    }
}