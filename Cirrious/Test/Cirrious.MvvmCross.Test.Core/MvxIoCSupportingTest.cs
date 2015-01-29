// MvxIoCSupportingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.Test.Core
{
    public class MvxIoCSupportingTest
    {
        private IMvxIoCProvider _ioc;

        protected IMvxIoCProvider Ioc
        {
            get { return _ioc; }
        }

        public void Setup()
        {
            ClearAll();
        }
        
        protected virtual IMvxIocOptions IocOptions
        {
            get { return null; }
        }

        protected virtual void ClearAll()
        {
            // fake set up of the IoC
            MvxSingleton.ClearAllSingletons();
            _ioc = MvxSimpleIoCContainer.Initialize(IocOptions);
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
    }
}
