// MvxIoCSupportingTest.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Test.Core
{
    using System.Globalization;
    using System.Threading;

    using MvvmCross.Core;
    using MvvmCross.Core.Platform;
    using Platform.Core;
    using Platform.IoC;
    using Platform.Platform;

    public class MvxIoCSupportingTest
    {
        private IMvxIoCProvider _ioc;

        protected IMvxIoCProvider Ioc => _ioc;

        public void Setup()
        {
            ClearAll();
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return null;
        }

        protected virtual void ClearAll()
        {
            // fake set up of the IoC
            MvxSingleton.ClearAllSingletons();
            _ioc = MvxSimpleIoCContainer.Initialize(CreateIocOptions());
            _ioc.RegisterSingleton(_ioc);
            _ioc.RegisterSingleton<IMvxTrace>(new TestTrace());
            InitializeSingletonCache();
            InitializeMvxSettings();
            MvxTrace.Initialize();
            AdditionalSetup();
        }

        private static void InitializeSingletonCache()
        {
            MvxSingletonCache.Initialize();
        }

        protected virtual void InitializeMvxSettings()
        {
            _ioc.RegisterSingleton<IMvxSettings>(new MvxSettings());
        }

        protected virtual void AdditionalSetup()
        {
            // nothing here..
        }

        protected void SetInvariantCulture()
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;
        }
    }
}