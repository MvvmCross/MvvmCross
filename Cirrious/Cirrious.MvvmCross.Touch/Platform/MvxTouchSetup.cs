// MvxTouchSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxTouchSetup
        : MvxSetup
          
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly IMvxTouchViewPresenter _presenter;

        protected MvxTouchSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
        {
            _presenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected override void InitializeDebugServices()
        {
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderBasedPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Touch", toReturn.Loaders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override sealed MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxTouchViewsContainer();
            RegisterTouchViewCreator(container);
            return container;
        }

        protected virtual void RegisterTouchViewCreator(MvxTouchViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxTouchViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxTouchViewDispatcherProvider(_presenter);
        }

        protected override void InitializePlatformServices()
        {
            Mvx.RegisterSingleton<IMvxTouchPlatformProperties>(new MvxTouchPlatformProperties());
            Mvx.RegisterSingleton(_presenter);

            Mvx.RegisterSingleton<IMvxReachability>(new MvxReachability());
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxTouchView));
        }
    }
}