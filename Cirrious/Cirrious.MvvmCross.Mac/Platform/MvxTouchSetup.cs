// MvxMacSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Mac.Views;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.Views;
using Cirrious.CrossCore.Mac.Views;

namespace Cirrious.MvvmCross.Mac.Platform
{
#warning We seem to have 2 MvxMacSetup files - TODO - sort this out
#warning We seem to have 2 MvxMacSetup files - TODO - sort this out
#warning We seem to have 2 MvxMacSetup files - TODO - sort this out
#warning We seem to have 2 MvxMacSetup files - TODO - sort this out
    public abstract class MvxMacSetup
        : MvxSetup
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
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Mac", toReturn.Finders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateMacViewsContainer();
            RegisterMacViewCreator(container);
            return container;
        }

        protected virtual IMvxMacViewsContainer CreateMacViewsContainer()
        {
            return new MvxMacViewsContainer();
        }

        protected void RegisterMacViewCreator(MvxMacViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxMacViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxMacViewDispatcher(_presenter);
        }

        protected override void InitializePlatformServices()
        {
            Mvx.RegisterSingleton(_presenter);
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
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
			var bindingBuilder = new MvxMacBindingBuilder(FillTargetFactories, FillValueConverters, FillBindingNames);
            return bindingBuilder;
        }

		protected virtual void FillBindingNames (Cirrious.MvvmCross.Binding.BindingContext.IMvxBindingNameRegistry obj)
		{
			// this base class does nothing
		}

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders
        {
            get { return new List<Type>(); }
        }

        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }
}