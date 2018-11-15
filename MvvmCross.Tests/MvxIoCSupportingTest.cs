// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Logging;

namespace MvvmCross.Tests
{
    public class MvxIoCSupportingTest
    {
        private TestLogger _logger;

        public IMvxIoCProvider Ioc { get; private set; }

        public void Setup()
        {
            ClearAll();
        }

        public void Reset()
        {
            MvxSingleton.ClearAllSingletons();
            Ioc = null;
        }

        protected virtual IMvxIocOptions CreateIocOptions()
        {
            return null;
        }

        public virtual void ClearAll(IMvxIocOptions options = null)
        {
            // fake set up of the IoC
            Reset();
            Ioc = MvxIoCProvider.Initialize(options ?? CreateIocOptions());
            Ioc.RegisterSingleton(Ioc);
            CreateLog();
            InitializeSingletonCache();
            InitializeMvxSettings();
            AdditionalSetup();
        }

        public void InitializeSingletonCache()
        {
            if (MvxSingletonCache.Instance == null)
                MvxSingletonCache.Initialize();

            if (MvxBindingSingletonCache.Instance == null)
                MvxBindingSingletonCache.Initialize();
        }

        protected virtual void InitializeMvxSettings()
        {
            Ioc.RegisterSingleton<IMvxSettings>(new MvxSettings());
        }

        protected virtual void AdditionalSetup()
        {
        }

        public void SetupTestLogger(TestLogger logger)
        {
            _logger = logger;
            CreateLog();
        }

        protected virtual void CreateLog()
        {
            var logProvider = new TestLogProvider(_logger);
            Ioc.RegisterSingleton<IMvxLogProvider>(logProvider);

            var globalLog = logProvider.GetLogFor<MvxLog>();
            MvxLog.Instance = globalLog;
            Ioc.RegisterSingleton(globalLog);

            var pluginLog = logProvider.GetLogFor("MvxPlugin");
        }

        public void SetInvariantCulture()
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;
        }
    }
}
