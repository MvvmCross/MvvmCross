#region Copyright
// <copyright file="MvxBaseTouchSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.Plugins;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;


#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Mac.Views;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Mac.Platform
{
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
			var toReturn = new MvxLoaderBasedPluginManager();
			var registry = new MvxLoaderPluginRegistry(".Mac", toReturn.Loaders);
			AddPluginsLoaders(registry);
			return toReturn;
		}
		
		protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
		{
			// none added by default
		}

        protected sealed override MvxViewsContainer CreateViewsContainer()
        {
			var container = new MvxMacViewsContainer();
            RegisterTouchViewCreator(container);            
            return container;
        }

        protected void RegisterTouchViewCreator(MvxMacViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxMacViewCreator>(container);
        }

        protected override MvvmCross.Interfaces.Views.IMvxViewDispatcherProvider CreateViewDispatcherProvider()
        {
            return new MvxMacViewDispatcherProvider(_presenter);
        }
	
		protected override void InitializePlatformServices ()
		{
			Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
		}

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return GetViewModelViewLookup(GetType().Assembly, typeof(IMvxMacView));
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
			var bindingBuilder = new MvxMacBindingBuilder(FillTargetFactories, FillValueConverters);
			return bindingBuilder;
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