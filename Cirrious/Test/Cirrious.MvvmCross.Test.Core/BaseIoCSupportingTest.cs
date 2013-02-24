// BaseIoCSupportingTest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.IoC;
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
            _ioc = MvxSimpleIoCContainer.Initialise();
            _ioc.RegisterSingleton(_ioc);
            _ioc.RegisterSingleton<IMvxTrace>(new TestTrace());
            MvxTrace.Initialize();
        }
    }
}