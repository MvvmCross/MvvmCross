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
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
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

            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof (IMvxTouchView));
        }

        protected override void InitializeLastChance()
        {
            InitialiseBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitialiseBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
			var bindingBuilder = new MvxTouchBindingBuilder(FillTargetFactories, FillValueConverters, FillBindingNames);
            return bindingBuilder;
        }

		protected virtual void FillBindingNames (Cirrious.MvvmCross.Binding.BindingContext.IMvxBindingNameRegistry obj)
		{
			// this base class does nothing
		}

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            var holders = ValueConverterHolders;
            if (holders == null)
                return;

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            var staticFiller = new MvxStaticBasedValueConverterRegistryFiller(registry);
            foreach (var converterHolder in holders)
            {
                filler.AddFieldConverters(converterHolder);
                staticFiller.AddStaticFieldConverters(converterHolder);
            }
        }

        protected virtual IEnumerable<Type> ValueConverterHolders
        {
            get { return null; }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }
}