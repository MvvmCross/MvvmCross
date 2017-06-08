// MvxTvosSetup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.tvOS;
using MvvmCross.Core.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform.tvOS.Platform;
using MvvmCross.Platform.tvOS.Views;
using MvvmCross.tvOS.Views;
using MvvmCross.tvOS.Views.Presenters;
using UIKit;

namespace MvvmCross.tvOS.Platform
{
    public abstract class MvxTvosSetup
        : MvxSetup
    {
        private readonly IMvxApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private IMvxTvosViewPresenter _presenter;

        protected MvxTvosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        protected MvxTvosSetup(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter)
        {
            _presenter = presenter;
            _applicationDelegate = applicationDelegate;
        }

        protected UIWindow Window => _window;

        protected IMvxApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxPluginManager();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateTvosViewsContainer();
            RegisterTvosViewCreator(container);
            return container;
        }

        protected virtual IMvxTvosViewsContainer CreateTvosViewsContainer()
        {
            return new MvxTvosViewsContainer();
        }

        protected virtual void RegisterTvosViewCreator(IMvxTvosViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxTvosViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxTvosViewDispatcher(Presenter);
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
            Mvx.RegisterSingleton<IMvxTvosSystem>(CreateTvosSystemProperties());
        }

        protected virtual MvxTvosSystem CreateTvosSystemProperties()
        {
            return new MvxTvosSystem();
        }

        [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
        protected virtual void RegisterOldStylePlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxTvosPlatformProperties>(new MvxTvosPlatformProperties());
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxTvosViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreatePresenter();
                return _presenter;
            }
        }

        protected virtual IMvxTvosViewPresenter CreatePresenter()
        {
            return new MvxTvosViewPresenter(_applicationDelegate, _window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxTvosModalHost>(presenter);
        }

        protected override void InitializeLastChance()
        {
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitializeBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxTvosBindingBuilder();
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
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

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new MvxPostfixAwareViewToViewModelNameMapping("View", "ViewController");
        }
    }
}