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
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.IoC;
    using MvvmCross.Platform.Platform;

    public class MvxIoCSupportingTest
    {
        private IMvxIoCProvider _ioc;

        protected IMvxIoCProvider Ioc => this._ioc;

        public void Setup()
        {
            this.ClearAll();
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return null;
        }

        protected virtual void ClearAll()
        {
            // fake set up of the IoC
            MvxSingleton.ClearAllSingletons();
            this._ioc = MvxSimpleIoCContainer.Initialize(this.CreateIocOptions());
            this._ioc.RegisterSingleton(this._ioc);
            this._ioc.RegisterSingleton<IMvxTrace>(new TestTrace());
            InitializeSingletonCache();
            this.InitializeMvxSettings();
            MvxTrace.Initialize();
            this.AdditionalSetup();
        }

        private static void InitializeSingletonCache()
        {
            MvxSingletonCache.Initialize();
        }

        protected virtual void InitializeMvxSettings()
        {
            this._ioc.RegisterSingleton<IMvxSettings>(new MvxSettings());
        }

        protected virtual void AdditionalSetup()
        {
            // nothing here..
        }

        protected void SetInvariantCulture()
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = invariantCulture;
            Thread.CurrentThread.CurrentUICulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;
        }
    }
}