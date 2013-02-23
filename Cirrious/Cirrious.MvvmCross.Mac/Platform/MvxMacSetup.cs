// MvxMacSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxMacSetup
        : MvxSetup
          , IMvxServiceProducer
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly IMvxMacViewPresenter _presenter;

        protected MvxMacSetup(MvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            _presenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderBasedPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Mac", toReturn.Loaders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override sealed MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxMacViewsContainer();
            RegisterTouchViewCreator(container);
            return container;
        }

        protected void RegisterTouchViewCreator(MvxMacViewsContainer container)
        {
            this.RegisterServiceInstance<IMvxMacViewCreator>(container);
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxMacViewDispatcherProvider(_presenter);
        }

        protected override void InitializePlatformServices()
        {
            this.RegisterServiceInstance<IMvxLifetime>(_applicationDelegate);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxMacView));
        }
    }
}