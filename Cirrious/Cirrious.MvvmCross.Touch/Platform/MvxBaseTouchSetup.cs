#region Copyright
// <copyright file="MvxBaseTouchSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public abstract class MvxBaseTouchSetup
        : MvxBaseSetup
        , IMvxServiceProducer<IMvxTrace>
        , IMvxServiceProducer<IMvxTouchNavigator>
        , IMvxServiceProducer<IMvxTouchViewCreator>
        , IMvxServiceProducer<IMvxTouchPlatformProperties>
        , IMvxServiceProducer<IMvxReachability>
        , IMvxServiceProducer<IMvxLifetime>
    {
        private readonly MvxApplicationDelegate _applicationDelegate;
        private readonly IMvxTouchViewPresenter _presenter;

        protected MvxBaseTouchSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
        {
			_presenter = presenter;
			_applicationDelegate = applicationDelegate;
        }

        protected override void InitializeDebugServices()
        {
            this.RegisterServiceInstance<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override MvvmCross.Interfaces.Plugins.IMvxPluginManager CreatePluginManager()
        {
            return new MvxFileBasedPluginManager("Touch");
        }

        protected sealed override MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxTouchViewsContainer();
            RegisterTouchViewCreator(container);            
            return container;
        }

        protected void RegisterTouchViewCreator(MvxTouchViewsContainer container)
        {
            this.RegisterServiceInstance<IMvxTouchViewCreator>(container);
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxTouchViewDispatcherProvider(_presenter);
        }
	
		protected override void InitializePlatformServices ()
		{
            this.RegisterServiceInstance<IMvxTouchPlatformProperties>(new MvxTouchPlatformProperties());

#warning Need To check whether IMvxReachability is OK for Singleton use...
            this.RegisterServiceInstance<IMvxReachability>(new MvxReachability());
            this.RegisterServiceInstance<IMvxLifetime>(_applicationDelegate);
		}

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxTouchView));
        }
    }
}