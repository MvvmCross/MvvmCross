// MvxTouchSetup.cs
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
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.Views;
using Cirrious.CrossCore.Touch.Views;

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

		protected override IMvxTrace CreateDebugTrace()
		{
			return new MvxDebugTrace();
		}

		protected override IMvxPluginManager CreatePluginManager()
		{
			var toReturn = new MvxLoaderPluginManager();
			var registry = new MvxLoaderPluginRegistry(".Touch", toReturn.Finders);
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

		protected override IMvxViewDispatcher CreateViewDispatcher()
		{
			return new MvxTouchViewDispatcher(_presenter);
		}

		protected override void InitializePlatformServices()
		{
			RegisterPlatformProperties();
			// for now we continue to register the old style platform properties
			RegisterOldStylePlatformProperties();
			RegisterPresenter();
			RegisterLifetime();
		}

		protected virtual void RegisterPlatformProperties()
		{
			Mvx.RegisterSingleton<IMvxTouchSystem>(CreateTouchSystemProperties());
		}

		protected virtual MvxTouchSystem CreateTouchSystemProperties()
		{
			return new MvxTouchSystem();
		}

		[Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
		protected virtual void RegisterOldStylePlatformProperties()
		{
			Mvx.RegisterSingleton<IMvxTouchPlatformProperties>(new MvxTouchPlatformProperties());
		}

		protected virtual void RegisterLifetime()
		{
			Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
		}

		protected virtual void RegisterPresenter()
		{
			Mvx.RegisterSingleton(_presenter);
			Mvx.RegisterSingleton<IMvxTouchModalHost>(_presenter);
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