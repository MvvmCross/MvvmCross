// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Core;
using MvvmCross.IoC;

namespace MvvmCross.Tests
{
    public class MvxIoCSupportingTest
    {
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

        public void SetInvariantCulture()
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;
        }
    }
}
